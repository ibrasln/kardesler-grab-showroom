using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using KardeslerGrab.Showroom.Utilities;
using IboshEngine.Runtime.Core.EventManagement;

namespace KardeslerGrabShowroom.Gameplay.Showroom
{
    public class Showroom : MonoBehaviour
    {
        private Grab.Grab[] _grabs;
        private int _currentGrabIndex = 0;
        private Grab.Grab _currentGrab;

        #region Built-In

        private void Start()
        {
            _grabs = GetComponentsInChildren<Grab.Grab>(true);
            SetCurrentGrab(0);
        }

        #endregion

        #region Movement

        private async UniTask MoveToPreviousGrabAsync()
        {
            await transform.DOMoveX(transform.position.x + Settings.ShowroomMovementDistance, Settings.ShowroomMovementDuration).SetEase(Ease.InOutSine).ToUniTask();
        }

        private async UniTask MoveToNextGrabAsync()
        {
            await transform.DOMoveX(transform.position.x - Settings.ShowroomMovementDistance, Settings.ShowroomMovementDuration).SetEase(Ease.InOutSine).ToUniTask();
        }

        #endregion

        #region Grab Management

        public async void GetPreviousGrab()
        {
            if (_currentGrabIndex <= 0)
            {
                return;
            }

            EventManagerProvider.Showroom.Broadcast(ShowroomEvent.OnShowroomMovementStarted);

            await MoveToPreviousGrabAsync();

            _currentGrab?.Dispose();

            SetCurrentGrab(_currentGrabIndex - 1);

            EventManagerProvider.Showroom.Broadcast(ShowroomEvent.OnShowroomMovementCompleted);
        }

        public async void GetNextGrab()
        {
            if (_currentGrabIndex >= _grabs.Length - 1)
            {
                return;
            }

            EventManagerProvider.Showroom.Broadcast(ShowroomEvent.OnShowroomMovementStarted);

            await MoveToNextGrabAsync();

            _currentGrab?.Dispose();

            SetCurrentGrab(_currentGrabIndex + 1);

            EventManagerProvider.Showroom.Broadcast(ShowroomEvent.OnShowroomMovementCompleted);
        }

        public void SetCurrentGrab(int index)
        {
            _currentGrabIndex = index;
            _currentGrab = _grabs[_currentGrabIndex];
            _currentGrab.Initialize();
        }

        #endregion
    }
}