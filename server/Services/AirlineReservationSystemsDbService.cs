using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Data;
using System.Text.Encodings.Web;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using AirlineReservationSystems.Data;

namespace AirlineReservationSystems
{
    public partial class AirlineReservationSystemsDbService
    {
        AirlineReservationSystemsDbContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly AirlineReservationSystemsDbContext context;
        private readonly NavigationManager navigationManager;

        public AirlineReservationSystemsDbService(AirlineReservationSystemsDbContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public async Task ExportAirportsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/airlinereservationsystemsdb/airports/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/airlinereservationsystemsdb/airports/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAirportsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/airlinereservationsystemsdb/airports/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/airlinereservationsystemsdb/airports/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAirportsRead(ref IQueryable<Models.AirlineReservationSystemsDb.Airport> items);

        public async Task<IQueryable<Models.AirlineReservationSystemsDb.Airport>> GetAirports(Query query = null)
        {
            var items = Context.Airports.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnAirportsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAirportCreated(Models.AirlineReservationSystemsDb.Airport item);
        partial void OnAfterAirportCreated(Models.AirlineReservationSystemsDb.Airport item);

        public async Task<Models.AirlineReservationSystemsDb.Airport> CreateAirport(Models.AirlineReservationSystemsDb.Airport airport)
        {
            OnAirportCreated(airport);

            Context.Airports.Add(airport);
            Context.SaveChanges();

            OnAfterAirportCreated(airport);

            return airport;
        }
        public async Task ExportFlightsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/airlinereservationsystemsdb/flights/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/airlinereservationsystemsdb/flights/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportFlightsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/airlinereservationsystemsdb/flights/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/airlinereservationsystemsdb/flights/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnFlightsRead(ref IQueryable<Models.AirlineReservationSystemsDb.Flight> items);

        public async Task<IQueryable<Models.AirlineReservationSystemsDb.Flight>> GetFlights(Query query = null)
        {
            var items = Context.Flights.AsQueryable();

            items = items.Include(i => i.Airport1);

            items = items.Include(i => i.Airport);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnFlightsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnFlightCreated(Models.AirlineReservationSystemsDb.Flight item);
        partial void OnAfterFlightCreated(Models.AirlineReservationSystemsDb.Flight item);

        public async Task<Models.AirlineReservationSystemsDb.Flight> CreateFlight(Models.AirlineReservationSystemsDb.Flight flight)
        {
            OnFlightCreated(flight);

            Context.Flights.Add(flight);
            Context.SaveChanges();

            OnAfterFlightCreated(flight);

            return flight;
        }
        public async Task ExportReservationsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/airlinereservationsystemsdb/reservations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/airlinereservationsystemsdb/reservations/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportReservationsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/airlinereservationsystemsdb/reservations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/airlinereservationsystemsdb/reservations/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnReservationsRead(ref IQueryable<Models.AirlineReservationSystemsDb.Reservation> items);

        public async Task<IQueryable<Models.AirlineReservationSystemsDb.Reservation>> GetReservations(Query query = null)
        {
            var items = Context.Reservations.AsQueryable();

            items = items.Include(i => i.Flight);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnReservationsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnReservationCreated(Models.AirlineReservationSystemsDb.Reservation item);
        partial void OnAfterReservationCreated(Models.AirlineReservationSystemsDb.Reservation item);

        public async Task<Models.AirlineReservationSystemsDb.Reservation> CreateReservation(Models.AirlineReservationSystemsDb.Reservation reservation)
        {
            OnReservationCreated(reservation);

            Context.Reservations.Add(reservation);
            Context.SaveChanges();

            OnAfterReservationCreated(reservation);

            return reservation;
        }

        partial void OnAirportDeleted(Models.AirlineReservationSystemsDb.Airport item);
        partial void OnAfterAirportDeleted(Models.AirlineReservationSystemsDb.Airport item);

        public async Task<Models.AirlineReservationSystemsDb.Airport> DeleteAirport(int? id)
        {
            var itemToDelete = Context.Airports
                              .Where(i => i.Id == id)
                              .Include(i => i.Flights)
                              .Include(i => i.Flights1)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAirportDeleted(itemToDelete);

            Context.Airports.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAirportDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnAirportGet(Models.AirlineReservationSystemsDb.Airport item);

        public async Task<Models.AirlineReservationSystemsDb.Airport> GetAirportById(int? id)
        {
            var items = Context.Airports
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            var itemToReturn = items.FirstOrDefault();

            OnAirportGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.AirlineReservationSystemsDb.Airport> CancelAirportChanges(Models.AirlineReservationSystemsDb.Airport item)
        {
            var entityToCancel = Context.Entry(item);
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;

            return item;
        }

        partial void OnAirportUpdated(Models.AirlineReservationSystemsDb.Airport item);
        partial void OnAfterAirportUpdated(Models.AirlineReservationSystemsDb.Airport item);

        public async Task<Models.AirlineReservationSystemsDb.Airport> UpdateAirport(int? id, Models.AirlineReservationSystemsDb.Airport airport)
        {
            OnAirportUpdated(airport);

            var itemToUpdate = Context.Airports
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(airport);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();

            OnAfterAirportUpdated(airport);

            return airport;
        }

        partial void OnFlightDeleted(Models.AirlineReservationSystemsDb.Flight item);
        partial void OnAfterFlightDeleted(Models.AirlineReservationSystemsDb.Flight item);

        public async Task<Models.AirlineReservationSystemsDb.Flight> DeleteFlight(int? id)
        {
            var itemToDelete = Context.Flights
                              .Where(i => i.Id == id)
                              .Include(i => i.Reservations)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnFlightDeleted(itemToDelete);

            Context.Flights.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterFlightDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnFlightGet(Models.AirlineReservationSystemsDb.Flight item);

        public async Task<Models.AirlineReservationSystemsDb.Flight> GetFlightById(int? id)
        {
            var items = Context.Flights
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Airport1);

            items = items.Include(i => i.Airport);

            var itemToReturn = items.FirstOrDefault();

            OnFlightGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.AirlineReservationSystemsDb.Flight> CancelFlightChanges(Models.AirlineReservationSystemsDb.Flight item)
        {
            var entityToCancel = Context.Entry(item);
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;

            return item;
        }

        partial void OnFlightUpdated(Models.AirlineReservationSystemsDb.Flight item);
        partial void OnAfterFlightUpdated(Models.AirlineReservationSystemsDb.Flight item);

        public async Task<Models.AirlineReservationSystemsDb.Flight> UpdateFlight(int? id, Models.AirlineReservationSystemsDb.Flight flight)
        {
            OnFlightUpdated(flight);

            var itemToUpdate = Context.Flights
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(flight);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();

            OnAfterFlightUpdated(flight);

            return flight;
        }

        partial void OnReservationDeleted(Models.AirlineReservationSystemsDb.Reservation item);
        partial void OnAfterReservationDeleted(Models.AirlineReservationSystemsDb.Reservation item);

        public async Task<Models.AirlineReservationSystemsDb.Reservation> DeleteReservation(int? id)
        {
            var itemToDelete = Context.Reservations
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnReservationDeleted(itemToDelete);

            Context.Reservations.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterReservationDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnReservationGet(Models.AirlineReservationSystemsDb.Reservation item);

        public async Task<Models.AirlineReservationSystemsDb.Reservation> GetReservationById(int? id)
        {
            var items = Context.Reservations
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Flight);

            var itemToReturn = items.FirstOrDefault();

            OnReservationGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.AirlineReservationSystemsDb.Reservation> CancelReservationChanges(Models.AirlineReservationSystemsDb.Reservation item)
        {
            var entityToCancel = Context.Entry(item);
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;

            return item;
        }

        partial void OnReservationUpdated(Models.AirlineReservationSystemsDb.Reservation item);
        partial void OnAfterReservationUpdated(Models.AirlineReservationSystemsDb.Reservation item);

        public async Task<Models.AirlineReservationSystemsDb.Reservation> UpdateReservation(int? id, Models.AirlineReservationSystemsDb.Reservation reservation)
        {
            OnReservationUpdated(reservation);

            var itemToUpdate = Context.Reservations
                              .Where(i => i.Id == id)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(reservation);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();

            OnAfterReservationUpdated(reservation);

            return reservation;
        }
    }
}
