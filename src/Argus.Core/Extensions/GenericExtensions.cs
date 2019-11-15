﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Argus.Extensions
{
    /// <summary>
    /// Generic extension methods.
    /// </summary>
    public static class GenericExtensions
    {
        //*********************************************************************************************************************
        //
        //            Module:  GenericExtensions
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  01/02/2014
        //      Last Updated:  03/27/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// Safely determines whether the value is null or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this T obj) where T : class
        {
            return obj == null;
        }

        /// <summary>
        /// Safely determines whether the value is null or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull<T>(this T? obj) where T : struct
        {
            return !obj.HasValue;
        }

        /// <summary>
        /// Returns a set of items off of the end of the IEnumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int itemCount)
        {
            return source.Skip(System.Math.Max(0, source.Count() - itemCount));
        }

        /// <summary>
        /// Attempt to copy properties from another class where the names match up (and only those
        /// properties).
        /// </summary>
        /// <typeparam name="TSelf"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="self"></param>
        /// <param name="source"></param>
        public static void CopyFrom<TSelf, TSource>(this TSelf self, TSource source)
        {
            foreach (PropertyInfo sourceProperty in source.GetType().GetRuntimeProperties())
            {
                PropertyInfo selfProperty = self.GetType().GetRuntimeProperty(sourceProperty.Name);

                if (selfProperty != null)
                {
                    var sourceValue = sourceProperty.GetValue(source, null);
                    selfProperty.SetValue(self, sourceValue, null);
                }
            }
        }

        /// <summary>
        /// Adds an object to the list only if it does not already exist in the list.  Null are checked for and will will not be added.
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<T> AddIfDoesntExist<T>(this List<T> ls, T value)
        {
            if (value == null)
            {
                return ls;
            }

            if (ls.Contains(value) == false)
            {
                ls.Add(value);
            }

            return ls;
        }

        /// <summary>
        /// Adds the provided item if the condition is true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ls"></param>
        /// <param name="value"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static List<T> AddIf<T>(this List<T> ls, T value, bool condition)
        {
            if (value == null)
            {
                return ls;
            }

            if (condition)
            {
                ls.Add(value);
            }

            return ls;
        }

        /// <summary>
        /// Checks to see if the current object is contained within the list provided.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="list"></param>
        public static bool In<T>(this T source, IEnumerable<T> list)
        {
            return list.ToList().Contains(source);
        }

        /// <summary>
        /// Checks to see if the current object is contained within the list provided.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool In<T>(this T source, params T[] list)
        {
            return list.Contains(source);
        }

        /// <summary>
        /// Turns an IEnumerable into a Markdown table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToMarkdownTable<T>(this IEnumerable<T> source)
        {
            var properties = typeof(T).GetRuntimeProperties();
            var fields = typeof(T)
                .GetRuntimeFields()
                .Where(f => f.IsPublic);

            var gettables = Enumerable.Union(
                properties.Select(p => new { p.Name, GetValue = (Func<object, object>)p.GetValue, Type = p.PropertyType }),
                fields.Select(p => new { p.Name, GetValue = (Func<object, object>)p.GetValue, Type = p.FieldType }));

            var maxColumnValues = source
                .Select(x => gettables.Select(p => p.GetValue(x)?.ToString()?.Length ?? 0))
                .Union(new[] { gettables.Select(p => p.Name.Length) }) // Include header in column sizes
                .Aggregate(
                    new int[gettables.Count()].AsEnumerable(),
                    (accumulate, x) => accumulate.Zip(x, System.Math.Max))
                .ToArray();

            var columnNames = gettables.Select(p => p.Name);

            var headerLine = "| " + string.Join(" | ", columnNames.Select((n, i) => n.PadRight(maxColumnValues[i]))) + " |";

            var isNumeric = new Func<Type, bool>(type =>
                type == typeof(Byte) ||
                type == typeof(SByte) ||
                type == typeof(UInt16) ||
                type == typeof(UInt32) ||
                type == typeof(UInt64) ||
                type == typeof(Int16) ||
                type == typeof(Int32) ||
                type == typeof(Int64) ||
                type == typeof(Decimal) ||
                type == typeof(Double) ||
                type == typeof(Single));

            var rightAlign = new Func<Type, char>(type => isNumeric(type) ? ':' : ' ');

            var headerDataDividerLine = "| " + string.Join("| ", gettables.Select((g, i) => new string('-', maxColumnValues[i]) + rightAlign(g.Type))) + "|";

            var lines = new[]
            {
                headerLine,
                headerDataDividerLine,
            }.Union(source.Select(s => "| " + string.Join(" | ", gettables.Select((n, i) => (n.GetValue(s)?.ToString() ?? "").PadRight(maxColumnValues[i]))) + " |"));

            return lines
                .Aggregate((p, c) => p + Environment.NewLine + c);
        }

    }
}