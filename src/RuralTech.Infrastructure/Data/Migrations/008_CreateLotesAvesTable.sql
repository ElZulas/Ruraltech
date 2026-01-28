-- Migración: Crear tablas para Lotes de Aves (Seguimiento por Lote)
-- Ejecuta este script en el SQL Editor de Supabase
-- Este módulo gestiona aves mediante seguimiento por lote (grupos de aves)

-- Crear tipo ENUM para estatus del lote
CREATE TYPE estatus_lote_aves AS ENUM (
    'ACTIVO',       -- Lote activo en producción
    'FINALIZADO',   -- Lote finalizado (vendido/sacrificado)
    'CANCELADO'     -- Lote cancelado
);

-- Crear tabla LotesAves
CREATE TABLE IF NOT EXISTS "LotesAves" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "UPPId" UUID NOT NULL,
    "InfraestructuraId" UUID NOT NULL, -- Debe ser NAVE_AVICOLA
    
    -- Identificación del lote
    "CodigoLote" VARCHAR(50) NOT NULL, -- Código único del lote (ej: "LOTE-2025-001")
    "NumeroLoteGranja" VARCHAR(50), -- Número de lote según la granja
    
    -- Información del lote
    "FechaIngreso" DATE NOT NULL, -- Fecha de llegada de las aves
    "CantidadInicial" INTEGER NOT NULL, -- Cantidad de aves al ingresar
    "CantidadActual" INTEGER NOT NULL, -- Cantidad actual (se actualiza con mortalidad)
    "Raza" VARCHAR(50) NOT NULL, -- Raza o línea genética
    "TipoAve" VARCHAR(50), -- Pollo de engorda, ponedora, reproductora, etc.
    "EdadDias" INTEGER, -- Edad en días al momento del ingreso
    
    -- Estado del lote
    "Estatus" estatus_lote_aves NOT NULL DEFAULT 'ACTIVO',
    "FechaFinalizacion" DATE, -- Fecha de finalización del lote
    
    -- Timestamps
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    
    -- Foreign Keys
    CONSTRAINT "FK_LotesAves_UPPs_UPPId" FOREIGN KEY ("UPPId") REFERENCES "UPPs"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_LotesAves_Infraestructuras_InfraestructuraId" FOREIGN KEY ("InfraestructuraId") REFERENCES "Infraestructuras"("Id") ON DELETE RESTRICT
);

-- Índice único compuesto: código de lote único dentro de la misma UPP
CREATE UNIQUE INDEX IF NOT EXISTS "IX_LotesAves_UPPId_CodigoLote" ON "LotesAves"("UPPId", "CodigoLote");

-- Índices para búsquedas
CREATE INDEX IF NOT EXISTS "IX_LotesAves_UPPId" ON "LotesAves"("UPPId");
CREATE INDEX IF NOT EXISTS "IX_LotesAves_InfraestructuraId" ON "LotesAves"("InfraestructuraId");
CREATE INDEX IF NOT EXISTS "IX_LotesAves_Estatus" ON "LotesAves"("Estatus");
CREATE INDEX IF NOT EXISTS "IX_LotesAves_FechaIngreso" ON "LotesAves"("FechaIngreso");

-- Constraint para validar que fecha de ingreso no sea futura
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_LotesAves_FechaIngreso_NoFutura'
    ) THEN
        ALTER TABLE "LotesAves"
        ADD CONSTRAINT "CK_LotesAves_FechaIngreso_NoFutura"
        CHECK ("FechaIngreso" <= CURRENT_DATE);
    END IF;
END $$;

-- Constraint para validar que cantidad actual no sea mayor a cantidad inicial
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_LotesAves_CantidadActual_Valida'
    ) THEN
        ALTER TABLE "LotesAves"
        ADD CONSTRAINT "CK_LotesAves_CantidadActual_Valida"
        CHECK ("CantidadActual" >= 0 AND "CantidadActual" <= "CantidadInicial");
    END IF;
END $$;

-- ============================================================================
-- Tabla: Registros de Mortalidad de Aves
-- ============================================================================

CREATE TABLE IF NOT EXISTS "RegistrosMortalidadAves" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "LoteAvesId" UUID NOT NULL,
    "Fecha" DATE NOT NULL,
    "CantidadMuertas" INTEGER NOT NULL,
    "CausaMuerte" VARCHAR(255), -- Opcional - causa de la mortalidad
    "Observaciones" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_RegistrosMortalidadAves_LotesAves_LoteAvesId" FOREIGN KEY ("LoteAvesId") REFERENCES "LotesAves"("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_RegistrosMortalidadAves_LoteAvesId" ON "RegistrosMortalidadAves"("LoteAvesId");
CREATE INDEX IF NOT EXISTS "IX_RegistrosMortalidadAves_Fecha" ON "RegistrosMortalidadAves"("Fecha");

-- Constraint para validar cantidad de muertas positiva
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_RegistrosMortalidadAves_CantidadMuertas_Positiva'
    ) THEN
        ALTER TABLE "RegistrosMortalidadAves"
        ADD CONSTRAINT "CK_RegistrosMortalidadAves_CantidadMuertas_Positiva"
        CHECK ("CantidadMuertas" > 0);
    END IF;
END $$;

-- ============================================================================
-- Tabla: Registros de Peso de Lote de Aves
-- ============================================================================

CREATE TABLE IF NOT EXISTS "RegistrosPesoLoteAves" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "LoteAvesId" UUID NOT NULL,
    "Fecha" DATE NOT NULL,
    "PesoPromedio" DECIMAL(10,3) NOT NULL, -- Peso promedio del lote en kg
    "MuestraTamaño" INTEGER, -- Cantidad de aves muestreadas
    "Observaciones" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_RegistrosPesoLoteAves_LotesAves_LoteAvesId" FOREIGN KEY ("LoteAvesId") REFERENCES "LotesAves"("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_RegistrosPesoLoteAves_LoteAvesId" ON "RegistrosPesoLoteAves"("LoteAvesId");
CREATE INDEX IF NOT EXISTS "IX_RegistrosPesoLoteAves_Fecha" ON "RegistrosPesoLoteAves"("Fecha");

-- Constraint para validar peso promedio positivo
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_RegistrosPesoLoteAves_PesoPromedio_Positivo'
    ) THEN
        ALTER TABLE "RegistrosPesoLoteAves"
        ADD CONSTRAINT "CK_RegistrosPesoLoteAves_PesoPromedio_Positivo"
        CHECK ("PesoPromedio" > 0);
    END IF;
END $$;

-- ============================================================================
-- Tabla: Vacunaciones de Lote de Aves
-- ============================================================================

CREATE TABLE IF NOT EXISTS "VacunacionesLoteAves" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "LoteAvesId" UUID NOT NULL,
    "NombreVacuna" VARCHAR(255) NOT NULL,
    "FechaAplicacion" DATE NOT NULL,
    "FechaProximaAplicacion" DATE,
    "CantidadAplicada" INTEGER, -- Cantidad de aves vacunadas
    "MetodoAplicacion" VARCHAR(50), -- Agua, spray, inyección, etc.
    "Observaciones" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_VacunacionesLoteAves_LotesAves_LoteAvesId" FOREIGN KEY ("LoteAvesId") REFERENCES "LotesAves"("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_VacunacionesLoteAves_LoteAvesId" ON "VacunacionesLoteAves"("LoteAvesId");
CREATE INDEX IF NOT EXISTS "IX_VacunacionesLoteAves_FechaAplicacion" ON "VacunacionesLoteAves"("FechaAplicacion");

-- ============================================================================
-- Tabla: Tratamientos de Lote de Aves
-- ============================================================================

CREATE TABLE IF NOT EXISTS "TratamientosLoteAves" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "LoteAvesId" UUID NOT NULL,
    "Condicion" VARCHAR(255) NOT NULL, -- Enfermedad o condición tratada
    "DescripcionTratamiento" TEXT NOT NULL,
    "FechaInicio" DATE NOT NULL,
    "FechaFin" DATE,
    "Medicamento" VARCHAR(255),
    "Dosis" VARCHAR(100),
    "Observaciones" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_TratamientosLoteAves_LotesAves_LoteAvesId" FOREIGN KEY ("LoteAvesId") REFERENCES "LotesAves"("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_TratamientosLoteAves_LoteAvesId" ON "TratamientosLoteAves"("LoteAvesId");
CREATE INDEX IF NOT EXISTS "IX_TratamientosLoteAves_FechaInicio" ON "TratamientosLoteAves"("FechaInicio");

-- Comentarios para documentación
COMMENT ON TABLE "LotesAves" IS 'Representa un lote de aves (grupo de aves que nacen juntas, se crían juntas). Seguimiento por lote según NOM-001-SAG/GAN-2015';
COMMENT ON COLUMN "LotesAves"."CodigoLote" IS 'Código único del lote dentro de la UPP. Ej: "LOTE-2025-001"';
COMMENT ON COLUMN "LotesAves"."InfraestructuraId" IS 'Debe ser una infraestructura de tipo NAVE_AVICOLA';
COMMENT ON COLUMN "LotesAves"."CantidadActual" IS 'Se actualiza automáticamente con los registros de mortalidad';
COMMENT ON COLUMN "LotesAves"."Estatus" IS 'Estado del lote: ACTIVO (en producción), FINALIZADO (vendido/sacrificado), CANCELADO';

COMMENT ON TABLE "RegistrosMortalidadAves" IS 'Registra la mortalidad diaria o periódica de un lote de aves';
COMMENT ON TABLE "RegistrosPesoLoteAves" IS 'Registra el peso promedio del lote en diferentes fechas para seguimiento de crecimiento';
COMMENT ON TABLE "VacunacionesLoteAves" IS 'Registra las vacunaciones aplicadas a un lote de aves';
COMMENT ON TABLE "TratamientosLoteAves" IS 'Registra los tratamientos médicos aplicados a un lote de aves';
