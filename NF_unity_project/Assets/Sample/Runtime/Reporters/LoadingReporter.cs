using Cysharp.Threading.Tasks;
using NFRuntime.Managements.SceneManagement;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sample.Runtime
{
    public class LoadingReporter : AReporter
    {
        public Scene UnityScene { get; private set; }
        public bool IsReady { get; private set; }
        int sceneBuildIndex = Convert.ToInt32(E_SCENE.Loading);

        protected override async Task PrepareAsync()
        {
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive);
        }

        protected override async Task ReportAsync(object value, float unityProgress, float logicProgress)
        {
            Debug.Log($"{unityProgress} - {logicProgress} - {value}");
            await Task.Delay(1000);

            if (unityProgress + logicProgress >= 2)
            {
                var ao = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneBuildIndex);
                if (ao != null)
                {
                    await ao;
                }
            }
        }
    }

}