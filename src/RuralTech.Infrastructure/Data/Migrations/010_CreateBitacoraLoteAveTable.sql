-- Migración: Crear tabla BitacoraLoteAve (Registro Diario)
-- Ejecuta este script en el SQL Editor de Supabase
-- Este módulo registra el desempeño diario del grupo de aves (acumulación cada 24 horas)

-- Crear tabla BitacoraLoteAve
CREATE TABLE IF NOT EXISTS "BitacorasLoteAve" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "LoteAvesId" UUID NOT NULL,
    "FechaRegistro" DATE NOT NULL, -- Día del reporte
    "DiaCiclo" INTEGER NOT NULL, -- Edad en días del lote (calculado: FechaRegistro - FechaIngreso del lote)
    "Mortalidad" INTEGER NOT NULL DEFAULT 0, -- Cantidad de aves muertas hoy
    "Descarte" INTEGER NOT NULL DEFAULT 0, -- Aves vivas sacadas por enfermedad
    "ConsumoKg" DECIMAL(10,3) NOT NULL, -- Kilos de alimento servidos (vital para Conversión Alimenticia)
    "PesoPromedio" DECIMAL(10,3), -- Muestreo de peso (opcional, se pesan ~10 pollos y se anota el promedio)
    "Observaciones" TEXT, -- Notas del operario (opcional)
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    
    -- Foreign Key
    CONSTRAINT "FK_BitacorasLoteAve_LotesAves_LoteAvesId" FOREIGN KEY ("LoteAvesId") REFERENCES "LotesAves"("Id") ON DELETE CASCADE
);

-- Índice único compuesto: no puede haber dos registros del mismo lote para la misma fecha
CREATE UNIQUE INDEX IF NOT EXISTS "IX_BitacorasLoteAve_LoteAvesId_FechaRegistro" ON "BitacorasLoteAve"("LoteAvesId", "FechaRegistro");

-- Índices para búsquedas
CREATE INDEX IF NOT EXISTS "IX_BitacorasLoteAve_LoteAvesId" ON "BitacorasLoteAve"("LoteAvesId");
CREATE INDEX IF NOT EXISTS "IX_BitacorasLoteAve_FechaRegistro" ON "BitacorasLoteAve"("FechaRegistro");
CREATE INDEX IF NOT EXISTS "IX_BitacorasLoteAve_DiaCiclo" ON "BitacorasLoteAve"("DiaCiclo");

-- Constraint para validar que fecha del registro no sea futura
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_BitacorasLoteAve_FechaRegistro_NoFutura'
    ) THEN
        ALTER TABLE "BitacorasLoteAve"
        ADD CONSTRAINT "CK_BitacorasLoteAve_FechaRegistro_NoFutura"
        CHECK ("FechaRegistro" <= CURRENT_DATE);
    END IF;
END $$;

-- Constraint para validar que mortalidad y descarte no sean negativos
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_BitacorasLoteAve_Mortalidad_NoNegativa'
    ) THEN
        ALTER TABLE "BitacorasLoteAve"
        ADD CONSTRAINT "CK_BitacorasLoteAve_Mortalidad_NoNegativa"
        CHECK ("Mortalidad" >= 0);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_BitacorasLoteAve_Descarte_NoNegativo'
    ) THEN
        ALTER TABLE "BitacorasLoteAve"
        ADD CONSTRAINT "CK_BitacorasLoteAve_Descarte_NoNegativo"
        CHECK ("Descarte" >= 0);
    END IF;
END $$;

-- Constraint para validar consumo no negativo
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_BitacorasLoteAve_ConsumoKg_NoNegativo'
    ) THEN
        ALTER TABLE "BitacorasLoteAve"
        ADD CONSTRAINT "CK_BitacorasLoteAve_ConsumoKg_NoNegativo"
        CHECK ("ConsumoKg" >= 0);
    END IF;
END $$;

-- Constraint para validar peso promedio positivo (si se proporciona)
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_BitacorasLoteAve_PesoPromedio_Positivo'
    ) THEN
        ALTER TABLE "BitacorasLoteAve"
        ADD CONSTRAINT "CK_BitacorasLoteAve_PesoPromedio_Positivo"
        CHECK ("PesoPromedio" IS NULL OR "PesoPromedio" > 0);
    END IF;
END $$;

-- Constraint para validar que día del ciclo no sea negativo
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_BitacorasLoteAve_DiaCiclo_NoNegativo'
    ) THEN
        ALTER TABLE "BitacorasLoteAve"
        ADD CONSTRAINT "CK_BitacorasLoteAve_DiaCiclo_NoNegativo"
        CHECK ("DiaCiclo" >= 0);
    END IF;
END $$;

-- Comentarios para documentación
COMMENT ON TABLE "BitacorasLoteAve" IS 'Registro diario de acumulación para el control de lotes. En avicultura no se registran eventos por pollo, sino el desempeño del grupo cada 24 horas';
COMMENT ON COLUMN "BitacorasLoteAve"."FechaRegistro" IS 'Día del reporte. No puede haber dos registros del mismo lote para la misma fecha';
COMMENT ON COLUMN "BitacorasLoteAve"."DiaCiclo" IS 'Edad en días del lote. Calculado: FechaRegistro - FechaIngreso del lote';
COMMENT ON COLUMN "BitacorasLoteAve"."Mortalidad" IS 'Cantidad de aves muertas hoy. Default: 0. Al guardar, debe restar al CantidadActual del Lote';
COMMENT ON COLUMN "BitacorasLoteAve"."Descarte" IS 'Aves vivas sacadas por enfermedad. Default: 0. Resta al inventario';
COMMENT ON COLUMN "BitacorasLoteAve"."ConsumoKg" IS 'Kilos de alimento servidos. Vital para calcular la Conversión Alimenticia';
COMMENT ON COLUMN "BitacorasLoteAve"."PesoPromedio" IS 'Muestreo de peso (si se hizo). Se pesan ~10 pollos y se anota el promedio';
COMMENT ON COLUMN "BitacorasLoteAve"."Observaciones" IS 'Notas del operario. Ejemplos: "Se fue la luz", "Mucho calor", etc.';
