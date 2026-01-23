# RuralTech - Gestión Inteligente de Ganado

Aplicación completa para gestión de ganado desarrollada con .NET 8 y React.

## Características

- ✅ Gestión completa de animales (CRUD)
- ✅ Registro de peso y seguimiento
- ✅ Control de vacunas y tratamientos
- ✅ Sistema de alertas automáticas
- ✅ Marketplace para productos ganaderos
- ✅ Autenticación JWT
- ✅ API REST completa
- ✅ Base de datos SQLite

## Tecnologías

### Backend
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- JWT Authentication
- BCrypt para hash de contraseñas

### Frontend
- React 18
- TypeScript
- Tailwind CSS
- Axios para llamadas API

## Estructura del Proyecto

```
RuralTech App/
├── src/
│   ├── RuralTech.API/          # API REST
│   ├── RuralTech.Core/          # Entidades y DTOs
│   └── RuralTech.Infrastructure/# Data Access y Servicios
└── client/                      # Frontend React
```

## Instalación y Ejecución

### Backend

1. Navegar a la carpeta del proyecto:
```bash
cd "RuralTech App/src/RuralTech.API"
```

2. Restaurar dependencias:
```bash
dotnet restore
```

3. Ejecutar la aplicación:
```bash
dotnet run
```

La API estará disponible en `https://localhost:5001` o `http://localhost:5000`

### Frontend

1. Navegar a la carpeta del cliente:
```bash
cd client
```

2. Instalar dependencias:
```bash
npm install
```

3. Ejecutar la aplicación:
```bash
npm run dev
```

El frontend estará disponible en `http://localhost:3000`

## Endpoints de la API

### Autenticación
- `POST /api/auth/register` - Registro de usuario
- `POST /api/auth/login` - Inicio de sesión
- `GET /api/auth/me` - Obtener usuario actual (requiere autenticación)

### Animales
- `GET /api/animals` - Listar animales del usuario
- `GET /api/animals/{id}` - Obtener animal por ID
- `POST /api/animals` - Crear nuevo animal
- `PUT /api/animals/{id}` - Actualizar animal
- `DELETE /api/animals/{id}` - Eliminar animal
- `POST /api/animals/{id}/weight` - Agregar registro de peso
- `POST /api/animals/{id}/vaccines` - Agregar vacuna
- `POST /api/animals/{id}/treatments` - Agregar tratamiento

### Alertas
- `GET /api/alerts` - Obtener alertas de vacunas próximas a vencer

### Productos (Marketplace)
- `GET /api/products` - Listar productos
- `GET /api/products/{id}` - Obtener producto por ID
- `POST /api/products` - Crear producto (requiere autenticación)
- `GET /api/products/categories` - Obtener categorías disponibles

## Base de Datos

La aplicación usa SQLite por defecto. La base de datos se crea automáticamente al ejecutar la aplicación por primera vez.

## Desarrollo

### Migraciones de Base de Datos

Si necesitas crear migraciones:

```bash
dotnet ef migrations add NombreMigracion --project ../RuralTech.Infrastructure --startup-project .
```

Aplicar migraciones:

```bash
dotnet ef database update --project ../RuralTech.Infrastructure --startup-project .
```

## Licencia

Este proyecto es de código abierto y está disponible bajo la licencia MIT.
