#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

# Run unit tests
WORKDIR /test

COPY ["PrimeNumber.Test/PrimeNumber.Test.csproj", "PrimeNumber.Test/"]
COPY ["PrimeNumber.Api/PrimeNumber.Api.csproj", "PrimeNumber.Api/"]
COPY ["PrimeNumber.Extensibility/PrimeNumber.Extensibility.csproj", "PrimeNumber.Extensibility/"]
COPY ["PrimeNumber.Domain/PrimeNumber.Domain.csproj", "PrimeNumber.Domain/"]
COPY ["PrimeNumber.Service/PrimeNumber.Service.csproj", "PrimeNumber.Service/"]
RUN dotnet restore "PrimeNumber.Test/PrimeNumber.Test.csproj"
COPY . .
WORKDIR "/test/PrimeNumber.Test"
RUN dotnet test --logger "console;verbosity=normal"

WORKDIR /src
COPY ["PrimeNumber.Api/PrimeNumber.Api.csproj", "PrimeNumber.Api/"]
COPY ["PrimeNumber.Test/PrimeNumber.Test.csproj", "PrimeNumber.Test/"]
COPY ["PrimeNumber.Api/PrimeNumber.Api.csproj", "PrimeNumber.Api/"]
COPY ["PrimeNumber.Extensibility/PrimeNumber.Extensibility.csproj", "PrimeNumber.Extensibility/"]
COPY ["PrimeNumber.Domain/PrimeNumber.Domain.csproj", "PrimeNumber.Domain/"]
COPY ["PrimeNumber.Service/PrimeNumber.Service.csproj", "PrimeNumber.Service/"]
RUN dotnet restore "PrimeNumber.Api/PrimeNumber.Api.csproj"
COPY . .
WORKDIR "/src/PrimeNumber.Api"
RUN dotnet build "PrimeNumber.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PrimeNumber.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrimeNumber.Api.dll"]