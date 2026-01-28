-- Migración: Actualizar tabla WeightRecords para soportar Bovinos
-- Ejecuta este script en el SQL Editor de Supabase
-- Este script agrega soporte para registros de peso de bovinos además de animales genéricos

-- Primero, hacer que AnimalId sea nullable (si no lo es ya)
-- Nota: Esto puede fallar si ya hay datos, en ese caso primero migra los datos
DO $$
BEGIN
    -- Verificar si AnimalId es NOT NULL
    IF EXISTS (
        SELECT 1 FROM information_schema.columns 
        WHERE table_name = 'WeightRecords' 
        AND column_name = 'AnimalId' 
        AND is_nullable = 'NO'
    ) THEN
        -- Hacer AnimalId nullable
        ALTER TABLE "WeightRecords" ALTER COLUMN "AnimalId" DROP NOT NULL;
    END IF;
END $$;

-- Agregar columna BovinoId a la tabla WeightRecords (opcional)
ALTER TABLE "WeightRecords" 
ADD COLUMN IF NOT EXISTS "BovinoId" UUID;

-- Agregar foreign key de WeightRecords a Bovinos (solo si no existe)
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'FK_WeightRecords_Bovinos_BovinoId'
    ) THEN
        ALTER TABLE "WeightRecords"
        ADD CONSTRAINT "FK_WeightRecords_Bovinos_BovinoId" 
        FOREIGN KEY ("BovinoId") REFERENCES "Bovinos"("Id") ON DELETE CASCADE;
    END IF;
END $$;

-- Constraint para validar que WeightRecord tenga al menos AnimalId o BovinoId
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_WeightRecords_AnimalId_O_BovinoId'
    ) THEN
        ALTER TABLE "WeightRecords"
        ADD CONSTRAINT "CK_WeightRecords_AnimalId_O_BovinoId"
        CHECK (
            ("AnimalId" IS NOT NULL AND "BovinoId" IS NULL) OR
            ("AnimalId" IS NULL AND "BovinoId" IS NOT NULL)
        );
    END IF;
END $$;

-- Índice para búsquedas de registros de peso por bovino
CREATE INDEX IF NOT EXISTS "IX_WeightRecords_BovinoId" ON "WeightRecords"("BovinoId");

-- Agregar columna Notes si no existe (para notas adicionales en registros de peso)
ALTER TABLE "WeightRecords" 
ADD COLUMN IF NOT EXISTS "Notes" TEXT;

-- Comentarios para documentación
COMMENT ON COLUMN "WeightRecords"."BovinoId" IS 'ID del bovino al que pertenece este registro de peso (opcional, mutuamente exclusivo con AnimalId)';
COMMENT ON COLUMN "WeightRecords"."Notes" IS 'Notas adicionales sobre el registro de peso';
