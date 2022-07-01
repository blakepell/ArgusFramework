/*
 * @author            : Blake Pell
 * @initial date      : 2015-01-03
 * @last updated      : 2021-04-22
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using Newtonsoft.Json.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Argus.Data
{
    /// <summary>
    /// Provides helper utilities to deal with JSON data.  Note: This code will require the JSON.Net assembly to be provided
    /// along with this library.
    /// </summary>
    public static class JsonUtilities
    {
        /// <summary>
        /// Converts JSON that is not nested into a DataTable.  Typically this would be JSON that represents the contents of a table that
        /// is not nested.
        /// </summary>
        /// <param name="json">The JSON for the table structure</param>
        /// <param name="tableName">The name of the table as defined in the JSON.  For this implementation the table name must be defined.</param>
        /// <code>
        ///  {
        ///  "Table1": [
        ///     {
        ///       "id": 0,
        ///       "item": "item 0"
        ///     },
        ///     {
        ///       "id": 1,
        ///       "item": "item 1"
        ///     }
        ///   ]
        /// }
        ///  </code>
        public static DataTable JsonToDataTableNew(string json, string tableName)
        {
            var dt = new DataTable(tableName);
            var root = JObject.Parse(json);
            var items = root[tableName] as JArray;

            if (items == null || items.Count == 0)
            {
                return dt;
            }

            // We know we have at least the first item, we'll use that to create the
            // properties on the DataTable.
            JObject item = items[0] as JObject;
            JProperty jprop;

            // Create the columns
            foreach (var p in item.Properties())
            {
                dt.Columns.Add(new DataColumn(p.Name));
            }

            JToken jtoken;
            JObject obj;

            for (int i = 0; i <= items.Count - 1; i++)
            {
                // Create the new row, put the values into the columns then add the row to the DataTable
                var dr = dt.NewRow();

                // Add each of the columns into a new row then put that new row into the DataTable
                obj = items[i] as JObject;
                jtoken = obj.First;

                while (jtoken != null)
                {
                    jprop = jtoken as JProperty;
                    dr[jprop.Name] = jprop.Value.ToString();
                    jtoken = jtoken.Next;
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// Converts a JSON string into an escaped QueryString
        /// </summary>
        /// <param name="json"></param>
        public static string JsonToQueryString(string json)
        {
            var jObj = (JObject) JsonConvert.DeserializeObject(json);

            if (jObj == null)
            {
                throw new Exception("The deserialized JSON object was null.");
            }

            return string.Join("&",
                               jObj.Children().Cast<JProperty>()
                                   .Select(jp => jp.Name + "=" + HttpUtility.UrlEncode(jp.Value.ToString())));
        }

        /// <summary>
        /// Flattens an untyped JSON object and returns its values in a dictionary.
        ///
        /// Object In:
        /// 
        /// ```
        /// {
        ///     id: 123,
        ///     birth_date: '11/20/1975',
        ///     name: {
        ///         first: 'Joe',
        ///         last: 'Schmoe'
        ///     },
        ///     other_val: false,
        ///     foods: ['Tacos', 'Ice Cream']
        /// }
        /// ```
        ///
        /// Out:
        ///
        /// ```
        /// id: 123
        /// birth_date: '11/20/1975'
        /// name.first: 'Joe'
        /// name.last: 'Schmoe'
        /// other_val: false
        /// foods.0: 'Tacos'
        /// foods.1: 'Ice Cream'
        /// </summary>
        /// <param name="json">JSON string</param>
        public static Dictionary<string, object> DeserializeAndFlatten(string json)
        {
            var dict = new Dictionary<string, object>();
            var token = JToken.Parse(json);
            FillDictionaryFromJToken(dict, token, "");
            return dict;
        }

        private static void FillDictionaryFromJToken(Dictionary<string, object> dict, JToken token, string prefix)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    foreach (var prop in token.Children<JProperty>())
                    {
                        FillDictionaryFromJToken(dict, prop.Value, Join(prefix, prop.Name));
                    }

                    break;

                case JTokenType.Array:
                    int index = 0;
                    foreach (var value in token.Children())
                    {
                        FillDictionaryFromJToken(dict, value, Join(prefix, index.ToString()));
                        index++;
                    }

                    break;

                default:
                    dict.Add(prefix, ((JValue)token).Value);
                    break;
            }
        }

        private static string Join(string prefix, string name)
        {
            return string.IsNullOrEmpty(prefix) ? name : $"{prefix}.{name}";
        }

    }
}