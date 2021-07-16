using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

using AirlineReservationSystems.Models.AirlineReservationSystemsDb;

namespace AirlineReservationSystems.Data
{
  public partial class AirlineReservationSystemsDbContext : Microsoft.EntityFrameworkCore.DbContext
  {
    public AirlineReservationSystemsDbContext(DbContextOptions<AirlineReservationSystemsDbContext> options):base(options)
    {
    }

    public AirlineReservationSystemsDbContext()
    {
    }

    partial void OnModelBuilding(ModelBuilder builder);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight>()
              .HasOne(i => i.Airport1)
              .WithMany(i => i.Flights1)
              .HasForeignKey(i => i.InitialAirportId)
              .HasPrincipalKey(i => i.Id);
        builder.Entity<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight>()
              .HasOne(i => i.Airport)
              .WithMany(i => i.Flights)
              .HasForeignKey(i => i.FinalAirportId)
              .HasPrincipalKey(i => i.Id);
        builder.Entity<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Reservation>()
              .HasOne(i => i.Flight)
              .WithMany(i => i.Reservations)
              .HasForeignKey(i => i.FlightId)
              .HasPrincipalKey(i => i.Id);

        builder.Entity<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Reservation>()
              .Property(p => p.Name)
              .HasDefaultValueSql("(N'')");


        builder.Entity<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight>()
              .Property(p => p.Date)
              .HasColumnType("datetime2");

        builder.Entity<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight>()
              .Property(p => p.FinalDate)
              .HasColumnType("datetime2");
        this.OnModelBuilding(builder);
    }


    public DbSet<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Airport> Airports
    {
      get;
      set;
    }

    public DbSet<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight> Flights
    {
      get;
      set;
    }

    public DbSet<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Reservation> Reservations
    {
      get;
      set;
    }
  }
}
