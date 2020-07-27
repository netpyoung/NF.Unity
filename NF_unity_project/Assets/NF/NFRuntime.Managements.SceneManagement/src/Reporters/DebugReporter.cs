using System;
using System.Threading.Tasks;

namespace NFRuntime.Managements.SceneManagement.Reporters
{
    public class DebugReporter : AReporter
    {
        protected override Task ReportAsync(object value, float unityProgress, float logicProgress)
        {
            UnityEngine.Debug.Log($"{unityProgress} - {logicProgress} - {value}");
            return Task.CompletedTask;
        }
    }
}
