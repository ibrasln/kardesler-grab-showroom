using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IboshEngine.Runtime.Systems.UISystem.Animation
{
    /// <summary>
    /// Manages button animations including idle pulse and press animations.
    /// </summary>
    public class ButtonAnimationController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public PressAnimationType PressAnimationType = PressAnimationType.Scale;

        [BoxGroup("Idle Pulse Settings")]
        [SerializeField]
        private bool enableIdlePulse;

        [BoxGroup("Idle Pulse Settings")]
        [SerializeField]
        [EnableIf("enableIdlePulse")]
        private float pulseScale = 1.1f;

        [BoxGroup("Idle Pulse Settings")]
        [SerializeField]
        [EnableIf("enableIdlePulse")]
        private float pulseDuration = 2f;

        [BoxGroup("Press Settings")]
        [SerializeField]
        [EnableIf("PressAnimationType", PressAnimationType.Scale)]
        private float pressScale = 0.85f;

        [BoxGroup("Press Settings")]
        [SerializeField]
        [EnableIf("PressAnimationType", PressAnimationType.Scale)]
        private float pressDuration = 0.2f;

        private Button _button;
        private CanvasGroup _canvasGroup;
        private Tween _idleTween;
        private Vector3 _initialScale;
        private Tween _pressTween;

        #region Built-In

        private void Awake()
        {
            _initialScale = transform.localScale;

            _button = GetComponent<Button>();

            if (PressAnimationType != PressAnimationType.Fade) return;
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            if (enableIdlePulse) StartIdlePulse();
        }

        private void OnDisable()
        {
            StopIdlePulse();
            _pressTween?.Kill();
            transform.localScale = _initialScale;
        }

        #endregion

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_button.interactable) return;

            _idleTween?.Kill();
            _pressTween?.Kill();

            switch (PressAnimationType)
            {
                case PressAnimationType.None:
                    break;
                case PressAnimationType.Scale:
                    _pressTween = transform
                        .DOScale(_initialScale * pressScale, pressDuration)
                        .SetEase(Ease.OutQuad);
                    break;
                case PressAnimationType.Fade:
                    _pressTween = _canvasGroup
                        .DOFade(0, pressDuration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pressTween?.Kill();

            switch (PressAnimationType)
            {
                case PressAnimationType.None:
                    break;
                case PressAnimationType.Scale:
                    _pressTween = transform
                        .DOScale(_initialScale, pressDuration)
                        .SetEase(Ease.OutBack).OnComplete(StartIdlePulse);
                    break;
                case PressAnimationType.Fade:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Enables idle pulse animation.
        /// </summary>
        public void EnableIdlePulse()
        {
            enableIdlePulse = true;
            StartIdlePulse();
        }

        /// <summary>
        /// Disables idle pulse animation.
        /// </summary>
        public void DisableIdlePulse()
        {
            enableIdlePulse = false;
            StopIdlePulse();
        }

        /// <summary>
        /// Starts idle pulse animation.
        /// </summary>
        private void StartIdlePulse()
        {
            if (!enableIdlePulse || !_button.interactable) return;

            _idleTween?.Kill();
            _idleTween = transform
                .DOScale(_initialScale * pulseScale, pulseDuration / 2)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        /// <summary>
        /// Stops idle pulse animation.
        /// </summary>
        private void StopIdlePulse()
        {
            if (_idleTween == null) return;

            _idleTween.Kill();
            _idleTween = null;
            transform.localScale = _initialScale;
        }
    }
}

/// <summary>
/// Enum defining different types of press animations for buttons.
/// </summary>
public enum PressAnimationType
{
    Scale,
    Fade,
    None
}