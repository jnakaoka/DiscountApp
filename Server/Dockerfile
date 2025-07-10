FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia tudo do contexto de build
COPY . .

# Restaura e publica o projeto do Server
RUN dotnet restore "DiscountApp.sln"
RUN dotnet publish "Server/Server.csproj" -c Release -o /app

# Runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Server.dll"]
