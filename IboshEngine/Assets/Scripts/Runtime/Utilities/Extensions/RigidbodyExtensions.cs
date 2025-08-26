using UnityEngine;

namespace IboshEngine.Runtime.Utilities.Extensions
{ 
    /// <summary>
    /// Extension methods for the Rigidbody and Rigidbody2D components in Unity, providing additional functionality for setting velocities along different axes.
    /// </summary>
    /// <remarks>
    /// The class includes extension methods for both Rigidbody and Rigidbody2D, offering convenient ways to set velocities in 2D and 3D space.
    /// Methods are provided for setting the overall velocity as well as individual components along the X, Y, and Z axes.
    /// </remarks>
    public static class RigidbodyExtensions
    {
        #region Rigidbody2D
        /// <summary>
        /// Sets the velocity of the Rigidbody2D to the specified 2D vector.
        /// </summary>
        /// <param name="rigidbody">The Rigidbody2D component.</param>
        /// <param name="velocity">The 2D velocity vector to set.</param>
        public static void SetVelocity(this Rigidbody2D rigidbody, Vector2 velocity)
        {
            rigidbody.linearVelocity = velocity;
        }

        /// <summary>
        /// Sets the X value of the velocity of the Rigidbody2D.
        /// </summary>
        /// <param name="rigidbody">The Rigidbody2D component.</param>
        /// <param name="xVelocity">The value to set as the X component of the velocity.</param>
        public static void SetVelocityX(this Rigidbody2D rigidbody, float xVelocity)
        {
            rigidbody.linearVelocity = new Vector2(xVelocity, rigidbody.linearVelocity.y);
        }

        /// <summary>
        /// Sets the Y value of the velocity of the Rigidbody2D.
        /// </summary>
        /// <param name="rigidbody">The Rigidbody2D component.</param>
        /// <param name="yVelocity">The value to set as the Y component of the velocity.</param>
        public static void SetVelocityY(this Rigidbody2D rigidbody, float yVelocity)
        {
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, yVelocity);
        }

        /// <summary>
        /// Sets the velocity of the Rigidbody2D to zero.
        /// </summary>
        /// <param name="rigidbody">The Rigidbody component.</param>
        public static void SetVelocityZero(this Rigidbody2D rigidbody)
        {
            rigidbody.linearVelocity = Vector2.zero;
        }
        #endregion

        #region Rigidbody
        /// <summary>
        /// Sets the velocity of the Rigidbody to the specified 3D vector.
        /// </summary>
        /// <param name="rigidbody">The Rigidbody component.</param>
        /// <param name="velocity">The 3D velocity vector to set.</param>
        public static void SetVelocity(this Rigidbody rigidbody, Vector3 velocity)
        {
            rigidbody.linearVelocity = velocity;
        }

        /// <summary>
        /// Sets the X value of the velocity of the Rigidbody.
        /// </summary>
        /// <param name="rigidbody">The Rigidbody component.</param>
        /// <param name="xVelocity">The value to set as the X component of the velocity.</param>
        public static void SetVelocityX(this Rigidbody rigidbody, float xVelocity)
        {
            var velocity = rigidbody.linearVelocity;
            velocity = new Vector3(xVelocity, velocity.y, velocity.z);
            rigidbody.linearVelocity = velocity;
        }

        /// <summary>
        /// Sets the Y value of the velocity of the Rigidbody.
        /// </summary>
        /// <param name="rigidbody">The Rigidbody component.</param>
        /// <param name="yVelocity">The value to set as the Y component of the velocity.</param>
        public static void SetVelocityY(this Rigidbody rigidbody, float yVelocity)
        {
            var velocity = rigidbody.linearVelocity;
            velocity = new Vector3(velocity.x, yVelocity, velocity.z);
            rigidbody.linearVelocity = velocity;
        }

        /// <summary>
        /// Sets the Z value of the velocity of the Rigidbody.
        /// </summary>
        /// <param name="rigidbody">The Rigidbody component.</param>
        /// <param name="zVelocity">The value to set as the Z component of the velocity.</param>
        public static void SetVelocityZ(this Rigidbody rigidbody, float zVelocity)
        {
            var velocity = rigidbody.linearVelocity;
            velocity = new Vector3(velocity.x, velocity.y, zVelocity);
            rigidbody.linearVelocity = velocity;
        }
        
        /// <summary>
        /// Sets the velocity of the Rigidbody to zero.
        /// </summary>
        /// <param name="rigidbody">The Rigidbody component.</param>
        public static void SetVelocityZero(this Rigidbody rigidbody)
        {
            rigidbody.linearVelocity = Vector3.zero;
        }
        #endregion
    }
}