```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4602/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Method                           | Mean         | Error        | StdDev       | Gen0       | Exceptions | Gen1      | Gen2      | Allocated    |
|--------------------------------- |-------------:|-------------:|-------------:|-----------:|-----------:|----------:|----------:|-------------:|
| A1_EntityIdenticalToTable        |     724.9 μs |      6.51 μs |      5.77 μs |     3.9063 |          - |         - |         - |     36.06 KB |
| A2_LimitedEntity                 |     936.3 μs |     10.92 μs |     10.21 μs |     5.8594 |          - |    1.9531 |         - |     50.81 KB |
| A3_MultipleEntitiesFromOneResult |     987.5 μs |     18.84 μs |     18.50 μs |     9.7656 |          - |    1.9531 |         - |     84.24 KB |
| A4_StoredProcedureToEntity       | 624,716.4 μs | 11,123.01 μs |  9,860.26 μs | 12000.0000 |          - | 7000.0000 | 2000.0000 |  91569.85 KB |
| B1_SelectionOverIndexedColumn    |     785.6 μs |      9.52 μs |      7.95 μs |     5.8594 |          - |         - |         - |     49.46 KB |
| B2_SelectionOverNonIndexedColumn |  85,020.3 μs |  1,670.52 μs |  2,925.79 μs |  3000.0000 |          - | 1666.6667 |  666.6667 |  21370.22 KB |
| B3_RangeQuery                    |  21,892.7 μs |    188.21 μs |    157.17 μs |   343.7500 |          - |  218.7500 |         - |   3048.51 KB |
| B4_InQuery                       |     918.3 μs |     16.07 μs |     14.25 μs |     9.7656 |          - |         - |         - |     87.42 KB |
| B5_TextSearch                    | 745,934.3 μs |  2,502.08 μs |  2,089.35 μs |          - |          - |         - |         - |   3472.99 KB |
| B6_PagingQuery                   |   1,443.0 μs |     28.13 μs |     27.62 μs |    15.6250 |          - |         - |         - |     128.3 KB |
| C1_AggregationCount              |  34,987.6 μs |    613.78 μs |    512.53 μs |          - |          - |         - |         - |     41.55 KB |
| C2_AggregationMax                |   1,293.0 μs |     22.91 μs |     21.43 μs |     1.9531 |          - |         - |         - |     23.06 KB |
| C3_AggregationSum                |  75,219.4 μs |    626.20 μs |    585.75 μs |          - |          - |         - |         - |     25.31 KB |
| D1_OneToManyRelationship         |   1,029.1 μs |      8.81 μs |      7.36 μs |     9.7656 |          - |    1.9531 |         - |     88.71 KB |
| D2_ManyToManyRelationship        |   5,821.9 μs |     53.06 μs |     47.04 μs |   140.6250 |          - |   54.6875 |         - |   1162.29 KB |
| D3_OptionalRelationship          | 557,507.8 μs | 11,079.27 μs | 23,126.56 μs | 15000.0000 |          - | 8000.0000 | 1000.0000 | 144094.98 KB |
| E1_ColumnSorting                 |   6,149.3 μs |     66.42 μs |     58.88 μs |   164.0625 |          - |   70.3125 |         - |   1353.88 KB |
| E2_Distinct                      |   2,202.0 μs |     27.76 μs |     24.60 μs |          - |          - |         - |         - |     23.72 KB |
| F1_NestedJSONQuery               |   1,440.8 μs |     27.43 μs |     25.66 μs |     9.7656 |          - |         - |         - |     84.94 KB |
| F2_JSONArrayQuery                |   1,737.6 μs |     34.41 μs |     49.35 μs |     7.8125 |          - |         - |         - |     71.34 KB |
| G1_Union                         |   1,465.8 μs |     19.33 μs |     18.08 μs |     5.8594 |          - |         - |         - |     60.07 KB |
| G2_Intersection                  |   1,476.5 μs |     20.97 μs |     17.51 μs |     5.8594 |          - |         - |         - |     61.36 KB |
