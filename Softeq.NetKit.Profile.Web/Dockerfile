FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
COPY Softeq.NetKit.Profile.Web/Softeq.NetKit.Profile.Web.csproj Softeq.NetKit.Profile.Web/
RUN dotnet restore Softeq.NetKit.Profile.Web/Softeq.NetKit.Profile.Web.csproj
COPY . .
WORKDIR /Softeq.NetKit.Profile.Web
RUN dotnet build Softeq.NetKit.Profile.Web.csproj -c Debug -o /app

FROM build AS publish
RUN dotnet publish Softeq.NetKit.Profile.Web.csproj -c Debug -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Softeq.NetKit.Profile.Web.dll"]