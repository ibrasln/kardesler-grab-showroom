using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace IboshEngine.Runtime.Systems.UISystem
{
	/// <summary>
	/// Manages the movement of a notification UI element.
	/// </summary>
	public class NotifyController : MonoBehaviour
	{
		private float _moveDistance = 10.0f;
		private float _moveDuration = 1.0f;

		private RectTransform _rectTransform;

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
		}

		private void OnEnable()
		{
			StartMoving();
		}

		private void OnDisable()
		{
			DOTween.Kill(transform);
		}

		private void StartMoving()
		{
			Vector3 startPosition = _rectTransform.localPosition;
			Vector3 targetPosition = startPosition + Vector3.up * _moveDistance;

			Sequence moveSequence = DOTween.Sequence();
			moveSequence.Append(_rectTransform.DOLocalMove(targetPosition, _moveDuration).SetEase(Ease.InOutSine))
						.Append(_rectTransform.DOLocalMove(startPosition, _moveDuration).SetEase(Ease.InOutSine))
						.SetLoops(-1, LoopType.Yoyo);
		}
	}
}