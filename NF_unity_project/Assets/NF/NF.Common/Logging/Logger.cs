using NF.Common.ObjectPool;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace NF.Logging
{
    public class Logger
    {
        private const string INVALID_FORMAT = "Invalid Format";
        ReaderWriterLockSlim mLock = new ReaderWriterLockSlim();
        ObjectPoolConcurrent<StringBuilder> mStringBuilderPool = new ObjectPoolConcurrent<StringBuilder>(5);
        long mSequenceNumber = 0; // TODO(pyoung): ulong support from 2.1
        DateTime mCachedDate = DateTime.MinValue;
        string mCachedFileName = "empty.log";

        public string BaseDir { get; private set; }

        public eLogLevel mLogLevel { get; set; }
        string LogFilePath
        {
            get
            {
                DateTime nowDate = DateTime.Now.Date;
                if (nowDate == mCachedDate)
                {
                    return mCachedFileName;
                }

                mCachedDate = nowDate;
                if (string.IsNullOrWhiteSpace(BaseDir))
                {
                    mCachedFileName = $"{nowDate:yyyy_MM_dd}.log";
                }
                else
                {
                    mCachedFileName = Path.Combine(BaseDir, $"{nowDate:yyyy_MM_dd}.log");
                }
                return mCachedFileName;
            }
        }

        public Logger(string baseDir, eLogLevel logLevel = eLogLevel.Trace)
        {
            BaseDir = baseDir;
            mLogLevel = logLevel;
        }

        #region LogHex
        public void TraceHex(string tag, Span<byte> bytes)
        {
            _LogHex(tag, eLogLevel.Trace, bytes);
        }
        public void DebugHex(string tag, Span<byte> bytes)
        {
            _LogHex(tag, eLogLevel.Debug, bytes);
        }
        public void InfoHex(string tag, Span<byte> bytes)
        {
            _LogHex(tag, eLogLevel.Info, bytes);
        }
        public void WarnHex(string tag, Span<byte> bytes)
        {
            _LogHex(tag, eLogLevel.Warn, bytes);
        }
        public void ErrorHex(string tag, Span<byte> bytes)
        {
            _LogHex(tag, eLogLevel.Error, bytes);
        }
        public void FatalHex(string tag, Span<byte> bytes)
        {
            _LogHex(tag, eLogLevel.Fatal, bytes);
        }

        #endregion LogHex

        #region Log
        public void Trace(string tag, string message, Exception exception = null)
        {
            _Log(tag, eLogLevel.Trace, message, exception);
        }

        public void Debug(string tag, string message, Exception exception = null)
        {
            _Log(tag, eLogLevel.Debug, message, exception);
        }
        public void Info(string tag, string message, Exception exception = null)
        {
            _Log(tag, eLogLevel.Info, message, exception);
        }

        public void Warn(string tag, string message, Exception exception = null)
        {
            _Log(tag, eLogLevel.Warn, message, exception);
        }

        public void Error(string tag, string message, Exception exception = null)
        {
            _Log(tag, eLogLevel.Error, message, exception);
        }

        public void Fatal(string tag, string message, Exception exception = null)
        {
            _Log(tag, eLogLevel.Fatal, message, exception);
        }
        #endregion Log

        #region LogFormat
        public void TraceFormat(string tag, string fmt, params object[] args)
        {
            _LogFormat(tag, eLogLevel.Trace, fmt, args);
        }
        public void DebugFormat(string tag, string fmt, params object[] args)
        {
            _LogFormat(tag, eLogLevel.Debug, fmt, args);
        }
        public void InfoFormat(string tag, string fmt, params object[] args)
        {
            _LogFormat(tag, eLogLevel.Info, fmt, args);
        }
        public void WarnFormat(string tag, string fmt, params object[] args)
        {
            _LogFormat(tag, eLogLevel.Warn, fmt, args);
        }
        public void ErrorFormat(string tag, string fmt, params object[] args)
        {
            _LogFormat(tag, eLogLevel.Error, fmt, args);
        }
        public void FatalFormat(string tag, string fmt, params object[] args)
        {
            _LogFormat(tag, eLogLevel.Fatal, fmt, args);
        }
        #endregion LogFormat

        bool IsValidLogLevel(eLogLevel logLevel)
        {
            return (int)logLevel >= (int)mLogLevel;
        }

        void _Log(string tag, eLogLevel logLevel, string message, Exception exception)
        {
            if (!IsValidLogLevel(logLevel))
            {
                return;
            }

            __Log(new LogInfo
            {
                DateTime = DateTime.Now,
                SequnceNumber = Interlocked.Increment(ref mSequenceNumber),
                Tag = tag,
                ThreadID = Thread.CurrentThread.ManagedThreadId,
                LogLevel = logLevel,
                Message = message,
                Exception = exception
            });
        }

        void _LogFormat(string tag, eLogLevel logLevel, string fmt, params object[] args)
        {
            if (!IsValidLogLevel(logLevel))
            {
                return;
            }

            Exception exception = null;
            string message;
            try
            {
                message = string.Format(fmt, args);
            }
            catch (Exception ex)
            {
                message = INVALID_FORMAT;
                exception = ex;
            }
            __Log(new LogInfo
            {
                DateTime = DateTime.Now,
                SequnceNumber = Interlocked.Increment(ref mSequenceNumber),
                Tag = tag,
                ThreadID = Thread.CurrentThread.ManagedThreadId,
                LogLevel = eLogLevel.Info,
                Message = message,
                Exception = exception
            });
        }

        void __Log(LogInfo logInfo)
        {
            mLock.EnterWriteLock();
            try
            {
                // TODO(pyoung): using span from 2.1, use arraypool?
                string logMessage = logInfo.ToString();
                using (FileStream fs = File.Open(LogFilePath, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(logMessage);
                        sw.Flush();
                    }
                }
            }
            finally
            {
                mLock.ExitWriteLock();
            }
        }

        void _LogHex(string tag, eLogLevel logLevel, Span<byte> bytes)
        {
            // ref: https://github.com/dotnet/runtime/blob/master/src/libraries/System.Private.CoreLib/src/System/BitConverter.cs#L364
            if (!IsValidLogLevel(logLevel))
            {
                return;
            }

            mStringBuilderPool.TryTake(out StringBuilder sb);
            sb.Clear();
            sb.Append("Log Hex\n");
            int acc = 0;
            for (int i = 0; i < bytes.Length; ++i)
            {
                sb.Append(bytes[i].ToString("X2"));
                if (++acc == 4)
                {
                    acc = 0;
                    sb.Append(" ");
                }
            }
            string message = sb.ToString();
            mStringBuilderPool.Return(sb);

            __Log(new LogInfo
            {
                DateTime = DateTime.Now,
                SequnceNumber = Interlocked.Increment(ref mSequenceNumber),
                Tag = tag,
                ThreadID = Thread.CurrentThread.ManagedThreadId,
                LogLevel = logLevel,
                Message = message,
                Exception = null
            });
        }
    }
}