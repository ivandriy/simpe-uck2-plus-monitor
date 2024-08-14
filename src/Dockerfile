FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN apt-get update 
RUN apt-get --yes install curl
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SimpleUCK2PlusMonitor/SimpleUCK2PlusMonitor.WebApi/SimpleUCK2PlusMonitor.WebApi/SimpleUCK2PlusMonitor.WebApi.csproj", "SimpleUCK2PlusMonitor/SimpleUCK2PlusMonitor.WebApi/SimpleUCK2PlusMonitor.WebApi/"]
RUN dotnet restore "SimpleUCK2PlusMonitor/SimpleUCK2PlusMonitor.WebApi/SimpleUCK2PlusMonitor.WebApi/SimpleUCK2PlusMonitor.WebApi.csproj"
COPY . .
WORKDIR "/src/SimpleUCK2PlusMonitor/SimpleUCK2PlusMonitor.WebApi/SimpleUCK2PlusMonitor.WebApi"
RUN dotnet build "SimpleUCK2PlusMonitor.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SimpleUCK2PlusMonitor.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleUCK2PlusMonitor.WebApi.dll"]
