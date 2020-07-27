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

        public Task<T> LoadAsync<T>(params object[] args) where T : ASceneGroup
        {
            return LoadSceneGroupAsync((T)Activator.CreateInstance(typeof(T), args));
        }

        async Task<T> LoadSceneGroupAsync<T>(T sceneGroup) where T : ASceneGroup
        {
            if (this.CurrentSceneGroup != null)
            {
                await this.CurrentSceneGroup.UnloadAsync();
            }
            this.CurrentSceneGroup = sceneGroup;
            return (T)await sceneGroup.LoadAsync(this.Reporter);
        }

        public Task<T> PushAsync<T>(params object[] args) where T : ASceneGroup
        {
            Stack.Push(this.CurrentSceneGroup);
            return LoadAsync<T>(args);
        }

        public Task<ASceneGroup> PopAsync()
        {
            return LoadSceneGroupAsync(Stack.Pop());
        }

        public void RegisterReporter<T>() where T : AReporter
        {
            this.Reporter = Activator.CreateInstance<T>();
        }

        public void RegisterReporter<T>(T reporter) where T : AReporter
        {
            this.Reporter = reporter;
        }
    }
}