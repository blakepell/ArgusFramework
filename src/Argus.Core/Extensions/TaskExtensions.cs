/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2022-04-01
 * @last updated      : 2022-08-25
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Argus.Extensions
{
    /// <summary>
    /// Extensions for the <see cref="Task"/> class.
    /// </summary>
    /// <remarks>Portions of this from internal implementations from dotnet/aspnetcore licensed under the MIT license.</remarks>
    public static class TaskExtensions
    {
        /// <summary>
        /// Times a <see cref="Task"/> out after the specified <paramref name="timeout"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="timeout"></param>
        /// <param name="filePath"></param>
        /// <param name="lineNumber"></param>
        /// <exception cref="TimeoutException"></exception>
        public static async Task<T> TimeoutAfter<T>(this Task<T> task, TimeSpan timeout, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = default)
        {
            // Don't create a timer if the task is already completed or the debugger is attached.
            if (task.IsCompleted || Debugger.IsAttached)
            {
                return await task;
            }

            var cts = new CancellationTokenSource();
            
            if (task == await Task.WhenAny(task, Task.Delay(timeout, cts.Token)))
            {
                cts.Cancel();
                return await task;
            }
            else
            {
                throw new TimeoutException(CreateMessage(timeout, filePath, lineNumber));
            }
        }

        /// <summary>
        /// Times a <see cref="Task"/> out after the specified <paramref name="timeout"/>.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="timeout"></param>
        /// <param name="filePath"></param>
        /// <param name="lineNumber"></param>
        /// <exception cref="TimeoutException"></exception>
        public static async Task TimeoutAfter(this Task task, TimeSpan timeout, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = default)
        {
            // Don't create a timer if the task is already completed or the debugger is attached.
            if (task.IsCompleted || Debugger.IsAttached)
            {
                await task;
                return;
            }

            var cts = new CancellationTokenSource();
            
            if (task == await Task.WhenAny(task, Task.Delay(timeout, cts.Token)))
            {
                cts.Cancel();
                await task;
            }
            else
            {
                throw new TimeoutException(CreateMessage(timeout, filePath, lineNumber));
            }
        }

        /// <summary>
        /// Creates the message returned if a timeout occurs including information on the caller.
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="filePath"></param>
        /// <param name="lineNumber"></param>
        private static string CreateMessage(TimeSpan timeout, string filePath, int lineNumber)
            => string.IsNullOrEmpty(filePath)
            ? $"The operation timed out after reaching the limit of {timeout.TotalMilliseconds}ms."
            : $"The operation at {filePath}:{lineNumber} timed out after reaching the limit of {timeout.TotalMilliseconds}ms.";

        /// <summary>
        /// Fire's and forgets a <see cref="Task"/>.
        /// </summary>
        /// <param name="task"></param>
        /// <remarks>
        /// This is mainly for use with async void events in UI applications and not for ASP.NET Core web sites.
        /// </remarks>
        public static async void FireAndForget(this Task task)
        {
            await task;
        }

        /// <summary>
        /// Fire's and forgets a <see cref="Task"/> that ignores suppresses any exceptions that are thrown.
        /// </summary>
        /// <param name="task"></param>
        /// <remarks>
        /// This is mainly for use with async void events in UI applications and not for ASP.NET Core web sites.
        /// </remarks>
        public static async void SafeFireAndForget(this Task task)
        {
            try
            {
                await task;
            }
            catch
            {
                // Eat exception
            }
        }
    }
    
}
