FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5206
COPY ["Indicators/Indicators.xml", "Indicators/"]

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Indicators/Indicators.csproj", "Indicators/"]
COPY ["Tools/Tools.csproj","Tools/"]
COPY ["Manager.Data/Manager.Data.csproj", "Manager.Data/"]
COPY ["Manager.Core/Manager.Core.csproj", "Manager.Core/"]
COPY ["Manager.Views/Manager.Views.csproj", "Manager.Views/"]
COPY ["Manager.Services/Manager.Services.csproj", "Manager.Services/"]
RUN dotnet restore "Indicators/Indicators.csproj"
COPY . .
WORKDIR "/src/Indicators"
RUN dotnet build "Indicators.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Indicators.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Indicators.dll"]
