using System;

namespace Argus.Utilities
{
    /// <summary>
    /// Safe methods that can execute lambda expressions and supress any exceptions that might occur.
    /// </summary>
    /// <remarks>
    /// I am aware of the ramifications of this and also that in general nearly all exceptions should
    /// be handled when at all possible.  Credit to Rick Strahl on this code.
    /// </remarks>
    public static class SafeExecute
    {

        /// <summary>
        /// Runs an operation and ignores any Exceptions that occur.
        /// Returns true or falls depending on whether catch was
        /// triggered
        /// </summary>
        /// <param name="operation">lambda that performs an operation that might throw</param>
        /// <returns></returns>
        /// <remarks>Rick Strahl: https://weblog.west-wind.com/posts/2018/Jun/16/Explicitly-Ignoring-Exceptions-in-C</remarks>
        public static bool IgnoreErrors(Action operation)
        {
            if (operation == null)
            {
                return false;
            }

            try
            {
                operation.Invoke();
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Runs an function that returns a value and ignores any Exceptions that occur.
        /// Returns true or falls depending on whether catch was
        /// triggered
        /// </summary>
        /// <param name="operation">parameterless lamda that returns a value of T</param>
        /// <param name="defaultValue">Default value returned if operation fails</param>
        /// <remarks>Rick Strahl: https://weblog.west-wind.com/posts/2018/Jun/16/Explicitly-Ignoring-Exceptions-in-C</remarks>
        public static T IgnoreErrors<T>(Func<T> operation, T defaultValue = default(T))
        {
            if (operation == null)
            {
                return defaultValue;
            }

            T result;

            try
            {
                result = operation.Invoke();
            }
            catch
            {
                result = defaultValue;
            }

            return result;
        }

    }
}
