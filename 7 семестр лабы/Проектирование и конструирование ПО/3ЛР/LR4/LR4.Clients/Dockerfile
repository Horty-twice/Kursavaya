FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7232
EXPOSE 5189

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LR4.Clients/LR4.Clients.csproj", "LR4.Clients/"]
RUN dotnet restore "LR4.Clients/LR4.Clients.csproj"
COPY . .
WORKDIR "/src/LR4.Clients"
RUN dotnet build "LR4.Clients.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LR4.Clients.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://*:7232
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "LR4.Clients.dll"]