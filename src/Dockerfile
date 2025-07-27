FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools:$PATH"
RUN dotnet restore
RUN dotnet build -c Release

# Wykonaj migracje dla każdego contextu!
WORKDIR /src/Api
RUN dotnet ef database update --no-build --context DictionariesDbContext
RUN dotnet ef database update --no-build --context AuthorizationDbContext
RUN dotnet ef database update --no-build --context CartDbContext
RUN dotnet ef database update --no-build --context PurchaseDbContext

WORKDIR /src
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Recommendations.Api.dll"]
