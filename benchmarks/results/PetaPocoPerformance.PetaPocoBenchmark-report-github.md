```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4602/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Method                           | Mean         | Error       | StdDev      | Gen0      | Exceptions | Gen1      | Gen2    | Allocated   |
|--------------------------------- |-------------:|------------:|------------:|----------:|-----------:|----------:|--------:|------------:|
| A1_EntityIdenticalToTable        |     707.0 μs |     8.56 μs |     7.15 μs |    0.9766 |          - |         - |       - |     7.99 KB |
| A2_LimitedEntity                 |     699.8 μs |    10.62 μs |     9.94 μs |    0.9766 |          - |         - |       - |     9.64 KB |
| A3_MultipleEntitiesFromOneResult |     711.7 μs |    12.27 μs |    11.48 μs |    0.9766 |          - |         - |       - |    13.03 KB |
| A4_StoredProcedureToEntity       | 505,504.8 μs | 8,774.44 μs | 8,207.62 μs | 1000.0000 |          - |         - |       - | 16490.15 KB |
| B1_SelectionOverIndexedColumn    |     727.3 μs |     9.20 μs |     8.15 μs |    0.9766 |          - |         - |       - |     14.3 KB |
| B2_SelectionOverNonIndexedColumn |  40,896.6 μs |   637.97 μs |   596.75 μs |  416.6667 |          - |  333.3333 | 83.3333 |  3308.08 KB |
| B3_RangeQuery                    |  29,962.5 μs |   188.27 μs |   176.10 μs |   31.2500 |          - |         - |       - |   468.38 KB |
| B4_InQuery                       |     835.4 μs |    13.71 μs |    12.83 μs |    1.9531 |          - |         - |       - |    20.34 KB |
| B5_TextSearch                    | 748,477.1 μs | 4,285.18 μs | 4,008.36 μs |         - |          - |         - |       - |   587.24 KB |
| B6_PagingQuery                   |   1,298.1 μs |    19.47 μs |    18.22 μs |    1.9531 |          - |         - |       - |    25.06 KB |
| C1_AggregationCount              |  35,138.1 μs |   318.53 μs |   297.95 μs |         - |     0.1333 |         - |       - |     8.79 KB |
| C2_AggregationMax                |   1,218.0 μs |    24.25 μs |    27.92 μs |         - |          - |         - |       - |     4.19 KB |
| C3_AggregationSum                |  86,614.7 μs |   673.82 μs |   597.32 μs |         - |          - |         - |       - |     4.41 KB |
| D1_OneToManyRelationship         |     795.5 μs |    15.54 μs |    29.20 μs |    1.9531 |          - |         - |       - |    17.94 KB |
| D2_ManyToManyRelationship        |  20,075.9 μs |   397.29 μs |   472.95 μs |   93.7500 |          - |   62.5000 |       - |   818.88 KB |
| D3_OptionalRelationship          | 186,133.8 μs | 3,665.33 μs | 7,569.54 μs | 2000.0000 |          - | 2000.0000 |       - | 24909.68 KB |
| E1_ColumnSorting                 |   4,823.1 μs |    95.55 μs |    89.37 μs |   15.6250 |          - |         - |       - |   156.96 KB |
| E2_Distinct                      |   2,267.9 μs |    44.61 μs |    56.42 μs |         - |          - |         - |       - |     4.78 KB |
| F1_NestedJSONQuery               |   1,517.1 μs |    22.62 μs |    21.16 μs |    3.9063 |          - |         - |       - |    45.25 KB |
| F2_JSONArrayQuery                |   1,792.7 μs |    35.59 μs |    33.29 μs |    1.9531 |          - |         - |       - |    18.12 KB |
| G1_Union                         |     719.9 μs |     7.05 μs |     6.25 μs |         - |          - |         - |       - |     3.45 KB |
| G2_Intersection                  |     727.7 μs |     9.21 μs |     8.61 μs |         - |          - |         - |       - |     3.25 KB |
