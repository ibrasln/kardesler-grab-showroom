using UnityEngine;

namespace IboshEngine.Runtime.Utilities.Extensions
{
	/// <summary>
	/// Extension methods for Transform.
	/// </summary>
	public static class TransformExtensions
	{
		/// <summary>
		/// Resets the position of the transform.
		/// </summary>
		public static void ResetPosition(this Transform transform)
		{
			transform.position = Vector3.zero;
		}

		/// <summary>
		/// Resets the local position of the transform.
		/// </summary>
		public static void ResetLocalPosition(this Transform transform)
		{
			transform.localPosition = Vector3.zero;
		}

		/// <summary>
		/// Sets the X component of the world position of the transform.
		/// </summary>
		/// <param name="x">The value to set the X component of the world position to.</param>
		public static void SetPositionX(this Transform transform, float x)
		{
			transform.position = new Vector3(x, transform.position.y, transform.position.z);
		}

		/// <summary>
		/// Sets the X component of the local position of the transform.
		/// </summary>
		/// <param name="x">The value to set the X component of the local position to.</param>
		public static void SetLocalPositionX(this Transform transform, float x)
		{
			transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
		}

		/// <summary>
		/// Sets the X component of the world rotation of the transform.
		/// </summary>
		/// <param name="x">The value to set the X component of the world rotation to.</param>
		public static void SetRotationX(this Transform transform, float x)
		{
			transform.rotation = Quaternion.Euler(x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
		}

		/// <summary>
		/// Sets the X component of the local rotation of the transform.
		/// </summary>
		/// <param name="x">The value to set the X component of the local rotation to.</param>
		public static void SetLocalRotationX(this Transform transform, float x)
		{
			transform.localRotation = Quaternion.Euler(x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
		}

		/// <summary>
		/// Sets the X component of the local scale of the transform.
		/// </summary>
		/// <param name="x">The value to set the X component of the local scale to.</param>
		public static void SetLocalScaleX(this Transform transform, float x)
		{
			transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
		}

		/// <summary>
		/// Resets the local position X component of the transform.
		/// </summary>
		public static void ResetLocalPositionX(this Transform transform)
		{
			transform.SetLocalPositionX(0);
		}

		/// <summary>
		/// Sets the Y component of the world position of the transform.
		/// </summary>
		/// <param name="y">The value to set the Y component of the world position to.</param>
		public static void SetPositionY(this Transform transform, float y)
		{
			transform.position = new Vector3(transform.position.x, y, transform.position.z);
		}

		/// <summary>
		/// Sets the Y component of the local position of the transform.
		/// </summary>
		/// <param name="y">The value to set the Y component of the local position to.</param>
		public static void SetLocalPositionY(this Transform transform, float y)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
		}

		/// <summary>
		/// Sets the Y component of the world rotation of the transform.
		/// </summary>
		/// <param name="y">The value to set the Y component of the world rotation to.</param>
		public static void SetRotationY(this Transform transform, float y)
		{
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, y, transform.rotation.eulerAngles.z);
		}

		/// <summary>
		/// Sets the Y component of the local rotation of the transform.
		/// </summary>
		/// <param name="y">The value to set the Y component of the local rotation to.</param>
		public static void SetLocalRotationY(this Transform transform, float y)
		{
			transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, y, transform.localRotation.eulerAngles.z);
		}

		/// <summary>
		/// Sets the Y component of the local scale of the transform.
		/// </summary>
		/// <param name="y">The value to set the Y component of the local scale to.</param>
		public static void SetLocalScaleY(this Transform transform, float y)
		{
			transform.localScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
		}

		/// <summary>
		/// Resets the local position Y component of the transform.
		/// </summary>
		public static void ResetLocalPositionY(this Transform transform)
		{
			transform.SetLocalPositionY(0);
		}

		/// <summary>
		/// Sets the Z component of the world position of the transform.
		/// </summary>
		/// <param name="z">The value to set the Z component of the world position to.</param>
		public static void SetPositionZ(this Transform transform, float z)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, z);
		}

		/// <summary>
		/// Sets the Z component of the local position of the transform.
		/// </summary>
		/// <param name="z">The value to set the Z component of the local position to.</param>
		public static void SetLocalPositionZ(this Transform transform, float z)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
		}

		/// <summary>
		/// Sets the Z component of the world rotation of the transform.
		/// </summary>
		/// <param name="z">The value to set the Z component of the world rotation to.</param>
		public static void SetRotationZ(this Transform transform, float z)
		{
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, z);
		}

		/// <summary>
		/// Sets the Z component of the local rotation of the transform.
		/// </summary>
		/// <param name="z">The value to set the Z component of the local rotation to.</param>
		public static void SetLocalRotationZ(this Transform transform, float z)
		{
			transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, z);
		}

		/// <summary>
		/// Sets the Z component of the local scale of the transform.
		/// </summary>
		/// <param name="z">The value to set the Z component of the local scale to.</param>
		public static void SetLocalScaleZ(this Transform transform, float z)
		{
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
		}

		/// <summary>
		/// Resets the local position Z component of the transform.
		/// </summary>
		public static void ResetLocalPositionZ(this Transform transform)
		{
			transform.SetLocalPositionZ(0);
		}
	}
}