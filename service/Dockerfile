FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /build

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS deploy
WORKDIR /app
COPY --from=build /build .
EXPOSE 80
RUN ls /app/wwwroot
ENTRYPOINT ["dotnet", "PTB.Web.dll"]
