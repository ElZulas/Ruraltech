# Configuración de Base de Datos - RuralTech

Este archivo se actualizará conforme se proporcionen los datos y parámetros necesarios.

## Información de Supabase

### Credenciales Actuales
- **Project URL**: `https://vaeufppoexhsmrlgjnai.supabase.co`
- **Database Host**: `db.vaeufppoexhsmrlgjnai.supabase.co`
- **Database Password**: `a.h.a.v'.s.m_34`
- **Anon Key**: `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZhZXVmcHBvZXhoc21ybGdqbmFpIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NjkwMDczODYsImV4cCI6MjA4NDU4MzM4Nn0.puEYw8T7YgsgCW8kd_tzSUj7V8gqhM5QysLxWO5nZPo`
- **Service Role Key**: `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZhZXVmcHBvZXhoc21ybGdqbmFpIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTc2OTAwNzM4NiwiZXhwIjoyMDg0NTgzMzg2fQ.4nUSmyDM8zxzGgfSZu57exWXc4MyLiZ8U7HhoHpyhC8`

### Connection String
```
Host=db.vaeufppoexhsmrlgjnai.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=a.h.a.v'.s.m_34;SSL Mode=Require;Trust Server Certificate=true
```

## Estructura de Tablas Actual

### Tablas Existentes
- `Users` - Usuarios del sistema
- `Animals` - Animales registrados
- `WeightRecords` - Registros de peso
- `Vaccines` - Vacunas aplicadas
- `Treatments` - Tratamientos médicos
- `Products` - Productos del marketplace

## 1. Entidad: USUARIO_PROPIETARIO (Dueño)

### Descripción
Representa al usuario "Administrador" o dueño legal del negocio. Es la cuenta raíz que posee los datos y tiene capacidad de crear UPPs (Unidades de Producción Pecuaria).

**Método de Acceso:** Correo Electrónico/número de teléfono + Contraseña

### Estructura de Campos

| Campo | Tipo de Dato | Requerido | Descripción | Reglas de Negocio / Validaciones |
|-------|--------------|-----------|-------------|-----------------------------------|
| `id_usuario` | UUID | Sí | Identificador único universal del usuario en el sistema | Generado automáticamente (v4). PK. |
| `nombre_completo` | VARCHAR(150) | Sí | Nombre y apellidos del usuario | Mínimo 2 palabras. Longitud máx: 150 caracteres. |
| `fecha_nacimiento` | DATE | Sí | Fecha de nacimiento del usuario | Validación: El usuario debe ser mayor de 18 años para registrarse como Propietario Legal. |
| `email` | VARCHAR(100) | Sí | Correo electrónico principal | Unique. Formato válido de email (Regex). Se usa para login y recuperación. |
| `password_hash` | VARCHAR(255) | Sí | Contraseña encriptada (bcrypt/argon2) | Seguridad: Mínimo 8 caracteres, al menos 1 número y 1 símbolo. |
| `telefono` | VARCHAR(20) | No | Número celular de contacto | Formato internacional (ej. +52...). |
| `created_at` | TIMESTAMP | Sí | Fecha de creación de la cuenta | Automático (Server Timestamp). |

### Notas Importantes
- Nunca guardar texto plano para contraseñas
- El email debe ser único en el sistema
- El teléfono es opcional pero recomendado
- La fecha de nacimiento se usa para validar que el usuario sea mayor de 18 años

## 2. Entidad: UPP (Unidad de Producción Pecuaria)

### Descripción
Representa la entidad legal y física (el rancho/granja) según la normativa NOM-001-SAG/GAN-2015. Actúa como el contenedor de todos los animales y colaboradores.

### Estructura de Campos

| Campo | Tipo de Dato | Requerido | Descripción | Reglas de Negocio / Validaciones |
|-------|--------------|-----------|-------------|-----------------------------------|
| `id_upp` | UUID | Sí | Identificador único de la UPP | Generado automáticamente. PK. |
| `id_propietario` | UUID | Sí | Referencia al dueño de la cuenta | FK -> `usuario_propietario`. |
| `clave_pgn` | VARCHAR(20) | Sí | Clave del Padrón Ganadero Nacional | Unique. Formato Alfanumérico. Validación: No debe contener espacios. |
| `nombre_predio` | VARCHAR(100) | Sí | Nombre común del rancho | Ej. "Rancho La Esperanza" |
| `propietario_legal` | VARCHAR(150) | Sí | Nombre del titular ante SINIIGA | Puede ser diferente al usuario de la app (ej. nombre del padre o razón social). |
| `codigo_qr_acceso` | VARCHAR(15) | Sí | Código único para invitación de staff | Unique. Generado por sistema al crear la UPP. Ej. "RANCH-X99" |
| `estado_mx` | CHAR(2) | Sí | Clave INEGI de la Entidad Federativa | Catálogo (ej. '31' = Yucatán). |
| `coordenadas` | GEOLOCATION | No | Latitud y Longitud del casco | Formato Decimal. Usado para validación de ubicación en auditorías. |
| `created_at` | TIMESTAMP | Sí | Fecha de creación de la UPP | Automático (Server Timestamp). |
| `updated_at` | TIMESTAMP | No | Fecha de última actualización | Automático (Server Timestamp). |

### Notas Importantes
- La UPP es el contenedor principal de todos los animales
- Un propietario puede tener múltiples UPPs
- El código QR de acceso se genera automáticamente al crear la UPP
- Las coordenadas son opcionales pero recomendadas para auditorías

## 3. Entidad: COLABORADOR (Usuario Hijo / Staff)

### Descripción
Cuentas operativas gestionadas por un Propietario. Están diseñadas para trabajadores de campo (modo kiosco) y tienen acceso limitado a una UPP específica.

**Método de Acceso:** Selección de UPP + PIN Numérico

### Estructura de Campos

| Campo | Tipo de Dato | Requerido | Descripción | Reglas de Negocio / Validaciones |
|-------|--------------|-----------|-------------|-----------------------------------|
| `id_colaborador` | UUID | Sí | Identificador del trabajador | PK. |
| `id_upp` | UUID | Sí | UPP a la que pertenece | FK -> upp. El colaborador solo existe en el contexto de esta UPP. |
| `nombre_alias` | VARCHAR(50) | Sí | Nombre corto o apodo para identificarlo | Ej. "Caporal Juan" |
| `pin_acceso_hash` | VARCHAR(255) | Sí | PIN numérico encriptado | Validación UI: Debe ser numérico de 4 a 6 dígitos. |
| `telefono_contacto` | VARCHAR(20) | No | Teléfono personal del trabajador | Opcional. Útil para notificaciones de alertas sanitarias vía WhatsApp/SMS. |
| `rol` | ENUM | Sí | Nivel de permisos | Valores: ENCARGADO (Total local), OPERARIO (Solo lectura/registro básico), VETERINARIO. |
| `estatus` | ENUM | Sí | Estado del acceso | Valores: ACTIVO, SUSPENDIDO. Si es suspendido, el login con PIN falla. |
| `created_at` | TIMESTAMP | Sí | Fecha de creación | Automático (Server Timestamp). |
| `updated_at` | TIMESTAMP | No | Fecha de última actualización | Automático (Server Timestamp). |

### Notas Importantes
- El colaborador solo existe en el contexto de una UPP específica
- El PIN debe ser numérico de 4 a 6 dígitos
- El estatus SUSPENDIDO bloquea el acceso con PIN
- Los roles definen el nivel de permisos dentro de la UPP

## Nuevos Parámetros y Configuraciones

_Se agregarán aquí conforme se proporcionen..._

## Cambios Implementados en el Código

### Backend (.NET)
- ✅ Entidad `User` actualizada:
  - `Id` cambiado de `int` a `Guid` (UUID)
  - Agregado campo `DateOfBirth` (DateTime)
  - `Email` limitado a VARCHAR(100)
  - `FullName` limitado a VARCHAR(150)
  - `Phone` limitado a VARCHAR(20)
- ✅ Validaciones implementadas:
  - Edad mínima de 18 años
  - Formato de email válido (Regex)
  - Nombre completo mínimo 2 palabras
  - Contraseña: mínimo 8 caracteres, 1 número, 1 símbolo
- ✅ Login permite email o teléfono
- ✅ Entidades relacionadas (`Animal`, `Product`) actualizadas para usar `Guid` en `UserId`/`SellerId`

### Frontend (React)
- ✅ Formulario de registro actualizado:
  - Campo de fecha de nacimiento agregado
  - Validación de edad mayor de 18 años
  - Placeholder actualizado para teléfono (formato internacional)
  - Mensajes de ayuda agregados

### Base de Datos
- ✅ Script SQL de migración creado (`002_UpdateUserTable.sql`)
- ✅ Estructura de tabla actualizada según especificaciones

---

**Última actualización:** 2025-01-21 - Entidades USUARIO_PROPIETARIO, UPP y COLABORADOR agregadas e implementadas

## Cambios Implementados - UPP

### Backend (.NET)
- ✅ Entidad `UPP` creada con todos los campos especificados
- ✅ Relación con `User` (Owner) configurada
- ✅ Relación opcional con `Animal` agregada
- ✅ Validaciones implementadas:
  - ClavePGN: alfanumérico, sin espacios, único
  - EstadoMX: exactamente 2 caracteres (clave INEGI)
  - Código QR generado automáticamente y único
- ✅ Controller completo con CRUD:
  - GET /api/upps - Listar UPPs del usuario
  - GET /api/upps/{id} - Obtener UPP específica
  - POST /api/upps - Crear nueva UPP
  - PUT /api/upps/{id} - Actualizar UPP
  - DELETE /api/upps/{id} - Eliminar UPP (solo si no tiene animales)

### Base de Datos
- ✅ Script SQL de migración creado (`003_CreateUPPTable.sql`)
- ✅ Tabla UPPs con todos los campos
- ✅ Índices únicos para ClavePGN y CodigoQRAcceso
- ✅ Foreign key a Users
- ✅ Campo UPPId agregado a Animals (opcional)

## Cambios Implementados - COLABORADOR

### Backend (.NET)
- ✅ Entidad `Colaborador` creada con todos los campos especificados
- ✅ Enums creados: `RolColaborador` (ENCARGADO, OPERARIO, VETERINARIO) y `EstatusColaborador` (ACTIVO, SUSPENDIDO)
- ✅ Relación con `UPP` configurada (Cascade Delete)
- ✅ Validaciones implementadas:
  - PIN: numérico de 4 a 6 dígitos
  - Nombre alias: máximo 50 caracteres
  - Rol: validación de valores enum
  - Estatus: validación de valores enum
- ✅ Controller completo con CRUD:
  - GET /api/colaboradores/upp/{uppId} - Listar colaboradores de una UPP
  - GET /api/colaboradores/{id} - Obtener colaborador específico
  - POST /api/colaboradores - Crear nuevo colaborador
  - PUT /api/colaboradores/{id} - Actualizar colaborador
  - PATCH /api/colaboradores/{id}/estatus - Actualizar estatus (ACTIVO/SUSPENDIDO)
  - DELETE /api/colaboradores/{id} - Eliminar colaborador
- ✅ Login de colaborador implementado:
  - POST /api/auth/colaborador/login - Login con UPPId + PIN
  - Genera JWT token con claims específicos (UPPId, Rol, TipoUsuario)
  - Valida estatus ACTIVO antes de permitir login
- ✅ DTOs creados: `CreateColaboradorDto`, `ColaboradorDto`, `ColaboradorLoginDto`

### Base de Datos
- ✅ Script SQL de migración creado (`004_CreateColaboradoresTable.sql`)
- ✅ Tabla Colaboradores con todos los campos
- ✅ Tipos ENUM creados para rol y estatus
- ✅ Foreign key a UPPs (Cascade Delete)
- ✅ Índice para búsquedas por UPPId
