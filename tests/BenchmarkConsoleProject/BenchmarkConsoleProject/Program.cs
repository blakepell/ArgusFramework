using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Argus.Extensions;
using System.Data;
using Newtonsoft.Json.Linq;

namespace BenchmarkConsoleProject
{
    [MemoryDiagnoser]
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Benchmark");
            var summary = BenchmarkRunner.Run<Program>();
        }

        private string _json;

        [GlobalSetup]
        public void SetupGlobal()
        {
            _json = System.IO.File.ReadAllText("test2.json");
        }

        [IterationSetup]
        public void Setup()
        {
        }

        [Benchmark]
        public void ArgusCurrent()
        {
            for (int i = 0; i < 10000; i++)
            {
                var dt = JsonToDataTableArgus(_json, "Employees");
            }
        }

        [Benchmark]
        public void Disqus()
        {
            for (int i = 0; i < 10000; i++)
            {
                var dt = JsonToDataTableDisqus(_json, "Employees");
            }
        }

        [Benchmark]
        public void ArgusNew()
        {
            for (int i = 0; i < 10000; i++)
            {
                var dt = JsonToDataTableNew(_json, "Employees");
            }
        }

        public static DataTable JsonToDataTableNew(string json, string tableName)
        {
            var dt = new DataTable(tableName);
            var root = JObject.Parse(json);
            var items = root[tableName] as JObject;

            if (items == null || items.Count == 0)
            {
                return dt;
            }

            // Create the columns
            foreach (var jprop in items.Properties())
            {
                dt.Columns.Add(new DataColumn(jprop.Name));
            }

            for (int i = 0; i <= items.Count - 1; i++)
            {
                // Create the new row, put the values into the columns then add the row to the DataTable
                var dr = dt.NewRow();

                foreach (JProperty item in items[i])
                {
                    dr[item.Name] = item.Value.ToString();
                }

                dt.Rows.Add(dr);
            }

            var sb = new StringBuilder();
            sb.Append("Rows: ").Append(dt.Rows.Count).Append("\r\n");
            sb.Append(dt.ToString(true, ","));

            System.IO.File.WriteAllText(@"C:\Temp\test.txt", sb.ToString(), System.Text.Encoding.ASCII);

            Environment.Exit(0);
            return dt;
        }

        public static DataTable JsonToDataTableArgus(string json, string tableName)
        {
            bool columnsCreated = false;
            var dt = new DataTable(tableName);

            var root = JObject.Parse(json);
            var items = (JArray)root[tableName];

            if (items == null)
            {
                return dt;
            }

            for (int i = 0; i <= items.Count - 1; i++)
            {
                // Create the columns once
                JObject item;
                JToken jtoken;

                if (columnsCreated == false)
                {
                    item = (JObject)items[i];
                    jtoken = item.First;

                    while (jtoken != null)
                    {
                        dt.Columns.Add(new DataColumn(((JProperty)jtoken).Name));
                        jtoken = jtoken.Next;
                    }

                    columnsCreated = true;
                }

                // Add each of the columns into a new row then put that new row into the DataTable
                item = (JObject)items[i];
                jtoken = item.First;

                // Create the new row, put the values into the columns then add the row to the DataTable
                var dr = dt.NewRow();

                while (jtoken != null)
                {
                    dr[((JProperty)jtoken).Name] = ((JProperty)jtoken).Value.ToString();
                    jtoken = jtoken.Next;
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        public static DataTable JsonToDataTableDisqus(string json, string tableName)
        {
            bool columnsCreated = false;
            DataTable dt = new DataTable(tableName);
            Newtonsoft.Json.Linq.JObject root = Newtonsoft.Json.Linq.JObject.Parse(json);
            Newtonsoft.Json.Linq.JArray items = (Newtonsoft.Json.Linq.JArray)root[tableName];
            Newtonsoft.Json.Linq.JObject item = default(Newtonsoft.Json.Linq.JObject);
            Newtonsoft.Json.Linq.JToken jtoken = default(Newtonsoft.Json.Linq.JToken);
            for (int i = 0; i <= items.Count - 1; i++)
            {
                if (columnsCreated == false)
                {
                    item = (Newtonsoft.Json.Linq.JObject)items[i];
                    jtoken = item.First;

                    while (jtoken != null)
                    {
                        dt.Columns.Add(new DataColumn(((Newtonsoft.Json.Linq.JProperty)jtoken).Name.ToString()));
                        jtoken = jtoken.Next;
                    }
                    columnsCreated = true;
                }
                item = (Newtonsoft.Json.Linq.JObject)items[i];
                jtoken = item.First;
                DataRow dr = dt.NewRow();
                while (jtoken != null)
                {
                    dr[((Newtonsoft.Json.Linq.JProperty)jtoken).Name.ToString()] = ((Newtonsoft.Json.Linq.JProperty)jtoken).Value.ToString();
                    jtoken = jtoken.Next;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

    }
}
