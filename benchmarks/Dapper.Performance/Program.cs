using BenchmarkDotNet.Running;
using DapperTests;

namespace Dapper.Performance;

internal class Program
{
    static void Main(string[] args)
    {
        BenchmarkSwitcher
            .FromAssembly(typeof(Program).Assembly)
            .Run(args);
    }
}
