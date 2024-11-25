podman build -t orm-comparison .
podman run -d --name orm-comparison -p 1444:1433 orm-comparison
sqlcmd -S 127.0.0.1,1444 -U SA -P Testingorms123 -Q "SELECT * FROM [WideWorldImporters].[Purchasing].[PurchaseOrders]"
