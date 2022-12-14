#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
USER api_user
RUN mkdir https
RUN sudo chmod 777 /https
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["HouseFinder_API/HouseFinder_API.csproj", "HouseFinder_API/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["BusinessObjects/BusinessObjects.csproj", "BusinessObjects/"]
RUN dotnet restore "HouseFinder_API/HouseFinder_API.csproj"
COPY . .
WORKDIR "/src/HouseFinder_API"
RUN dotnet build "HouseFinder_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HouseFinder_API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "HouseFinder_API.dll"]
