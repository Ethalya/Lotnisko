using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirlineReservationSystems.Models.AirlineReservationSystemsDb
{
  [Table("Airport", Schema = "dbo")]
  public partial class Airport
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
      get;
      set;
    }

    public ICollection<Flight> Flights { get; set; }
    public ICollection<Flight> Flights1 { get; set; }
    public string Name
    {
      get;
      set;
    }
  }
}
