using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Argus.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ITempDataDictionary"/>.
    /// </summary>
    public static class TempDataExtensions
    {
        //*********************************************************************************************************************
        //
        //             Class:  TempDataExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  06/07/2017
        //      Last Updated:  11/17/2019
        //    Programmer(s):   Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Puts an object into the TempData by first serializing it to JSON.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tempData"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonSerializer.Serialize(value);
        }

        /// <summary>
        ///     Gets an object from the TempData by deserializing it from JSON.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tempData"></param>
        /// <param name="key"></param>
        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;
            tempData.TryGetValue(key, out o);

            return o == null ? null : JsonSerializer.Deserialize<T>((string) o);
        }
    }
}