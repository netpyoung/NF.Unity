using NFRuntime.Managements.SceneManagement;
using System.Threading.Tasks;

namespace Sample.Runtime
{
    internal class SG_B : ASceneGroup<Scene3, Scene4>
    {
        public override Task PreLoadAsync(AReporter reporter, Scene3 t1, Scene4 t2)
        {
            t1.Hello();
            t2.Hello();
            return Task.CompletedTask;
        }
    }
}