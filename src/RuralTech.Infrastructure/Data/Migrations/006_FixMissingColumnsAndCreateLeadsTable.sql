-- Migración: Agregar columnas faltantes y crear tabla LandingPageLeads
-- Ejecuta este script en el SQL Editor de Supabase

-- Agregar UpdatedAt a Animals si no existe
ALTER TABLE "Animals" ADD COLUMN IF NOT EXISTS "UpdatedAt" TIMESTAMP WITH TIME ZONE;

-- Agregar UpdatedAt a Products si no existe
ALTER TABLE "Products" ADD COLUMN IF NOT EXISTS "UpdatedAt" TIMESTAMP WITH TIME ZONE;

-- Agregar UpdatedAt a Colaboradores si no existe (verificar primero si la tabla existe)
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

-- Verificar y actualizar Users para usar UUID si es necesario
-- (Esto es solo si Users todavía usa SERIAL en lugar de UUID)
DO $$
BEGIN
    -- Si Users.Id es INTEGER, necesitamos verificar las relaciones
    -- Por ahora solo agregamos las columnas faltantes
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                   WHERE table_name = 'Users' AND column_name = 'DateOfBirth') THEN
        ALTER TABLE "Users" ADD COLUMN "DateOfBirth" DATE;
    END IF;
END $$;
