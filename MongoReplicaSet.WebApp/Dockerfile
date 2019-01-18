FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY MongoReplicaSet.WebApp/MongoReplicaSet.WebApp.csproj MongoReplicaSet.WebApp/
RUN dotnet restore MongoReplicaSet.WebApp/MongoReplicaSet.WebApp.csproj
COPY . .
WORKDIR /src/MongoReplicaSet.WebApp
RUN dotnet build MongoReplicaSet.WebApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish MongoReplicaSet.WebApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MongoReplicaSet.WebApp.dll"]
