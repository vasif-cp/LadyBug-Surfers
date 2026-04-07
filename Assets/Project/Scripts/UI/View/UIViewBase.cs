using DG.Tweening;
using UnityEngine;

namespace LS.UI.View
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIViewBase : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Tween _fadeTween;

        private const float FadeDuration = 0.25f;

        protected CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }

        public virtual void Show()
        {
            _fadeTween?.Kill();
            _fadeTween = CanvasGroup.DOFade(1.0f, FadeDuration).OnComplete(() =>
            {
                CanvasGroup.interactable = true;
                CanvasGroup.blocksRaycasts = true;
            });
        }

        public virtual void Hide()
        {
            _fadeTween?.Kill();
            _fadeTween = CanvasGroup.DOFade(0.0f, FadeDuration).OnComplete(() =>
            {
                CanvasGroup.interactable = false;
                CanvasGroup.blocksRaycasts = false;
            });
        }
    }
}
