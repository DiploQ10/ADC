# ADC - Instrucciones para Docker

## Requisitos Previos
- Docker Desktop instalado y ejecutándose
- Docker Compose instalado (incluido con Docker Desktop)

## Estructura de Contenedores

La aplicación está contenerizada con los siguientes servicios:

### 1. PostgreSQL (postgres)
- **Imagen**: postgres:16-alpine
- **Puerto**: 5432
- **Base de datos**: adc
- **Usuario**: alex
- **Contraseña**: 1234
- **Volumen persistente**: postgres_data

### 2. API (.NET 9)
- **Puerto**: 5000 (http://localhost:5000)
- **Ambiente**: Development
- **Swagger UI**: http://localhost:5000/swagger

## Comandos de Docker

### Construir e iniciar los contenedores
```bash
docker-compose up -d --build
```

### Ver los logs de los contenedores
```bash
# Ver todos los logs
docker-compose logs -f

# Ver logs solo de la API
docker-compose logs -f api

# Ver logs solo de PostgreSQL
docker-compose logs -f postgres
```

### Detener los contenedores
```bash
docker-compose down
```

### Detener y eliminar volúmenes (datos de la BD)
```bash
docker-compose down -v
```

### Ver el estado de los contenedores
```bash
docker-compose ps
```

### Reconstruir solo la API
```bash
docker-compose up -d --build api
```

### Acceder al contenedor de la API
```bash
docker exec -it adc-api bash
```

### Acceder al contenedor de PostgreSQL
```bash
docker exec -it adc-postgres psql -U alex -d adc
```

## Endpoints de la API

Una vez que los contenedores estén ejecutándose:
- API: http://localhost:5000
- Swagger UI: http://localhost:5000/swagger
- PostgreSQL: localhost:5432

## Configuración del Ambiente

El archivo `docker-compose.yml` está configurado para el ambiente de **Development**. 

La cadena de conexión a la base de datos se configura automáticamente para conectarse al contenedor de PostgreSQL.

## Solución de Problemas

### El contenedor de la API no inicia
```bash
# Ver los logs detallados
docker-compose logs api
```

### La base de datos no responde
```bash
# Verificar el estado de salud
docker-compose ps

# Reiniciar el contenedor de PostgreSQL
docker-compose restart postgres
```

### Limpiar todo y empezar de nuevo
```bash
# Detener y eliminar todo
docker-compose down -v --remove-orphans

# Limpiar imágenes no utilizadas
docker system prune -a

# Reconstruir desde cero
docker-compose up -d --build
```

## Notas Importantes

1. La API espera a que PostgreSQL esté completamente listo antes de iniciar (healthcheck configurado)
2. Los datos de PostgreSQL persisten en un volumen Docker llamado `postgres_data`
3. El puerto 5000 en tu máquina local se mapea al puerto 8080 del contenedor
4. El ambiente está configurado como Development por defecto
