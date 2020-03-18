FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5208

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Reports/Reports.csproj", "Reports/"]
COPY ["Tools/Tools.csproj", "Tools/"]
COPY ["Manager.Data/Manager.Data.csproj", "Manager.Data/"]
COPY ["Manager.Core/Manager.Core.csproj", "Manager.Core/"]
COPY ["Manager.Views/Manager.Views.csproj", "Manager.Views/"]
COPY ["Manager.Services/Manager.Services.csproj", "Manager.Services/"]
RUN dotnet restore "Reports/Reports.csproj"
COPY . .
WORKDIR "/src/Reports"
RUN dotnet build "Reports.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Reports.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Reports.dll"]