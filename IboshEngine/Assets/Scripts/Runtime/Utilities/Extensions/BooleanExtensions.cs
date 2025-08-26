namespace IboshEngine.Runtime.Utilities.Extensions
{
    /// <summary>
    /// Extension methods for boolean values.
    /// </summary>
    public static class BooleanExtensions
    {
        /// <summary>
        /// Converts a boolean value to an integer.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <returns>1 if the value is true, 0 if the value is false.</returns>
        public static int ToInt(this bool value)
        {
            return value ? 1 : 0;
        }
    }
}