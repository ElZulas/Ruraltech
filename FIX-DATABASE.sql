-- ============================================
-- SCRIPT PARA ARREGLAR LA BASE DE DATOS
-- Ejecuta esto en el SQL Editor de Supabase
-- ============================================

-- 1. Agregar UpdatedAt a Animals
ALTER TABLE "Animals" ADD COLUMN IF NOT EXISTS "UpdatedAt" TIMESTAMP WITH TIME ZONE;

-- 2. Agregar UpdatedAt a Products  
ALTER TABLE "Products" ADD COLUMN IF NOT EXISTS "UpdatedAt" TIMESTAMP WITH TIME ZONE;

-- 3. Crear tabla LandingPageLeads (la más importante para el formulario)
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

-- 4. Crear índices para LandingPageLeads
CREATE INDEX IF NOT EXISTS "IX_LandingPageLeads_Email" ON "LandingPageLeads"("Email");
CREATE INDEX IF NOT EXISTS "IX_LandingPageLeads_CreatedAt" ON "LandingPageLeads"("CreatedAt");

-- Verificar que todo se creó correctamente
SELECT 
    'Animals tiene UpdatedAt: ' || EXISTS(
        SELECT 1 FROM information_schema.columns 
        WHERE table_name = 'Animals' AND column_name = 'UpdatedAt'
    ) as animals_updated,
    'LandingPageLeads existe: ' || EXISTS(
        SELECT 1 FROM information_schema.tables 
        WHERE table_name = 'LandingPageLeads'
    ) as leads_table_exists;
