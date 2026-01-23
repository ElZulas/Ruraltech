-- Migración: Actualizar tabla Users para USUARIO_PROPIETARIO
-- Ejecuta este script en el SQL Editor de Supabase
-- IMPORTANTE: Si ya tienes datos, primero haz backup o elimina los datos existentes

-- Paso 1: Eliminar constraints y foreign keys que dependen de Users.Id
ALTER TABLE "Animals" DROP CONSTRAINT IF EXISTS "FK_Animals_Users_UserId";
ALTER TABLE "Products" DROP CONSTRAINT IF EXISTS "FK_Products_Users_SellerId";

-- Paso 2: Eliminar índices
DROP INDEX IF EXISTS "IX_Animals_UserId";
DROP INDEX IF EXISTS "IX_Products_SellerId";

-- Paso 3: Eliminar la columna Id antigua y crear una nueva como UUID
ALTER TABLE "Users" DROP CONSTRAINT IF EXISTS "PK_Users";
ALTER TABLE "Users" DROP COLUMN IF EXISTS "Id" CASCADE;

-- Paso 4: Crear nueva columna Id como UUID
ALTER TABLE "Users" ADD COLUMN "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid();

-- Paso 5: Agregar campo fecha_nacimiento
ALTER TABLE "Users" ADD COLUMN IF NOT EXISTS "DateOfBirth" DATE NOT NULL DEFAULT '1990-01-01';

-- Paso 6: Actualizar restricciones de longitud
ALTER TABLE "Users" ALTER COLUMN "Email" TYPE VARCHAR(100);
ALTER TABLE "Users" ALTER COLUMN "FullName" TYPE VARCHAR(150);
ALTER TABLE "Users" ALTER COLUMN "Phone" TYPE VARCHAR(20);
ALTER TABLE "Users" ALTER COLUMN "PasswordHash" TYPE VARCHAR(255);

-- Paso 7: Actualizar tablas dependientes para usar UUID
-- Animals
ALTER TABLE "Animals" DROP COLUMN IF EXISTS "UserId";
ALTER TABLE "Animals" ADD COLUMN "UserId" UUID;
ALTER TABLE "Animals" ADD CONSTRAINT "FK_Animals_Users_UserId" 
    FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE;

-- Products
ALTER TABLE "Products" DROP COLUMN IF EXISTS "SellerId";
ALTER TABLE "Products" ADD COLUMN "SellerId" UUID;
ALTER TABLE "Products" ADD CONSTRAINT "FK_Products_Users_SellerId" 
    FOREIGN KEY ("SellerId") REFERENCES "Users"("Id") ON DELETE CASCADE;

-- Paso 8: Recrear índices
CREATE INDEX IF NOT EXISTS "IX_Animals_UserId" ON "Animals"("UserId");
CREATE INDEX IF NOT EXISTS "IX_Products_SellerId" ON "Products"("SellerId");
