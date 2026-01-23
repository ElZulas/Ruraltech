using Microsoft.EntityFrameworkCore;
using RuralTech.Core.Entities;
using RuralTech.Infrastructure.Data;
using RuralTech.Infrastructure.Services;

namespace RuralTech.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        // Verificar si ya hay datos
        if (context.Users.Any())
        {
            return; // Ya hay datos, no hacer seed
        }

        // Crear usuario de prueba
        var testUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "demo@ruraltech.com",
            PasswordHash = PasswordHasher.HashPassword("Demo123!"),
            FullName = "Usuario Demo",
            DateOfBirth = new DateTime(1990, 1, 1), // Mayor de 18 años
            Phone = "+57 300 123 4567",
            Location = "Cundinamarca, Colombia",
            CreatedAt = DateTime.UtcNow
        };
        context.Users.Add(testUser);
        context.SaveChanges();

        // Crear animales de prueba
        var animals = new List<Animal>
        {
            new Animal
            {
                Name = "Luna",
                Breed = "Holstein",
                BirthDate = new DateTime(2022, 3, 15),
                Sex = "Hembra",
                CurrentWeight = 450,
                LastVaccineDate = new DateTime(2024, 12, 10),
                UserId = testUser.Id,
                CreatedAt = DateTime.UtcNow
            },
            new Animal
            {
                Name = "Toro Max",
                Breed = "Angus",
                BirthDate = new DateTime(2021, 8, 20),
                Sex = "Macho",
                CurrentWeight = 680,
                LastVaccineDate = new DateTime(2024, 10, 5),
                UserId = testUser.Id,
                CreatedAt = DateTime.UtcNow
            },
            new Animal
            {
                Name = "Esperanza",
                Breed = "Jersey",
                BirthDate = new DateTime(2023, 1, 10),
                Sex = "Hembra",
                CurrentWeight = 320,
                LastVaccineDate = new DateTime(2024, 11, 15),
                UserId = testUser.Id,
                CreatedAt = DateTime.UtcNow
            }
        };

        foreach (var animal in animals)
        {
            context.Animals.Add(animal);
            context.SaveChanges();

            // Agregar historial de peso
            var weightRecords = new List<WeightRecord>
            {
                new WeightRecord { AnimalId = animal.Id, Weight = animal.CurrentWeight - 50, Date = animal.BirthDate.AddMonths(6), CreatedAt = DateTime.UtcNow },
                new WeightRecord { AnimalId = animal.Id, Weight = animal.CurrentWeight - 30, Date = animal.BirthDate.AddMonths(12), CreatedAt = DateTime.UtcNow },
                new WeightRecord { AnimalId = animal.Id, Weight = animal.CurrentWeight - 15, Date = DateTime.UtcNow.AddMonths(-3), CreatedAt = DateTime.UtcNow },
                new WeightRecord { AnimalId = animal.Id, Weight = animal.CurrentWeight, Date = DateTime.UtcNow, CreatedAt = DateTime.UtcNow }
            };
            context.WeightRecords.AddRange(weightRecords);

            // Agregar vacunas
            var vaccines = new List<Vaccine>
            {
                new Vaccine
                {
                    AnimalId = animal.Id,
                    Name = "Brucelosis",
                    DateApplied = animal.LastVaccineDate.Value,
                    NextDueDate = animal.LastVaccineDate.Value.AddMonths(6),
                    CreatedAt = DateTime.UtcNow
                },
                new Vaccine
                {
                    AnimalId = animal.Id,
                    Name = "Fiebre Aftosa",
                    DateApplied = animal.LastVaccineDate.Value.AddMonths(-1),
                    NextDueDate = animal.LastVaccineDate.Value.AddMonths(5),
                    CreatedAt = DateTime.UtcNow
                }
            };
            context.Vaccines.AddRange(vaccines);
        }

        context.SaveChanges();

        // Crear productos de prueba
        var products = new List<Product>
        {
            new Product
            {
                Name = "Toro Angus Semental",
                Description = "Toro Angus de 3 años, excelente genética y certificado",
                Price = 25000000,
                Category = "Ganado",
                SellerId = testUser.Id,
                Location = "Cundinamarca",
                Phone = "+57 300 123 4567",
                WhatsApp = "+57 300 123 4567",
                Rating = 4.8m,
                ReviewCount = 12,
                IsFeatured = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "Ivermectina 1% - 500ml",
                Description = "Desparasitante de amplio espectro para bovinos",
                Price = 45000,
                Category = "Medicinas",
                SellerId = testUser.Id,
                Location = "Bogotá",
                Phone = "+57 301 987 6543",
                Rating = 4.9m,
                ReviewCount = 34,
                IsFeatured = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "Báscula Ganadera 2000kg",
                Description = "Báscula digital para ganado, alta precisión",
                Price = 1200000,
                Category = "Herramientas",
                SellerId = testUser.Id,
                Location = "Medellín",
                Phone = "+57 302 456 7890",
                Rating = 4.7m,
                ReviewCount = 8,
                IsFeatured = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };
        context.Products.AddRange(products);
        context.SaveChanges();
    }
}
