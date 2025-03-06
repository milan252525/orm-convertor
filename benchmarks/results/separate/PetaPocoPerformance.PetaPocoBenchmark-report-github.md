```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4890/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Method                           | Mean         | Error        | StdDev      | Gen0      | Exceptions | Gen1      | Gen2     | Allocated   |
|--------------------------------- |-------------:|-------------:|------------:|----------:|-----------:|----------:|---------:|------------:|
| A1_EntityIdenticalToTable        |     718.0 μs |     13.34 μs |    12.48 μs |    0.9766 |          - |         - |        - |     7.99 KB |
| A2_LimitedEntity                 |     732.1 μs |     13.38 μs |    11.87 μs |    0.9766 |          - |         - |        - |     9.64 KB |
| A3_MultipleEntitiesFromOneResult |     734.0 μs |      8.72 μs |     8.16 μs |    0.9766 |          - |         - |        - |    13.03 KB |
| A4_StoredProcedureToEntity       | 508,202.5 μs | 10,163.77 μs | 9,982.18 μs | 1000.0000 |          - |         - |        - | 16490.15 KB |
| B1_SelectionOverIndexedColumn    |     754.5 μs |      9.91 μs |     9.27 μs |    0.9766 |          - |    0.9766 |        - |     14.3 KB |
| B2_SelectionOverNonIndexedColumn |  40,649.0 μs |    452.49 μs |   401.12 μs |  461.5385 |          - |  461.5385 | 153.8462 |  3308.13 KB |
| B3_RangeQuery                    |  30,063.5 μs |    203.78 μs |   180.65 μs |   31.2500 |          - |         - |        - |   468.38 KB |
| B4_InQuery                       |     856.7 μs |     12.13 μs |    10.76 μs |    1.9531 |          - |    1.9531 |        - |    20.34 KB |
| B5_TextSearch                    | 754,661.1 μs | 10,244.74 μs | 9,582.94 μs |         - |          - |         - |        - |   587.24 KB |
| B6_PagingQuery                   |   1,326.2 μs |     14.93 μs |    13.24 μs |    1.9531 |          - |    1.9531 |        - |    25.06 KB |
| C1_AggregationCount              |  35,530.7 μs |    343.10 μs |   320.93 μs |         - |     0.1333 |         - |        - |     8.79 KB |
| C2_AggregationMax                |   1,240.3 μs |     17.54 μs |    16.41 μs |         - |          - |         - |        - |     4.19 KB |
| C3_AggregationSum                |  86,057.9 μs |    396.42 μs |   370.81 μs |         - |          - |         - |        - |     4.41 KB |
| D1_OneToManyRelationship         |     768.4 μs |     11.53 μs |    10.79 μs |    1.9531 |          - |         - |        - |    17.94 KB |
| D2_ManyToManyRelationship        |   4,479.1 μs |     65.81 μs |    61.56 μs |   39.0625 |          - |   39.0625 |        - |   381.13 KB |
| D3_OptionalRelationship          | 174,686.3 μs |  3,456.09 μs | 6,232.05 μs | 2000.0000 |          - | 2000.0000 |        - | 24909.68 KB |
| E1_ColumnSorting                 |   4,632.8 μs |     66.33 μs |    58.80 μs |   15.6250 |          - |    7.8125 |        - |   156.96 KB |
| E2_Distinct                      |   2,192.8 μs |     33.93 μs |    31.74 μs |         - |          - |         - |        - |     4.78 KB |
| F1_JSONObjectQuery               |   1,483.4 μs |     17.47 μs |    16.34 μs |    3.9063 |          - |         - |        - |    45.25 KB |
| F2_JSONArrayQuery                |   1,723.7 μs |     20.19 μs |    16.86 μs |    1.9531 |          - |         - |        - |    18.12 KB |
| G1_Union                         |     714.1 μs |      9.80 μs |     9.16 μs |         - |          - |         - |        - |     3.45 KB |
| G2_Intersection                  |     721.8 μs |      7.15 μs |     6.69 μs |         - |          - |         - |        - |     3.25 KB |
| H1_Metadata                      |     851.0 μs |      9.02 μs |     8.44 μs |         - |          - |         - |        - |      5.1 KB |
