-- Migración: Crear tabla EventosBovino (Historial Clínico)
-- Ejecuta este script en el SQL Editor de Supabase
-- Este módulo registra eventos clínicos inmutables de bovinos con detalles flexibles en JSON

-- Crear tipo ENUM para tipo de evento
CREATE TYPE tipo_evento_bovino AS ENUM (
    'VACUNACION',         -- Vacunación
    'TRATAMIENTO_MEDICO', -- Tratamiento médico
    'PESAJE',             -- Pesaje
    'MONTA_IA',           -- Monta / Inseminación Artificial
    'PALPACION',          -- Palpación (Diagnóstico de gestación)
    'PARTO',              -- Parto
    'MOVIMIENTO'          -- Movimiento (Cambio de corral)
);

-- Crear tabla EventosBovino
CREATE TABLE IF NOT EXISTS "EventosBovino" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "BovinoId" UUID NOT NULL,
    "TipoEvento" tipo_evento_bovino NOT NULL,
    "FechaEvento" DATE NOT NULL, -- Fecha real del evento (puede ser diferente a CreatedAt)
    "DetallesJson" JSONB NOT NULL, -- Datos específicos del evento (flexible según tipo)
    "CostoAsociado" DECIMAL(18,2) NOT NULL, -- Gasto realizado (Medicina/Veterinario)
    "RegistradoPorUserId" UUID, -- Usuario que registró (opcional, para auditoría)
    "RegistradoPorColaboradorId" UUID, -- Colaborador que registró (opcional, para auditoría)
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(), -- Fecha de registro en el sistema
    
    -- Foreign Keys
    CONSTRAINT "FK_EventosBovino_Bovinos_BovinoId" FOREIGN KEY ("BovinoId") REFERENCES "Bovinos"("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_EventosBovino_Users_RegistradoPorUserId" FOREIGN KEY ("RegistradoPorUserId") REFERENCES "Users"("Id") ON DELETE SET NULL,
    CONSTRAINT "FK_EventosBovino_Colaboradores_RegistradoPorColaboradorId" FOREIGN KEY ("RegistradoPorColaboradorId") REFERENCES "Colaboradores"("Id") ON DELETE SET NULL
);

-- Índices para búsquedas
CREATE INDEX IF NOT EXISTS "IX_EventosBovino_BovinoId" ON "EventosBovino"("BovinoId");
CREATE INDEX IF NOT EXISTS "IX_EventosBovino_TipoEvento" ON "EventosBovino"("TipoEvento");
CREATE INDEX IF NOT EXISTS "IX_EventosBovino_FechaEvento" ON "EventosBovino"("FechaEvento");
CREATE INDEX IF NOT EXISTS "IX_EventosBovino_CreatedAt" ON "EventosBovino"("CreatedAt");
CREATE INDEX IF NOT EXISTS "IX_EventosBovino_RegistradoPorUserId" ON "EventosBovino"("RegistradoPorUserId");
CREATE INDEX IF NOT EXISTS "IX_EventosBovino_RegistradoPorColaboradorId" ON "EventosBovino"("RegistradoPorColaboradorId");

-- Índice GIN para búsquedas en el campo JSONB
CREATE INDEX IF NOT EXISTS "IX_EventosBovino_DetallesJson" ON "EventosBovino" USING GIN ("DetallesJson");

-- Constraint para validar que fecha del evento no sea futura
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_EventosBovino_FechaEvento_NoFutura'
    ) THEN
        ALTER TABLE "EventosBovino"
        ADD CONSTRAINT "CK_EventosBovino_FechaEvento_NoFutura"
        CHECK ("FechaEvento" <= CURRENT_DATE);
    END IF;
END $$;

-- Constraint para validar que solo uno de los dos (RegistradoPorUserId o RegistradoPorColaboradorId) esté presente
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_EventosBovino_RegistradoPor_Exclusivo'
    ) THEN
        ALTER TABLE "EventosBovino"
        ADD CONSTRAINT "CK_EventosBovino_RegistradoPor_Exclusivo"
        CHECK (
            ("RegistradoPorUserId" IS NULL AND "RegistradoPorColaboradorId" IS NOT NULL) OR
            ("RegistradoPorUserId" IS NOT NULL AND "RegistradoPorColaboradorId" IS NULL) OR
            ("RegistradoPorUserId" IS NOT NULL AND "RegistradoPorColaboradorId" IS NULL)
        );
    END IF;
END $$;

-- Constraint para validar costo no negativo
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM pg_constraint 
        WHERE conname = 'CK_EventosBovino_CostoAsociado_NoNegativo'
    ) THEN
        ALTER TABLE "EventosBovino"
        ADD CONSTRAINT "CK_EventosBovino_CostoAsociado_NoNegativo"
        CHECK ("CostoAsociado" >= 0);
    END IF;
END $$;

-- Comentarios para documentación
COMMENT ON TABLE "EventosBovino" IS 'Registro inmutable de una intervención realizada a un animal específico. Historial clínico completo del bovino';
COMMENT ON COLUMN "EventosBovino"."TipoEvento" IS 'Categoría de la acción: VACUNACION, TRATAMIENTO_MEDICO, PESAJE, MONTA_IA, PALPACION, PARTO, MOVIMIENTO';
COMMENT ON COLUMN "EventosBovino"."FechaEvento" IS 'Fecha real del evento. Puede ser diferente a CreatedAt si el ganadero lo anota días después';
COMMENT ON COLUMN "EventosBovino"."DetallesJson" IS 'Datos específicos del evento en formato JSON. Permite guardar dosis, nombres de medicamentos, kilos de peso, etc. sin alterar la estructura de la tabla';
COMMENT ON COLUMN "EventosBovino"."CostoAsociado" IS 'Gasto realizado (Medicina/Veterinario). Requerido para módulo de Finanzas';
COMMENT ON COLUMN "EventosBovino"."RegistradoPorUserId" IS 'Usuario que creó el registro (si es propietario). Vital para auditoría';
COMMENT ON COLUMN "EventosBovino"."RegistradoPorColaboradorId" IS 'Colaborador que creó el registro (si es staff). Vital para auditoría';
COMMENT ON COLUMN "EventosBovino"."CreatedAt" IS 'Fecha de registro en el sistema. Puede ser diferente a FechaEvento';

-- Ejemplos de estructura JSON para diferentes tipos de eventos:
-- VACUNACION: {"nombreVacuna": "Fiebre Aftosa", "dosis": "2ml", "lote": "LOT-2025-001", "veterinario": "Dr. García"}
-- TRATAMIENTO_MEDICO: {"medicamento": "Penicilina", "dosis": "5ml", "via": "Intramuscular", "diagnostico": "Infección respiratoria"}
-- PESAJE: {"peso": 450.5, "unidad": "kg", "condicionCorporal": "Buena"}
-- MONTA_IA: {"toro": "Toro-123", "tecnico": "Juan Pérez", "resultado": "Exitosa"}
-- PALPACION: {"resultado": "Gestante", "mesesGestacion": 5, "veterinario": "Dr. López"}
-- PARTO: {"criasNacidas": 1, "sexoCria": "M", "pesoCria": 35.2, "complicaciones": "Ninguna"}
-- MOVIMIENTO: {"infraestructuraOrigen": "Corral 1", "infraestructuraDestino": "Potrero Norte", "razon": "Rotación de pastos"}
