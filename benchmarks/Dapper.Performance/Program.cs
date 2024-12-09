using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Dapper.Performance;

internal class Program
{
    static void Main(string[] args)
    {
        var testConfig = DefaultConfig.Instance
            .AddJob(
                Job.Default
                    .WithWarmupCount(1)
                    .WithIterationCount(1)
            );

        var defaultConfig = DefaultConfig.Instance;

        BenchmarkSwitcher
            .FromAssembly(typeof(Program).Assembly)
            .Run(args, testConfig);
    }
}
