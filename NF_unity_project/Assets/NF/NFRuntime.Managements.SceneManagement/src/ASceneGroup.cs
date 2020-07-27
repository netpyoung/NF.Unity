using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NFRuntime.Managements.SceneManagement
{
    public abstract class ASceneGroup
    {
        protected abstract Type[] SceneTypes { get; }
        public AScene[] Scenes { get; private set; }
        public virtual E_SCENE_LOAD_ORDERING Ordering { get; private set; } = E_SCENE_LOAD_ORDERING.RANDOM;

        public bool IsLoaded
        {
            get
            {
                if (Scenes == null)
                {
                    return false;
                }
                return Scenes.All(x => x.IsLoaded);
            }
        }

        public float Progress
        {
            get
            {
                if (Scenes == null)
                {
                    return 0;
                }
                return Scenes.Sum(x => x.Progress) / Scenes.Length;
            }
        }

        internal async UniTask<ASceneGroup> LoadAsync(AReporter reporter)
        {
            reporter.SetCurrentSceneGroup(this);
            await reporter.InternalPrepareAsync();
            if (Ordering == E_SCENE_LOAD_ORDERING.RANDOM)
            {
                this.Scenes = SceneTypes.Select(x => (AScene)Activator.CreateInstance(x)).ToArray();
                var sceneLoadTasks = Scenes.Select(x => x.LoadAsync(reporter));
                await UniTask.WhenAll(sceneLoadTasks)
                    .ContinueWith(async () => await PreLoadAsync(Scenes, reporter))
                    .ContinueWith(async () => await reporter.ReportEnd());
            }
            else
            {
                this.Scenes = SceneTypes.Select(x => (AScene)Activator.CreateInstance(x)).ToArray();
                foreach (var scene in Scenes)
                {
                    await scene.LoadAsync(reporter);
                }
                await PreLoadAsync(Scenes, reporter);
                await reporter.ReportEnd();
            }
            reporter.SetCurrentSceneGroup(null);
            await AfterLoadAsync(Scenes);
            return this;
        }

        protected abstract Task PreLoadAsync(in AScene[] scenes, in AReporter reporter);
        protected abstract Task AfterLoadAsync(in AScene[] scenes);

        internal UniTask UnloadAsync()
        {
            var sceneLoadTasks = Scenes.Select(x => x.UnloadAsync());
            return UniTask.WhenAll(sceneLoadTasks)
                .ContinueWith(() => this.Scenes = null);
        }
    }

    public abstract class ASceneGroup<T0> : ASceneGroup
        where T0 : AScene
    {
        protected override Type[] SceneTypes => new Type[] { typeof(T0) };

        protected override Task PreLoadAsync(in AScene[] scenes, in AReporter reporter)
        {
            return PreloadAsync(reporter, scenes[0] as T0);
        }

        public virtual Task PreloadAsync(AReporter reporter, T0 t1)
        {
            return Task.CompletedTask;
        }
        protected override Task AfterLoadAsync(in AScene[] scenes)
        {
            return AfterLoadAsync(scenes[0] as T0);
        }
        public virtual Task AfterLoadAsync(T0 t1)
        {
            return Task.CompletedTask;
        }
    }

    public abstract class ASceneGroup<T0, T1> : ASceneGroup
        where T0 : AScene
        where T1 : AScene
    {
        protected override Type[] SceneTypes => new Type[] { typeof(T0), typeof(T1) };

        protected override Task PreLoadAsync(in AScene[] scenes, in AReporter reporter)
        {
            return PreLoadAsync(reporter, scenes[0] as T0, scenes[1] as T1);
        }

        public virtual Task PreLoadAsync(AReporter reporter, T0 t1, T1 t2)
        {
            return Task.CompletedTask;
        }
        protected override Task AfterLoadAsync(in AScene[] scenes)
        {
            return AfterLoadAsync(scenes[0] as T0, scenes[1] as T1);
        }
        public virtual Task AfterLoadAsync(T0 t1, T1 t2)
        {
            return Task.CompletedTask;
        }
    }

    public abstract class ASceneGroup<T0, T1, T2> : ASceneGroup
        where T0 : AScene
        where T1 : AScene
        where T2 : AScene
    {
        protected override Type[] SceneTypes => new Type[] { typeof(T0), typeof(T1), typeof(T2) };

        protected override Task PreLoadAsync(in AScene[] scenes, in AReporter reporter)
        {
            return PreLoadAsync(reporter, scenes[0] as T0, scenes[1] as T1, scenes[2] as T2);
        }

        public virtual Task PreLoadAsync(AReporter reporter, T0 t1, T1 t2, T2 t3)
        {
            return Task.CompletedTask;
        }
        protected override Task AfterLoadAsync(in AScene[] scenes)
        {
            return AfterLoadAsync(scenes[0] as T0, scenes[1] as T1, scenes[2] as T2);
        }
        public virtual Task AfterLoadAsync(T0 t1, T1 t2, T2 t3)
        {
            return Task.CompletedTask;
        }
    }
}