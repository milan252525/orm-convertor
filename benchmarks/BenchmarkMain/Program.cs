using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using DapperPerformance;
using EF6Performance;
using EFCorePerformance;
using Linq2dbPerformance;
using NHibernatePerformance;
using PetaPocoPerformance;
using RepoDBPerformance;

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
                typeof(Linq2dbBenchmarks),
                typeof(RepoDBBenchmark),
                typeof(EFCoreBenchmarks),
                typeof(EF6Benchmarks),
                typeof(NHibernateBenchmarks)
            ])
            .Run(args, testConfig);
    }
}
