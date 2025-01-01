```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4602/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Method                           | Mean         | Error       | StdDev      | Median       | Gen0      | Exceptions | Gen1      | Gen2      | Allocated   |
|--------------------------------- |-------------:|------------:|------------:|-------------:|----------:|-----------:|----------:|----------:|------------:|
| A1_EntityIdenticalToTable        |     725.9 μs |     6.99 μs |     6.20 μs |     725.4 μs |         - |          - |         - |         - |     6.24 KB |
| A2_LimitedEntity                 |     699.7 μs |     8.22 μs |     6.87 μs |     700.1 μs |         - |          - |         - |         - |     4.74 KB |
| A3_MultipleEntitiesFromOneResult |     720.4 μs |     6.87 μs |     6.09 μs |     721.0 μs |         - |          - |         - |         - |     7.28 KB |
| A4_StoredProcedureToEntity       | 523,674.1 μs | 9,488.80 μs | 8,875.83 μs | 524,124.9 μs | 2000.0000 |          - | 1000.0000 |         - | 24294.84 KB |
| B1_SelectionOverIndexedColumn    |     731.4 μs |    10.21 μs |     9.55 μs |     730.2 μs |    0.9766 |          - |         - |         - |     8.63 KB |
| B2_SelectionOverNonIndexedColumn |  45,245.6 μs |   750.27 μs |   701.81 μs |  45,324.4 μs |  833.3333 |          - |  500.0000 |  250.0000 |  7112.75 KB |
| B3_RangeQuery                    |  30,484.1 μs |   266.92 μs |   236.62 μs |  30,500.6 μs |   93.7500 |          - |   31.2500 |         - |   989.93 KB |
| B4_InQuery                       |     835.3 μs |    14.83 μs |    13.87 μs |     836.5 μs |    1.9531 |          - |         - |         - |    16.92 KB |
| B5_TextSearch                    | 748,551.8 μs | 5,469.15 μs | 5,115.85 μs | 747,347.8 μs |         - |          - |         - |         - |  1169.29 KB |
| B6_PagingQuery                   |   1,287.1 μs |    20.84 μs |    18.47 μs |   1,284.6 μs |    3.9063 |          - |         - |         - |    32.59 KB |
| C1_AggregationCount              |  34,841.7 μs |   323.58 μs |   302.67 μs |  34,948.1 μs |         - |          - |         - |         - |     2.75 KB |
| C2_AggregationMax                |   1,217.6 μs |    21.69 μs |    20.29 μs |   1,220.3 μs |         - |          - |         - |         - |     1.97 KB |
| C3_AggregationSum                |  87,542.3 μs | 1,550.32 μs | 1,723.18 μs |  86,925.3 μs |         - |          - |         - |         - |     2.09 KB |
| D1_OneToManyRelationship         |     764.4 μs |    14.99 μs |    12.52 μs |     761.2 μs |    1.9531 |          - |         - |         - |    16.46 KB |
| D2_ManyToManyRelationship        |  20,651.9 μs |   409.48 μs |   600.21 μs |  20,782.1 μs |   31.2500 |          - |         - |         - |   446.27 KB |
| D3_OptionalRelationship          | 182,586.2 μs | 2,458.47 μs | 2,179.37 μs | 181,763.9 μs | 5000.0000 |          - | 2000.0000 | 1000.0000 | 40924.88 KB |
| E1_ColumnSorting                 |   6,032.1 μs |   275.12 μs |   798.18 μs |   5,917.0 μs |   39.0625 |          - |    7.8125 |         - |    349.4 KB |
| E2_Distinct                      |   2,249.7 μs |    42.20 μs |   114.10 μs |   2,203.5 μs |         - |          - |         - |         - |     2.42 KB |
| F1_NestedJSONQuery               |   1,489.6 μs |    14.98 μs |    12.51 μs |   1,486.5 μs |    1.9531 |          - |         - |         - |    25.94 KB |
| F2_JSONArrayQuery                |   1,691.9 μs |    16.80 μs |    15.72 μs |   1,691.0 μs |         - |          - |         - |         - |    10.27 KB |
| G1_Union                         |     699.1 μs |    13.38 μs |    12.51 μs |     700.5 μs |         - |          - |         - |         - |     2.39 KB |
| G2_Intersection                  |     709.9 μs |    13.78 μs |    12.89 μs |     712.3 μs |         - |          - |         - |         - |     2.17 KB |
