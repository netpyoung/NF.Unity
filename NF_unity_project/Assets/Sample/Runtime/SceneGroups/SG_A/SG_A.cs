using NFRuntime.Managements.SceneManagement;
using System.Threading.Tasks;

namespace Sample.Runtime
{
    internal class SG_A : ASceneGroup<Scene2>
    {
        public override Task PreloadAsync(AReporter reporter, Scene2 t1)
        {
            t1.Hello();
            reporter.Report("hello", 0.3f);
            return Task.CompletedTask;
        }
    }
}