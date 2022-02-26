#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EAuction.API/BuyerAPI/BuyerAPI.csproj", "EAuction.API/BuyerAPI/"]
COPY ["EAuction.Core/EAuction.Core.Common/EAuction.Core.Common.csproj", "EAuction.Core/EAuction.Core.Common/"]
COPY ["EAuction.Core/Buyer.Core/Buyer.Core.csproj", "EAuction.Core/Buyer.Core/"]
COPY ["EAuction.Infrastructure/EAuction.Persistence/EAuction.Persistence.csproj", "EAuction.Infrastructure/EAuction.Persistence/"]
COPY ["EAuction.Infrastructure/EAuction.Common/EAuction.Infrastructure.Common.csproj", "EAuction.Infrastructure/EAuction.Common/"]
COPY ["EAuction.Infrastructure/EAuction.Messaging/EAuction.Messaging.csproj", "EAuction.Infrastructure/EAuction.Messaging/"]
RUN dotnet restore "EAuction.API/BuyerAPI/BuyerAPI.csproj"
COPY . .
WORKDIR "/src/EAuction.API/BuyerAPI"
RUN dotnet build "BuyerAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BuyerAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EAuction.Buyer.API.dll"]
