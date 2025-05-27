# Стейдж 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Копіюємо увесь код
COPY . .

# Публікуємо головний проєкт
WORKDIR /app/LibraryREST
RUN dotnet publish -c Release -o /app/out

# Стейдж 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "LibraryREST.dll"]
