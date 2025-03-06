```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4890/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Method                           | Mean         | Error       | StdDev      | Gen0      | Exceptions | Gen1      | Gen2      | Allocated   |
|--------------------------------- |-------------:|------------:|------------:|----------:|-----------:|----------:|----------:|------------:|
| A1_EntityIdenticalToTable        |     754.9 μs |    11.41 μs |    10.67 μs |         - |          - |         - |         - |     6.24 KB |
| A2_LimitedEntity                 |     724.6 μs |     8.18 μs |     7.25 μs |         - |          - |         - |         - |     4.74 KB |
| A3_MultipleEntitiesFromOneResult |     744.0 μs |     9.24 μs |     8.65 μs |         - |          - |         - |         - |     7.28 KB |
| A4_StoredProcedureToEntity       | 526,920.3 μs | 7,870.12 μs | 6,976.65 μs | 2000.0000 |          - | 1000.0000 |         - | 24294.84 KB |
| B1_SelectionOverIndexedColumn    |     745.6 μs |     8.63 μs |     8.08 μs |    0.9766 |          - |         - |         - |     8.63 KB |
| B2_SelectionOverNonIndexedColumn |  44,139.4 μs |   827.39 μs |   773.94 μs |  818.1818 |          - |  454.5455 |  181.8182 |  7112.71 KB |
| B3_RangeQuery                    |  30,485.7 μs |   224.07 μs |   209.59 μs |   93.7500 |          - |   31.2500 |         - |   989.93 KB |
| B4_InQuery                       |     859.2 μs |    12.23 μs |    11.44 μs |    1.9531 |          - |         - |         - |    16.92 KB |
| B5_TextSearch                    | 749,282.0 μs | 5,961.65 μs | 5,576.53 μs |         - |          - |         - |         - |  1169.29 KB |
| B6_PagingQuery                   |   1,317.7 μs |    13.35 μs |    12.49 μs |    3.9063 |          - |         - |         - |    32.59 KB |
| C1_AggregationCount              |  35,610.4 μs |   255.47 μs |   238.96 μs |         - |          - |         - |         - |     2.75 KB |
| C2_AggregationMax                |   1,235.3 μs |    15.96 μs |    14.93 μs |         - |          - |         - |         - |     1.97 KB |
| C3_AggregationSum                |  87,866.0 μs |   699.50 μs |   584.11 μs |         - |          - |         - |         - |     2.09 KB |
| D1_OneToManyRelationship         |     779.9 μs |     8.83 μs |     7.37 μs |    1.9531 |          - |         - |         - |    16.46 KB |
| D2_ManyToManyRelationship        |   4,633.3 μs |    90.70 μs |   107.97 μs |   46.8750 |          - |   39.0625 |         - |   433.35 KB |
| D3_OptionalRelationship          | 184,021.4 μs | 3,283.60 μs | 3,071.48 μs | 5000.0000 |          - | 2000.0000 | 1000.0000 | 40924.63 KB |
| E1_ColumnSorting                 |   4,679.9 μs |    30.40 μs |    25.39 μs |   39.0625 |          - |    7.8125 |         - |    349.4 KB |
| E2_Distinct                      |   2,206.2 μs |    43.15 μs |    42.38 μs |         - |          - |         - |         - |     2.42 KB |
| F1_JSONObjectQuery               |   1,465.5 μs |    23.99 μs |    22.44 μs |    1.9531 |          - |         - |         - |    21.71 KB |
| F2_JSONArrayQuery                |   1,706.9 μs |    29.84 μs |    26.45 μs |         - |          - |         - |         - |     6.04 KB |
| G1_Union                         |     734.5 μs |     7.35 μs |     6.14 μs |         - |          - |         - |         - |     2.39 KB |
| G2_Intersection                  |     737.3 μs |     8.95 μs |     8.37 μs |         - |          - |         - |         - |     2.17 KB |
| H1_Metadata                      |     842.1 μs |    11.64 μs |     9.72 μs |         - |          - |         - |         - |     1.98 KB |
