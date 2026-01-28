# Migración: Tabla LandingPageLeads

## Instrucciones para ejecutar la migración en Supabase

1. Accede a tu proyecto de Supabase: https://vaeufppoexhsmrlgjnai.supabase.co
2. Ve a la sección **SQL Editor**
3. Copia y pega el siguiente script SQL:

```sql
-- Tabla LandingPageLeads
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

-- Índice para búsquedas por email
CREATE INDEX IF NOT EXISTS "IX_LandingPageLeads_Email" ON "LandingPageLeads"("Email");

-- Índice para búsquedas por fecha de creación
CREATE INDEX IF NOT EXISTS "IX_LandingPageLeads_CreatedAt" ON "LandingPageLeads"("CreatedAt");
```

4. Haz clic en **Run** para ejecutar el script
5. Verifica que la tabla se haya creado correctamente en la sección **Table Editor**

## Verificación

Después de ejecutar la migración, puedes verificar que todo funciona:

1. Reinicia la API (si está corriendo)
2. Prueba el endpoint: `POST /api/landingpage/register-lead`
3. Verifica los registros en Supabase: `GET /api/landingpage/leads`

## Estructura de la Tabla

- **Id**: UUID (identificador único)
- **Name**: Nombre completo del interesado (requerido, máx 150 caracteres)
- **Email**: Email del interesado (requerido, máx 100 caracteres)
- **Phone**: Teléfono de contacto (opcional, máx 20 caracteres)
- **Message**: Mensaje o comentarios (opcional, máx 1000 caracteres)
- **CreatedAt**: Fecha de creación (automático)
- **IsContacted**: Indica si el lead ya fue contactado (default: false)
- **ContactedAt**: Fecha en que se contactó al lead (opcional)
- **Notes**: Notas internas sobre el lead (opcional)
