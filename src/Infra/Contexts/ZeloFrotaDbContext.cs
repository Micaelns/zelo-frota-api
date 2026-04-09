using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Contexts;

public class ZeloFrotaDbContext(DbContextOptions<ZeloFrotaDbContext> options) : DbContext(options)
{
    public DbSet<Destination> Destinations => Set<Destination>();
    public DbSet<VehicleType> VehicleTypes => Set<VehicleType>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Travel> Travels => Set<Travel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Destination>(entity =>
        {
            entity.HasKey(v => v.Id);
        });

        modelBuilder.Entity<Travel>(entity =>
        {
            entity.HasKey(v => v.Id);
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(v => v.Id);

            entity.Property(v => v.Plate)
                .IsRequired()
                .HasMaxLength(10);
        });

        modelBuilder.Entity<VehicleType>(entity =>
        {
            entity.HasKey(v => v.Id);
        });

    }
}
