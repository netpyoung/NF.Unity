using System.Threading.Tasks;
using UnityEngine;

namespace NFRuntime.Managements.ResourceManagement
{
    public interface IUnityResourceManager
    {
        Task<T> LoadAsync<T>(string name) where T : Object;
    }
}