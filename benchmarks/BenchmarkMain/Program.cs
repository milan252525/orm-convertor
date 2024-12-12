using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using DapperPerformance;
using PetaPocoPerformance;

namespace BenchmarkMain;

internal class Program
{
    static void Main(string[] args)
    {
        var testConfig = DefaultConfig.Instance
            .AddJob(
                Job.Default
                    .WithWarmupCount(3)
                    .WithIterationCount(10)
            );

        var defaultConfig = DefaultConfig.Instance;

        BenchmarkSwitcher
            .FromTypes([
                typeof(DapperBenchmark),
                typeof(PetaPocoBenchmark),
            ])
            .Run(args, testConfig);
    }
}
