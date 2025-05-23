namespace Oxigin.Attendance.Core.Extensions
{
    /// <summary>
    /// A collection of validation extensions.
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Validate an object and raise an exception if the object is invalid.
        /// </summary>
        /// <typeparam name="T">The type of the object instance.</typeparam>
        /// <param name="obj">The object instance.</param>
        /// <param name="argName">The name of the argument/property that the object instance represents.</param>
        /// <returns>The object instance passed for allowing chaining.</returns>
        /// <exception cref="ArgumentNullException">Raised when no valid object is passed.</exception>
        public static T ThrowIfNull<T>(this T obj, string argName)
        {
            if (obj == null) throw new ArgumentNullException($"A valid {argName} is required.");

            return obj;
        }
    }
}
