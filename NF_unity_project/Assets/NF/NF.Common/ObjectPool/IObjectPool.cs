namespace NF.Common.ObjectPool
{
    // TODO(pyoung):https://www.stevejgordon.co.uk/talk-writing-high-performance-csharp-and-net-code
    // https://docs.microsoft.com/ko-kr/dotnet/api/system.threading.threadlocal-1?view=netcore-3.1
    public interface IObjectPool<T>
    {
        bool TryTake(out T t);
        bool Return(T t);
        int UsedSize { get; }
        int CachedSize { get; }
    }
}