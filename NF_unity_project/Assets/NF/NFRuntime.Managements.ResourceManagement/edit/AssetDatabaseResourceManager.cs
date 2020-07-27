using NFRuntime.Managements.ResourceManagement;
using System.Threading.Tasks;
using UnityEngine;

namespace NFEditor.Managements.ResourceManagement
{
    public class AssetDatabaseResourceManager : IUnityResourceManager
    {
        // TODO(pyoung): default path
        public Task<T> LoadAsync<T>(string name) where T : Object
        {
            return Task.FromResult(UnityEditor.AssetDatabase.LoadAssetAtPath<T>(name));
        }
    }

}