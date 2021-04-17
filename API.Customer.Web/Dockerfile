#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["API.Customer.Web/API.Customer.Web.csproj", "API.Customer/"]
RUN dotnet restore "API.Customer.Web/API.Customer.Web.csproj"
COPY . .
WORKDIR "/src/API.Customer.Web"
RUN dotnet build "API.Customer.Web.csproj" -c Release -o /app/build

COPY ["API.Customer.Web/API.Customer.pfx" "API.Customer/"]
RUN update-ca-certificates

FROM build AS publish
RUN dotnet publish "API.Customer.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.Customer.Web.dll"]