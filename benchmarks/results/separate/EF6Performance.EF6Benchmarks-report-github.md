```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4890/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Method                           | Mean           | Error        | StdDev       | Exceptions | Gen0       | Gen1       | Gen2      | Allocated    |
|--------------------------------- |---------------:|-------------:|-------------:|-----------:|-----------:|-----------:|----------:|-------------:|
| A1_EntityIdenticalToTable        |       858.7 μs |     15.23 μs |     13.50 μs |          - |     9.7656 |          - |         - |     90.43 KB |
| A2_LimitedEntity                 |       879.6 μs |     13.62 μs |     12.74 μs |          - |    13.6719 |          - |         - |    115.26 KB |
| A3_MultipleEntitiesFromOneResult |       940.7 μs |     16.04 μs |     15.01 μs |          - |    25.3906 |     1.9531 |         - |    211.62 KB |
| A4_StoredProcedureToEntity       |   504,929.5 μs |  6,388.68 μs |  5,663.39 μs |          - |  4000.0000 |  1000.0000 |         - |  35270.68 KB |
| B1_SelectionOverIndexedColumn    |       898.6 μs |     13.36 μs |     12.50 μs |          - |    11.7188 |          - |         - |    102.35 KB |
| B2_SelectionOverNonIndexedColumn |   126,396.1 μs |  2,472.15 μs |  3,700.20 μs |          - |  6000.0000 |  3000.0000 | 1000.0000 |  50018.56 KB |
| B3_RangeQuery                    |    22,911.0 μs |    439.34 μs |    451.17 μs |          - |   812.5000 |   656.2500 |         - |   6747.98 KB |
| B4_InQuery                       |     1,412.8 μs |     17.41 μs |     16.29 μs |          - |    42.9688 |     7.8125 |         - |    354.18 KB |
| B5_TextSearch                    |   742,945.5 μs |  4,246.46 μs |  3,545.99 μs |          - |          - |          - |         - |   8113.77 KB |
| B6_PagingQuery                   |     1,716.0 μs |     29.29 μs |     25.97 μs |          - |    31.2500 |     3.9063 |         - |    265.26 KB |
| C1_AggregationCount              |    35,382.9 μs |    167.41 μs |    156.60 μs |          - |          - |          - |         - |    117.97 KB |
| C2_AggregationMax                |     1,356.1 μs |     16.84 μs |     14.06 μs |          - |     7.8125 |          - |         - |     77.09 KB |
| C3_AggregationSum                |    69,709.4 μs |    354.02 μs |    331.15 μs |          - |          - |          - |         - |     81.72 KB |
| D1_OneToManyRelationship         |     1,566.9 μs |     23.81 μs |     22.28 μs |          - |    11.7188 |          - |         - |    114.97 KB |
| D2_ManyToManyRelationship        |     6,834.0 μs |     49.83 μs |     38.90 μs |          - |   156.2500 |    31.2500 |         - |   1396.08 KB |
| D3_OptionalRelationship          | 2,103,385.1 μs | 19,431.86 μs | 18,176.57 μs |          - | 33000.0000 | 17000.0000 | 2000.0000 | 278562.74 KB |
| E1_ColumnSorting                 |     6,339.6 μs |     91.16 μs |     85.27 μs |          - |   234.3750 |   156.2500 |         - |   2000.95 KB |
| E2_Distinct                      |     2,336.5 μs |     41.15 μs |     38.49 μs |          - |     7.8125 |          - |         - |     76.19 KB |
| F1_JSONObjectQuery               |     1,490.3 μs |     21.05 μs |     19.69 μs |          - |     7.8125 |          - |         - |     73.38 KB |
| F2_JSONArrayQuery                |     1,772.7 μs |     34.08 μs |     39.25 μs |          - |     5.8594 |          - |         - |     57.24 KB |
| G1_Union                         |     1,628.2 μs |     11.72 μs |     10.39 μs |          - |    15.6250 |          - |         - |     136.1 KB |
| G2_Intersection                  |     1,633.6 μs |     14.25 μs |     12.63 μs |          - |    15.6250 |          - |         - |    135.98 KB |
| H1_Metadata                      |       906.3 μs |      7.93 μs |      7.03 μs |          - |     3.9063 |          - |         - |     46.52 KB |
