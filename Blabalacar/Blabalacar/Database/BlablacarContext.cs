using Blabalacar.Models;
using Blabalacar.Models.Auto;
using Blabalacar.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Route = Blabalacar.Models.Route;

namespace Blabalacar.Database;

public class BlablacarContext: IdentityDbContext<User,ApplicationRole,Guid>
{
    private readonly IConfiguration _configuration;
    public BlablacarContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserTrip>()
            .HasKey(userTrip => new {userTrip.TripId, userTrip.UserId});
        modelBuilder.Entity<UserTrip>()
            .HasOne(userTrip => userTrip.User)
            .WithMany(user => user.UserTrips)
            .HasForeignKey(userTrip => userTrip.UserId);
        modelBuilder.Entity<UserTrip>()
            .HasOne(userTrip => userTrip.Trip)
            .WithMany(trip => trip.UserTrips)
            .HasForeignKey(userTrip => userTrip.TripId);

        modelBuilder.Entity<User>()
            .HasKey(user => user.Id);
        modelBuilder.Entity<User>()
            .Property(user => user.Name)
            .HasMaxLength(50)
            .IsRequired();


        modelBuilder.Entity<Trip>()
            .HasKey(trip => trip.Id);
        modelBuilder.Entity<Trip>()
            .HasOne(trip => trip.Route)
            .WithMany(route => route.Trips)
            .HasForeignKey(trip=>trip.RouteId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Trip>()
            .Property(trip => trip.DepartureAt)
            .IsRequired();


        modelBuilder.Entity<Route>()
            .HasKey(route => route.Id);
        modelBuilder.Entity<Route>()
            .Property(route => route.StartRoute)
            .HasMaxLength(50);
        modelBuilder.Entity<Route>()
            .Property(route => route.EndRoute)
            .HasMaxLength(50);
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-OAMN13B;Database=Blablacar;Trusted_Connection=True");
    }

    public DbSet<UserTrip> UserTrips { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Trip> Trip { get; set; }
    public DbSet<Route> Route { get; set; }
}