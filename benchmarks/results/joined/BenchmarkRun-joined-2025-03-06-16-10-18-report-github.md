```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4890/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Namespace             | Type                 | Method                           | Mean           | Error        | StdDev       | Gen0       | Exceptions | Gen1       | Gen2      | Allocated    |
|---------------------- |--------------------- |--------------------------------- |---------------:|-------------:|-------------:|-----------:|-----------:|-----------:|----------:|-------------:|
| DapperPerformance     | DapperBenchmark      | A1_EntityIdenticalToTable        |       750.2 μs |      8.06 μs |      7.54 μs |          - |          - |          - |         - |      6.24 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | A1_EntityIdenticalToTable        |       854.1 μs |      9.79 μs |      8.17 μs |     9.7656 |          - |          - |         - |     90.43 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | A1_EntityIdenticalToTable        |       809.1 μs |      6.32 μs |      5.91 μs |     0.9766 |          - |          - |         - |     14.49 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | A1_EntityIdenticalToTable        |       839.2 μs |     11.01 μs |     10.30 μs |     0.9766 |          - |          - |         - |     12.74 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | A1_EntityIdenticalToTable        |       742.1 μs |      7.41 μs |      6.93 μs |     3.9063 |          - |          - |         - |     36.06 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | A1_EntityIdenticalToTable        |       721.8 μs |      7.93 μs |      7.42 μs |     0.9766 |          - |          - |         - |      7.99 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | A1_EntityIdenticalToTable        |       747.5 μs |      7.36 μs |      6.52 μs |     0.9766 |          - |     0.9766 |         - |      8.77 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | A2_LimitedEntity                 |       717.6 μs |      8.61 μs |      8.05 μs |          - |          - |          - |         - |      4.74 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | A2_LimitedEntity                 |       882.1 μs |     11.83 μs |     11.07 μs |    13.6719 |          - |          - |         - |    115.18 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | A2_LimitedEntity                 |       798.7 μs |      8.09 μs |      7.56 μs |     1.9531 |          - |          - |         - |     16.11 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | A2_LimitedEntity                 |       858.4 μs |      6.92 μs |      6.47 μs |     1.9531 |          - |          - |         - |     18.25 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | A2_LimitedEntity                 |       957.5 μs |     12.34 μs |     11.55 μs |     5.8594 |          - |     1.9531 |         - |      50.7 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | A2_LimitedEntity                 |       726.0 μs |     14.04 μs |     13.14 μs |     0.9766 |          - |     0.9766 |         - |      9.64 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | A2_LimitedEntity                 |       734.9 μs |     12.35 μs |     11.55 μs |     0.9766 |          - |     0.9766 |         - |      7.99 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | A3_MultipleEntitiesFromOneResult |       736.4 μs |      5.78 μs |      5.41 μs |          - |          - |          - |         - |      7.28 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | A3_MultipleEntitiesFromOneResult |       944.7 μs |      9.97 μs |      9.33 μs |    25.3906 |          - |     1.9531 |         - |    211.61 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | A3_MultipleEntitiesFromOneResult |       831.7 μs |     11.16 μs |     10.44 μs |     1.9531 |          - |          - |         - |     25.85 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | A3_MultipleEntitiesFromOneResult |       898.1 μs |      8.72 μs |      7.73 μs |     1.9531 |          - |          - |         - |     27.74 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | A3_MultipleEntitiesFromOneResult |     1,000.6 μs |      6.58 μs |      5.49 μs |     9.7656 |          - |     1.9531 |         - |        84 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | A3_MultipleEntitiesFromOneResult |       736.1 μs |      3.44 μs |      3.05 μs |     0.9766 |          - |          - |         - |     13.03 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | A3_MultipleEntitiesFromOneResult |       744.9 μs |     10.79 μs |     10.09 μs |     0.9766 |          - |          - |         - |     10.38 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | A4_StoredProcedureToEntity       |   523,239.3 μs | 10,064.10 μs |  9,413.96 μs |  2000.0000 |          - |  1000.0000 |         - |  24294.84 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | A4_StoredProcedureToEntity       |   514,077.2 μs |  9,590.71 μs |  9,848.95 μs |  4000.0000 |          - |  1000.0000 |         - |  35270.68 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | A4_StoredProcedureToEntity       |   521,573.3 μs |  7,951.50 μs |  7,048.80 μs |  3000.0000 |          - |  1000.0000 |         - |  29008.69 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | A4_StoredProcedureToEntity       |   519,491.2 μs |  8,289.35 μs |  7,753.86 μs |  1000.0000 |          - |          - |         - |  16490.93 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | A4_StoredProcedureToEntity       |   621,738.7 μs |  3,010.65 μs |  2,350.51 μs | 12000.0000 |          - |  7000.0000 | 2000.0000 |  91569.96 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | A4_StoredProcedureToEntity       |   505,966.7 μs |  9,686.22 μs | 10,766.21 μs |  1000.0000 |          - |          - |         - |  16490.15 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | A4_StoredProcedureToEntity       |   523,242.0 μs |  9,392.18 μs | 10,049.53 μs |  1000.0000 |          - |  1000.0000 |         - |  17007.14 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | B1_SelectionOverIndexedColumn    |       744.0 μs |      9.47 μs |      8.40 μs |     0.9766 |          - |          - |         - |      8.63 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | B1_SelectionOverIndexedColumn    |       904.8 μs |     14.96 μs |     13.27 μs |    11.7188 |          - |          - |         - |    102.35 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | B1_SelectionOverIndexedColumn    |       806.2 μs |     10.03 μs |      9.38 μs |     0.9766 |          - |          - |         - |     12.88 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | B1_SelectionOverIndexedColumn    |       868.4 μs |     10.42 μs |      9.74 μs |          - |          - |          - |         - |     14.63 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | B1_SelectionOverIndexedColumn    |       816.8 μs |      8.32 μs |      7.38 μs |     5.8594 |          - |          - |         - |     49.46 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | B1_SelectionOverIndexedColumn    |       758.7 μs |     10.50 μs |      9.82 μs |     0.9766 |          - |     0.9766 |         - |      14.3 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | B1_SelectionOverIndexedColumn    |       761.5 μs |      7.95 μs |      7.05 μs |     0.9766 |          - |          - |         - |      9.79 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | B2_SelectionOverNonIndexedColumn |    43,611.6 μs |    427.98 μs |    357.38 μs |   833.3333 |          - |   500.0000 |  250.0000 |   7112.75 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | B2_SelectionOverNonIndexedColumn |   125,169.4 μs |  2,497.61 μs |  4,987.99 μs |  6000.0000 |          - |  3000.0000 | 1000.0000 |  50014.45 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | B2_SelectionOverNonIndexedColumn |    48,016.3 μs |    625.62 μs |    554.60 μs |   727.2727 |          - |   363.6364 |   90.9091 |   5847.86 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | B2_SelectionOverNonIndexedColumn |    43,707.9 μs |    812.59 μs |    760.10 μs |   454.5455 |          - |   363.6364 |   90.9091 |   3314.16 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | B2_SelectionOverNonIndexedColumn |    86,418.9 μs |  1,689.91 μs |  1,808.18 μs |  2833.3333 |          - |  1666.6667 |  500.0000 |  21371.02 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | B2_SelectionOverNonIndexedColumn |    41,099.7 μs |    794.20 μs |    780.01 μs |   461.5385 |          - |   461.5385 |  153.8462 |   3308.54 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | B2_SelectionOverNonIndexedColumn |    43,052.4 μs |    397.39 μs |    371.71 μs |   416.6667 |          - |   416.6667 |   83.3333 |   3410.07 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | B3_RangeQuery                    |    30,393.2 μs |    171.55 μs |    160.47 μs |    93.7500 |          - |    31.2500 |         - |    989.93 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | B3_RangeQuery                    |    22,823.1 μs |    440.23 μs |    411.79 μs |   812.5000 |          - |   656.2500 |         - |   6747.98 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | B3_RangeQuery                    |    21,102.2 μs |    127.92 μs |     99.87 μs |    93.7500 |          - |    31.2500 |         - |    819.92 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | B3_RangeQuery                    |    21,908.8 μs |    378.29 μs |    335.35 μs |    31.2500 |          - |          - |         - |    475.55 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | B3_RangeQuery                    |    22,288.6 μs |    327.01 μs |    305.88 μs |   343.7500 |          - |   218.7500 |         - |   3048.82 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | B3_RangeQuery                    |    29,935.3 μs |    383.19 μs |    319.98 μs |    31.2500 |          - |          - |         - |    468.38 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | B3_RangeQuery                    |    30,745.6 μs |    259.73 μs |    230.24 μs |    31.2500 |          - |          - |         - |    480.34 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | B4_InQuery                       |       855.9 μs |     15.72 μs |     14.71 μs |     1.9531 |          - |          - |         - |     16.92 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | B4_InQuery                       |     1,408.2 μs |      8.66 μs |      8.10 μs |    42.9688 |          - |     7.8125 |         - |    354.18 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | B4_InQuery                       |     1,146.7 μs |     14.03 μs |     12.43 μs |     1.9531 |          - |          - |         - |     19.85 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | B4_InQuery                       |       953.6 μs |     14.95 μs |     13.98 μs |     1.9531 |          - |          - |         - |     19.58 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | B4_InQuery                       |       945.4 μs |     15.11 μs |     14.14 μs |     9.7656 |          - |          - |         - |     87.42 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | B4_InQuery                       |       854.7 μs |     11.81 μs |     10.47 μs |     1.9531 |          - |     1.9531 |         - |     20.34 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | B4_InQuery                       |       875.6 μs |     11.10 μs |     10.38 μs |     1.9531 |          - |          - |         - |     16.64 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | B5_TextSearch                    |   747,728.5 μs |  3,349.08 μs |  2,968.87 μs |          - |          - |          - |         - |   1169.29 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | B5_TextSearch                    |   746,263.5 μs |  4,895.88 μs |  4,340.07 μs |          - |          - |          - |         - |   8113.77 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | B5_TextSearch                    |   744,754.3 μs |  4,073.95 μs |  3,611.45 μs |          - |          - |          - |         - |    979.42 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | B5_TextSearch                    |   745,319.3 μs |  4,815.91 μs |  4,021.50 μs |          - |          - |          - |         - |    592.78 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | B5_TextSearch                    |   746,464.3 μs |  3,967.33 μs |  3,516.93 μs |          - |          - |          - |         - |   3472.99 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | B5_TextSearch                    |   747,720.8 μs |  4,753.81 μs |  4,446.71 μs |          - |          - |          - |         - |    587.24 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | B5_TextSearch                    |   745,550.9 μs |  2,224.67 μs |  1,972.11 μs |          - |          - |          - |         - |    599.41 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | B6_PagingQuery                   |     1,327.4 μs |     17.61 μs |     15.61 μs |     3.9063 |          - |          - |         - |     32.59 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | B6_PagingQuery                   |     1,710.2 μs |     10.93 μs |      9.69 μs |    31.2500 |          - |     3.9063 |         - |    265.26 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | B6_PagingQuery                   |     1,405.5 μs |     13.68 μs |     12.13 μs |     3.9063 |          - |          - |         - |     40.52 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | B6_PagingQuery                   |     1,457.7 μs |     14.23 μs |     12.62 μs |     1.9531 |          - |          - |         - |     26.58 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | B6_PagingQuery                   |     1,446.8 μs |      8.71 μs |      6.80 μs |    15.6250 |          - |     1.9531 |         - |    128.29 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | B6_PagingQuery                   |     1,314.2 μs |      8.71 μs |      7.72 μs |     1.9531 |          - |     1.9531 |         - |     25.06 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | B6_PagingQuery                   |     1,556.5 μs |     30.99 μs |     31.82 μs |     1.9531 |          - |          - |         - |     18.25 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | C1_AggregationCount              |    35,005.5 μs |    281.51 μs |    235.07 μs |          - |          - |          - |         - |      2.75 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | C1_AggregationCount              |    35,364.0 μs |    325.50 μs |    304.48 μs |          - |          - |          - |         - |    117.99 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | C1_AggregationCount              |    35,286.5 μs |    228.51 μs |    202.57 μs |          - |          - |          - |         - |     12.04 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | C1_AggregationCount              |    35,372.2 μs |    276.33 μs |    244.96 μs |          - |          - |          - |         - |     12.46 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | C1_AggregationCount              |    35,552.6 μs |    241.88 μs |    226.25 μs |          - |          - |          - |         - |     41.75 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | C1_AggregationCount              |    35,289.3 μs |    207.22 μs |    161.78 μs |          - |     0.1333 |          - |         - |      8.79 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | C1_AggregationCount              |   377,349.4 μs |  7,217.76 μs | 18,107.95 μs |  8000.0000 |          - |  8000.0000 | 2000.0000 |   59994.3 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | C2_AggregationMax                |     1,240.0 μs |     17.58 μs |     15.58 μs |          - |          - |          - |         - |      1.97 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | C2_AggregationMax                |     1,366.5 μs |     19.48 μs |     17.27 μs |     7.8125 |          - |          - |         - |     77.09 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | C2_AggregationMax                |     1,303.6 μs |     25.17 μs |     26.94 μs |          - |          - |          - |         - |      5.69 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | C2_AggregationMax                |     1,380.1 μs |     25.47 μs |     22.58 μs |          - |          - |          - |         - |      8.12 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | C2_AggregationMax                |     1,291.0 μs |      9.74 μs |      9.11 μs |     1.9531 |          - |          - |         - |     23.06 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | C2_AggregationMax                |     1,227.3 μs |     14.70 μs |     13.75 μs |          - |          - |          - |         - |      4.19 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | C2_AggregationMax                |     1,264.3 μs |     23.73 μs |     22.20 μs |          - |          - |          - |         - |      1.77 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | C3_AggregationSum                |    86,498.6 μs |  1,391.95 μs |  1,162.34 μs |          - |          - |          - |         - |      2.09 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | C3_AggregationSum                |    69,826.8 μs |    562.86 μs |    526.50 μs |          - |          - |          - |         - |     81.72 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | C3_AggregationSum                |    87,824.8 μs |    450.84 μs |    376.48 μs |          - |          - |          - |         - |      6.85 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | C3_AggregationSum                |    86,145.4 μs |    656.61 μs |    582.07 μs |          - |          - |          - |         - |      9.25 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | C3_AggregationSum                |    75,136.8 μs |    482.51 μs |    427.74 μs |          - |          - |          - |         - |     25.31 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | C3_AggregationSum                |    85,943.8 μs |    846.39 μs |    750.30 μs |          - |          - |          - |         - |      4.41 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | C3_AggregationSum                |    86,898.7 μs |  1,211.51 μs |  1,133.24 μs |          - |          - |          - |         - |      1.35 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | D1_OneToManyRelationship         |       780.9 μs |      9.79 μs |      9.16 μs |     1.9531 |          - |          - |         - |     16.46 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | D1_OneToManyRelationship         |     1,581.1 μs |     29.26 μs |     27.37 μs |    11.7188 |          - |          - |         - |    115.27 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | D1_OneToManyRelationship         |       895.3 μs |     12.24 μs |     10.85 μs |     1.9531 |          - |          - |         - |     26.44 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | D1_OneToManyRelationship         |     2,996.8 μs |     34.88 μs |     30.92 μs |          - |          - |          - |         - |     24.58 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | D1_OneToManyRelationship         |     1,041.9 μs |     11.25 μs |     10.52 μs |     9.7656 |          - |     1.9531 |         - |     88.71 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | D1_OneToManyRelationship         |       769.7 μs |      4.41 μs |      3.91 μs |     1.9531 |          - |          - |         - |     17.94 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | D1_OneToManyRelationship         |       806.0 μs |      9.84 μs |      9.21 μs |     1.9531 |          - |     1.9531 |         - |     20.23 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | D2_ManyToManyRelationship        |     4,584.3 μs |     46.15 μs |     40.91 μs |    46.8750 |          - |    39.0625 |         - |    433.35 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | D2_ManyToManyRelationship        |     6,913.3 μs |     83.64 μs |     74.15 μs |   156.2500 |          - |    31.2500 |         - |   1395.95 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | D2_ManyToManyRelationship        |    14,964.1 μs |    193.69 μs |    181.17 μs |   312.5000 |          - |    62.5000 |   31.2500 |   2555.79 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | D2_ManyToManyRelationship        |     9,383.2 μs |    100.51 μs |     89.10 μs |    31.2500 |          - |    15.6250 |         - |    352.15 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | D2_ManyToManyRelationship        |     5,940.0 μs |     71.27 μs |     66.67 μs |   140.6250 |          - |    54.6875 |         - |   1162.43 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | D2_ManyToManyRelationship        |     4,479.8 μs |     73.82 μs |     69.05 μs |    39.0625 |          - |    31.2500 |         - |    381.13 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | D2_ManyToManyRelationship        |     6,030.8 μs |     64.88 μs |     60.69 μs |   101.5625 |     1.3516 |    46.8750 |         - |    854.28 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | D3_OptionalRelationship          |   187,693.2 μs |  3,039.85 μs |  2,694.75 μs |  5000.0000 |          - |  2000.0000 | 1000.0000 |  40923.17 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | D3_OptionalRelationship          | 1,993,145.6 μs | 19,299.27 μs | 18,052.55 μs | 33000.0000 |          - | 17000.0000 | 2000.0000 | 278566.55 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | D3_OptionalRelationship          | 2,548,411.2 μs | 12,032.29 μs | 10,666.31 μs | 21000.0000 |          - |  3000.0000 |         - | 175866.23 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | D3_OptionalRelationship          |    91,326.3 μs |  1,458.24 μs |  1,364.04 μs |  1000.0000 |          - |   500.0000 |  166.6667 |   9083.71 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | D3_OptionalRelationship          |   533,489.2 μs | 10,627.26 μs | 22,647.55 μs | 15000.0000 |          - |  8000.0000 | 1000.0000 | 144094.98 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | D3_OptionalRelationship          |   172,029.7 μs |  2,692.25 μs |  2,518.33 μs |  2666.6667 |          - |  1666.6667 |  666.6667 |  24909.64 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | D3_OptionalRelationship          |   393,630.9 μs |  7,823.30 μs | 10,443.88 μs |  2000.0000 |          - |  2000.0000 |         - |  20945.39 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | E1_ColumnSorting                 |     4,725.4 μs |     64.97 μs |     57.60 μs |    39.0625 |          - |     7.8125 |         - |     349.4 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | E1_ColumnSorting                 |     6,203.7 μs |     71.06 μs |     63.00 μs |   234.3750 |          - |   156.2500 |         - |   2000.95 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | E1_ColumnSorting                 |     4,884.8 μs |     80.25 μs |     71.14 μs |    39.0625 |          - |     7.8125 |         - |    348.07 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | E1_ColumnSorting                 |     4,845.0 μs |     49.33 μs |     43.73 μs |    15.6250 |          - |          - |         - |    162.54 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | E1_ColumnSorting                 |     6,315.5 μs |     36.78 μs |     30.71 μs |   164.0625 |          - |    70.3125 |         - |   1354.02 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | E1_ColumnSorting                 |     4,622.0 μs |     20.24 μs |     15.80 μs |    15.6250 |          - |     7.8125 |         - |    156.96 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | E1_ColumnSorting                 |     4,963.1 μs |     46.22 μs |     40.97 μs |    15.6250 |          - |    15.6250 |         - |    162.62 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | E2_Distinct                      |     2,181.4 μs |     34.26 μs |     33.64 μs |          - |          - |          - |         - |      2.42 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | E2_Distinct                      |     2,340.4 μs |     30.74 μs |     27.25 μs |     7.8125 |          - |          - |         - |     76.19 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | E2_Distinct                      |     2,270.4 μs |     19.08 μs |     16.92 μs |          - |          - |          - |         - |      8.54 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | E2_Distinct                      |     2,373.5 μs |     38.35 μs |     34.00 μs |          - |          - |          - |         - |      9.34 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | E2_Distinct                      |     2,229.0 μs |     20.96 μs |     18.58 μs |          - |          - |          - |         - |     23.72 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | E2_Distinct                      |     2,148.0 μs |     20.19 μs |     17.90 μs |          - |          - |          - |         - |      4.78 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | E2_Distinct                      |     2,180.3 μs |     26.25 μs |     24.55 μs |          - |          - |          - |         - |      1.66 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | F1_JSONObjectQuery               |     1,459.4 μs |     15.68 μs |     13.90 μs |     1.9531 |          - |          - |         - |     21.71 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | F1_JSONObjectQuery               |     1,490.3 μs |     19.40 μs |     17.20 μs |     7.8125 |          - |          - |         - |     73.38 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | F1_JSONObjectQuery               |     1,544.1 μs |     17.91 μs |     16.75 μs |     5.8594 |          - |          - |         - |     51.68 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | F1_JSONObjectQuery               |     1,551.9 μs |     11.10 μs |      9.84 μs |     1.9531 |          - |          - |         - |     27.09 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | F1_JSONObjectQuery               |     1,493.5 μs |     28.34 μs |     30.33 μs |     9.7656 |          - |          - |         - |     86.13 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | F1_JSONObjectQuery               |     1,477.6 μs |     18.12 μs |     16.06 μs |     3.9063 |          - |          - |         - |     45.25 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | F1_JSONObjectQuery               |     1,457.0 μs |     12.62 μs |     11.19 μs |     1.9531 |          - |     1.9531 |         - |     31.82 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | F2_JSONArrayQuery                |     1,722.9 μs |     29.94 μs |     26.54 μs |          - |          - |          - |         - |      6.04 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | F2_JSONArrayQuery                |     1,770.9 μs |     34.82 μs |     32.57 μs |     5.8594 |          - |          - |         - |     57.24 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | F2_JSONArrayQuery                |     1,839.0 μs |     28.79 μs |     26.93 μs |          - |          - |          - |         - |     16.86 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | F2_JSONArrayQuery                |     1,795.2 μs |     15.89 μs |     14.87 μs |          - |          - |          - |         - |      11.3 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | F2_JSONArrayQuery                |     1,760.7 μs |     21.26 μs |     18.85 μs |     7.8125 |          - |          - |         - |     74.31 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | F2_JSONArrayQuery                |     1,717.7 μs |     19.46 μs |     18.20 μs |     1.9531 |          - |          - |         - |     18.12 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | F2_JSONArrayQuery                |     1,732.3 μs |     34.25 μs |     32.03 μs |          - |          - |          - |         - |      8.25 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | G1_Union                         |       727.7 μs |      7.98 μs |      7.47 μs |          - |          - |          - |         - |      2.39 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | G1_Union                         |     1,613.2 μs |     19.38 μs |     17.18 μs |    15.6250 |          - |          - |         - |     136.1 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | G1_Union                         |     1,561.4 μs |     20.90 μs |     18.52 μs |     1.9531 |          - |          - |         - |     20.35 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | G1_Union                         |     1,579.5 μs |     14.93 μs |     12.47 μs |     1.9531 |          - |          - |         - |     18.03 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | G1_Union                         |     1,537.9 μs |     22.82 μs |     21.35 μs |     5.8594 |          - |          - |         - |     60.07 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | G1_Union                         |       714.7 μs |     10.02 μs |      9.37 μs |          - |          - |          - |         - |      3.45 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | G1_Union                         |       723.0 μs |      8.96 μs |      8.38 μs |          - |          - |          - |         - |       1.4 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | G2_Intersection                  |       739.1 μs |     10.36 μs |      9.19 μs |          - |          - |          - |         - |      2.17 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | G2_Intersection                  |     1,622.1 μs |     21.74 μs |     19.27 μs |    15.6250 |          - |          - |         - |    135.98 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | G2_Intersection                  |     1,566.1 μs |     23.00 μs |     21.52 μs |     1.9531 |          - |          - |         - |     21.73 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | G2_Intersection                  |     1,596.2 μs |     15.27 μs |     13.53 μs |     1.9531 |          - |          - |         - |     17.91 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | G2_Intersection                  |     1,534.4 μs |     23.15 μs |     20.52 μs |     5.8594 |          - |          - |         - |     61.36 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | G2_Intersection                  |       717.9 μs |      9.20 μs |      8.60 μs |          - |          - |          - |         - |      3.25 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | G2_Intersection                  |       730.0 μs |      7.10 μs |      6.29 μs |          - |          - |          - |         - |       1.3 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| DapperPerformance     | DapperBenchmark      | H1_Metadata                      |       851.0 μs |      8.20 μs |      7.27 μs |          - |          - |          - |         - |      1.98 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EF6Performance        | EF6Benchmarks        | H1_Metadata                      |       899.3 μs |      7.65 μs |      7.16 μs |     3.9063 |          - |          - |         - |     46.52 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| EFCorePerformance     | EFCoreBenchmarks     | H1_Metadata                      |       814.5 μs |      9.25 μs |      8.20 μs |     0.9766 |          - |          - |         - |     10.99 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| Linq2dbPerformance    | Linq2dbBenchmarks    | H1_Metadata                      |       924.7 μs |      6.58 μs |      6.16 μs |          - |          - |          - |         - |      5.61 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| NHibernatePerformance | NHibernateBenchmarks | H1_Metadata                      |       866.4 μs |      8.62 μs |      8.06 μs |     3.9063 |          - |          - |         - |     35.53 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| PetaPocoPerformance   | PetaPocoBenchmark    | H1_Metadata                      |       843.0 μs |      7.05 μs |      6.59 μs |          - |          - |          - |         - |       5.1 KB |
|                       |                      |                                  |                |              |              |            |            |            |           |              |
| RepoDBPerformance     | RepoDBBenchmark      | H1_Metadata                      |       844.0 μs |      6.83 μs |      6.05 μs |          - |          - |          - |         - |      1.21 KB |
