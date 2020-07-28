#if SUPPORT_SPINE
using Spine.Unity;
using System.Collections;
using UnityEngine;

namespace NFRuntime.Supports.SupporSpine
{
    [DisallowMultipleComponent]
    public class WaitForSpineAnimation : IEnumerator
    {
        public SkeletonAnimation SkeletonAnimation { get; }
        readonly IEnumerator mCompleteEnumerator;

        public WaitForSpineAnimation(SkeletonAnimation skeleton_animation, string animation_name)
        {
            this.SkeletonAnimation = skeleton_animation;
            var state = this.SkeletonAnimation.AnimationState;
            var track = state.SetAnimation(trackIndex: 0, animationName: animation_name, loop: false);
            this.mCompleteEnumerator = new WaitForSpineAnimationComplete(track);
        }

        object IEnumerator.Current
        {
            get
            {
                return this.mCompleteEnumerator.Current;
            }
        }

        bool IEnumerator.MoveNext()
        {
            return this.mCompleteEnumerator.MoveNext();
        }

        void IEnumerator.Reset()
        {
            this.mCompleteEnumerator.Reset();
        }
    }
}
#endif // SUPPORT_SPINE