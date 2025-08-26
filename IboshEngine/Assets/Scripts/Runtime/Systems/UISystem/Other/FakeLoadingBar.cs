using DG.Tweening;
using IboshEngine.Runtime.Core.EventManagement;
using UnityEngine;
using UnityEngine.UI;

namespace IboshEngine.Runtime.Systems.UISystem
{
    /// <summary>
    /// Manages a fake loading bar animation for demonstration purposes.
    /// </summary>
    public class FakeLoadingBar : MonoBehaviour
    {
        [SerializeField] private Slider loadingSlider;
        [SerializeField] private float loadTime = 2f;

        private void Start()
        {
            loadTime = Random.Range(loadTime - 1, loadTime + 2);
            loadingSlider.value = 0f;
            AnimateLoadingBar();
        }

        private void AnimateLoadingBar()
        {
            loadingSlider.DOValue(1f, loadTime).OnComplete(OnLoadingCompleted);
        }

        private void OnLoadingCompleted()
        {
            //EventManagerProvider.UI.Broadcast(UIEvent.OnLoadingCompleted);
        }
    }
}