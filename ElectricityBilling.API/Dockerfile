# -------------------------------------------------
# BASE / RUNTIME
# -------------------------------------------------

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

RUN apt-get update \
    && apt-get install -y --no-install-recommends ca-certificates \
    && update-ca-certificates \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

EXPOSE 8080


# -------------------------------------------------
# BUILD
# -------------------------------------------------

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY ["ElectricityBilling.API/ElectricityBilling.API.csproj", "ElectricityBilling.API/"]
COPY ["ElectricityBilling.Application/ElectricityBilling.Application.csproj", "ElectricityBilling.Application/"]
COPY ["ElectricityBilling.Domain/ElectricityBilling.Domain.csproj", "ElectricityBilling.Domain/"]
COPY ["ElectricityBilling.Infrastructure/ElectricityBilling.Infrastructure.csproj", "ElectricityBilling.Infrastructure/"]

RUN dotnet restore "ElectricityBilling.API/ElectricityBilling.API.csproj"

COPY . .

WORKDIR "/src/ElectricityBilling.API"

RUN dotnet publish "ElectricityBilling.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


# -------------------------------------------------
# FINAL
# -------------------------------------------------

FROM base AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ElectricityBilling.API.dll"]