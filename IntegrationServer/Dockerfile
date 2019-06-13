FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 5203

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ["IntegrationServer/IntegrationServer.csproj", "IntegrationServer/"]
COPY ["Tools/Tools.csproj", "Tools/"]
COPY ["Manager.Data/Manager.Data.csproj", "Manager.Data/"]
COPY ["Manager.Core/Manager.Core.csproj", "Manager.Core/"]
COPY ["Manager.Views/Manager.Views.csproj", "Manager.Views/"]
COPY ["Manager.Services/Manager.Services.csproj", "Manager.Services/"]
RUN dotnet restore "IntegrationServer/IntegrationServer.csproj"
COPY . .
WORKDIR "/src/IntegrationServer"
RUN dotnet build "IntegrationServer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "IntegrationServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "IntegrationServer.dll"]