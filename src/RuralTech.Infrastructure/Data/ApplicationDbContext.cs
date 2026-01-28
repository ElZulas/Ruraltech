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
    public DbSet<Infraestructura> Infraestructuras { get; set; }
    public DbSet<Bovino> Bovinos { get; set; }
    public DbSet<EventoBovino> EventosBovino { get; set; }
    public DbSet<LoteAves> LotesAves { get; set; }
    public DbSet<RegistroMortalidadAves> RegistrosMortalidadAves { get; set; }
    public DbSet<RegistroPesoLoteAves> RegistrosPesoLoteAves { get; set; }
    public DbSet<VacunacionLoteAves> VacunacionesLoteAves { get; set; }
    public DbSet<TratamientoLoteAves> TratamientosLoteAves { get; set; }
    public DbSet<BitacoraLoteAve> BitacorasLoteAve { get; set; }
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
            entity.HasOne(e => e.Infraestructura)
                .WithMany(i => i.Animals)
                .HasForeignKey(e => e.InfraestructuraId)
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
            entity.HasOne(e => e.Bovino)
                .WithMany(b => b.WeightHistory)
                .HasForeignKey(e => e.BovinoId)
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

        modelBuilder.Entity<Infraestructura>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.UPP)
                .WithMany(u => u.Infraestructuras)
                .HasForeignKey(e => e.UPPId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TipoInstalacion).IsRequired().HasConversion<string>();
            entity.Property(e => e.CapacidadMaxima).IsRequired();
            entity.Property(e => e.SuperficieHectareas).HasPrecision(5, 2);
            entity.Property(e => e.Estatus).IsRequired().HasConversion<string>();
            entity.Property(e => e.CreatedAt).IsRequired();
            
            // Índice único compuesto: nombre único dentro de la misma UPP
            entity.HasIndex(e => new { e.UPPId, e.Nombre }).IsUnique();
        });

        modelBuilder.Entity<Bovino>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Relación con UPP
            entity.HasOne(e => e.UPP)
                .WithMany(u => u.Bovinos)
                .HasForeignKey(e => e.UPPId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Relación con Infraestructura (opcional)
            entity.HasOne(e => e.Infraestructura)
                .WithMany()
                .HasForeignKey(e => e.InfraestructuraId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Relaciones recursivas (parentales)
            entity.HasOne(e => e.Madre)
                .WithMany()
                .HasForeignKey(e => e.MadreId)
                .OnDelete(DeleteBehavior.Restrict); // No eliminar si tiene hijos
            
            entity.HasOne(e => e.Padre)
                .WithMany()
                .HasForeignKey(e => e.PadreId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Propiedades
            entity.Property(e => e.Nombre).HasMaxLength(50); // Opcional
            entity.Property(e => e.AreteSiniiga).HasMaxLength(14);
            entity.HasIndex(e => e.AreteSiniiga).IsUnique().HasFilter("\"AreteSiniiga\" IS NOT NULL");
            entity.Property(e => e.AreteTrabajo).IsRequired().HasMaxLength(10); // Requerido
            entity.Property(e => e.RazaPredominante).IsRequired().HasMaxLength(50); // Requerido
            entity.Property(e => e.Sexo).IsRequired().HasMaxLength(1).IsFixedLength(); // Requerido
            entity.Property(e => e.FechaNacimiento).IsRequired(); // Requerido
            entity.Property(e => e.EstadoProductivo).IsRequired().HasConversion<string>();
            entity.Property(e => e.EstatusSistema).IsRequired().HasConversion<string>();
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<LoteAves>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Relación con UPP
            entity.HasOne(e => e.UPP)
                .WithMany(u => u.LotesAves)
                .HasForeignKey(e => e.UPPId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Relación con Infraestructura (debe ser NAVE_AVICOLA)
            entity.HasOne(e => e.Infraestructura)
                .WithMany()
                .HasForeignKey(e => e.InfraestructuraId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Propiedades
            entity.Property(e => e.CodigoLote).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => new { e.UPPId, e.CodigoLote }).IsUnique();
            entity.Property(e => e.NumeroLoteGranja).HasMaxLength(50);
            entity.Property(e => e.FechaIngreso).IsRequired();
            entity.Property(e => e.CantidadInicial).IsRequired();
            entity.Property(e => e.CantidadActual).IsRequired();
            entity.Property(e => e.Raza).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TipoAve).HasMaxLength(50);
            entity.Property(e => e.Estatus).IsRequired().HasConversion<string>();
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<RegistroMortalidadAves>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.LoteAves)
                .WithMany(l => l.RegistrosMortalidad)
                .HasForeignKey(e => e.LoteAvesId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.Fecha).IsRequired();
            entity.Property(e => e.CantidadMuertas).IsRequired();
            entity.Property(e => e.CausaMuerte).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<RegistroPesoLoteAves>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.LoteAves)
                .WithMany(l => l.RegistrosPeso)
                .HasForeignKey(e => e.LoteAvesId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.Fecha).IsRequired();
            entity.Property(e => e.PesoPromedio).IsRequired().HasPrecision(10, 3);
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<VacunacionLoteAves>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.LoteAves)
                .WithMany(l => l.Vacunaciones)
                .HasForeignKey(e => e.LoteAvesId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.NombreVacuna).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FechaAplicacion).IsRequired();
            entity.Property(e => e.MetodoAplicacion).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<TratamientoLoteAves>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.LoteAves)
                .WithMany(l => l.Tratamientos)
                .HasForeignKey(e => e.LoteAvesId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.Condicion).IsRequired().HasMaxLength(255);
            entity.Property(e => e.DescripcionTratamiento).IsRequired();
            entity.Property(e => e.FechaInicio).IsRequired();
            entity.Property(e => e.Medicamento).HasMaxLength(255);
            entity.Property(e => e.Dosis).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<BitacoraLoteAve>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Relación con LoteAves
            entity.HasOne(e => e.LoteAves)
                .WithMany(l => l.Bitacoras)
                .HasForeignKey(e => e.LoteAvesId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Propiedades
            entity.Property(e => e.FechaRegistro).IsRequired();
            entity.Property(e => e.DiaCiclo).IsRequired();
            entity.Property(e => e.Mortalidad).IsRequired().HasDefaultValue(0);
            entity.Property(e => e.Descarte).IsRequired().HasDefaultValue(0);
            entity.Property(e => e.ConsumoKg).IsRequired().HasPrecision(10, 3);
            entity.Property(e => e.PesoPromedio).HasPrecision(10, 3);
            entity.Property(e => e.Observaciones).HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).IsRequired();
            
            // Índice único compuesto: no puede haber dos registros del mismo lote para la misma fecha
            entity.HasIndex(e => new { e.LoteAvesId, e.FechaRegistro }).IsUnique();
            
            // Índices para búsquedas
            entity.HasIndex(e => e.LoteAvesId);
            entity.HasIndex(e => e.FechaRegistro);
        });

        modelBuilder.Entity<EventoBovino>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Relación con Bovino
            entity.HasOne(e => e.Bovino)
                .WithMany(b => b.Eventos)
                .HasForeignKey(e => e.BovinoId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Relación con User (si lo registró un propietario)
            entity.HasOne(e => e.RegistradoPorUser)
                .WithMany()
                .HasForeignKey(e => e.RegistradoPorUserId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Relación con Colaborador (si lo registró un colaborador)
            entity.HasOne(e => e.RegistradoPorColaborador)
                .WithMany()
                .HasForeignKey(e => e.RegistradoPorColaboradorId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Propiedades
            entity.Property(e => e.TipoEvento).IsRequired().HasConversion<string>();
            entity.Property(e => e.FechaEvento).IsRequired();
            entity.Property(e => e.DetallesJson).IsRequired().HasColumnType("jsonb"); // JSONB en PostgreSQL
            entity.Property(e => e.CostoAsociado).IsRequired().HasPrecision(18, 2);
            entity.Property(e => e.CreatedAt).IsRequired();
            
            // Índices
            entity.HasIndex(e => e.BovinoId);
            entity.HasIndex(e => e.TipoEvento);
            entity.HasIndex(e => e.FechaEvento);
            entity.HasIndex(e => e.CreatedAt);
        });
    }
}
