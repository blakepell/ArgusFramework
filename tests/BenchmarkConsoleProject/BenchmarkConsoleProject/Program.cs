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

        [Benchmark]
        public void TestOne()
        {
        }

        [Benchmark]
        public void TestTwo()
        {
        }

    }
}
