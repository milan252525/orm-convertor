using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using DapperPerformance;
using EF6Performance;
using EFCorePerformance;
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
                typeof(EFCoreBenchmarks),
                typeof(EF6Benchmarks)
            ])
            .Run(args, testConfig);
    }
}
