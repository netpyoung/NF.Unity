using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NFRuntime.Managements.SceneManagement
{
    public class SceneManager
    {
        public Stack<ASceneGroup> Stack = new Stack<ASceneGroup>();
        public ASceneGroup CurrentSceneGroup { get; private set; } = null;
        public AReporter Reporter { get; private set; } = null;

        public async Task<T> LoadAsync<T>(params object[] args) where T : ASceneGroup
        {
            T t = Activator.CreateInstance(typeof(T), args) as T;
            await LoadSceneGroupAsync(t);
            return t;
        }

        async Task<ASceneGroup> LoadSceneGroupAsync(ASceneGroup sceneGroup)
        {
            if (this.CurrentSceneGroup != null)
            {
                await this.CurrentSceneGroup.UnloadAsync();
            }
            this.CurrentSceneGroup = sceneGroup;
            return await sceneGroup.LoadAsync(this.Reporter);
        }

        public async Task<T> PushAsync<T>(params object[] args) where T : ASceneGroup
        {
            Stack.Push(this.CurrentSceneGroup);
            return await LoadAsync<T>(args);
        }

        public Task<ASceneGroup> PopAsync()
        {
            return LoadSceneGroupAsync(Stack.Pop());
        }

        public void RegisterReporter<T>() where T : AReporter
        {
            this.Reporter = Activator.CreateInstance<T>();
        }
    }
}