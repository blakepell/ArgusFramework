using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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

        struct LineSt
        {
            //public string Text;
            //public string FormattedText; 
            public int number;
            public bool reverse;
        }

        class LineCl
        {
            //public string Text { get; set; }
            //public string FormattedText { get; set; }
            public int number { get; set; }
            public bool reverse { get; set; }
        }

        [GlobalSetup]
        public void SetupGlobal()
        {
            LineStruct = new List<LineSt>();
            LineClass = new List<LineCl>();
        }

        [IterationSetup]
        public void Setup()
        {
            LineStruct.Clear();
            LineClass.Clear();
        }

        private List<LineSt> LineStruct { get; set; }

        private List<LineCl> LineClass { get; set; }

        [Benchmark]
        public void TestOne()
        {
            for (int i = 0; i < 10000000; i++)
            {
                var value = new LineSt
                {
                    //Text = "This is a new line in the struct.",
                    //FormattedText = "This is a new line in the struct.",
                    number = 1,
                    reverse = false
                };

                //value.FormattedText = $"{value.number}: {value.Text}";
                LineStruct.Add(value);
            }
        }

        [Benchmark]
        public void TestTwo()
        {
            for (int i = 0; i < 10000000; i++)
            {
                var value = new LineCl
                {
                    //Text = "This is a new line in the struct.",
                    //FormattedText = "This is a new line in the struct.",
                    number = 1,
                    reverse = false
                };

                //value.FormattedText = $"{value.number}: {value.Text}";
                LineClass.Add(value);
            }
        }

    }
}
