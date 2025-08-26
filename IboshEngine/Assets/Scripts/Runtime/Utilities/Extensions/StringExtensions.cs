namespace IboshEngine.Runtime.Utilities.Extensions
{
    /// <summary>
    /// Extension methods for string.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Converts a string to an integer. If the conversion fails, it returns the specified default value.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <param name="defaultValue">The default value to return if the conversion fails.</param>
        /// <returns>The converted integer or the default value if the conversion fails.</returns>
        public static int ToInt(this string value, int defaultValue = 0)
        {
            return int.TryParse(value, out int result) ? result : defaultValue;
        }
    }
}