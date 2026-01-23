-- Migración: Crear tabla UPP (Unidad de Producción Pecuaria)
-- Ejecuta este script en el SQL Editor de Supabase

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

-- Índices únicos
CREATE UNIQUE INDEX IF NOT EXISTS "IX_UPPs_ClavePGN" ON "UPPs"("ClavePGN");
CREATE UNIQUE INDEX IF NOT EXISTS "IX_UPPs_CodigoQRAcceso" ON "UPPs"("CodigoQRAcceso");

-- Índice para búsquedas por propietario
CREATE INDEX IF NOT EXISTS "IX_UPPs_OwnerId" ON "UPPs"("OwnerId");

-- Actualizar tabla Animals para agregar relación con UPP
ALTER TABLE "Animals" ADD COLUMN IF NOT EXISTS "UPPId" UUID;
ALTER TABLE "Animals" ADD CONSTRAINT "FK_Animals_UPPs_UPPId" 
    FOREIGN KEY ("UPPId") REFERENCES "UPPs"("Id") ON DELETE SET NULL;

CREATE INDEX IF NOT EXISTS "IX_Animals_UPPId" ON "Animals"("UPPId");
