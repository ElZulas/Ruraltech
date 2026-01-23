-- Migración: Crear tabla Colaboradores
-- Ejecuta este script en el SQL Editor de Supabase

-- Crear tipos ENUM para rol y estatus
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

-- Índice para búsquedas por UPP
CREATE INDEX IF NOT EXISTS "IX_Colaboradores_UPPId" ON "Colaboradores"("UPPId");
