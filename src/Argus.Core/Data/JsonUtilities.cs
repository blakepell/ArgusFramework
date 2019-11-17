using System.Data;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Argus.Data
{
    /// <summary>
    ///     Provides helper utilities to deal with JSON data.  Note: This code will require the JSON.Net assembly to be provided
    ///     along with this library.
    /// </summary>
    public class JsonUtilities
    {
        //*********************************************************************************************************************
        //
        //             Class:  JsonUtilities
        //      Organization:  http://www.blakepell.com
        //      Initial Date:  01/03/2015
        //      Last Updated:  11/17/2019
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        ///     Converts JSON that is not nested into a DataTable.  Typically this would be JSON that represents the contents of a table that
        ///     is not nested.
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
        public static DataTable JsonToDataTable(string json, string tableName)
        {
            bool columnsCreated = false;
            var dt = new DataTable(tableName);

            var root = JObject.Parse(json);
            var items = (JArray) root[tableName];

            for (int i = 0; i <= items.Count - 1; i++)
            {
                // Create the columns once
                var item = default(JObject);
                var jtoken = default(JToken);

                if (columnsCreated == false)
                {
                    item = (JObject) items[i];
                    jtoken = item.First;

                    while (jtoken != null)
                    {
                        dt.Columns.Add(new DataColumn(((JProperty) jtoken).Name));
                        jtoken = jtoken.Next;
                    }

                    columnsCreated = true;
                }

                // Add each of the columns into a new row then put that new row into the DataTable
                item = (JObject) items[i];
                jtoken = item.First;

                // Create the new row, put the values into the columns then add the row to the DataTable
                var dr = dt.NewRow();

                while (jtoken != null)
                {
                    dr[((JProperty) jtoken).Name] = ((JProperty) jtoken).Value.ToString();
                    jtoken = jtoken.Next;
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        ///     Converts a JSON string into an escaped QueryString
        /// </summary>
        /// <param name="json"></param>
        public static string JsonToQueryString(string json)
        {
            var jObj = (JObject) JsonConvert.DeserializeObject(json);

            string query = string.Join("&",
                                       jObj.Children().Cast<JProperty>()
                                           .Select(jp => jp.Name + "=" + HttpUtility.UrlEncode(jp.Value.ToString())));

            return query;
        }
    }
}