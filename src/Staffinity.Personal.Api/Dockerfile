FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Staffinity.Personal.Api/Staffinity.Personal.Api.csproj", "src/Staffinity.Personal.Api/"]
COPY ["src/Staffinity.Personal.Application/Staffinity.Personal.Application.csproj", "src/Staffinity.Personal.Application/"]
COPY ["src/Staffinity.Personal.Domain/Staffinity.Personal.Domain.csproj", "src/Staffinity.Personal.Domain/"]
COPY ["src/Staffinity.Personal.Infrastructure/Staffinity.Personal.Infrastructure.csproj", "src/Staffinity.Personal.Infrastructure/"]
RUN dotnet restore "src/Staffinity.Personal.Api/Staffinity.Personal.Api.csproj"
COPY . .
WORKDIR "/src/src/Staffinity.Personal.Api"
RUN dotnet build "./Staffinity.Personal.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Staffinity.Personal.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Staffinity.Personal.Api.dll"]
