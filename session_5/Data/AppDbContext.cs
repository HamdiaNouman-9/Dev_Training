namespace Data;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<ParkingLot> ParkingLots { get; set; }
    public DbSet<Floor> Floors { get; set; }
    public DbSet<ParkingSpot> ParkingSpots { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<ParkingTicket> ParkingTickets { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>()
            .HasDiscriminator<string>("VehicleType")
            .HasValue<Car>("Car")
            .HasValue<Motorcycle>("Motorcycle")
            .HasValue<Truck>("Truck");

modelBuilder.Entity<ParkingSpot>()
    .HasDiscriminator<string>("SpotType")
    .HasValue<CompactSpot>("Compact")
    .HasValue<HandicappedSpot>("Handicapped")
    .HasValue<LargeSpot>("Large");   
    }
}