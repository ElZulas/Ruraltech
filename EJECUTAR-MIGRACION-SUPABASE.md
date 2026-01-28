# ⚠️ IMPORTANTE: Ejecutar esta migración en Supabase

## Problema detectado
La base de datos de Supabase no tiene:
1. La columna `UpdatedAt` en la tabla `Animals`
2. La tabla `LandingPageLeads` para guardar registros de la landing page

## Solución

### Paso 1: Acceder a Supabase
1. Ve a: https://vaeufppoexhsmrlgjnai.supabase.co
2. Inicia sesión
3. Ve a **SQL Editor** en el menú lateral

### Paso 2: Ejecutar la migración
Copia y pega este script completo en el SQL Editor:

```sql
-- Agregar UpdatedAt a Animals si no existe
ALTER TABLE "Animals" ADD COLUMN IF NOT EXISTS "UpdatedAt" TIMESTAMP WITH TIME ZONE;

-- Agregar UpdatedAt a Products si no existe
ALTER TABLE "Products" ADD COLUMN IF NOT EXISTS "UpdatedAt" TIMESTAMP WITH TIME ZONE;

-- Agregar UpdatedAt a Colaboradores si existe
DO $$
BEGIN
    IF EXISTS (SELECT FROM information_schema.tables WHERE table_name = 'Colaboradores') THEN
        ALTER TABLE "Colaboradores" ADD COLUMN IF NOT EXISTS "UpdatedAt" TIMESTAMP WITH TIME ZONE;
    END IF;
END $$;

-- Crear tabla LandingPageLeads
CREATE TABLE IF NOT EXISTS "LandingPageLeads" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Name" TEXT NOT NULL,
    "Email" TEXT NOT NULL,
    "Phone" TEXT,
    "Message" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "IsContacted" BOOLEAN NOT NULL DEFAULT FALSE,
    "ContactedAt" TIMESTAMP WITH TIME ZONE,
    "Notes" TEXT
);

-- Índices para LandingPageLeads
CREATE INDEX IF NOT EXISTS "IX_LandingPageLeads_Email" ON "LandingPageLeads"("Email");
CREATE INDEX IF NOT EXISTS "IX_LandingPageLeads_CreatedAt" ON "LandingPageLeads"("CreatedAt");
```

### Paso 3: Ejecutar
1. Haz clic en **Run** o presiona `Ctrl+Enter`
2. Deberías ver un mensaje de éxito

### Paso 4: Verificar
1. Ve a **Table Editor** en Supabase
2. Verifica que exista la tabla `LandingPageLeads`
3. Verifica que la tabla `Animals` tenga la columna `UpdatedAt`

## Después de ejecutar
Una vez ejecutada la migración, el formulario de la landing page debería funcionar correctamente y guardar los registros en la base de datos.
