-- Migración para crear tabla LandingPageLeads
-- Ejecuta este script en el SQL Editor de Supabase

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

-- Comentarios en la tabla
COMMENT ON TABLE "LandingPageLeads" IS 'Registros de usuarios interesados desde la landing page';
COMMENT ON COLUMN "LandingPageLeads"."Name" IS 'Nombre completo del interesado';
COMMENT ON COLUMN "LandingPageLeads"."Email" IS 'Email del interesado';
COMMENT ON COLUMN "LandingPageLeads"."Phone" IS 'Teléfono de contacto (opcional)';
COMMENT ON COLUMN "LandingPageLeads"."Message" IS 'Mensaje o comentarios adicionales (opcional)';
COMMENT ON COLUMN "LandingPageLeads"."IsContacted" IS 'Indica si el lead ya ha sido contactado';
COMMENT ON COLUMN "LandingPageLeads"."ContactedAt" IS 'Fecha en que se contactó al lead';
COMMENT ON COLUMN "LandingPageLeads"."Notes" IS 'Notas internas sobre el lead';
