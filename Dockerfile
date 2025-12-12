# --- Stage 1: Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["src/Staffinity.Personal.Api/Staffinity.Personal.Api.csproj", "src/Staffinity.Personal.Api/"]
COPY ["src/Staffinity.Personal.Domain/Staffinity.Personal.Domain.csproj", "src/Staffinity.Personal.Domain/"]
COPY ["src/Staffinity.Personal.Application/Staffinity.Personal.Application.csproj", "src/Staffinity.Personal.Application/"]
COPY ["src/Staffinity.Personal.Infrastructure/Staffinity.Personal.Infrastructure.csproj", "src/Staffinity.Personal.Infrastructure/"]

RUN dotnet restore "src/Staffinity.Personal.Api/Staffinity.Personal.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/Staffinity.Personal.Api"
RUN dotnet build "Staffinity.Personal.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Staffinity.Personal.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Stage 2: Final Image Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Staffinity.Personal.Api.dll"]