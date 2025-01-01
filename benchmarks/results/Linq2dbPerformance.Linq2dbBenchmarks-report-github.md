```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4602/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Method                           | Mean         | Error        | StdDev       | Gen0      | Exceptions | Gen1     | Gen2     | Allocated   |
|--------------------------------- |-------------:|-------------:|-------------:|----------:|-----------:|---------:|---------:|------------:|
| A1_EntityIdenticalToTable        |     854.7 μs |     11.91 μs |     11.14 μs |         - |          - |        - |        - |    12.74 KB |
| A2_LimitedEntity                 |     870.9 μs |     16.23 μs |     14.39 μs |    1.9531 |          - |        - |        - |    18.52 KB |
| A3_MultipleEntitiesFromOneResult |     923.1 μs |     15.31 μs |     14.32 μs |    1.9531 |          - |        - |        - |    27.74 KB |
| A4_StoredProcedureToEntity       | 560,683.3 μs | 11,175.76 μs | 17,725.95 μs | 1000.0000 |          - |        - |        - | 16490.93 KB |
| B1_SelectionOverIndexedColumn    |     936.2 μs |     17.75 μs |     18.22 μs |         - |          - |        - |        - |    14.63 KB |
| B2_SelectionOverNonIndexedColumn |  46,567.2 μs |    835.13 μs |    697.37 μs |  400.0000 |          - | 300.0000 | 100.0000 |   3314.9 KB |
| B3_RangeQuery                    |  22,188.3 μs |    325.67 μs |    288.69 μs |   31.2500 |          - |        - |        - |   475.55 KB |
| B4_InQuery                       |     964.0 μs |     10.80 μs |     10.10 μs |    1.9531 |          - |        - |        - |    19.58 KB |
| B5_TextSearch                    | 766,619.3 μs | 12,312.03 μs | 11,516.68 μs |         - |          - |        - |        - |   592.66 KB |
| B6_PagingQuery                   |   1,505.2 μs |     30.07 μs |     26.65 μs |    1.9531 |          - |        - |        - |    26.58 KB |
| C1_AggregationCount              |  36,570.6 μs |    589.82 μs |    551.72 μs |         - |          - |        - |        - |    12.32 KB |
| C2_AggregationMax                |   1,452.4 μs |     28.54 μs |     26.70 μs |         - |          - |        - |        - |     8.12 KB |
| C3_AggregationSum                |  89,889.1 μs |  1,145.26 μs |  1,071.28 μs |         - |          - |        - |        - |     9.25 KB |
| D1_OneToManyRelationship         |   3,083.2 μs |     46.82 μs |     39.10 μs |         - |          - |        - |        - |    24.58 KB |
| D2_ManyToManyRelationship        |   9,850.1 μs |    195.79 μs |    183.14 μs |   31.2500 |          - |  15.6250 |        - |      352 KB |
| D3_OptionalRelationship          |  98,033.1 μs |  1,762.71 μs |  1,562.59 μs | 1000.0000 |          - | 600.0000 | 200.0000 |  9083.86 KB |
| E1_ColumnSorting                 |   5,333.6 μs |    106.03 μs |    148.64 μs |   15.6250 |          - |        - |        - |   162.54 KB |
| E2_Distinct                      |   2,461.6 μs |     47.95 μs |     44.86 μs |         - |          - |        - |        - |     9.34 KB |
| F1_NestedJSONQuery               |   1,644.1 μs |     16.65 μs |     13.91 μs |    3.9063 |          - |        - |        - |    39.88 KB |
| F2_JSONArrayQuery                |   1,933.3 μs |     37.99 μs |     56.86 μs |         - |          - |        - |        - |     16.8 KB |
| G1_Union                         |   1,607.0 μs |     19.68 μs |     16.44 μs |    1.9531 |          - |        - |        - |    18.03 KB |
| G2_Intersection                  |   1,621.3 μs |     17.22 μs |     15.27 μs |    1.9531 |          - |        - |        - |    17.91 KB |
