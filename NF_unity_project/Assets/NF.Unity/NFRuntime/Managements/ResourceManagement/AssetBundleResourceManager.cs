using System.Threading.Tasks;
using NFRuntime.Managements.ResourceManagement.AssetBundleLoad;

namespace NFRuntime.Managements.ResourceManagement
{
    public class AssetBundleResourceManager : IUnityResourceManager
    {
        private readonly AssetBundleRoader _loader;
        public AssetBundleResourceManager(AssetBundleRoader manager)
        {
            this._loader = manager;
        }

        public async Task<T> LoadAsync<T>(string assetName) where T : UnityEngine.Object
        {
            var bundle = await this._loader.LoadAssetBundleAsync(assetName);
            var asset = await this._loader.LoadAssetAsync<T>(bundle, assetName);
            this._loader.UnloadAssetBundle(assetName);
            return asset;
        }
    }
}