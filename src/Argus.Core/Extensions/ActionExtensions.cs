/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2021-05-06
 * @last updated      : 2021-05-06
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Argus.Extensions
{
    /// <summary>
    /// Extensions for <see cref="Action"/>
    /// </summary>
    public static class ActionExtensions
    {
        /// <summary>
        /// Provides the ability to await an <see cref="Action"/>.
        /// </summary>
        /// <param name="action"></param>
        public static TaskAwaiter GetAwaiter(this Action action)
        {
            var task = new Task(action);
            task.Start();

            return task.GetAwaiter();
        }
    }
}
