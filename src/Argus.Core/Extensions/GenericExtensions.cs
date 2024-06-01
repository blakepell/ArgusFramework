/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2014-01-02
 * @last updated      : 2023-06-27
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT
 */

namespace Argus.Extensions
{
    /// <summary>
    /// Generic extension methods.
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// Safely determines whether the value is null or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public static bool IsNull<T>(this T obj) where T : class
        {
            return obj == null;
        }

        /// <summary>
        /// Safely determines whether the value is null or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public static bool IsNull<T>(this T? obj) where T : struct
        {
            return !obj.HasValue;
        }


        /// <summary>
        /// Sets a property's value via reflection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <remarks>Not efficient but very handy.</remarks>
        public static void SetValue<T>(this T @this, string propertyName, object value)
        {
            var prop = @this.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            prop?.SetValue(@this, value, null);
        }

        /// <summary>
        /// Get's a property value via reflection as an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="propertyName"></param>
        /// <remarks>Not efficient but very handy.</remarks>
        public static object GetValue<T>(this T @this, string propertyName)
        {
            var prop = @this.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            return prop?.GetValue(@this, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public static bool ReferenceEquals<T>(this T first, T second) where T : class => (object)first == (object)second;

#if NETSTANDARD2_0
        /// <summary>
        /// Returns a set of items off of the end of the IEnumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="itemCount"></param>
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int itemCount)
        {
            return source.Skip(System.Math.Max(0, source.Count() - itemCount));
        }
#endif

        /// <summary>
        /// Executes an action for each item in the IEnumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (var item in enumeration)
            {
                action(item);
            }
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
            foreach (var sourceProperty in source.GetType().GetRuntimeProperties())
            {
                var selfProperty = self.GetType().GetRuntimeProperty(sourceProperty.Name);

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
        public static bool In<T>(this T source, params T[] list)
        {
            return list.Contains(source);
        }

        /// <summary>
        /// Remove element from array at given index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            var destination = new T[source.Length - 1];

            if (index > 0)
            {
                Array.Copy(source, 0, destination, 0, index);
            }

            if (index < source.Length - 1)
            {
                Array.Copy(source, index + 1, destination, index, source.Length - index - 1);
            }

            return destination;
        }

        /// <summary>
        /// Turns an IEnumerable into a Markdown table.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        public static string ToMarkdownTable<T>(this IEnumerable<T> source)
        {
            var properties = typeof(T).GetRuntimeProperties();

            var fields = typeof(T)
                         .GetRuntimeFields()
                         .Where(f => f.IsPublic);

            var gettables = properties.Select(p => new { p.Name, GetValue = (Func<object, object>)p.GetValue, Type = p.PropertyType }).Union(fields.Select(p => new { p.Name, GetValue = (Func<object, object>)p.GetValue, Type = p.FieldType }));

            var maxColumnValues = source
                                  .Select(x => gettables.Select(p => p.GetValue(x)?.ToString()?.Length ?? 0))
                                  .Union(new[] { gettables.Select(p => p.Name.Length) }) // Include header in column sizes
                                  .Aggregate(
                                      new int[gettables.Count()].AsEnumerable(),
                                      (accumulate, x) => accumulate.Zip(x, System.Math.Max))
                                  .ToArray();

            var columnNames = gettables.Select(p => p.Name);

            string headerLine = "| " + string.Join(" | ", columnNames.Select((n, i) => n.PadRight(maxColumnValues[i]))) + " |";

            var isNumeric = new Func<Type, bool>(type =>
                                                     type == typeof(byte) ||
                                                     type == typeof(sbyte) ||
                                                     type == typeof(ushort) ||
                                                     type == typeof(uint) ||
                                                     type == typeof(ulong) ||
                                                     type == typeof(short) ||
                                                     type == typeof(int) ||
                                                     type == typeof(long) ||
                                                     type == typeof(decimal) ||
                                                     type == typeof(double) ||
                                                     type == typeof(float));

            var rightAlign = new Func<Type, char>(type => isNumeric(type) ? ':' : ' ');

            string headerDataDividerLine = "| " + string.Join("| ", gettables.Select((g, i) => new string('-', maxColumnValues[i]) + rightAlign(g.Type))) + "|";

            var lines = new[]
            {
                headerLine,
                headerDataDividerLine
            }.Union(source.Select(s => "| " + string.Join(" | ", gettables.Select((n, i) => (n.GetValue(s)?.ToString() ?? "").PadRight(maxColumnValues[i]))) + " |"));

            return lines
                .Aggregate((p, c) => p + Environment.NewLine + c);
        }

        /// <summary>
        /// Get's an attribute from a generic object.
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public static TAttribute GetAttribute<T, TAttribute>(this T value) where TAttribute : Attribute
        {
            return value.GetType()
                        .GetCustomAttributes(false)
                        .OfType<TAttribute>()
                        .SingleOrDefault();
        }
    }
}