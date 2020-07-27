using System;

namespace NF.Logging
{
    public struct LogInfo
    {
        public DateTime DateTime;
        public long SequnceNumber;
        public string Tag;
        public int ThreadID;
        public eLogLevel LogLevel;
        public string Message;
        public Exception Exception;

        public override string ToString()
        {
            if (Exception == null)
            {
                return $"{DateTime:HH:mm:ss.ffff}|{string.Format("{0,-5:#}", LogLevel.ToString())}|s/{string.Format("{0,-5:#}", SequnceNumber)}|t/{string.Format("{0,-5:#}", ThreadID)}|{string.Format("{0,5:#}", Tag)}|{Message}";
            }
            else
            {
                return $"{DateTime:HH:mm:ss.ffff}|{string.Format("{0,-5:#}", LogLevel.ToString())}|s/{string.Format("{0,-5:#}", SequnceNumber)}|t/{string.Format("{0,-5:#}", ThreadID)}|{string.Format("{0,5:#}", Tag)}|{Message}\n{Exception}";
            }
        }
    }
}