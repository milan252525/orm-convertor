# Stage 1: Build the Angular frontend
FROM node:22-alpine AS angular-build
WORKDIR /frontend

# Install dependencies
COPY ORMConvertorAPI/frontend/package*.json ./
RUN npm ci

# Copy app source files and build
COPY ORMConvertorAPI/frontend .
RUN npx ng build --configuration production --base-href /orm/ --deploy-url /orm/ --output-path=dist


# Stage 2: Build and publish the .NET app, also bring in frontend static assets
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dotnet-build
WORKDIR /app

COPY . .
RUN dotnet restore ./ORMConvertorAPI/ORMConvertorAPI.csproj

# Copy compiled Angular app from previous stage into wwwroot
RUN rm -rf ORMConvertorAPI/wwwroot
COPY --from=angular-build /frontend/dist/browser/ ORMConvertorAPI/wwwroot/

WORKDIR /app/ORMConvertorAPI
RUN dotnet publish -c Release -o out --no-restore

# Stage 3: Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=dotnet-build /app/ORMConvertorAPI/out ./
EXPOSE 5072
ENV ASPNETCORE_URLS=http://+:5072
ENTRYPOINT ["dotnet", "ORMConvertorAPI.dll"]
