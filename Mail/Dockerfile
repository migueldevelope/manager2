FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 5201
COPY ["Mail/Mail.xml", "Mail/"]

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ["Mail/Mail.csproj", "Mail/"]
COPY ["Tools/Tools.csproj","Tools/"]
COPY ["Manager.Data/Manager.Data.csproj", "Manager.Data/"]
COPY ["Manager.Core/Manager.Core.csproj", "Manager.Core/"]
COPY ["Manager.Views/Manager.Views.csproj", "Manager.Views/"]
COPY ["Manager.Services/Manager.Services.csproj", "Manager.Services/"]
RUN dotnet restore "Mail/Mail.csproj"
COPY . .
WORKDIR "/src/Mail"
RUN dotnet build "Mail.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Mail.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Mail.dll"]