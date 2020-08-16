using UnityEngine;

namespace NFRuntime.Util.Components
{
    [DisallowMultipleComponent]
    public class WaitForAnimation : CustomYieldInstruction
    {
        public Animation Animation { get; }
        public string AnimationName { get; }

        public WaitForAnimation(Animation animation, string animationName)
        {
            this.Animation = animation;
            this.AnimationName = animationName;
            this.Animation.PlayQueued(animationName);
        }

        public override bool keepWaiting => this.Animation.IsPlaying(this.AnimationName);
    }
}