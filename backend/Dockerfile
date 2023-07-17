#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["N5.WebAPI/N5.WebAPI.csproj", "N5.WebAPI/"]
COPY ["N5.Infrastructure/N5.Infrastructure.csproj", "N5.Infrastructure/"]
COPY ["N5.Core/N5.Core.csproj", "N5.Core/"]
COPY ["N5.Shared/N5.Shared.csproj", "N5.Shared/"]
RUN dotnet restore "N5.WebAPI/N5.WebAPI.csproj"
COPY . .
WORKDIR "/src/N5.WebAPI"
RUN dotnet build "N5.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "N5.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "N5.WebAPI.dll"]