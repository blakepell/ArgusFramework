using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

namespace BenchmarkConsoleProject
{
    [MemoryDiagnoser]
    public class Program
    {
        [Benchmark]
        public void One()
        {

        }

        [Benchmark]
        public void Two()
        {

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Benchmark Starting");
            var summary = BenchmarkRunner.Run<Program>();
        }
    }
}
