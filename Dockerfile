# Usa la imagen del SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia los csproj y restaura dependencias
COPY ["src/Staffinity.Personal.Api/Staffinity.Personal.Api.csproj", "src/Staffinity.Personal.Api/"]
COPY ["src/Staffinity.Personal.Application/Staffinity.Personal.Application.csproj", "src/Staffinity.Personal.Application/"]
COPY ["src/Staffinity.Personal.Domain/Staffinity.Personal.Domain.csproj", "src/Staffinity.Personal.Domain/"]
COPY ["src/Staffinity.Personal.Infrastructure/Staffinity.Personal.Infrastructure.csproj", "src/Staffinity.Personal.Infrastructure/"]

RUN dotnet restore "src/Staffinity.Personal.Api/Staffinity.Personal.Api.csproj"

# Copia todo el resto y compila
COPY . .
WORKDIR "/src/src/Staffinity.Personal.Api"
RUN dotnet build -c Release -o /app/build

# Publica la app
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Imagen final ligera para correr la app
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Instalar curl para el Healthcheck de Docker (Opcional, pero recomendado)
USER root
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
USER app

ENTRYPOINT ["dotnet", "Staffinity.Personal.Api.dll"]