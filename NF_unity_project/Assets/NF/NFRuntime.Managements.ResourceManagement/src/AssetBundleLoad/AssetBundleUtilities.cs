using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#else
using UnityEngine;
#endif

namespace NFRuntime.Managements.ResourceManagement.AssetBundleLoad
{
    public static partial class AssetBundleUtilities
    {
        public static string GetAssetBundleFolderPath()
        {
            return Path.GetFullPath($"{UnityEngine.Application.persistentDataPath}/AssetBundles/{GetPlatformName()}");
        }

        public static string GetPlatformName()
        {
#if UNITY_EDITOR
            return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
            return GetPlatformForAssetBundles(UnityEngine.Application.platform);
#endif
        }

#if UNITY_EDITOR
        private static string GetPlatformForAssetBundles(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.iOS:
                    return "iOS";
                default:
                    throw new PlatformNotSupportedException("Platform not supported for AssetBundles");

            }
        }
#else
        private static string GetPlatformForAssetBundles(RuntimePlatform platform)
        {
            switch(platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                default:
                    throw new PlatformNotSupportedException("Platform not supported for AssetBundles");
            }
        }
#endif
    }
}