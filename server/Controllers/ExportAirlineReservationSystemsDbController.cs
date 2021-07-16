using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirlineReservationSystems.Data;

namespace AirlineReservationSystems
{
    public partial class ExportAirlineReservationSystemsDbController : ExportController
    {
        private readonly AirlineReservationSystemsDbContext context;

        public ExportAirlineReservationSystemsDbController(AirlineReservationSystemsDbContext context)
        {
            this.context = context;
        }
        [HttpGet("/export/AirlineReservationSystemsDb/airports/csv")]
        [HttpGet("/export/AirlineReservationSystemsDb/airports/csv(fileName='{fileName}')")]
        public FileStreamResult ExportAirportsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.Airports, Request.Query), fileName);
        }

        [HttpGet("/export/AirlineReservationSystemsDb/airports/excel")]
        [HttpGet("/export/AirlineReservationSystemsDb/airports/excel(fileName='{fileName}')")]
        public FileStreamResult ExportAirportsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.Airports, Request.Query), fileName);
        }
        [HttpGet("/export/AirlineReservationSystemsDb/flights/csv")]
        [HttpGet("/export/AirlineReservationSystemsDb/flights/csv(fileName='{fileName}')")]
        public FileStreamResult ExportFlightsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.Flights, Request.Query), fileName);
        }

        [HttpGet("/export/AirlineReservationSystemsDb/flights/excel")]
        [HttpGet("/export/AirlineReservationSystemsDb/flights/excel(fileName='{fileName}')")]
        public FileStreamResult ExportFlightsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.Flights, Request.Query), fileName);
        }
        [HttpGet("/export/AirlineReservationSystemsDb/reservations/csv")]
        [HttpGet("/export/AirlineReservationSystemsDb/reservations/csv(fileName='{fileName}')")]
        public FileStreamResult ExportReservationsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.Reservations, Request.Query), fileName);
        }

        [HttpGet("/export/AirlineReservationSystemsDb/reservations/excel")]
        [HttpGet("/export/AirlineReservationSystemsDb/reservations/excel(fileName='{fileName}')")]
        public FileStreamResult ExportReservationsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.Reservations, Request.Query), fileName);
        }
    }
}
