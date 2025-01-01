```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4602/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Method                           | Mean           | Error        | StdDev       | Gen0       | Exceptions | Gen1       | Gen2      | Allocated    |
|--------------------------------- |---------------:|-------------:|-------------:|-----------:|-----------:|-----------:|----------:|-------------:|
| A1_EntityIdenticalToTable        |       838.3 μs |     14.96 μs |     17.22 μs |     9.7656 |          - |          - |         - |     90.43 KB |
| A2_LimitedEntity                 |       855.7 μs |     16.29 μs |     16.00 μs |    13.6719 |          - |          - |         - |     115.3 KB |
| A3_MultipleEntitiesFromOneResult |       923.4 μs |      8.64 μs |      8.08 μs |    25.3906 |          - |     1.9531 |         - |    211.65 KB |
| A4_StoredProcedureToEntity       |   508,269.8 μs | 10,096.02 μs | 10,367.87 μs |  4000.0000 |          - |  1000.0000 |         - |  35270.68 KB |
| B1_SelectionOverIndexedColumn    |       876.3 μs |     17.48 μs |     22.11 μs |    11.7188 |          - |          - |         - |    102.35 KB |
| B2_SelectionOverNonIndexedColumn |   148,236.5 μs |  2,879.38 μs |  3,427.70 μs |  7000.0000 |          - |  4000.0000 | 1250.0000 |  50013.77 KB |
| B3_RangeQuery                    |    22,851.1 μs |    437.40 μs |    409.14 μs |   812.5000 |          - |   656.2500 |         - |   6747.98 KB |
| B4_InQuery                       |     1,396.5 μs |     26.58 μs |     24.86 μs |    42.9688 |          - |     7.8125 |         - |    354.18 KB |
| B5_TextSearch                    |   748,245.0 μs |  6,234.43 μs |  5,831.69 μs |          - |          - |          - |         - |   8113.77 KB |
| B6_PagingQuery                   |     1,677.3 μs |     21.72 μs |     19.26 μs |    31.2500 |          - |     3.9063 |         - |    265.26 KB |
| C1_AggregationCount              |    35,187.7 μs |    295.58 μs |    262.02 μs |          - |          - |          - |         - |    118.12 KB |
| C2_AggregationMax                |     1,350.3 μs |     26.61 μs |     26.14 μs |     7.8125 |          - |          - |         - |     77.09 KB |
| C3_AggregationSum                |    70,859.5 μs |    801.93 μs |    750.13 μs |          - |          - |          - |         - |     81.72 KB |
| D1_OneToManyRelationship         |     1,545.6 μs |     21.18 μs |     19.81 μs |    11.7188 |          - |          - |         - |    114.97 KB |
| D2_ManyToManyRelationship        |     6,828.5 μs |    111.59 μs |     98.92 μs |   156.2500 |          - |    31.2500 |         - |   1396.06 KB |
| D3_OptionalRelationship          | 2,056,087.6 μs | 20,519.90 μs | 18,190.35 μs | 33000.0000 |          - | 17000.0000 | 2000.0000 | 278561.76 KB |
| E1_ColumnSorting                 |     6,118.8 μs |     60.35 μs |     56.46 μs |   234.3750 |          - |   156.2500 |         - |   2000.95 KB |
| E2_Distinct                      |     2,291.7 μs |     23.96 μs |     20.01 μs |     7.8125 |          - |          - |         - |     76.47 KB |
| F1_NestedJSONQuery               |     1,508.3 μs |     15.49 μs |     13.73 μs |     9.7656 |          - |          - |         - |     94.18 KB |
| F2_JSONArrayQuery                |     1,770.7 μs |     33.72 μs |     29.89 μs |     7.8125 |          - |          - |         - |     64.25 KB |
| G1_Union                         |     1,594.3 μs |     25.61 μs |     23.95 μs |    15.6250 |          - |          - |         - |     136.1 KB |
| G2_Intersection                  |     1,588.6 μs |     29.30 μs |     25.97 μs |    15.6250 |          - |          - |         - |    135.98 KB |
