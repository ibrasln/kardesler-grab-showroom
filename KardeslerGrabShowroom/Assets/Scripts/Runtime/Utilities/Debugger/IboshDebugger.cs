using UnityEngine;

namespace IboshEngine.Runtime.Utilities.Debugger
{
    /// <summary>
    ///     Utility class providing methods for enhanced debugging in Unity.
    /// </summary>
    /// <remarks>
    ///     The class includes methods for logging messages, errors, and warnings with customizable colors. It also supports
    ///     conditional logging based on the debug build setting.
    /// </remarks>
    public static class IboshDebugger
    {
        /// <summary>
        ///     Enum representing various debug color options for message formatting.
        /// </summary>
        public enum DebugColor
        {
            Black,
            Blue,
            Cyan,
            Gray,
            Green,
            Magenta,
            Red,
            White,
            Yellow
        }

        /// <summary>
        ///     Logs a regular message to the Unity console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void LogMessage(string message)
        {
            if (!Debug.isDebugBuild) return;

            Debug.Log(message);
        }

        /// <summary>
        ///     Logs a formatted message to the Unity console with the specified color.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="messageColor">The color of the message text.</param>
        public static void LogMessage(string message, DebugColor messageColor)
        {
            if (!Debug.isDebugBuild) return;

            Debug.Log($"<color={GetDebugColor(messageColor)}> {message}</color>");
        }

        /// <summary>
        ///     Logs a formatted message with a header to the Unity console, each with specified colors.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="header">The header for the log message.</param>
        /// <param name="messageColor">The color of the message text.</param>
        /// <param name="headerColor">The color of the header text.</param>
        public static void LogMessage(string message, string header, DebugColor messageColor, DebugColor headerColor)
        {
            if (!Debug.isDebugBuild) return;

            Debug.Log(
                $"<color={GetDebugColor(headerColor)}> [{header}] </color> <color={GetDebugColor(messageColor)}> {message}</color>");
        }

        /// <summary>
        ///     Logs a formatted array of messages with a header to the Unity console, each with specified colors.
        /// </summary>
        /// <param name="messages">The array of messages to log.</param>
        /// <param name="header">The header for the log messages.</param>
        /// <param name="messageColor">The color of the message text.</param>
        /// <param name="headerColor">The color of the header text.</param>
        public static void LogMessage(string[] messages, string header, DebugColor messageColor, DebugColor headerColor)
        {
            if (!Debug.isDebugBuild) return;

            Debug.Log(
                $"<color={GetDebugColor(headerColor)}> [{header}] </color> <color={GetDebugColor(messageColor)}> {string.Join(",", messages)}</color>");
        }

        /// <summary>
        ///     Logs an error message to the Unity console.
        /// </summary>
        /// <param name="error">The error message to log.</param>
        public static void LogError(string error)
        {
            Debug.LogError(error);
        }

        /// <summary>
        ///     Logs an error message to the Unity console with the specified color.
        /// </summary>
        /// <param name="error">The error message to log.</param>
        /// <param name="errorColor">The color of the error message text.</param>
        public static void LogError(string error, DebugColor errorColor)
        {
            Debug.LogError($"<color={GetDebugColor(errorColor)}> {error}</color>");
        }

        /// <summary>
        ///     Logs an error message with a header to the Unity console, each with specified colors.
        /// </summary>
        /// <param name="error">The error message to log.</param>
        /// <param name="header">The header for the log error.</param>
        /// <param name="messageColor">The color of the error message text.</param>
        /// <param name="headerColor">The color of the header text.</param>
        public static void LogError(string error, string header, DebugColor messageColor, DebugColor headerColor)
        {
            Debug.LogError(
                $"<color={GetDebugColor(headerColor)}> [{header}] </color> <color={GetDebugColor(messageColor)}> {error}</color>");
        }

        /// <summary>
        ///     Logs a warning message to the Unity console.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        public static void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        /// <summary>
        ///     Logs a warning message to the Unity console with the specified color.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        /// <param name="messageColor">The color of the warning message text.</param>
        public static void LogWarning(string message, DebugColor messageColor)
        {
            Debug.LogWarning($"<color={GetDebugColor(messageColor)}> {message}</color>");
        }

        /// <summary>
        ///     Logs a warning message with a header to the Unity console, each with specified colors.
        /// </summary>
        /// <param name="warning">The warning message to log.</param>
        /// <param name="header">The header for the log warning.</param>
        /// <param name="messageColor">The color of the warning message text.</param>
        /// <param name="headerColor">The color of the header text.</param>
        public static void LogWarning(string warning, string header, DebugColor messageColor, DebugColor headerColor)
        {
            Debug.LogWarning(
                $"<color={GetDebugColor(headerColor)}> [{header}] </color> <color={GetDebugColor(messageColor)}> {warning}</color>");
        }

        /// <summary>
        ///     Converts the DebugColor enum value to its corresponding string representation.
        /// </summary>
        /// <param name="colorEnum">The DebugColor enum value.</param>
        /// <returns>The string representation of the specified color.</returns>
        private static string GetDebugColor(DebugColor colorEnum)
        {
            return colorEnum switch
            {
                DebugColor.Black => "black",
                DebugColor.Blue => "blue",
                DebugColor.Cyan => "cyan",
                DebugColor.Gray => "gray",
                DebugColor.Green => "green",
                DebugColor.Magenta => "magenta",
                DebugColor.Red => "red",
                DebugColor.White => "white",
                DebugColor.Yellow => "yellow",
                _ => default
            };
        }
    }
}

