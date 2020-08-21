using UnityEngine;

namespace NFRuntime.Util.Components
{
    // ref: http://tsubakit1.hateblo.jp/entry/2016/02/11/021743

    [DisallowMultipleComponent]
    public class WaitForAnimator : CustomYieldInstruction
    {
        public Animator Animator { get; }
        private readonly int mHash;
        private readonly int mLayerIndex;

        public WaitForAnimator(Animator animator, int layerIndex)
        {
            this.mLayerIndex = layerIndex;
            this.Animator = animator;
            this.mHash = animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash;
        }

        public override bool keepWaiting
        {
            get
            {
                var currentAnimatorState = this.Animator.GetCurrentAnimatorStateInfo(this.mLayerIndex);
                return (currentAnimatorState.fullPathHash == this.mHash) && (currentAnimatorState.normalizedTime < 1);
            }
        }
    }
}