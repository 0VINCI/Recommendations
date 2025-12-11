FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet nuget locals all --clear \
    && dotnet tool install --global dotnet-ef --version 8.0.8 --add-source https://api.nuget.org/v3/index.json --ignore-failed-sources
ENV PATH="$PATH:/root/.dotnet/tools:$PATH"
RUN dotnet restore
RUN dotnet build -c Release

# Wykonaj migracje dla każdego contextu!
WORKDIR /src/Api
RUN dotnet ef database update --configuration Release --no-build --context DictionariesDbContext --project /src/Dictionaries/Recommendations.Dictionaries.Infrastructure/Recommendations.Dictionaries.Infrastructure.csproj --startup-project /src/Api/Recommendations.Api/Recommendations.Api.csproj
RUN dotnet ef database update --configuration Release --no-build --context AuthorizationDbContext --project /src/Authorization/Recommendations.Authorization.Infrastructure/Recommendations.Authorization.Infrastructure.csproj --startup-project /src/Api/Recommendations.Api/Recommendations.Api.csproj
RUN dotnet ef database update --configuration Release --no-build --context CartDbContext --project /src/Cart/Recommendations.Cart.Infrastructure/Recommendations.Cart.Infrastructure.csproj --startup-project /src/Api/Recommendations.Api/Recommendations.Api.csproj
RUN dotnet ef database update --configuration Release --no-build --context PurchaseDbContext --project /src/Purchase/Recommendations.Purchase.Core/Recommendations.Purchase.Core.csproj --startup-project /src/Api/Recommendations.Api/Recommendations.Api.csproj

WORKDIR /src
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Recommendations.Api.dll"]

