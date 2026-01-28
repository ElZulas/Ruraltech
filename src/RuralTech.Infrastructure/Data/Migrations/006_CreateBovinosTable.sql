-- Migración: Crear tabla Bovinos
-- Ejecuta este script en el SQL Editor de Supabase
-- Este módulo gestiona los activos biológicos de la UPP con trazabilidad individual estricta (SINIIGA)

-- Crear tipos ENUM para estado productivo y estatus del sistema
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

-- Crear tabla Bovinos
CREATE TABLE IF NOT EXISTS "Bovinos" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "UPPId" UUID NOT NULL,
    "InfraestructuraId" UUID,
    
    -- Identificadores
    "AreteSiniiga" VARCHAR(14), -- Formato: MX + 00 + Estado (2 letras) + 8 dígitos consecutivos (opcional al nacer, obligatorio para ventas)
    "AreteTrabajo" VARCHAR(10) NOT NULL, -- Número visual corto (Chapa) - REQUERIDO
    
    -- Datos básicos
    "Nombre" VARCHAR(50), -- Apodo del animal (opcional)
    "FechaNacimiento" DATE NOT NULL, -- Fecha real o estimada - REQUERIDO
    "Sexo" CHAR(1) NOT NULL, -- M (Macho) o H (Hembra), inmutable - REQUERIDO
    "RazaPredominante" VARCHAR(50) NOT NULL, -- Viene de Catálogo - REQUERIDO
    
    -- Relaciones parentales (recursivas)
    "MadreId" UUID, -- FK recursiva -> Bovino (opcional)
    "PadreId" UUID, -- FK recursiva -> Bovino (opcional)
    
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

-- Índice único para AreteSiniiga (solo cuando no es NULL)
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Bovinos_AreteSiniiga" 
ON "Bovinos"("AreteSiniiga") 
WHERE "AreteSiniiga" IS NOT NULL;

-- Índices para búsquedas por UPP
CREATE INDEX IF NOT EXISTS "IX_Bovinos_UPPId" ON "Bovinos"("UPPId");

-- Índices para búsquedas por infraestructura
CREATE INDEX IF NOT EXISTS "IX_Bovinos_InfraestructuraId" ON "Bovinos"("InfraestructuraId");

-- Índices para relaciones parentales (búsquedas de descendencia)
CREATE INDEX IF NOT EXISTS "IX_Bovinos_MadreId" ON "Bovinos"("MadreId");
CREATE INDEX IF NOT EXISTS "IX_Bovinos_PadreId" ON "Bovinos"("PadreId");

-- Índices para búsquedas por estado y estatus
CREATE INDEX IF NOT EXISTS "IX_Bovinos_EstadoProductivo" ON "Bovinos"("EstadoProductivo");
CREATE INDEX IF NOT EXISTS "IX_Bovinos_EstatusSistema" ON "Bovinos"("EstatusSistema");

-- Índice para búsquedas por arete de trabajo
CREATE INDEX IF NOT EXISTS "IX_Bovinos_AreteTrabajo" ON "Bovinos"("AreteTrabajo");

-- Constraints para validaciones (solo si no existen)
-- Nota: La validación principal se hace en el backend, pero esto ayuda a nivel de BD

-- Constraint para validar formato de AreteSiniiga
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_Bovinos_AreteSiniiga_Formato'
    ) THEN
        ALTER TABLE "Bovinos"
        ADD CONSTRAINT "CK_Bovinos_AreteSiniiga_Formato"
        CHECK (
            "AreteSiniiga" IS NULL OR 
            (
                LENGTH("AreteSiniiga") = 14 AND
                SUBSTRING("AreteSiniiga", 1, 2) = 'MX' AND
                SUBSTRING("AreteSiniiga", 3, 2) = '00' AND
                SUBSTRING("AreteSiniiga", 5, 2) ~ '^[A-Z]{2}$' AND
                SUBSTRING("AreteSiniiga", 7, 8) ~ '^\d{8}$'
            )
        );
    END IF;
END $$;

-- Constraint para validar que Sexo sea M o H
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_Bovinos_Sexo'
    ) THEN
        ALTER TABLE "Bovinos"
        ADD CONSTRAINT "CK_Bovinos_Sexo"
        CHECK ("Sexo" IS NULL OR "Sexo" IN ('M', 'H'));
    END IF;
END $$;

-- Constraint para validar que un animal no sea su propia madre (solo si se especifica)
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_Bovinos_MadreId_Diferente'
    ) THEN
        ALTER TABLE "Bovinos"
        ADD CONSTRAINT "CK_Bovinos_MadreId_Diferente"
        CHECK ("MadreId" IS NULL OR "Id" != "MadreId");
    END IF;
END $$;

-- Constraint para validar que un animal no sea su propio padre
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_Bovinos_PadreId_Diferente'
    ) THEN
        ALTER TABLE "Bovinos"
        ADD CONSTRAINT "CK_Bovinos_PadreId_Diferente"
        CHECK ("PadreId" IS NULL OR "Id" != "PadreId");
    END IF;
END $$;

-- Constraint para validar que padre y madre no sean el mismo animal
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_Bovinos_Padre_Madre_Diferentes'
    ) THEN
        ALTER TABLE "Bovinos"
        ADD CONSTRAINT "CK_Bovinos_Padre_Madre_Diferentes"
        CHECK ("PadreId" IS NULL OR "PadreId" != "MadreId");
    END IF;
END $$;

-- Constraint para validar que fecha de nacimiento no sea futura
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_Bovinos_FechaNacimiento_NoFutura'
    ) THEN
        ALTER TABLE "Bovinos"
        ADD CONSTRAINT "CK_Bovinos_FechaNacimiento_NoFutura"
        CHECK ("FechaNacimiento" IS NULL OR "FechaNacimiento" <= CURRENT_DATE);
    END IF;
END $$;

-- Comentarios para documentación
COMMENT ON TABLE "Bovinos" IS 'Representa a un animal mayor (Vaca, Toro, Becerro) que requiere trazabilidad individual estricta según SINIIGA';
COMMENT ON COLUMN "Bovinos"."AreteSiniiga" IS 'Identificador Oficial Nacional. Formato: MX (País) + 00 (Especie) + XX (Estado) + 00000000 (Consecutivo). Opcional al nacer, obligatorio para ventas';
COMMENT ON COLUMN "Bovinos"."AreteTrabajo" IS 'Número visual corto (Chapa) usado para manejo diario. Ej: "504"';
COMMENT ON COLUMN "Bovinos"."Nombre" IS 'Apodo del animal. Ej: "La Prieta"';
COMMENT ON COLUMN "Bovinos"."FechaNacimiento" IS 'Fecha real o estimada. Vital para calcular edad y curvas de crecimiento. No puede ser futura';
COMMENT ON COLUMN "Bovinos"."Sexo" IS 'Género biológico: M (Macho) o H (Hembra). Inmutable después de guardado';
COMMENT ON COLUMN "Bovinos"."RazaPredominante" IS 'Genética principal. Viene de Catálogo. Ej: "Brahman"';
COMMENT ON COLUMN "Bovinos"."MadreId" IS 'Madre biológica. FK Recursiva -> Bovino. El ID no puede ser el mismo que el del animal';
COMMENT ON COLUMN "Bovinos"."PadreId" IS 'Padre biológico. FK Recursiva -> Bovino (opcional)';
COMMENT ON COLUMN "Bovinos"."EstadoProductivo" IS 'Etapa de vida actual del animal';
COMMENT ON COLUMN "Bovinos"."EstatusSistema" IS 'Disponibilidad en inventario. ACTIVO = En rancho';
