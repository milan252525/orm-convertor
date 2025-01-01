```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4602/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Method                           | Mean         | Error        | StdDev       | Gen0      | Exceptions | Gen1      | Gen2      | Allocated   |
|--------------------------------- |-------------:|-------------:|-------------:|----------:|-----------:|----------:|----------:|------------:|
| A1_EntityIdenticalToTable        |     751.6 μs |      4.72 μs |      3.94 μs |    0.9766 |          - |         - |         - |     8.77 KB |
| A2_LimitedEntity                 |     742.1 μs |      4.98 μs |      4.41 μs |    0.9766 |          - |    0.9766 |         - |     7.99 KB |
| A3_MultipleEntitiesFromOneResult |     761.0 μs |      7.10 μs |      5.54 μs |    0.9766 |          - |         - |         - |    10.38 KB |
| A4_StoredProcedureToEntity       | 544,451.2 μs | 10,380.74 μs | 10,195.28 μs | 1000.0000 |          - | 1000.0000 |         - | 17007.14 KB |
| B1_SelectionOverIndexedColumn    |     770.8 μs |     12.53 μs |     11.72 μs |    0.9766 |          - |         - |         - |     9.79 KB |
| B2_SelectionOverNonIndexedColumn |  45,632.4 μs |    887.95 μs |    986.95 μs |  454.5455 |          - |  454.5455 |   90.9091 |   3410.5 KB |
| B3_RangeQuery                    |  32,171.0 μs |    338.54 μs |    300.11 μs |         - |          - |         - |         - |   480.37 KB |
| B4_InQuery                       |     888.9 μs |     16.65 μs |     17.10 μs |    1.9531 |          - |         - |         - |    16.64 KB |
| B5_TextSearch                    | 764,192.5 μs |  7,716.84 μs |  7,218.34 μs |         - |          - |         - |         - |   599.41 KB |
| B6_PagingQuery                   |   1,581.8 μs |     24.96 μs |     24.51 μs |    1.9531 |          - |         - |         - |    18.25 KB |
| C1_AggregationCount              | 399,798.3 μs |  7,931.36 μs | 19,155.12 μs | 8000.0000 |          - | 8000.0000 | 2000.0000 |  59987.7 KB |
| C2_AggregationMax                |   1,290.0 μs |     23.35 μs |     20.70 μs |         - |          - |         - |         - |     1.77 KB |
| C3_AggregationSum                |  88,679.6 μs |    813.49 μs |    679.30 μs |         - |          - |         - |         - |     1.35 KB |
| D1_OneToManyRelationship         |     813.2 μs |      8.88 μs |      7.87 μs |    1.9531 |          - |    1.9531 |         - |    20.23 KB |
| D2_ManyToManyRelationship        |   5,591.5 μs |    110.32 μs |    139.52 μs |   85.9375 |     1.0000 |   46.8750 |         - |   758.12 KB |
| D3_OptionalRelationship          | 396,543.1 μs |  7,442.79 μs |  8,272.64 μs | 2000.0000 |          - | 2000.0000 |         - | 20945.39 KB |
| E1_ColumnSorting                 |   5,132.4 μs |     94.40 μs |    104.93 μs |   15.6250 |          - |         - |         - |   162.62 KB |
| E2_Distinct                      |   2,275.9 μs |     41.89 μs |     37.13 μs |         - |          - |         - |         - |     1.66 KB |
| F1_NestedJSONQuery               |   1,557.4 μs |     29.59 μs |     24.71 μs |    5.8594 |          - |         - |         - |    52.21 KB |
| F2_JSONArrayQuery                |   1,736.0 μs |     34.21 μs |     55.25 μs |         - |          - |         - |         - |    14.87 KB |
| G1_Union                         |     701.8 μs |      9.51 μs |      8.43 μs |         - |          - |         - |         - |      1.4 KB |
| G2_Intersection                  |     704.2 μs |     10.46 μs |      9.27 μs |         - |          - |         - |         - |      1.3 KB |
