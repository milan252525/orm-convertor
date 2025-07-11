Angular frontend is compiled and deployed to the ASP.NET Core application.

# Build command
`ng build --configuration production --base-href /orm/ --deploy-url /orm/ && rmdir /s /q "..\wwwroot" && mkdir "..\wwwroot" && xcopy /s /e /y "dist\browser" "..\wwwroot\"`
