using System.Threading.Tasks;

namespace NFRuntime.Managements.SceneManagement
{
    public abstract class AReporter
    {
        public ASceneGroup CurrentSceneGroup { get; private set; }
        float _accumulatedLogicProgress = 0f;
        public float AccumulatedLogicProgress
        {
            get => _accumulatedLogicProgress;
            private set
            {
                _accumulatedLogicProgress += value;
                if (_accumulatedLogicProgress > 1)
                {
                    _accumulatedLogicProgress = 1;
                }
            }
        }
        public Task Report(in object value)
        {
            return ReportAsync(value, CurrentSceneGroup.Progress, 0f);
        }

        public Task Report(in object value, in float logicProgressAmount)
        {
            AccumulatedLogicProgress += logicProgressAmount;
            return ReportAsync(value, CurrentSceneGroup.Progress, AccumulatedLogicProgress);
        }

        internal Task InternalPrepareAsync()
        {
            return PrepareAsync();
        }

        internal Task ReportSceneLoadingProgress()
        {
            return ReportAsync(null, CurrentSceneGroup.Progress, 0f);
        }

        internal Task ReportEnd()
        {
            return ReportAsync(null, 1, 1);
        }

        protected virtual Task PrepareAsync()
        {
            return Task.CompletedTask;
        }

        protected abstract Task ReportAsync(object value, float unityProgress, float logicProgress);

        internal void SetCurrentSceneGroup(ASceneGroup sceneGroup)
        {
            this.CurrentSceneGroup = sceneGroup;
            this._accumulatedLogicProgress = 0;
        }
    }
}
