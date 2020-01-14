FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5209
COPY ["Mobile/Mobile.xml", "Manager/"]

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Mobile/Mobile.csproj", "Mobile/"]
COPY ["Tools/Tools.csproj", "Tools/"]
COPY ["Manager.Data/Manager.Data.csproj", "Manager.Data/"]
COPY ["Manager.Core/Manager.Core.csproj", "Manager.Core/"]
COPY ["Manager.Views/Manager.Views.csproj", "Manager.Views/"]
COPY ["Manager.Services/Manager.Services.csproj", "Manager.Services/"]
RUN dotnet restore "Mobile/Mobile.csproj"
COPY . .
WORKDIR "/src/Mobile"
RUN dotnet build "Mobile.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Mobile.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
RUN mkdir audios
ENTRYPOINT ["dotnet", "Mobile.dll"]