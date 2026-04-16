using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Contexts;

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
            entity.Property(e => e.VehicleId)
            .IsRequired();
            entity.Property(e => e.DestinationId)
                .IsRequired();
            entity.Property(t => t.Id)
                .ValueGeneratedNever();

            entity.HasOne<Vehicle>()
                  .WithMany(v => v.Travels)
                  .HasForeignKey(t => t.VehicleId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(t => new { t.VehicleId, t.End, t.Start });
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(v => v.Id);

            entity.Property(v => v.Plate)
                .IsRequired()
                .HasMaxLength(10);

            // necessário por causa do campo privado (_travels)
            entity.Navigation(v => v.Travels)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        modelBuilder.Entity<VehicleType>(entity =>
        {
            entity.HasKey(v => v.Id);
        });

    }
}
