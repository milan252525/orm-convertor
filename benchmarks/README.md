# Database setup
Dockerfile is provided to initialize Microsoft SQL Server database and fill it with data. Tested using podman.

Build image:
`podman build -t orm-comparison .`

Run container: 
`podman run -d --name orm-comparison -p 1444:1433 orm-comparison`

Test connection:
`sqlcmd -S 127.0.0.1,1444 -U SA -P Testingorms123 -Q "SELECT * FROM [WideWorldImporters].[Purchasing].[PurchaseOrders]"`

# Sources
- [Wide World Importers sample databases for Microsoft SQL](https://learn.microsoft.com/en-us/sql/samples/wide-world-importers-what-is?view=sql-server-ver16)
- [BAK file with WWI database](https://github.com/microsoft/sql-server-samples/releases/download/wide-world-importers-v1.0/WideWorldImporters-Full.bak)
- [Microsoft SQL Server image on DockerHuber](https://hub.docker.com/r/microsoft/mssql-server)
