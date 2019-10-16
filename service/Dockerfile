FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /src
COPY . .
RUN dotnet publish PTB.Web/PTB.Web.csproj -c Release -o /build

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS deploy
WORKDIR /app
COPY --from=build /build .
ENTRYPOINT ["dotnet", "PTB.Web.dll"]
