# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY ["ADC.Api/ADC.Api.csproj", "ADC.Api/"]
COPY ["ADC.Persistence/ADC.Persistence.csproj", "ADC.Persistence/"]
COPY ["ADC.Infraestructure/ADC.Infraestructure.csproj", "ADC.Infraestructure/"]
COPY ["ADC.Domain/ADC.Domain.csproj", "ADC.Domain/"]

RUN dotnet restore "ADC.Api/ADC.Api.csproj"

# Copiar el resto del código fuente
COPY . .

# Compilar el proyecto
WORKDIR "/src/ADC.Api"
RUN dotnet build "ADC.Api.csproj" -c Release -o /app/build

# Etapa de publicación
FROM build AS publish
RUN dotnet publish "ADC.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copiar archivos publicados
COPY --from=publish /app/publish .

# Configurar el punto de entrada
ENTRYPOINT ["dotnet", "ADC.Api.dll"]
