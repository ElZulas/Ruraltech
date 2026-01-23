-- Migración inicial para Supabase PostgreSQL
-- Ejecuta este script en el SQL Editor de Supabase

-- Tabla Users
CREATE TABLE IF NOT EXISTS "Users" (
    "Id" SERIAL PRIMARY KEY,
    "Email" TEXT NOT NULL UNIQUE,
    "PasswordHash" TEXT NOT NULL,
    "FullName" TEXT,
    "Phone" TEXT,
    "Location" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW()
);

-- Tabla Animals
CREATE TABLE IF NOT EXISTS "Animals" (
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Breed" TEXT,
    "BirthDate" DATE,
    "Sex" TEXT,
    "CurrentWeight" DECIMAL(10,2),
    "LastVaccineDate" DATE,
    "UserId" INTEGER NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Animals_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users"("Id") ON DELETE CASCADE
);

-- Tabla WeightRecords
CREATE TABLE IF NOT EXISTS "WeightRecords" (
    "Id" SERIAL PRIMARY KEY,
    "AnimalId" INTEGER NOT NULL,
    "Weight" DECIMAL(10,2) NOT NULL,
    "Date" DATE NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_WeightRecords_Animals_AnimalId" FOREIGN KEY ("AnimalId") REFERENCES "Animals"("Id") ON DELETE CASCADE
);

-- Tabla Vaccines
CREATE TABLE IF NOT EXISTS "Vaccines" (
    "Id" SERIAL PRIMARY KEY,
    "AnimalId" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "DateApplied" DATE NOT NULL,
    "NextDueDate" DATE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Vaccines_Animals_AnimalId" FOREIGN KEY ("AnimalId") REFERENCES "Animals"("Id") ON DELETE CASCADE
);

-- Tabla Treatments
CREATE TABLE IF NOT EXISTS "Treatments" (
    "Id" SERIAL PRIMARY KEY,
    "AnimalId" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "DateApplied" DATE NOT NULL,
    "Notes" TEXT,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Treatments_Animals_AnimalId" FOREIGN KEY ("AnimalId") REFERENCES "Animals"("Id") ON DELETE CASCADE
);

-- Tabla Products
CREATE TABLE IF NOT EXISTS "Products" (
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL,
    "Description" TEXT,
    "Price" DECIMAL(18,2) NOT NULL,
    "Category" TEXT,
    "SellerId" INTEGER NOT NULL,
    "Location" TEXT,
    "Phone" TEXT,
    "WhatsApp" TEXT,
    "Rating" DECIMAL(3,2),
    "ReviewCount" INTEGER DEFAULT 0,
    "IsFeatured" BOOLEAN DEFAULT FALSE,
    "IsActive" BOOLEAN DEFAULT TRUE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT NOW(),
    CONSTRAINT "FK_Products_Users_SellerId" FOREIGN KEY ("SellerId") REFERENCES "Users"("Id") ON DELETE CASCADE
);

-- Índices para mejor rendimiento
CREATE INDEX IF NOT EXISTS "IX_Animals_UserId" ON "Animals"("UserId");
CREATE INDEX IF NOT EXISTS "IX_WeightRecords_AnimalId" ON "WeightRecords"("AnimalId");
CREATE INDEX IF NOT EXISTS "IX_Vaccines_AnimalId" ON "Vaccines"("AnimalId");
CREATE INDEX IF NOT EXISTS "IX_Treatments_AnimalId" ON "Treatments"("AnimalId");
CREATE INDEX IF NOT EXISTS "IX_Products_SellerId" ON "Products"("SellerId");

-- Datos de prueba (opcional)
INSERT INTO "Users" ("Email", "PasswordHash", "FullName", "Phone", "Location", "CreatedAt")
VALUES 
    ('demo@ruraltech.com', '$2a$11$KIXxKIXxKIXxKIXxKIXxOu', 'Usuario Demo', '+57 300 123 4567', 'Cundinamarca, Colombia', NOW())
ON CONFLICT ("Email") DO NOTHING;
