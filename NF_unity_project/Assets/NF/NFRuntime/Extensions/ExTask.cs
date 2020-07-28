using System.Collections;
using System.Threading.Tasks;

namespace NFRuntime.Extensions
{
    public static class ExTask
    {
        public static IEnumerator AsIEnumerator(this Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsFaulted)
            {
                if (task.Exception != null)
                {
                    throw task.Exception;
                }
            }
        }
    }
}