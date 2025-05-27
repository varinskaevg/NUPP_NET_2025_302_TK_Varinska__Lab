# Стейдж 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src

# Копіюємо всі *.csproj файли проектів окремо, щоб кешувати restore
COPY LibraryREST/LibraryREST.csproj LibraryREST/
COPY Library.Infrastructure/Library.Infrastructure.csproj Library.Infrastructure/


# Відновлюємо залежності для рішення (рекомендується, якщо є *.sln)
COPY *.sln .
RUN dotnet restore LibraryREST/LibraryREST.csproj

# Копіюємо весь код
COPY . .

WORKDIR /src/LibraryREST
RUN dotnet publish -c Release -o /app/out

# Стейдж 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "LibraryREST.dll"]
