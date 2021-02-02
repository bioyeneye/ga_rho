#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["GlobalAccelerex.API/GlobalAccelerex.API.csproj", "GlobalAccelerex.API/"]
RUN dotnet restore "GlobalAccelerex.API/GlobalAccelerex.API.csproj"
COPY . .
WORKDIR "/src/GlobalAccelerex.API"
RUN dotnet build "GlobalAccelerex.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GlobalAccelerex.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GlobalAccelerex.API.dll"]