#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["API.Customer.Web/API.Customer.Web.csproj", "API.Customer.Web/"]
COPY ["API.Customer.Business/API.Customer.Business.csproj", "API.Customer.Business/"]
COPY ["API.Customer.Data/API.Customer.Data.csproj", "API.Customer.Data/"]
RUN dotnet restore "API.Customer.Web/API.Customer.Web.csproj"
COPY . .
WORKDIR "/src/API.Customer.Web"
RUN dotnet build "API.Customer.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.Customer.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Customer.Web.dll"]