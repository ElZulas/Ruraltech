# Guía Completa de Configuración de Supabase

## Paso 1: Crear Proyecto en Supabase

1. Ve a [https://supabase.com](https://supabase.com)
2. Crea una cuenta o inicia sesión
3. Haz clic en "New Project"
4. Completa el formulario:
   - **Name**: RuralTech
   - **Database Password**: Crea una contraseña segura (guárdala bien)
   - **Region**: Elige la más cercana (ej: South America)
   - **Pricing Plan**: Free tier está bien para empezar
5. Espera a que se cree el proyecto (2-3 minutos)

## Paso 2: Obtener Credenciales

Una vez creado el proyecto:

1. Ve a **Settings** (⚙️) > **API**
2. Anota estos valores:
   - **Project URL**: `https://vaeufppoexhsmrlgjnai.supabase.co`
   - **anon public key**: `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZhZXVmcHBvZXhoc21ybGdqbmFpIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NjkwMDczODYsImV4cCI6MjA4NDU4MzM4Nn0.puEYw8T7YgsgCW8kd_tzSUj7V8gqhM5QysLxWO5nZPo`
   - **service_role key**: `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZhZXVmcHBvZXhoc21ybGdqbmFpIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTc2OTAwNzM4NiwiZXhwIjoyMDg0NTgzMzg2fQ.4nUSmyDM8zxzGgfSZu57exWXc4MyLiZ8U7HhoHpyhC8` (⚠️ Mantén esto secreto)

3. Ve a **Settings** > **Database**
4. En "Connection string" selecciona **URI**
5. Copia la cadena de conexión (formato: `postgresql://postgres:a.h.a.v'.s.m_34@db.vaeufppoexhsmrlgjnai.supabase.co:5432/postgres`)

## Paso 3: Crear las Tablas en Supabase

1. Ve a **SQL Editor** en el menú lateral
2. Haz clic en **New Query**
3. Copia y pega el siguiente SQL:

```sql
-- ============================================
-- RURALTECH DATABASE SCHEMA
-- ============================================

-- Tabla Users
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Email" TEXT NOT NULL UNIQUE,
    "PasswordHash" TEXT NOT NULL,
    "FullName" TEXT NOT NULL,
    "Phone" TEXT,
    "Location" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- Tabla Animals
CREATE TABLE IF NOT EXISTS "Animals" (
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Breed" TEXT,
    "BirthDate" DATE,
    "Sex" TEXT,
    "CurrentWeight" DECIMAL(10,2),
    "LastVaccineDate" DATE,
    "UserId" INTEGER NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Animals_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
);

-- Tabla WeightRecords
CREATE TABLE IF NOT EXISTS "WeightRecords" (
    "Id" SERIAL PRIMARY KEY,
    "AnimalId" INTEGER NOT NULL,
    "Weight" DECIMAL(10,2) NOT NULL,
    "Date" DATE NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_WeightRecords_Animals_AnimalId" FOREIGN KEY ("AnimalId") REFERENCES "Animals"("Id") ON DELETE CASCADE
);

-- Tabla Vaccines
CREATE TABLE IF NOT EXISTS "Vaccines" (
    "Id" SERIAL PRIMARY KEY,
    "AnimalId" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "DateApplied" DATE NOT NULL,
    "NextDueDate" DATE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Vaccines_Animals_AnimalId" FOREIGN KEY ("AnimalId") REFERENCES "Animals"("Id") ON DELETE CASCADE
);

-- Tabla Treatments
CREATE TABLE IF NOT EXISTS "Treatments" (
    "Id" SERIAL PRIMARY KEY,
    "AnimalId" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "DateApplied" DATE NOT NULL,
    "Notes" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Treatments_Animals_AnimalId" FOREIGN KEY ("AnimalId") REFERENCES "Animals"("Id") ON DELETE CASCADE
);

-- Tabla Products
CREATE TABLE IF NOT EXISTS "Products" (
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Description" TEXT,
    "Price" DECIMAL(18,2) NOT NULL,
    "Category" TEXT,
    "SellerId" INTEGER NOT NULL,
    "Location" TEXT,
    "Phone" TEXT,
    "WhatsApp" TEXT,
    "Rating" DECIMAL(3,2),
    "ReviewCount" INTEGER DEFAULT 0,
    "IsFeatured" BOOLEAN DEFAULT FALSE,
    "IsActive" BOOLEAN DEFAULT TRUE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Products_Users_SellerId" FOREIGN KEY ("SellerId") REFERENCES "Users"("Id") ON DELETE CASCADE
);

-- Índices para mejor rendimiento
CREATE INDEX IF NOT EXISTS "IX_Animals_UserId" ON "Animals"("UserId");
CREATE INDEX IF NOT EXISTS "IX_WeightRecords_AnimalId" ON "WeightRecords"("AnimalId");
CREATE INDEX IF NOT EXISTS "IX_Vaccines_AnimalId" ON "Vaccines"("AnimalId");
CREATE INDEX IF NOT EXISTS "IX_Treatments_AnimalId" ON "Treatments"("AnimalId");
CREATE INDEX IF NOT EXISTS "IX_Products_SellerId" ON "Products"("SellerId");

-- Datos de prueba (opcional)
-- Contraseña: Demo123! (hash BCrypt)
INSERT INTO "Users" ("Email", "PasswordHash", "FullName", "Phone", "Location", "CreatedAt")
VALUES 
    ('demo@ruraltech.com', '$2a$11$KIXxKIXxKIXxKIXxKIXxOu', 'Usuario Demo', '+57 300 123 4567', 'Cundinamarca, Colombia', NOW())
ON CONFLICT ("Email") DO NOTHING;
```

4. Haz clic en **Run** (o presiona Ctrl+Enter)
5. Deberías ver "Success. No rows returned"

## Paso 4: Configurar el API

Edita `RuralTech App/src/RuralTech.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=db.xxxxx.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=TU_PASSWORD_AQUI;SSL Mode=Require;Trust Server Certificate=true"
  },
  "Supabase": {
    "Url": "https://xxxxx.supabase.co",
    "AnonKey": "tu-anon-key-aqui",
    "ServiceRoleKey": "tu-service-role-key-aqui"
  }
}
```

**Reemplaza:**
- `xxxxx` con tu Project Reference
- `TU_PASSWORD_AQUI` con la contraseña de la base de datos que creaste
- `tu-anon-key-aqui` con tu anon public key
- `tu-service-role-key-aqui` con tu service role key

## Paso 5: Probar la Conexión

1. Reinicia el API:
```powershell
cd "RuralTech App\src\RuralTech.API"
dotnet run
```

2. Deberías ver en los logs:
   - "Database created/verified successfully"
   - "Database seeded successfully"

3. Prueba el endpoint:
   - Ve a `http://localhost:5000/swagger`
   - Prueba `POST /api/auth/register`

## Verificar que Funciona

1. Ve a Supabase > **Table Editor**
2. Deberías ver las tablas: Users, Animals, WeightRecords, Vaccines, Treatments, Products
3. Si ejecutaste los datos de prueba, deberías ver un usuario en la tabla Users

## Troubleshooting

### Error: "Connection refused"
- Verifica que la connection string esté correcta
- Asegúrate de que el proyecto de Supabase esté activo (no pausado)

### Error: "SSL connection required"
- Asegúrate de tener `SSL Mode=Require;Trust Server Certificate=true` en la connection string

### Error: "Password authentication failed"
- Verifica que la contraseña sea correcta
- Puedes resetear la contraseña en Settings > Database > Reset Database Password

### Las tablas no aparecen
- Verifica que ejecutaste el SQL correctamente
- Ve a Table Editor y refresca la página
- Verifica que no haya errores en el SQL Editor

## Siguiente Paso

Una vez configurado Supabase, puedes:
1. Compilar la aplicación Flutter para Android
2. Compilar el instalador de Windows
3. Probar las descargas desde la página de landing
