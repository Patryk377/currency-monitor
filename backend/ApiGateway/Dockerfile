FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ApiGateway.csproj ./
RUN dotnet restore ApiGateway.csproj

COPY . ./
RUN dotnet publish ApiGateway.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app ./
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
