using NFRuntime.Managements.SceneManagement;
using UnityEngine;

namespace Sample.Runtime
{
    public class Scene4 : AScene<E_SCENE>
    {
        public override E_SCENE SceneEnum => E_SCENE.Scene4;

        internal void Hello()
        {
            Debug.Log($"{nameof(Scene4)}");
        }
    }
}