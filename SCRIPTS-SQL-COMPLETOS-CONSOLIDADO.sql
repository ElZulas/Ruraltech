-- ============================================================================
-- SCRIPTS SQL COMPLETOS - RURALTECH
-- Ejecuta este script completo en el SQL Editor de Supabase
-- Orden de ejecución: Este script contiene todas las migraciones en orden
-- ============================================================================

-- ============================================================================
-- MIGRACIÓN 001: Esquema Inicial (Users, Animals, WeightRecords, Vaccines, Treatments, Products)
-- ============================================================================

-- Tabla Users
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Email" VARCHAR(100) NOT NULL UNIQUE,
    "PasswordHash" VARCHAR(255) NOT NULL,
    "FullName" VARCHAR(150) NOT NULL,
    "DateOfBirth" DATE NOT NULL,
    "Phone" VARCHAR(20),
    "Location" VARCHAR(255),
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- Tabla Animals (genéricos)
CREATE TABLE IF NOT EXISTS "Animals" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "Breed" VARCHAR(255) NOT NULL,
    "BirthDate" DATE NOT NULL,
    "Sex" VARCHAR(50) NOT NULL,
    "CurrentWeight" DECIMAL(10,2) NOT NULL,
    "LastVaccineDate" DATE,
    "UserId" UUID NOT NULL,
    "UPPId" UUID,
    "InfraestructuraId" UUID,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    CONSTRAINT "FK_Animals_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
);

-- Tabla WeightRecords
CREATE TABLE IF NOT EXISTS "WeightRecords" (
    "Id" SERIAL PRIMARY KEY,
    "AnimalId" INTEGER,
    "Weight" DECIMAL(10,2) NOT NULL,
    "Date" DATE NOT NULL,
    "Notes" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_WeightRecords_Animals_AnimalId" FOREIGN KEY ("AnimalId") REFERENCES "Animals"("Id") ON DELETE CASCADE
);

-- Tabla Vaccines
CREATE TABLE IF NOT EXISTS "Vaccines" (
    "Id" SERIAL PRIMARY KEY,
    "AnimalId" INTEGER NOT NULL,
    "Name" VARCHAR(255) NOT NULL,
    "DateApplied" DATE NOT NULL,
    "NextDueDate" DATE,
    "Notes" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Vaccines_Animals_AnimalId" FOREIGN KEY ("AnimalId") REFERENCES "Animals"("Id") ON DELETE CASCADE
);

-- Tabla Treatments
CREATE TABLE IF NOT EXISTS "Treatments" (
    "Id" SERIAL PRIMARY KEY,
    "AnimalId" INTEGER NOT NULL,
    "Condition" VARCHAR(255) NOT NULL,
    "TreatmentDescription" TEXT NOT NULL,
    "Date" DATE NOT NULL,
    "Notes" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Treatments_Animals_AnimalId" FOREIGN KEY ("AnimalId") REFERENCES "Animals"("Id") ON DELETE CASCADE
);

-- Tabla Products
CREATE TABLE IF NOT EXISTS "Products" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "Description" TEXT,
    "Price" DECIMAL(18,2) NOT NULL,
    "Category" VARCHAR(100) NOT NULL,
    "SellerId" UUID NOT NULL,
    "Location" VARCHAR(255),
    "Phone" VARCHAR(20),
    "WhatsApp" VARCHAR(20),
    "Rating" DECIMAL(3,2),
    "ReviewCount" INTEGER DEFAULT 0,
    "IsFeatured" BOOLEAN DEFAULT FALSE,
    "IsActive" BOOLEAN DEFAULT TRUE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Products_Users_SellerId" FOREIGN KEY ("SellerId") REFERENCES "Users"("Id") ON DELETE RESTRICT
);

-- Índices iniciales
CREATE INDEX IF NOT EXISTS "IX_Animals_UserId" ON "Animals"("UserId");
CREATE INDEX IF NOT EXISTS "IX_WeightRecords_AnimalId" ON "WeightRecords"("AnimalId");
CREATE INDEX IF NOT EXISTS "IX_Vaccines_AnimalId" ON "Vaccines"("AnimalId");
CREATE INDEX IF NOT EXISTS "IX_Treatments_AnimalId" ON "Treatments"("AnimalId");
CREATE INDEX IF NOT EXISTS "IX_Products_SellerId" ON "Products"("SellerId");

-- ============================================================================
-- MIGRACIÓN 002: Actualizar Users (convertir Id a UUID y agregar DateOfBirth)
-- ============================================================================

-- Nota: Si Users ya tiene datos, este script debe ejecutarse con cuidado
-- Este script asume que Users.Id ya es UUID (si no, requiere migración de datos)

-- ============================================================================
-- MIGRACIÓN 003: Crear tabla UPPs
-- ============================================================================

CREATE TABLE IF NOT EXISTS "UPPs" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "OwnerId" UUID NOT NULL,
    "ClavePGN" VARCHAR(20) NOT NULL,
    "NombrePredio" VARCHAR(100) NOT NULL,
    "PropietarioLegal" VARCHAR(150) NOT NULL,
    "CodigoQRAcceso" VARCHAR(15) NOT NULL,
    "EstadoMX" CHAR(2) NOT NULL,
    "Latitude" DECIMAL(10,8),
    "Longitude" DECIMAL(11,8),
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    CONSTRAINT "FK_UPPs_Users_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "Users"("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_UPPs_ClavePGN" ON "UPPs"("ClavePGN");
CREATE UNIQUE INDEX IF NOT EXISTS "IX_UPPs_CodigoQRAcceso" ON "UPPs"("CodigoQRAcceso");
CREATE INDEX IF NOT EXISTS "IX_UPPs_OwnerId" ON "UPPs"("OwnerId");

-- Actualizar Animals para relacionar con UPPs
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'FK_Animals_UPPs_UPPId'
    ) THEN
        ALTER TABLE "Animals"
        ADD CONSTRAINT "FK_Animals_UPPs_UPPId" 
        FOREIGN KEY ("UPPId") REFERENCES "UPPs"("Id") ON DELETE SET NULL;
    END IF;
END $$;

CREATE INDEX IF NOT EXISTS "IX_Animals_UPPId" ON "Animals"("UPPId");

-- ============================================================================
-- MIGRACIÓN 004: Crear tabla Colaboradores
-- ============================================================================

CREATE TYPE rol_colaborador AS ENUM ('ENCARGADO', 'OPERARIO', 'VETERINARIO');
CREATE TYPE estatus_colaborador AS ENUM ('ACTIVO', 'SUSPENDIDO');

CREATE TABLE IF NOT EXISTS "Colaboradores" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "UPPId" UUID NOT NULL,
    "NombreAlias" VARCHAR(50) NOT NULL,
    "PinAccesoHash" VARCHAR(255) NOT NULL,
    "TelefonoContacto" VARCHAR(20),
    "Rol" rol_colaborador NOT NULL,
    "Estatus" estatus_colaborador NOT NULL DEFAULT 'ACTIVO',
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    CONSTRAINT "FK_Colaboradores_UPPs_UPPId" FOREIGN KEY ("UPPId") REFERENCES "UPPs"("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_Colaboradores_UPPId" ON "Colaboradores"("UPPId");

-- ============================================================================
-- MIGRACIÓN 005: Crear tabla Infraestructuras
-- ============================================================================

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

CREATE TABLE IF NOT EXISTS "Infraestructuras" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "UPPId" UUID NOT NULL,
    "Nombre" VARCHAR(50) NOT NULL,
    "TipoInstalacion" tipo_instalacion NOT NULL,
    "CapacidadMaxima" INTEGER NOT NULL,
    "SuperficieHectareas" DECIMAL(5,2),
    "Estatus" estatus_infraestructura NOT NULL DEFAULT 'DISPONIBLE',
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    CONSTRAINT "FK_Infraestructuras_UPPs_UPPId" FOREIGN KEY ("UPPId") REFERENCES "UPPs"("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Infraestructuras_UPPId_Nombre" ON "Infraestructuras"("UPPId", "Nombre");
CREATE INDEX IF NOT EXISTS "IX_Infraestructuras_UPPId" ON "Infraestructuras"("UPPId");
CREATE INDEX IF NOT EXISTS "IX_Infraestructuras_TipoInstalacion" ON "Infraestructuras"("TipoInstalacion");
CREATE INDEX IF NOT EXISTS "IX_Infraestructuras_Estatus" ON "Infraestructuras"("Estatus");

-- Agregar columna InfraestructuraId a Animals
ALTER TABLE "Animals" 
ADD COLUMN IF NOT EXISTS "InfraestructuraId" UUID;

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

CREATE INDEX IF NOT EXISTS "IX_Animals_InfraestructuraId" ON "Animals"("InfraestructuraId");

-- ============================================================================
-- MIGRACIÓN 006: Crear tabla Bovinos
-- ============================================================================

CREATE TYPE estado_productivo AS ENUM (
    'CRIA',          -- Cría
    'DESTETADO',     -- Destetado
    'VAQUILLA',      -- Vaquilla
    'TORO_ENGORDA',  -- Toro Engorda
    'VACA_ORDENA',   -- Vaca Ordeña
    'VACA_SECA'      -- Vaca Seca
);

CREATE TYPE estatus_sistema AS ENUM (
    'ACTIVO',   -- En rancho
    'VENDIDO',  -- Vendido
    'MUERTO',   -- Muerto
    'ROBADO'    -- Robado
);

CREATE TABLE IF NOT EXISTS "Bovinos" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "UPPId" UUID NOT NULL,
    "InfraestructuraId" UUID,
    
    -- Identificadores
    "AreteSiniiga" VARCHAR(14),
    "AreteTrabajo" VARCHAR(10) NOT NULL,
    
    -- Datos básicos
    "Nombre" VARCHAR(50),
    "FechaNacimiento" DATE NOT NULL,
    "Sexo" CHAR(1) NOT NULL,
    "RazaPredominante" VARCHAR(50) NOT NULL,
    
    -- Relaciones parentales (recursivas)
    "MadreId" UUID,
    "PadreId" UUID,
    
    -- Estado y estatus
    "EstadoProductivo" estado_productivo NOT NULL,
    "EstatusSistema" estatus_sistema NOT NULL DEFAULT 'ACTIVO',
    
    -- Timestamps
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    
    -- Foreign Keys
    CONSTRAINT "FK_Bovinos_UPPs_UPPId" FOREIGN KEY ("UPPId") REFERENCES "UPPs"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Bovinos_Infraestructuras_InfraestructuraId" FOREIGN KEY ("InfraestructuraId") REFERENCES "Infraestructuras"("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_Bovinos_Bovinos_MadreId" FOREIGN KEY ("MadreId") REFERENCES "Bovinos"("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Bovinos_Bovinos_PadreId" FOREIGN KEY ("PadreId") REFERENCES "Bovinos"("Id") ON DELETE SET NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Bovinos_AreteSiniiga" 
ON "Bovinos"("AreteSiniiga") 
WHERE "AreteSiniiga" IS NOT NULL;

CREATE INDEX IF NOT EXISTS "IX_Bovinos_UPPId" ON "Bovinos"("UPPId");
CREATE INDEX IF NOT EXISTS "IX_Bovinos_InfraestructuraId" ON "Bovinos"("InfraestructuraId");
CREATE INDEX IF NOT EXISTS "IX_Bovinos_MadreId" ON "Bovinos"("MadreId");
CREATE INDEX IF NOT EXISTS "IX_Bovinos_PadreId" ON "Bovinos"("PadreId");
CREATE INDEX IF NOT EXISTS "IX_Bovinos_EstadoProductivo" ON "Bovinos"("EstadoProductivo");
CREATE INDEX IF NOT EXISTS "IX_Bovinos_EstatusSistema" ON "Bovinos"("EstatusSistema");
CREATE INDEX IF NOT EXISTS "IX_Bovinos_AreteTrabajo" ON "Bovinos"("AreteTrabajo");

-- Constraints para Bovinos
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_Bovinos_AreteSiniiga_Formato') THEN
        ALTER TABLE "Bovinos" ADD CONSTRAINT "CK_Bovinos_AreteSiniiga_Formato"
        CHECK (
            "AreteSiniiga" IS NULL OR 
            (LENGTH("AreteSiniiga") = 14 AND SUBSTRING("AreteSiniiga", 1, 2) = 'MX' 
             AND SUBSTRING("AreteSiniiga", 3, 2) = '00' 
             AND SUBSTRING("AreteSiniiga", 5, 2) ~ '^[A-Z]{2}$' 
             AND SUBSTRING("AreteSiniiga", 7, 8) ~ '^\d{8}$')
        );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_Bovinos_Sexo') THEN
        ALTER TABLE "Bovinos" ADD CONSTRAINT "CK_Bovinos_Sexo"
        CHECK ("Sexo" IN ('M', 'H'));
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_Bovinos_MadreId_Diferente') THEN
        ALTER TABLE "Bovinos" ADD CONSTRAINT "CK_Bovinos_MadreId_Diferente"
        CHECK ("MadreId" IS NULL OR "Id" != "MadreId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_Bovinos_PadreId_Diferente') THEN
        ALTER TABLE "Bovinos" ADD CONSTRAINT "CK_Bovinos_PadreId_Diferente"
        CHECK ("PadreId" IS NULL OR "Id" != "PadreId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_Bovinos_Padre_Madre_Diferentes') THEN
        ALTER TABLE "Bovinos" ADD CONSTRAINT "CK_Bovinos_Padre_Madre_Diferentes"
        CHECK ("PadreId" IS NULL OR "PadreId" != "MadreId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_Bovinos_FechaNacimiento_NoFutura') THEN
        ALTER TABLE "Bovinos" ADD CONSTRAINT "CK_Bovinos_FechaNacimiento_NoFutura"
        CHECK ("FechaNacimiento" <= CURRENT_DATE);
    END IF;
END $$;

-- ============================================================================
-- MIGRACIÓN 007: Actualizar WeightRecords para soportar Bovinos
-- ============================================================================

DO $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM information_schema.columns 
        WHERE table_name = 'WeightRecords' AND column_name = 'AnimalId' AND is_nullable = 'NO'
    ) THEN
        ALTER TABLE "WeightRecords" ALTER COLUMN "AnimalId" DROP NOT NULL;
    END IF;
END $$;

ALTER TABLE "WeightRecords" ADD COLUMN IF NOT EXISTS "BovinoId" UUID;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'FK_WeightRecords_Bovinos_BovinoId') THEN
        ALTER TABLE "WeightRecords"
        ADD CONSTRAINT "FK_WeightRecords_Bovinos_BovinoId" 
        FOREIGN KEY ("BovinoId") REFERENCES "Bovinos"("Id") ON DELETE CASCADE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_WeightRecords_AnimalId_O_BovinoId') THEN
        ALTER TABLE "WeightRecords"
        ADD CONSTRAINT "CK_WeightRecords_AnimalId_O_BovinoId"
        CHECK (
            ("AnimalId" IS NOT NULL AND "BovinoId" IS NULL) OR
            ("AnimalId" IS NULL AND "BovinoId" IS NOT NULL)
        );
    END IF;
END $$;

CREATE INDEX IF NOT EXISTS "IX_WeightRecords_BovinoId" ON "WeightRecords"("BovinoId");

-- ============================================================================
-- MIGRACIÓN 008: Crear tablas para Lotes de Aves (Seguimiento por Lote)
-- ============================================================================

CREATE TYPE estatus_lote_aves AS ENUM (
    'ACTIVO',       -- Lote activo en producción
    'FINALIZADO',   -- Lote finalizado (vendido/sacrificado)
    'CANCELADO'     -- Lote cancelado
);

CREATE TABLE IF NOT EXISTS "LotesAves" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "UPPId" UUID NOT NULL,
    "InfraestructuraId" UUID NOT NULL,
    "CodigoLote" VARCHAR(50) NOT NULL,
    "NumeroLoteGranja" VARCHAR(50),
    "FechaIngreso" DATE NOT NULL,
    "CantidadInicial" INTEGER NOT NULL,
    "CantidadActual" INTEGER NOT NULL,
    "Raza" VARCHAR(50) NOT NULL,
    "TipoAve" VARCHAR(50),
    "EdadDias" INTEGER,
    "Estatus" estatus_lote_aves NOT NULL DEFAULT 'ACTIVO',
    "FechaFinalizacion" DATE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    CONSTRAINT "FK_LotesAves_UPPs_UPPId" FOREIGN KEY ("UPPId") REFERENCES "UPPs"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_LotesAves_Infraestructuras_InfraestructuraId" FOREIGN KEY ("InfraestructuraId") REFERENCES "Infraestructuras"("Id") ON DELETE RESTRICT
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_LotesAves_UPPId_CodigoLote" ON "LotesAves"("UPPId", "CodigoLote");
CREATE INDEX IF NOT EXISTS "IX_LotesAves_UPPId" ON "LotesAves"("UPPId");
CREATE INDEX IF NOT EXISTS "IX_LotesAves_InfraestructuraId" ON "LotesAves"("InfraestructuraId");
CREATE INDEX IF NOT EXISTS "IX_LotesAves_Estatus" ON "LotesAves"("Estatus");
CREATE INDEX IF NOT EXISTS "IX_LotesAves_FechaIngreso" ON "LotesAves"("FechaIngreso");

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_LotesAves_FechaIngreso_NoFutura') THEN
        ALTER TABLE "LotesAves" ADD CONSTRAINT "CK_LotesAves_FechaIngreso_NoFutura"
        CHECK ("FechaIngreso" <= CURRENT_DATE);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_LotesAves_CantidadActual_Valida') THEN
        ALTER TABLE "LotesAves" ADD CONSTRAINT "CK_LotesAves_CantidadActual_Valida"
        CHECK ("CantidadActual" >= 0 AND "CantidadActual" <= "CantidadInicial");
    END IF;
END $$;

-- Tabla: Registros de Mortalidad de Aves
CREATE TABLE IF NOT EXISTS "RegistrosMortalidadAves" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "LoteAvesId" UUID NOT NULL,
    "Fecha" DATE NOT NULL,
    "CantidadMuertas" INTEGER NOT NULL,
    "CausaMuerte" VARCHAR(255),
    "Observaciones" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_RegistrosMortalidadAves_LotesAves_LoteAvesId" FOREIGN KEY ("LoteAvesId") REFERENCES "LotesAves"("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_RegistrosMortalidadAves_LoteAvesId" ON "RegistrosMortalidadAves"("LoteAvesId");
CREATE INDEX IF NOT EXISTS "IX_RegistrosMortalidadAves_Fecha" ON "RegistrosMortalidadAves"("Fecha");

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_RegistrosMortalidadAves_CantidadMuertas_Positiva') THEN
        ALTER TABLE "RegistrosMortalidadAves" ADD CONSTRAINT "CK_RegistrosMortalidadAves_CantidadMuertas_Positiva"
        CHECK ("CantidadMuertas" > 0);
    END IF;
END $$;

-- Tabla: Registros de Peso de Lote de Aves
CREATE TABLE IF NOT EXISTS "RegistrosPesoLoteAves" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "LoteAvesId" UUID NOT NULL,
    "Fecha" DATE NOT NULL,
    "PesoPromedio" DECIMAL(10,3) NOT NULL,
    "MuestraTamaño" INTEGER,
    "Observaciones" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_RegistrosPesoLoteAves_LotesAves_LoteAvesId" FOREIGN KEY ("LoteAvesId") REFERENCES "LotesAves"("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_RegistrosPesoLoteAves_LoteAvesId" ON "RegistrosPesoLoteAves"("LoteAvesId");
CREATE INDEX IF NOT EXISTS "IX_RegistrosPesoLoteAves_Fecha" ON "RegistrosPesoLoteAves"("Fecha");

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_RegistrosPesoLoteAves_PesoPromedio_Positivo') THEN
        ALTER TABLE "RegistrosPesoLoteAves" ADD CONSTRAINT "CK_RegistrosPesoLoteAves_PesoPromedio_Positivo"
        CHECK ("PesoPromedio" > 0);
    END IF;
END $$;

-- Tabla: Vacunaciones de Lote de Aves
CREATE TABLE IF NOT EXISTS "VacunacionesLoteAves" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "LoteAvesId" UUID NOT NULL,
    "NombreVacuna" VARCHAR(255) NOT NULL,
    "FechaAplicacion" DATE NOT NULL,
    "FechaProximaAplicacion" DATE,
    "CantidadAplicada" INTEGER,
    "MetodoAplicacion" VARCHAR(50),
    "Observaciones" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_VacunacionesLoteAves_LotesAves_LoteAvesId" FOREIGN KEY ("LoteAvesId") REFERENCES "LotesAves"("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_VacunacionesLoteAves_LoteAvesId" ON "VacunacionesLoteAves"("LoteAvesId");
CREATE INDEX IF NOT EXISTS "IX_VacunacionesLoteAves_FechaAplicacion" ON "VacunacionesLoteAves"("FechaAplicacion");

-- Tabla: Tratamientos de Lote de Aves
CREATE TABLE IF NOT EXISTS "TratamientosLoteAves" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "LoteAvesId" UUID NOT NULL,
    "Condicion" VARCHAR(255) NOT NULL,
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

-- ============================================================================
-- MIGRACIÓN 009: Crear tabla EventosBovino (Historial Clínico)
-- ============================================================================

CREATE TYPE tipo_evento_bovino AS ENUM (
    'VACUNACION',         -- Vacunación
    'TRATAMIENTO_MEDICO', -- Tratamiento médico
    'PESAJE',             -- Pesaje
    'MONTA_IA',           -- Monta / Inseminación Artificial
    'PALPACION',          -- Palpación (Diagnóstico de gestación)
    'PARTO',              -- Parto
    'MOVIMIENTO'          -- Movimiento (Cambio de corral)
);

CREATE TABLE IF NOT EXISTS "EventosBovino" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "BovinoId" UUID NOT NULL,
    "TipoEvento" tipo_evento_bovino NOT NULL,
    "FechaEvento" DATE NOT NULL,
    "DetallesJson" JSONB NOT NULL,
    "CostoAsociado" DECIMAL(18,2) NOT NULL,
    "RegistradoPorUserId" UUID,
    "RegistradoPorColaboradorId" UUID,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_EventosBovino_Bovinos_BovinoId" FOREIGN KEY ("BovinoId") REFERENCES "Bovinos"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_EventosBovino_Users_RegistradoPorUserId" FOREIGN KEY ("RegistradoPorUserId") REFERENCES "Users"("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_EventosBovino_Colaboradores_RegistradoPorColaboradorId" FOREIGN KEY ("RegistradoPorColaboradorId") REFERENCES "Colaboradores"("Id") ON DELETE SET NULL
);

CREATE INDEX IF NOT EXISTS "IX_EventosBovino_BovinoId" ON "EventosBovino"("BovinoId");
CREATE INDEX IF NOT EXISTS "IX_EventosBovino_TipoEvento" ON "EventosBovino"("TipoEvento");
CREATE INDEX IF NOT EXISTS "IX_EventosBovino_FechaEvento" ON "EventosBovino"("FechaEvento");
CREATE INDEX IF NOT EXISTS "IX_EventosBovino_CreatedAt" ON "EventosBovino"("CreatedAt");
CREATE INDEX IF NOT EXISTS "IX_EventosBovino_DetallesJson" ON "EventosBovino" USING GIN ("DetallesJson");

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_EventosBovino_FechaEvento_NoFutura') THEN
        ALTER TABLE "EventosBovino" ADD CONSTRAINT "CK_EventosBovino_FechaEvento_NoFutura"
        CHECK ("FechaEvento" <= CURRENT_DATE);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_EventosBovino_RegistradoPor_Exclusivo') THEN
        ALTER TABLE "EventosBovino" ADD CONSTRAINT "CK_EventosBovino_RegistradoPor_Exclusivo"
        CHECK (
            ("RegistradoPorUserId" IS NULL AND "RegistradoPorColaboradorId" IS NOT NULL) OR
            ("RegistradoPorUserId" IS NOT NULL AND "RegistradoPorColaboradorId" IS NULL) OR
            ("RegistradoPorUserId" IS NOT NULL AND "RegistradoPorColaboradorId" IS NULL)
        );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_EventosBovino_CostoAsociado_NoNegativo') THEN
        ALTER TABLE "EventosBovino" ADD CONSTRAINT "CK_EventosBovino_CostoAsociado_NoNegativo"
        CHECK ("CostoAsociado" >= 0);
    END IF;
END $$;

-- ============================================================================
-- MIGRACIÓN 010: Crear tabla BitacoraLoteAve (Registro Diario)
-- ============================================================================

CREATE TABLE IF NOT EXISTS "BitacorasLoteAve" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "LoteAvesId" UUID NOT NULL,
    "FechaRegistro" DATE NOT NULL,
    "DiaCiclo" INTEGER NOT NULL,
    "Mortalidad" INTEGER NOT NULL DEFAULT 0,
    "Descarte" INTEGER NOT NULL DEFAULT 0,
    "ConsumoKg" DECIMAL(10,3) NOT NULL,
    "PesoPromedio" DECIMAL(10,3),
    "Observaciones" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_BitacorasLoteAve_LotesAves_LoteAvesId" FOREIGN KEY ("LoteAvesId") REFERENCES "LotesAves"("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_BitacorasLoteAve_LoteAvesId_FechaRegistro" ON "BitacorasLoteAve"("LoteAvesId", "FechaRegistro");
CREATE INDEX IF NOT EXISTS "IX_BitacorasLoteAve_LoteAvesId" ON "BitacorasLoteAve"("LoteAvesId");
CREATE INDEX IF NOT EXISTS "IX_BitacorasLoteAve_FechaRegistro" ON "BitacorasLoteAve"("FechaRegistro");
CREATE INDEX IF NOT EXISTS "IX_BitacorasLoteAve_DiaCiclo" ON "BitacorasLoteAve"("DiaCiclo");

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_BitacorasLoteAve_FechaRegistro_NoFutura') THEN
        ALTER TABLE "BitacorasLoteAve" ADD CONSTRAINT "CK_BitacorasLoteAve_FechaRegistro_NoFutura"
        CHECK ("FechaRegistro" <= CURRENT_DATE);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_BitacorasLoteAve_Mortalidad_NoNegativa') THEN
        ALTER TABLE "BitacorasLoteAve" ADD CONSTRAINT "CK_BitacorasLoteAve_Mortalidad_NoNegativa"
        CHECK ("Mortalidad" >= 0);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_BitacorasLoteAve_Descarte_NoNegativo') THEN
        ALTER TABLE "BitacorasLoteAve" ADD CONSTRAINT "CK_BitacorasLoteAve_Descarte_NoNegativo"
        CHECK ("Descarte" >= 0);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_BitacorasLoteAve_ConsumoKg_NoNegativo') THEN
        ALTER TABLE "BitacorasLoteAve" ADD CONSTRAINT "CK_BitacorasLoteAve_ConsumoKg_NoNegativo"
        CHECK ("ConsumoKg" >= 0);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_BitacorasLoteAve_PesoPromedio_Positivo') THEN
        ALTER TABLE "BitacorasLoteAve" ADD CONSTRAINT "CK_BitacorasLoteAve_PesoPromedio_Positivo"
        CHECK ("PesoPromedio" IS NULL OR "PesoPromedio" > 0);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_constraint WHERE conname = 'CK_BitacorasLoteAve_DiaCiclo_NoNegativo') THEN
        ALTER TABLE "BitacorasLoteAve" ADD CONSTRAINT "CK_BitacorasLoteAve_DiaCiclo_NoNegativo"
        CHECK ("DiaCiclo" >= 0);
    END IF;
END $$;

-- ============================================================================
-- FIN DEL SCRIPT CONSOLIDADO
-- ============================================================================
