#region Using Statements
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
#endregion

namespace IboshEngine.Runtime.Systems.UISystem.Animation
{
    /// <summary>
    /// Controls UI element animations with various animation types including scale, fade, and directional movements.
    /// Uses DOTween for smooth animations.
    /// </summary>
    public class AnimationController : MonoBehaviour
    {
        [BoxGroup("General Settings")]
        [SerializeField]
        private AnimationType animationType = AnimationType.Scale;

        [BoxGroup("General Settings")]
        public float AnimationDuration = 0.1f;

        [FoldoutGroup("General Settings/Delays")]
        [SerializeField]
        private float showDelay;

        [FoldoutGroup("General Settings/Delays")]
        public float HideDelay;

        [ShowIf("animationType", AnimationType.Scale)]
        [FoldoutGroup("Scale Settings")]
        [SerializeField]
        private float initialScale = .85f;

        [ShowIf("animationType", AnimationType.Scale)]
        [FoldoutGroup("Scale Settings")]
        [SerializeField]
        private float targetScale = 1;

        private CanvasGroup _canvasGroup;
        private bool _isShown;
        private Vector3 _originalPosition;
        private RectTransform _rectTransform;

        #region Built-In
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalPosition = _rectTransform.anchoredPosition;
            Hide();
        }
        #endregion

        #region Animation Controls
        /// <summary>
        /// Shows the UI element with the configured animation type.
        /// </summary>
        /// <param name="onComplete">Optional callback to execute when the animation completes</param>
        public void Show(TweenCallback onComplete = null)
        {
            if (_isShown) return;

            switch (animationType)
            {
                case AnimationType.Scale:
                    _rectTransform.localScale = Vector3.one * initialScale;
                    _rectTransform.DOScale(targetScale, AnimationDuration).SetDelay(showDelay).SetEase(Ease.OutBack)
                        .OnComplete(onComplete);
                    break;

                case AnimationType.Fade:
                    if (_canvasGroup == null)
                        _canvasGroup = gameObject.GetComponent<CanvasGroup>();
                    _canvasGroup.alpha = 0;
                    _canvasGroup.DOFade(1, AnimationDuration).SetDelay(showDelay).OnComplete(onComplete);
                    break;

                case AnimationType.MoveFromLeft:
                case AnimationType.MoveFromRight:
                case AnimationType.MoveFromTop:
                case AnimationType.MoveFromBottom:
                    _rectTransform.DOAnchorPos(_originalPosition, AnimationDuration).SetDelay(showDelay).SetEase(Ease.OutCubic)
                        .OnComplete(onComplete);
                    break;
            }

            _isShown = true;
        }

        /// <summary>
        /// Hides the UI element with the configured animation type.
        /// </summary>
        public void Hide()
        {
            switch (animationType)
            {
                case AnimationType.Scale:
                    _rectTransform.DOScale(initialScale, AnimationDuration).SetDelay(HideDelay).SetEase(Ease.InBack);
                    break;

                case AnimationType.Fade:
                    if (_canvasGroup == null)
                        _canvasGroup = gameObject.GetComponent<CanvasGroup>();
                    _canvasGroup.DOFade(0, AnimationDuration).SetDelay(HideDelay);
                    break;

                case AnimationType.MoveFromLeft:
                    _rectTransform.DOAnchorPos(new Vector2(-Screen.width, _originalPosition.y), AnimationDuration)
                        .SetDelay(HideDelay).SetEase(Ease.InCubic);
                    break;

                case AnimationType.MoveFromRight:
                    _rectTransform.DOAnchorPos(new Vector2(Screen.width, _originalPosition.y), AnimationDuration)
                        .SetDelay(HideDelay).SetEase(Ease.InCubic);
                    break;

                case AnimationType.MoveFromTop:
                    _rectTransform.DOAnchorPos(new Vector2(_originalPosition.x, Screen.height), AnimationDuration)
                        .SetDelay(HideDelay).SetEase(Ease.InCubic);
                    break;

                case AnimationType.MoveFromBottom:
                    _rectTransform.DOAnchorPos(new Vector2(_originalPosition.x, -Screen.height), AnimationDuration)
                        .SetDelay(HideDelay).SetEase(Ease.InCubic);
                    break;
            }

            _isShown = false;
        }
        #endregion
    }
}