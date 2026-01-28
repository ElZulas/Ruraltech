-- Migración: Crear tabla Infraestructuras
-- Ejecuta este script en el SQL Editor de Supabase
-- Este módulo permite subdividir la UPP en áreas lógicas y físicas (corrales, potreros, naves, etc.)

-- Crear tipos ENUM para tipo de instalación y estatus
CREATE TYPE tipo_instalacion AS ENUM (
    'POTRERO',        -- Pastoreo
    'CORRAL',         -- Manejo/Engorda
    'NAVE_AVICOLA',   -- Pollos
    'SALA_ORDENA',    -- Sala de ordeña
    'CUARENTENA'      -- Aislamiento
);

CREATE TYPE estatus_infraestructura AS ENUM (
    'DISPONIBLE',         -- Disponible para uso
    'OCUPADO',            -- Actualmente en uso
    'EN_MANTENIMIENTO',   -- En mantenimiento
    'DESCANSO'            -- Barbecho/Descanso
);

-- Crear tabla Infraestructuras
CREATE TABLE IF NOT EXISTS "Infraestructuras" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "UPPId" UUID NOT NULL,
    "Nombre" VARCHAR(50) NOT NULL,
    "TipoInstalacion" tipo_instalacion NOT NULL,
    "CapacidadMaxima" INTEGER NOT NULL,
    "SuperficieHectareas" DECIMAL(5,2), -- Solo para potreros
    "Estatus" estatus_infraestructura NOT NULL DEFAULT 'DISPONIBLE',
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    CONSTRAINT "FK_Infraestructuras_UPPs_UPPId" FOREIGN KEY ("UPPId") REFERENCES "UPPs"("Id") ON DELETE CASCADE
);

-- Índice único compuesto: nombre único dentro de la misma UPP
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Infraestructuras_UPPId_Nombre" ON "Infraestructuras"("UPPId", "Nombre");

-- Índice para búsquedas por UPP
CREATE INDEX IF NOT EXISTS "IX_Infraestructuras_UPPId" ON "Infraestructuras"("UPPId");

-- Índice para búsquedas por tipo de instalación
CREATE INDEX IF NOT EXISTS "IX_Infraestructuras_TipoInstalacion" ON "Infraestructuras"("TipoInstalacion");

-- Índice para búsquedas por estatus
CREATE INDEX IF NOT EXISTS "IX_Infraestructuras_Estatus" ON "Infraestructuras"("Estatus");

-- Agregar columna InfraestructuraId a la tabla Animals (opcional, para saber en qué corral está)
ALTER TABLE "Animals" 
ADD COLUMN IF NOT EXISTS "InfraestructuraId" UUID;

-- Agregar foreign key de Animals a Infraestructuras (solo si no existe)
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'FK_Animals_Infraestructuras_InfraestructuraId'
    ) THEN
        ALTER TABLE "Animals"
        ADD CONSTRAINT "FK_Animals_Infraestructuras_InfraestructuraId" 
        FOREIGN KEY ("InfraestructuraId") REFERENCES "Infraestructuras"("Id") ON DELETE SET NULL;
    END IF;
END $$;

-- Índice para búsquedas de animales por infraestructura
CREATE INDEX IF NOT EXISTS "IX_Animals_InfraestructuraId" ON "Animals"("InfraestructuraId");

-- Comentarios para documentación
COMMENT ON TABLE "Infraestructuras" IS 'Representa cualquier subdivisión física dentro de la UPP donde se alojan animales o se realizan manejos';
COMMENT ON COLUMN "Infraestructuras"."Nombre" IS 'Nombre común para identificarlo (ej: "Potrero La Loma", "Nave 3"). No debe duplicarse dentro de la misma UPP';
COMMENT ON COLUMN "Infraestructuras"."TipoInstalacion" IS 'Clasificación funcional del área';
COMMENT ON COLUMN "Infraestructuras"."CapacidadMaxima" IS 'Cuántos animales caben cómodamente. Útil para alertas de hacinamiento (Bienestar Animal)';
COMMENT ON COLUMN "Infraestructuras"."SuperficieHectareas" IS 'Tamaño del terreno (solo Potreros). Vital para calcular la Carga Animal (Cabezas por Hectárea) y evitar sobrepastoreo';
COMMENT ON COLUMN "Infraestructuras"."Estatus" IS 'Disponibilidad actual de la infraestructura';
