FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5209
COPY ["Mobile/Mobile.xml", "Manager/"]

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
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
RUN apt-get update
#RUN apt-get -y install ffmpeg
ENTRYPOINT ["dotnet", "Mobile.dll"]