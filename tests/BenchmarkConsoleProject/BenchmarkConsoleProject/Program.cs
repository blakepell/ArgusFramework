﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Threading;

namespace BenchmarkConsoleProject
{
    class Program
    {
        [Benchmark]
        public void Slow() => Thread.Sleep(100);

        [Benchmark]
        public void Fast() => Thread.Sleep(25);

        static void Main(string[] args)
        {
            Console.WriteLine("BenchmarkDotNet - Hello World!");
            var summary = BenchmarkRunner.Run<Program>();
        }
    }
}
