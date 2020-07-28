#if SUPPORT_DOTWEEN
using DG.Tweening;
using NFRuntime.Attributes;
using NFRuntime.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NFRuntime.Supports.SupportDoTween
{
    // ref: http://qiita.com/RyotaMurohoshi/items/83ebfa4d19264996b8cb
    [DisallowMultipleComponent]
    public class SliderAnimating : MonoBehaviour
    {
        [ComponentPath(nameof(slider_foreground))]
        protected Slider slider_foreground = null;
        [ComponentPath(nameof(slider_background))]
        protected Slider slider_background = null;

        virtual protected void Awake()
        {
            this.InitComponentPathAttributes();
        }

        public Coroutine AnimateRatio(
           float value,
           float frontDuration = 0.1F,
           float backgroundDuration = 0.3F,
           float backgroundDelay = 0.2F)
        {
            return StartCoroutine(AnimateRatioEnumerator(value, frontDuration, backgroundDuration, backgroundDelay));
        }

        IEnumerator AnimateRatioEnumerator(
           float value,
           float frontDuration,
           float backgroundDuration,
           float backgroundDelay)
        {
            var clampedValue = Mathf.Clamp01(value);

            var frontTweenner = slider_foreground.DOValue(clampedValue, frontDuration).SetEase(Ease.InQuad);

            if (slider_background)
            {
                yield return slider_background
                     .DOValue(clampedValue, backgroundDuration)
                     .SetDelay(backgroundDelay + frontDuration)
                     .SetEase(Ease.InOutQuad)
                     .WaitForCompletion();
            }
            else
            {
                yield return frontTweenner.WaitForCompletion();
            }
        }
    }
}
#endif // SUPPORT_DOTWEEN