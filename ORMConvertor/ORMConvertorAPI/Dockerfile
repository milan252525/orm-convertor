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

# Copy everything including Advisor C code
COPY . .

RUN dotnet restore ./ORMConvertorAPI/ORMConvertorAPI.csproj

# Copy compiled Angular app from previous stage into wwwroot
RUN rm -rf ORMConvertorAPI/wwwroot
COPY --from=angular-build /frontend/dist/browser/ ORMConvertorAPI/wwwroot/

# Build the shared C library for Advisor
RUN apt-get update && apt-get install -y build-essential libglpk-dev \
  && gcc -fPIC -shared -o /app/Advisor/libadvisor.so Advisor/ilp.c -lglpk

# Publish .NET (after .so is present)
WORKDIR /app/ORMConvertorAPI
RUN dotnet publish -c Release -o out --no-restore

# Copy the built .so into the published output dir so runtime image gets it
RUN cp /app/Advisor/libadvisor.so /app/ORMConvertorAPI/out/

# Stage 3: Create runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install glpk runtime (so .so can be loaded)
RUN apt-get update && apt-get install -y libglpk40

COPY --from=dotnet-build /app/ORMConvertorAPI/out ./
EXPOSE 5072
ENV ASPNETCORE_URLS=http://+:5072
ENTRYPOINT ["dotnet", "ORMConvertorAPI.dll"]
