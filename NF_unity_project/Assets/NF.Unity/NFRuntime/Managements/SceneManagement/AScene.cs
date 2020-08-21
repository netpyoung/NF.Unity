using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace NFRuntime.Managements.SceneManagement
{
    public abstract class AScene
    {
        public abstract int SceneBuildIndex { get; }
        public AsyncOperation LoadAsyncOperation { get; private set; }
        public bool IsLoaded => (LoadAsyncOperation != null) ? this.LoadAsyncOperation.isDone : false;
        public float Progress => (LoadAsyncOperation != null) ? this.LoadAsyncOperation.progress : 0;

        public async UniTask LoadAsync(AReporter reporter, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode.Additive)
        {
            await reporter.Report(null);
            this.LoadAsyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(this.SceneBuildIndex, loadSceneMode);
            while (!this.LoadAsyncOperation.isDone)
            {
                await reporter.Report(null);
                await UniTask.Delay(16);
            }
            await reporter.Report(null);
        }

        public UniTask UnloadAsync()
        {
            return UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(this.SceneBuildIndex)
                .ToUniTask()
                .ContinueWith(() => this.LoadAsyncOperation = null);
        }
    }

    public abstract class AScene<T> : AScene where T : Enum
    {
        public abstract T SceneEnum { get; }
        public override int SceneBuildIndex => Convert.ToInt32(SceneEnum);
    }
}
