#region Using Statements
using Sirenix.OdinInspector;
using IboshEngine.Runtime.Utilities.Extensions;
using IboshEngine.Runtime.Systems.UISystem.Animation;
using UnityEngine;
using UnityEngine.Events;
#endregion

namespace IboshEngine.Runtime.Systems.UISystem.Panels
{
    /// <summary>
    /// Base class for UI panels in the game, handling visibility, animations, and event management.
    /// Requires a CanvasGroup component for functionality.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class IboshPanel : MonoBehaviour
    {
        public bool IsActive => CanvasGroup.alpha > 0;
        [FoldoutGroup("Events")] public UnityEvent OnPanelShown;
        [FoldoutGroup("Events")] public UnityEvent OnPanelHide;

        protected AnimationController[] animationControllers;
        private CanvasGroup _blockingOverlay;
        private CanvasGroup _canvasGroup;

        protected CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }

        #region Built-In

        protected virtual void Awake()
        {
            animationControllers = GetComponentsInChildren<AnimationController>();
            _blockingOverlay = transform.Find("Image_BlockingOverlay").GetComponent<CanvasGroup>();
            _blockingOverlay.transform.SetAsLastSibling();
        }

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        #endregion

        #region Event Subscription

        protected virtual void SubscribeToEvents()
        {
        }

        protected virtual void UnsubscribeFromEvents()
        {
        }

        #endregion

        #region Panel Visibility Management

        /// <summary>
        /// Shows the panel and plays show animations if in play mode.
        /// </summary>
        [ButtonGroup("PanelVisibility")]
        public virtual void Show()
        {
            SetPanelVisibility(true);
            if (Application.isPlaying) PlayShowAnimations();
        }

        /// <summary>
        /// Hides the panel and plays hide animations if in play mode.
        /// </summary>
        [ButtonGroup("PanelVisibility")]
        public virtual void Hide()
        {
            ShowBlockingOverlay();

            if (Application.isPlaying)
                PlayHideAnimations();
            else
                SetPanelVisibility(false);
        }

        /// <summary>
        /// Shows the blocking overlay to prevent input during transitions.
        /// </summary>
        protected void ShowBlockingOverlay()
        {
            if (!Application.isPlaying) return;
            _blockingOverlay.interactable = false;
            _blockingOverlay.blocksRaycasts = true;
        }

        /// <summary>
        /// Hides the blocking overlay after transitions complete.
        /// </summary>
        protected void HideBlockingOverlay()
        {
            if (!Application.isPlaying) return;
            _blockingOverlay.blocksRaycasts = false;
        }

        /// <summary>
        /// Plays show animations for all animation controllers.
        /// </summary>
        private void PlayShowAnimations()
        {
            int completedAnimations = 0;
            if (animationControllers is null) return;

            int totalAnimations = animationControllers.Length;

            foreach (AnimationController animController in animationControllers)
                animController.Show(() =>
                {
                    completedAnimations++;
                    if (completedAnimations >= totalAnimations) HideBlockingOverlay();
                });

            if (totalAnimations <= 0) HideBlockingOverlay();
        }

        /// <summary>
        /// Plays hide animations for all animation controllers.
        /// </summary>
        private void PlayHideAnimations()
        {
            float longestDuration = 0f;

            foreach (AnimationController animController in animationControllers)
            {
                float hideDuration =
                    animController.AnimationDuration + animController.HideDelay;
                if (hideDuration > longestDuration) longestDuration = hideDuration;

                animController.Hide();
            }

            Invoke(nameof(FinishHidePanel), longestDuration);
        }

        /// <summary>
        /// Completes the hide panel operation after animations finish.
        /// </summary>
        private void FinishHidePanel()
        {
            SetPanelVisibility(false);
        }

        /// <summary>
        /// Sets the panel's visibility state and triggers appropriate events.
        /// </summary>
        /// <param name="isVisible">Whether the panel should be visible</param>
        private void SetPanelVisibility(bool isVisible)
        {
            if (this == null || CanvasGroup == null) return;

            if (isVisible)
            {
                CanvasGroup.Show();
                OnPanelShown?.Invoke();
            }
            else
            {
                CanvasGroup.Hide();
                OnPanelHide?.Invoke();
            }
        }

        /// <summary>
        /// Toggles the panel's visibility state.
        /// </summary>
        [ButtonGroup("PanelVisibility")]
        public void TogglePanel()
        {
            if (CanvasGroup.alpha == 0)
                Show();
            else Hide();
        }

        #endregion
    }
}