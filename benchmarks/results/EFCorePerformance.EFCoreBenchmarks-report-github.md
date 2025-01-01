```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4602/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Method                           | Mean           | Error        | StdDev       | Gen0       | Exceptions | Gen1      | Gen2    | Allocated    |
|--------------------------------- |---------------:|-------------:|-------------:|-----------:|-----------:|----------:|--------:|-------------:|
| A1_EntityIdenticalToTable        |       785.7 μs |      6.71 μs |      5.95 μs |     0.9766 |          - |         - |       - |     14.05 KB |
| A2_LimitedEntity                 |       778.0 μs |      9.84 μs |      9.21 μs |     1.9531 |          - |         - |       - |     16.26 KB |
| A3_MultipleEntitiesFromOneResult |       807.0 μs |      8.75 μs |      7.76 μs |     1.9531 |          - |         - |       - |     25.85 KB |
| A4_StoredProcedureToEntity       |   527,061.5 μs |  8,933.88 μs |  8,356.76 μs |  3000.0000 |          - | 1000.0000 |       - |  29008.69 KB |
| B1_SelectionOverIndexedColumn    |       795.4 μs |      9.23 μs |      8.64 μs |     0.9766 |          - |         - |       - |     12.88 KB |
| B2_SelectionOverNonIndexedColumn |    47,444.4 μs |    940.31 μs |  1,154.78 μs |   727.2727 |          - |  363.6364 | 90.9091 |   5847.62 KB |
| B3_RangeQuery                    |    21,694.5 μs |    311.56 μs |    276.19 μs |    93.7500 |          - |   31.2500 |       - |    819.92 KB |
| B4_InQuery                       |     1,123.5 μs |     11.63 μs |      9.71 μs |     1.9531 |          - |         - |       - |     19.85 KB |
| B5_TextSearch                    |   743,444.4 μs |  2,830.53 μs |  2,209.90 μs |          - |          - |         - |       - |    979.42 KB |
| B6_PagingQuery                   |     1,384.5 μs |     20.01 μs |     17.74 μs |     3.9063 |          - |         - |       - |     40.52 KB |
| C1_AggregationCount              |    35,123.7 μs |    183.94 μs |    153.60 μs |          - |          - |         - |       - |     12.04 KB |
| C2_AggregationMax                |     1,281.8 μs |     13.71 μs |     12.16 μs |          - |          - |         - |       - |      5.69 KB |
| C3_AggregationSum                |    89,713.6 μs |  1,676.76 μs |  1,794.11 μs |          - |          - |         - |       - |      6.85 KB |
| D1_OneToManyRelationship         |       875.8 μs |      8.80 μs |      7.81 μs |     1.9531 |          - |         - |       - |     26.33 KB |
| D2_ManyToManyRelationship        |    14,536.9 μs |    136.58 μs |    121.07 μs |   312.5000 |          - |   62.5000 | 31.2500 |   2555.72 KB |
| D3_OptionalRelationship          | 2,474,371.3 μs | 28,106.95 μs | 24,916.08 μs | 21000.0000 |          - | 3000.0000 |       - | 175866.23 KB |
| E1_ColumnSorting                 |     4,907.5 μs |     97.19 μs |     86.16 μs |    39.0625 |          - |    7.8125 |       - |    348.07 KB |
| E2_Distinct                      |     2,295.3 μs |     44.82 μs |     51.61 μs |          - |          - |         - |       - |      8.54 KB |
| F1_NestedJSONQuery               |     1,752.7 μs |     34.07 μs |     92.68 μs |     5.8594 |          - |         - |       - |     51.68 KB |
| F2_JSONArrayQuery                |     1,800.5 μs |     35.72 μs |     39.71 μs |          - |          - |         - |       - |     16.86 KB |
| G1_Union                         |     1,537.0 μs |     18.14 μs |     16.97 μs |     1.9531 |          - |         - |       - |     20.35 KB |
| G2_Intersection                  |     1,534.3 μs |     25.05 μs |     23.43 μs |     1.9531 |          - |         - |       - |     21.73 KB |
