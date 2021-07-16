using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineReservationSystems.Models.AirlineReservationSystemsDb
{
  [Table("Reservation", Schema = "dbo")]
  public partial class Reservation
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
      get;
      set;
    }
    public string UserId
    {
      get;
      set;
    }
    public int NoPlaces
    {
      get;
      set;
    }
    public int? FlightId
    {
      get;
      set;
    }
    public Flight Flight { get; set; }
    public string Name
    {
      get;
      set;
    }
  }
}
