```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4890/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600H with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Method                           | Mean         | Error        | StdDev       | Gen0       | Exceptions | Gen1      | Gen2      | Allocated    |
|--------------------------------- |-------------:|-------------:|-------------:|-----------:|-----------:|----------:|----------:|-------------:|
| A1_EntityIdenticalToTable        |     747.6 μs |      9.23 μs |      8.63 μs |     3.9063 |          - |         - |         - |     36.06 KB |
| A2_LimitedEntity                 |     963.9 μs |      8.79 μs |      8.22 μs |     5.8594 |          - |    1.9531 |         - |      50.7 KB |
| A3_MultipleEntitiesFromOneResult |   1,003.9 μs |     11.66 μs |      9.74 μs |     9.7656 |          - |    1.9531 |         - |     84.24 KB |
| A4_StoredProcedureToEntity       | 630,088.1 μs |  9,170.28 μs |  8,129.21 μs | 12000.0000 |          - | 7000.0000 | 2000.0000 |  91569.85 KB |
| B1_SelectionOverIndexedColumn    |     821.3 μs |     12.94 μs |     12.10 μs |     5.8594 |          - |         - |         - |     49.46 KB |
| B2_SelectionOverNonIndexedColumn |  85,266.4 μs |  1,683.68 μs |  3,323.43 μs |  3000.0000 |          - | 1666.6667 |  666.6667 |  21370.22 KB |
| B3_RangeQuery                    |  22,011.3 μs |    176.63 μs |    156.58 μs |   343.7500 |          - |  218.7500 |         - |   3048.51 KB |
| B4_InQuery                       |     956.5 μs |     16.34 μs |     14.49 μs |     9.7656 |          - |         - |         - |     87.42 KB |
| B5_TextSearch                    | 748,448.3 μs |  2,402.52 μs |  2,247.32 μs |          - |          - |         - |         - |   3472.99 KB |
| B6_PagingQuery                   |   1,486.7 μs |     13.78 μs |     12.89 μs |    15.6250 |          - |         - |         - |     128.3 KB |
| C1_AggregationCount              |  35,744.2 μs |    257.46 μs |    240.83 μs |          - |          - |         - |         - |     41.74 KB |
| C2_AggregationMax                |   1,283.6 μs |      6.86 μs |      5.73 μs |     1.9531 |          - |         - |         - |     23.06 KB |
| C3_AggregationSum                |  75,470.1 μs |    311.68 μs |    260.26 μs |          - |          - |         - |         - |     25.31 KB |
| D1_OneToManyRelationship         |   1,054.4 μs |     16.64 μs |     14.75 μs |     9.7656 |          - |    1.9531 |         - |     88.71 KB |
| D2_ManyToManyRelationship        |   5,993.3 μs |    106.94 μs |     94.80 μs |   140.6250 |          - |   54.6875 |         - |   1162.29 KB |
| D3_OptionalRelationship          | 544,512.0 μs | 10,812.50 μs | 12,451.69 μs | 14000.0000 |          - | 7000.0000 |         - | 144093.99 KB |
| E1_ColumnSorting                 |   6,260.5 μs |     57.92 μs |     51.35 μs |   164.0625 |          - |   70.3125 |         - |   1353.88 KB |
| E2_Distinct                      |   2,238.9 μs |     18.04 μs |     16.00 μs |          - |          - |         - |         - |     23.72 KB |
| F1_JSONObjectQuery               |   1,509.5 μs |     22.40 μs |     19.85 μs |     9.7656 |          - |         - |         - |     86.13 KB |
| F2_JSONArrayQuery                |   1,766.8 μs |     21.34 μs |     18.92 μs |     7.8125 |          - |         - |         - |     74.31 KB |
| G1_Union                         |   1,543.0 μs |     13.10 μs |     11.61 μs |     5.8594 |          - |         - |         - |     60.07 KB |
| G2_Intersection                  |   1,537.5 μs |     14.56 μs |     13.62 μs |     5.8594 |          - |         - |         - |     61.36 KB |
| H1_Metadata                      |     864.2 μs |     10.97 μs |     10.26 μs |     3.9063 |          - |         - |         - |     35.53 KB |
