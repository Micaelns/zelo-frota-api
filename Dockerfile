# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas o csproj primeiro (melhora cache)
#COPY *.sln ./
#COPY src/Api/Api.csproj src/Api/
#COPY src/Domain/Domain.csproj src/Domain/
#COPY src/Infra/Infra.csproj src/Infra/

# Copia o restante
COPY . .
RUN dotnet restore

RUN dotnet publish src/Api/Api.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Api.dll"]