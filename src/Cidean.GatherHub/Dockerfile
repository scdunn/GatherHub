FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY src/Cidean.GatherHub/Cidean.GatherHub.csproj src/Cidean.GatherHub/
RUN dotnet restore src/Cidean.GatherHub/Cidean.GatherHub.csproj
COPY . .
WORKDIR /src/src/Cidean.GatherHub
RUN dotnet build Cidean.GatherHub.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Cidean.GatherHub.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Cidean.GatherHub.dll"]
