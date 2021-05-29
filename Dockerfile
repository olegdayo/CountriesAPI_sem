#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["CountriesAPI_sem/CountriesAPI_sem.csproj", "CountriesAPI_sem/"]
RUN dotnet restore "CountriesAPI_sem/CountriesAPI_sem.csproj"
COPY . .
WORKDIR "/src/CountriesAPI_sem"
RUN dotnet build "CountriesAPI_sem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CountriesAPI_sem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CountriesAPI_sem.dll"]