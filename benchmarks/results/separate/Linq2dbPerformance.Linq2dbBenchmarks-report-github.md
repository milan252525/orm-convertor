```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4890/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Method                           | Mean         | Error        | StdDev      | Gen0      | Exceptions | Gen1     | Gen2     | Allocated   |
|--------------------------------- |-------------:|-------------:|------------:|----------:|-----------:|---------:|---------:|------------:|
| A1_EntityIdenticalToTable        |     837.3 μs |      9.54 μs |     8.93 μs |    0.9766 |          - |        - |        - |    12.74 KB |
| A2_LimitedEntity                 |     856.3 μs |     10.19 μs |     9.04 μs |    1.9531 |          - |        - |        - |    18.25 KB |
| A3_MultipleEntitiesFromOneResult |     898.4 μs |     10.98 μs |     9.73 μs |    1.9531 |          - |        - |        - |    27.63 KB |
| A4_StoredProcedureToEntity       | 523,333.8 μs | 10,090.95 μs | 9,910.66 μs | 1000.0000 |          - |        - |        - | 16490.93 KB |
| B1_SelectionOverIndexedColumn    |     875.1 μs |     12.81 μs |    11.35 μs |         - |          - |        - |        - |    14.63 KB |
| B2_SelectionOverNonIndexedColumn |  47,604.1 μs |    926.60 μs |   910.04 μs |  454.5455 |          - | 363.6364 |  90.9091 |  3314.49 KB |
| B3_RangeQuery                    |  21,329.9 μs |    154.34 μs |   144.37 μs |   31.2500 |          - |        - |        - |   475.55 KB |
| B4_InQuery                       |     950.1 μs |     11.26 μs |    10.53 μs |    1.9531 |          - |        - |        - |    19.58 KB |
| B5_TextSearch                    | 754,156.6 μs |  7,481.48 μs | 6,998.18 μs |         - |          - |        - |        - |   592.66 KB |
| B6_PagingQuery                   |   1,442.8 μs |     17.95 μs |    15.92 μs |    1.9531 |          - |        - |        - |    26.58 KB |
| C1_AggregationCount              |  35,456.7 μs |    314.90 μs |   294.56 μs |         - |          - |        - |        - |    12.31 KB |
| C2_AggregationMax                |   1,380.9 μs |     26.35 μs |    27.06 μs |         - |          - |        - |        - |     8.12 KB |
| C3_AggregationSum                |  87,183.4 μs |  1,009.64 μs |   944.42 μs |         - |          - |        - |        - |     9.25 KB |
| D1_OneToManyRelationship         |   3,006.6 μs |     34.17 μs |    31.96 μs |         - |          - |        - |        - |    24.58 KB |
| D2_ManyToManyRelationship        |   9,355.6 μs |    184.42 μs |   204.99 μs |   31.2500 |          - |  15.6250 |        - |   352.24 KB |
| D3_OptionalRelationship          |  91,791.0 μs |  1,330.01 μs | 1,244.09 μs | 1000.0000 |          - | 500.0000 | 166.6667 |  9083.71 KB |
| E1_ColumnSorting                 |   4,842.0 μs |     52.57 μs |    49.17 μs |   15.6250 |          - |        - |        - |   162.54 KB |
| E2_Distinct                      |   2,297.3 μs |     22.06 μs |    19.55 μs |         - |          - |        - |        - |     9.34 KB |
| F1_JSONObjectQuery               |   1,563.6 μs |     14.69 μs |    13.03 μs |    1.9531 |          - |        - |        - |    27.09 KB |
| F2_JSONArrayQuery                |   1,804.8 μs |     35.58 μs |    34.95 μs |         - |          - |        - |        - |     11.3 KB |
| G1_Union                         |   1,588.2 μs |     16.64 μs |    15.56 μs |    1.9531 |          - |        - |        - |    18.03 KB |
| G2_Intersection                  |   1,596.4 μs |     25.49 μs |    23.85 μs |    1.9531 |          - |        - |        - |    17.91 KB |
| H1_Metadata                      |     935.5 μs |     12.26 μs |    10.86 μs |         - |          - |        - |        - |     5.61 KB |
