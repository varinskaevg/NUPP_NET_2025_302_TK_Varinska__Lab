# Стейдж 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копіюємо csproj і відновлюємо залежності
COPY *.csproj ./
RUN dotnet restore

# Копіюємо решту проєкту і збираємо
COPY . ./
RUN dotnet publish -c Release -o out

# Стейдж 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "LibraryREST.dll"]
