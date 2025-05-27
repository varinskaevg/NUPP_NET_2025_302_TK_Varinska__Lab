# Стейдж 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 🔁 Важливо: правильний шлях
COPY LibraryREST/LibraryREST.csproj LibraryREST/
RUN dotnet restore LibraryREST/LibraryREST.csproj

# Копіюємо все інше
COPY . .
WORKDIR /src/LibraryREST
RUN dotnet publish -c Release -o /app/out

# Стейдж 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "LibraryREST.dll"]
