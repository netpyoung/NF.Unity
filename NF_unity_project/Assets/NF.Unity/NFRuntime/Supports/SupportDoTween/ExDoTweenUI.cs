#if SUPPORT_DOTWEEN
using DG.Tweening;
using UnityEngine;

namespace NFRuntime.Supports.SupporSpine
{
    // ref: http://qiita.com/RyotaMurohoshi/items/dca738fb69548aae3cfb
    public static class ExDoTweenUI
    {
        public static Tweener FadeOut(this CanvasGroup canvasGroup, float duration)
        {
            return canvasGroup.DOFade(0.0F, duration);
        }

        public static Tweener FadeIn(this CanvasGroup canvasGroup, float duration)
        {
            return canvasGroup.DOFade(1.0F, duration);
        }
    }
}
#endif // SUPPORT_DOTWEEN