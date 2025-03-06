```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4890/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Method                           | Mean           | Error        | StdDev       | Exceptions | Gen0       | Gen1      | Gen2    | Allocated    |
|--------------------------------- |---------------:|-------------:|-------------:|-----------:|-----------:|----------:|--------:|-------------:|
| A1_EntityIdenticalToTable        |       808.9 μs |      8.34 μs |      6.51 μs |          - |     0.9766 |         - |       - |     14.05 KB |
| A2_LimitedEntity                 |       806.1 μs |      9.60 μs |      8.98 μs |          - |     1.9531 |         - |       - |      16.3 KB |
| A3_MultipleEntitiesFromOneResult |       829.5 μs |      7.60 μs |      7.11 μs |          - |     1.9531 |         - |       - |     25.92 KB |
| A4_StoredProcedureToEntity       |   525,861.8 μs |  9,993.75 μs | 10,262.85 μs |          - |  3000.0000 | 1000.0000 |       - |  29008.71 KB |
| B1_SelectionOverIndexedColumn    |       808.4 μs |      6.99 μs |      5.84 μs |          - |     0.9766 |         - |       - |     12.88 KB |
| B2_SelectionOverNonIndexedColumn |    45,740.2 μs |    861.48 μs |    846.09 μs |          - |   727.2727 |  363.6364 | 90.9091 |   5847.92 KB |
| B3_RangeQuery                    |    21,265.9 μs |    258.19 μs |    241.51 μs |          - |    93.7500 |   31.2500 |       - |    819.92 KB |
| B4_InQuery                       |     1,152.0 μs |     14.62 μs |     12.96 μs |          - |     1.9531 |         - |       - |     19.85 KB |
| B5_TextSearch                    |   742,988.5 μs |  3,833.47 μs |  3,201.12 μs |          - |          - |         - |       - |    979.53 KB |
| B6_PagingQuery                   |     1,418.4 μs |     23.37 μs |     20.72 μs |          - |     3.9063 |         - |       - |     40.52 KB |
| C1_AggregationCount              |    35,349.3 μs |    158.04 μs |    131.97 μs |          - |          - |         - |       - |     12.04 KB |
| C2_AggregationMax                |     1,295.7 μs |     11.21 μs |     10.49 μs |          - |          - |         - |       - |      5.69 KB |
| C3_AggregationSum                |    88,260.5 μs |    754.57 μs |    705.82 μs |          - |          - |         - |       - |      6.85 KB |
| D1_OneToManyRelationship         |       889.4 μs |      8.46 μs |      7.92 μs |          - |     1.9531 |         - |       - |     26.33 KB |
| D2_ManyToManyRelationship        |    14,587.2 μs |    201.42 μs |    178.55 μs |          - |   312.5000 |   62.5000 | 31.2500 |   2555.76 KB |
| D3_OptionalRelationship          | 2,456,257.5 μs | 12,731.94 μs | 11,909.47 μs |          - | 21000.0000 | 3000.0000 |       - | 175866.23 KB |
| E1_ColumnSorting                 |     4,984.7 μs |     73.24 μs |     68.51 μs |          - |    39.0625 |    7.8125 |       - |    348.07 KB |
| E2_Distinct                      |     2,244.2 μs |     34.23 μs |     30.34 μs |          - |          - |         - |       - |      8.54 KB |
| F1_JSONObjectQuery               |     1,569.2 μs |     19.40 μs |     18.15 μs |          - |     5.8594 |         - |       - |     51.68 KB |
| F2_JSONArrayQuery                |     1,823.0 μs |     34.14 μs |     30.27 μs |          - |          - |         - |       - |     16.86 KB |
| G1_Union                         |     1,562.8 μs |     29.17 μs |     27.28 μs |          - |     1.9531 |         - |       - |     20.35 KB |
| G2_Intersection                  |     1,579.8 μs |     17.14 μs |     15.19 μs |          - |     1.9531 |         - |       - |     21.73 KB |
| H1_Metadata                      |       830.4 μs |      6.47 μs |      6.06 μs |          - |     0.9766 |         - |       - |     10.99 KB |
