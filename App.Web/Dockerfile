FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 10000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["App.Web/App.Web.csproj", "App.Web/"]
COPY ["App.Service/App.Service.csproj", "App.Service/"]
COPY ["App.Dto/App.Dto.csproj", "App.Dto/"]
COPY ["App.Repository/App.Repository.csproj", "App.Repository/"]
COPY ["App.Entity/App.Entity.csproj", "App.Entity/"]
COPY ["App.Core/App.Core.csproj", "App.Core/"]
COPY ["App.Util/App.Util.csproj", "App.Util/"]
RUN dotnet restore "App.Web/App.Web.csproj"
COPY . .
WORKDIR "/src/App.Web"
RUN dotnet build "App.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "App.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 10000
ENTRYPOINT ["dotnet", "App.Web.dll"]
