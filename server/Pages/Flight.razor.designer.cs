using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using AirlineReservationSystems.Models.AirlineReservationSystemsDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AirlineReservationSystems.Models;

namespace AirlineReservationSystems.Pages
{
    public partial class FlightComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        protected AirlineReservationSystemsDbService AirlineReservationSystemsDb { get; set; }
        protected RadzenDataGrid<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight> grid0;
        protected RadzenDataGrid<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Reservation> grid1;

        IEnumerable<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight> _getFlightsResult;
        protected IEnumerable<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight> getFlightsResult
        {
            get
            {
                return _getFlightsResult;
            }
            set
            {
                if (!object.Equals(_getFlightsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getFlightsResult", NewValue = value, OldValue = _getFlightsResult };
                    _getFlightsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight _master;
        protected AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight master
        {
            get
            {
                return _master;
            }
            set
            {
                if (!object.Equals(_master, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "master", NewValue = value, OldValue = _master };
                    _master = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Reservation> _Reservations;
        protected IEnumerable<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Reservation> Reservations
        {
            get
            {
                return _Reservations;
            }
            set
            {
                if (!object.Equals(_Reservations, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "Reservations", NewValue = value, OldValue = _Reservations };
                    _Reservations = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Security.InitializeAsync(AuthenticationStateProvider);
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                await Load();
            }
        }
        protected async System.Threading.Tasks.Task Load()
        {
            var airlineReservationSystemsDbGetFlightsResult = await AirlineReservationSystemsDb.GetFlights(new Query() { Expand = "Airport1,Airport" });
            getFlightsResult = airlineReservationSystemsDbGetFlightsResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddFlight>("Add Flight", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowDoubleClick(DataGridRowMouseEventArgs<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight> args)
        {
            await DialogService.OpenAsync<EditFlight>("Edit Flight", new Dictionary<string, object>() { {"Id", args.Data.Id} });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight args)
        {
            master = args;

            var airlineReservationSystemsDbGetReservationsResult = await AirlineReservationSystemsDb.GetReservations(new Query() { Filter = $@"i => i.FlightId == {args.Id}" });
            Reservations = airlineReservationSystemsDbGetReservationsResult;
        }

        protected async System.Threading.Tasks.Task Button1Click(MouseEventArgs args, dynamic data)
        {
            await DialogService.OpenAsync<EditFlight>("Edit Flight", new Dictionary<string, object>() { {"Id", data.Id} });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var airlineReservationSystemsDbDeleteFlightResult = await AirlineReservationSystemsDb.DeleteFlight(data.Id);
                    if (airlineReservationSystemsDbDeleteFlightResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception airlineReservationSystemsDbDeleteFlightException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Flight" });
            }
        }

        protected async System.Threading.Tasks.Task ReservationAddButtonClick(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddReservation>("Add Reservation", new Dictionary<string, object>() { {"FlightId", master.Id} });
            await grid1.Reload();
        }

        protected async System.Threading.Tasks.Task Grid1RowSelect(AirlineReservationSystems.Models.AirlineReservationSystemsDb.Reservation args)
        {
            var dialogResult = await DialogService.OpenAsync<EditReservation>("Edit Reservation", new Dictionary<string, object>() { {"Id", args.Id} });
            await grid1.Reload();
        }

        protected async System.Threading.Tasks.Task ReservationDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var airlineReservationSystemsDbDeleteReservationResult = await AirlineReservationSystemsDb.DeleteReservation(data.Id);
                    if (airlineReservationSystemsDbDeleteReservationResult != null)
                    {
                        await grid1.Reload();
                    }
                }
            }
            catch (System.Exception airlineReservationSystemsDbDeleteReservationException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Flight" });
            }
        }
    }
}
