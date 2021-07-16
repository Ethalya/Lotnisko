using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineReservationSystems.Models.AirlineReservationSystemsDb
{
  [Table("Flight", Schema = "dbo")]
  public partial class Flight
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
      get;
      set;
    }

    public ICollection<Reservation> Reservations { get; set; }
    public int NoFlight
    {
      get;
      set;
    }
    public int NoPlane
    {
      get;
      set;
    }
    public int? InitialAirportId
    {
      get;
      set;
    }
    public Airport Airport1 { get; set; }
    public int? FinalAirportId
    {
      get;
      set;
    }
    public Airport Airport { get; set; }
    public DateTime Date
    {
      get;
      set;
    }
    public DateTime FinalDate
    {
      get;
      set;
    }
    public int Places
    {
      get;
      set;
    }
    public int FreePlaces
    {
      get;
      set;
    }
  }
}
