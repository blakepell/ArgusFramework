/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2022-06-14
 * @last updated      : 2022-06-14
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.Runtime.InteropServices;

namespace Argus.Extensions
{
    /// <summary>
    /// Pointer/Handle extension methods.
    /// </summary>
    public static class IntPtrExtensions
    {
        /// <summary>
        /// Marshals data from an unmanaged block of memory to a managed object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ptr"></param>
        /// <remarks>https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.marshal.ptrtostructure</remarks>
        public static T MarshalAs<T>(this IntPtr ptr) => (T)Marshal.PtrToStructure(ptr, typeof(T));

        /// <summary>
        /// Marshals data from a managed object to an unmanaged block of memory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ptr"></param>
        /// <param name="obj"></param>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.marshal.structuretoptr
        /// </remarks>
        public static void MarshalAs<T>(this IntPtr ptr, T obj) => Marshal.StructureToPtr(obj, ptr, false);
    }
}