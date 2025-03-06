```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4890/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Method                           | Mean         | Error       | StdDev       | Exceptions | Gen0      | Gen1      | Gen2      | Allocated   |
|--------------------------------- |-------------:|------------:|-------------:|-----------:|----------:|----------:|----------:|------------:|
| A1_EntityIdenticalToTable        |     733.1 μs |     9.36 μs |      8.75 μs |          - |    0.9766 |    0.9766 |         - |     8.77 KB |
| A2_LimitedEntity                 |     738.7 μs |     6.07 μs |      5.67 μs |          - |    0.9766 |    0.9766 |         - |     7.99 KB |
| A3_MultipleEntitiesFromOneResult |     749.0 μs |    11.41 μs |     10.68 μs |          - |    0.9766 |         - |         - |    10.38 KB |
| A4_StoredProcedureToEntity       | 522,721.1 μs | 9,998.28 μs | 11,113.06 μs |          - | 1000.0000 | 1000.0000 |         - | 17007.14 KB |
| B1_SelectionOverIndexedColumn    |     761.8 μs |    14.65 μs |     13.71 μs |          - |    0.9766 |         - |         - |     9.79 KB |
| B2_SelectionOverNonIndexedColumn |  43,870.1 μs |   557.47 μs |    494.18 μs |          - |  416.6667 |  416.6667 |   83.3333 |  3410.56 KB |
| B3_RangeQuery                    |  31,348.7 μs |   182.49 μs |    170.70 μs |          - |         - |         - |         - |   480.37 KB |
| B4_InQuery                       |     870.4 μs |    13.97 μs |     13.07 μs |          - |    1.9531 |         - |         - |    16.64 KB |
| B5_TextSearch                    | 747,283.8 μs | 2,774.96 μs |  2,317.21 μs |          - |         - |         - |         - |   599.41 KB |
| B6_PagingQuery                   |   1,536.2 μs |     9.00 μs |      7.51 μs |          - |    1.9531 |    1.9531 |         - |    18.25 KB |
| C1_AggregationCount              | 368,388.0 μs | 7,281.90 μs | 17,999.04 μs |          - | 8000.0000 | 8000.0000 | 2000.0000 |  59992.3 KB |
| C2_AggregationMax                |   1,237.1 μs |    23.54 μs |     20.87 μs |          - |         - |         - |         - |     1.77 KB |
| C3_AggregationSum                |  86,452.4 μs | 1,262.58 μs |  1,181.02 μs |          - |         - |         - |         - |     1.35 KB |
| D1_OneToManyRelationship         |     808.6 μs |     6.98 μs |      6.19 μs |          - |    1.9531 |    1.9531 |         - |    20.23 KB |
| D2_ManyToManyRelationship        |   5,954.7 μs |   111.54 μs |    104.33 μs |     1.0938 |   93.7500 |   46.8750 |         - |   850.42 KB |
| D3_OptionalRelationship          | 387,596.1 μs | 7,311.00 μs |  6,481.01 μs |          - | 2000.0000 | 2000.0000 |         - | 20945.39 KB |
| E1_ColumnSorting                 |   4,868.1 μs |    39.06 μs |     36.54 μs |          - |   15.6250 |         - |         - |   162.62 KB |
| E2_Distinct                      |   2,151.0 μs |    32.27 μs |     30.18 μs |          - |         - |         - |         - |     1.66 KB |
| F1_JSONObjectQuery               |   1,461.1 μs |    15.14 μs |     14.16 μs |          - |    1.9531 |    1.9531 |         - |    31.82 KB |
| F2_JSONArrayQuery                |   1,699.4 μs |    10.19 μs |      9.53 μs |          - |         - |         - |         - |     8.25 KB |
| G1_Union                         |     724.9 μs |     5.17 μs |      4.58 μs |          - |         - |         - |         - |      1.4 KB |
| G2_Intersection                  |     730.5 μs |     8.96 μs |      8.38 μs |          - |         - |         - |         - |      1.3 KB |
| H1_Metadata                      |     839.0 μs |     8.13 μs |      7.20 μs |          - |         - |         - |         - |     1.21 KB |
