using Cysharp.Threading.Tasks;
using System.Threading.Tasks;

namespace NFRuntime.Managements.ResourceManagement
{
    public class ResourcesResourceManager : IUnityResourceManager
    {
        public async Task<T> LoadAsync<T>(string name) where T : UnityEngine.Object
        {
            var request = UnityEngine.Resources.LoadAsync<T>(name);
            await request;
            return request.asset as T;
        }
    }
}
