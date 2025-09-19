# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy only the project first for better layer caching
COPY MatchMakerPro/MatchMakerPro.csproj MatchMakerPro/
RUN dotnet restore MatchMakerPro/MatchMakerPro.csproj

# now copy everything and publish
COPY . .
RUN dotnet publish MatchMakerPro/MatchMakerPro.csproj -c Release -o /app /p:UseAppHost=false

# run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app ./

# .NET 8 container images default to port 8080
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "MatchMakerPro.dll"]
