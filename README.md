[![ORMConvertor tests](https://github.com/milan252525/orm-comparison/actions/workflows/ormconvertor-tests.yml/badge.svg)](https://github.com/milan252525/orm-comparison/actions/workflows/ormconvertor-tests.yml)

# Thesis topic
**Framework-Agnostic Query Adaptation: Ensuring SQL Compatibility Across .NET Database Frameworks**

[Master thesis](https://is.cuni.cz/studium/dipl_st/index.php?id=&tid=&do=main&doo=detail&did=277574) of Milan Abrahám 

There are numerous .NET frameworks available for accessing database systems, each with its own unique features and methods for executing SQL queries. These frameworks differ in their support for database functionalities, and when an application is migrated from one .NET framework to another, query compatibility issues may arise. For example, certain features or SQL constructs supported by one framework may not be available in another, leading to difficulties in query execution and application performance.

The goal of this thesis is to perform a static and experimental comparison of selected .NET frameworks used for database access, focusing on their query execution capabilities and feature support. The student will analyze differences in how these frameworks handle SQL queries and identify potential compatibility issues when switching between frameworks. Based on the findings, the student will design a solution to adapt queries when migrating between frameworks, ensuring smooth transitions even when feature support varies across frameworks.

# ORMs
The compared ORMs are:
- [Dapper](https://github.com/DapperLib/Dapper)
- [PetaPoco](https://github.com/CollaboratingPlatypus/PetaPoco)
- [RepoDB](https://github.com/mikependon/RepoDB)
- [linq2db](https://github.com/linq2db/linq2db)
- [NHibernate](https://github.com/nhibernate)
- [Entity Framework Core](https://github.com/dotnet/efcore)
- [Entity Framework 6](https://github.com/dotnet/ef6)


# Repository structure
The repository is structured into several directories. The `thesis` folder contains the LaTeX source files for the thesis. The `diagrams` directory holds diagrams created using [draw.io](https://www.drawio.com/). Experimental comparisons, including unit tests and benchmarks, are located in the `benchmarks` directory. The `ORMConvertor` folder includes a prototype tool for translating between different .NET ORM frameworks. Finally, `notes` contains thesis-related notes written in Czech.

# ORMConvertor
The translation tool is currently hosted at [http://116.203.208.55/orm/](http://116.203.208.55/orm/).
