FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7283
EXPOSE 5169

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LR4/LR4.csproj", "LR4/"]
RUN dotnet restore "LR4/LR4.csproj"
COPY . .
WORKDIR "/src/LR4"
RUN dotnet build "LR4.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LR4.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://*:7283
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "LR4.dll"]