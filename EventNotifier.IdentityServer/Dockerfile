#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EventNotifier.IdentityServer/EventNotifier.IdentityServer.csproj", "EventNotifier.IdentityServer/"]
COPY ["EventNotifier/EventNotifier.csproj", "EventNotifier/"]
RUN dotnet restore "EventNotifier.IdentityServer/EventNotifier.IdentityServer.csproj"
COPY . .
WORKDIR "/src/EventNotifier.IdentityServer"
RUN dotnet build "EventNotifier.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventNotifier.IdentityServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventNotifier.IdentityServer.dll"]