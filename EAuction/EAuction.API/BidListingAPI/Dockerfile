#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EAuction.API/BidListingAPI/BidListingAPI.csproj", "EAuction.API/BidListingAPI/"]
COPY ["EAuction.Core/BidListing.Core/BidListing.Core.csproj", "EAuction.Core/BidListing.Core/"]
COPY ["EAuction.Infrastructure/EAuction.Persistence/EAuction.Persistence.csproj", "EAuction.Infrastructure/EAuction.Persistence/"]
COPY ["EAuction.Infrastructure/EAuction.Common/EAuction.Infrastructure.Common.csproj", "EAuction.Infrastructure/EAuction.Common/"]
COPY ["EAuction.Infrastructure/EAuction.Messaging/EAuction.Messaging.csproj", "EAuction.Infrastructure/EAuction.Messaging/"]
RUN dotnet restore "EAuction.API/BidListingAPI/BidListingAPI.csproj"
COPY . .
WORKDIR "/src/EAuction.API/BidListingAPI"
RUN dotnet build "BidListingAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BidListingAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EAuction.BidListing.API.dll"]
