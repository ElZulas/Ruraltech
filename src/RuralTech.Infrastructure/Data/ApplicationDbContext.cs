using Microsoft.EntityFrameworkCore;
using RuralTech.Core.Entities;

namespace RuralTech.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UPP> UPPs { get; set; }
    public DbSet<Colaborador> Colaboradores { get; set; }
    public DbSet<Animal> Animals { get; set; }
    public DbSet<Vaccine> Vaccines { get; set; }
    public DbSet<Treatment> Treatments { get; set; }
    public DbSet<WeightRecord> WeightRecords { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(150);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.DateOfBirth).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                .WithMany(u => u.Animals)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.UPP)
                .WithMany(u => u.Animals)
                .HasForeignKey(e => e.UPPId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Breed).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<Vaccine>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Animal)
                .WithMany(a => a.Vaccines)
                .HasForeignKey(e => e.AnimalId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<Treatment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Animal)
                .WithMany(a => a.Treatments)
                .HasForeignKey(e => e.AnimalId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WeightRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Animal)
                .WithMany(a => a.WeightHistory)
                .HasForeignKey(e => e.AnimalId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Seller)
                .WithMany(u => u.Products)
                .HasForeignKey(e => e.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<UPP>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ClavePGN).IsUnique();
            entity.HasIndex(e => e.CodigoQRAcceso).IsUnique();
            entity.HasOne(e => e.Owner)
                .WithMany(u => u.UPPs)
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.ClavePGN).IsRequired().HasMaxLength(20);
            entity.Property(e => e.NombrePredio).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PropietarioLegal).IsRequired().HasMaxLength(150);
            entity.Property(e => e.CodigoQRAcceso).IsRequired().HasMaxLength(15);
            entity.Property(e => e.EstadoMX).IsRequired().HasMaxLength(2).IsFixedLength();
            entity.Property(e => e.Latitude).HasPrecision(10, 8);
            entity.Property(e => e.Longitude).HasPrecision(11, 8);
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<Colaborador>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.UPP)
                .WithMany(u => u.Colaboradores)
                .HasForeignKey(e => e.UPPId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.NombreAlias).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PinAccesoHash).IsRequired().HasMaxLength(255);
            entity.Property(e => e.TelefonoContacto).HasMaxLength(20);
            entity.Property(e => e.Rol).IsRequired().HasConversion<string>();
            entity.Property(e => e.Estatus).IsRequired().HasConversion<string>();
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
}
