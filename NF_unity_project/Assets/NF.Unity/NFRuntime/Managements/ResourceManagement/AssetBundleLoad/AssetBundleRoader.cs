using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace NFRuntime.Managements.ResourceManagement.AssetBundleLoad
{
    public class AssetBundleRoader
    {
        private AssetBundleManifest mManifest = null;
        private string mVariant = string.Empty;
        private readonly Dictionary<string, Task<AssetBundle>> mCurrentlyLoading = new Dictionary<string, Task<AssetBundle>>();
        private readonly Dictionary<string, AssetBundleContainer> mLoadedAssetBundles = new Dictionary<string, AssetBundleContainer>();
        private readonly Dictionary<string, HashSet<string>> mVariants = new Dictionary<string, HashSet<string>>();

        public async Task InitializeAsync()
        {
            AssetBundle bundle = await LoadIndividualAssetBundleAsync(AssetBundleUtilities.GetPlatformName());
            this.mManifest = await LoadAssetAsync<AssetBundleManifest>(bundle, "AssetBundleManifest");
            ProcessVariants(this.mManifest, mVariants);
        }

        public async Task<T> LoadAssetAsync<T>(AssetBundle bundle, string assetName) where T : UnityEngine.Object
        {
            Assert.IsNotNull(bundle);
            Assert.IsFalse(string.IsNullOrEmpty(assetName));

            AssetBundleRequest request = bundle.LoadAssetAsync<T>(assetName);
            await request;
            return request.asset as T;
        }

        #region Internal
        internal async Task<AssetBundle> LoadAssetBundleAsync(string name)
        {
            Task<AssetBundle> bundleTask = this.LoadIndividualAssetBundleAsync(name);
            string[] dependencies = this.mManifest.GetAllDependencies(name);

            List<Task<AssetBundle>> tasks = new List<Task<AssetBundle>>(dependencies.Length + 1);
            tasks.Add(bundleTask);
            foreach (string dependency in dependencies)
            {
                tasks.Add(LoadIndividualAssetBundleAsync(dependency));
            }
            await Task.WhenAll(tasks.ToArray());
            return bundleTask.Result;
        }

        internal void UnloadAssetBundle(string assetName)
        {
            UnloadIndividualAssetBundle(assetName);
            string[] dependencies = this.mManifest.GetAllDependencies(assetName);
            foreach (string dependency in dependencies)
            {
                UnloadIndividualAssetBundle(dependency);
            }
        }
        #endregion Internal

        private void UnloadIndividualAssetBundle(in string name)
        {
            string variantName = ResolveVariantName(name);
            if (mLoadedAssetBundles.TryGetValue(variantName, out AssetBundleContainer container))
            {
                container.Decrement();
                if (container.Reference == 0)
                {
                    container.Bundle.Unload(false);
                    mLoadedAssetBundles.Remove(variantName);
                }
            }
        }
        private async Task<AssetBundle> LoadIndividualAssetBundleAsync(string name)
        {
            string variantName = ResolveVariantName(name);

            // find in currently loaded assetbundles
            if (mLoadedAssetBundles.TryGetValue(variantName, out AssetBundleContainer container))
            {
                container.Increment();
                return container.Bundle;
            }

            if (mCurrentlyLoading.TryGetValue(variantName, out Task<AssetBundle> task))
            {
                // currently already started loading, wait for it to complete
                AssetBundle bundle = await task;
                mLoadedAssetBundles[variantName].Increment();
                return bundle;
            }
            else
            {
                // not loaded, start loading
                Task<AssetBundle> request = LoadAssetBundleActualAsync(variantName);
                mCurrentlyLoading.Add(variantName, request);
                AssetBundle bundle = await request;
                container = new AssetBundleContainer
                {
                    Bundle = bundle,
                };
                mLoadedAssetBundles.Add(variantName, container);
                mCurrentlyLoading.Remove(variantName);
                return bundle;
            }
        }
        private async Task<AssetBundle> LoadAssetBundleActualAsync(string name)
        {
            string path = $"{AssetBundleUtilities.GetAssetBundleFolderPath()}/{name}";
            Assert.IsTrue(File.Exists(path), $"AssetBundle at {path} not found");

            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
            await request;
            return request.assetBundle as AssetBundle;
        }

        private string ResolveVariantName(string assetBundleName)
        {
            string[] array = assetBundleName.Split('.');
            string baseName = array[0];
            if (mVariants.TryGetValue(baseName, out HashSet<string> variants))
            {
                if (variants.Contains(mVariant))
                {
                    return $"{baseName}.{mVariant}";
                }
            }
            return assetBundleName;
        }

        private void ProcessVariants(AssetBundleManifest manifest, Dictionary<string, HashSet<string>> variants)
        {
            variants.Clear();
            string[] bundleNames = manifest.GetAllAssetBundlesWithVariant();
            foreach (string bundleName in bundleNames)
            {
                string[] array = bundleName.Split('.');
                string baseName = array[0];
                string variant = array[1];

                if (variants.TryGetValue(baseName, out HashSet<string> set))
                {
                    set.Add(variant);
                }
                else
                {
                    set = new HashSet<string> { variant };
                    variants.Add(baseName, set);
                }
            }
        }
    }
}