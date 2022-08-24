#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HandsOn.API/HandsOn.API.csproj", "HandsOn.API/"]
RUN dotnet restore "HandsOn.API/HandsOn.API.csproj"
COPY . .
WORKDIR "/src/HandsOn.API"
RUN dotnet build "HandsOn.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HandsOn.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HandsOn.API.dll"]