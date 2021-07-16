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
    public partial class FlightClientComponent : ComponentBase
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
            var airlineReservationSystemsDbGetFlightsResult = await AirlineReservationSystemsDb.GetFlights(new Query() { Filter = $@"i => i.Date.Date == @0", FilterParameters = new object[] { DateTime.Now.Date }, Expand = "Airport1,Airport" });
            getFlightsResult = airlineReservationSystemsDbGetFlightsResult;
        }

        protected async System.Threading.Tasks.Task Datepicker0Change(DateTime? args)
        {
            var airlineReservationSystemsDbGetFlightsResult = await AirlineReservationSystemsDb.GetFlights(new Query() { Filter = $@"i => i.Date.Date == @0", FilterParameters = new object[] { args.Value.Date }, Expand = "Airport1,Airport" });
            getFlightsResult = airlineReservationSystemsDbGetFlightsResult;
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight args)
        {
            await DialogService.OpenAsync<FlightBuy>("FlightBuy", new Dictionary<string, object>() { {"FlightId", args.Id} });
        }
    }
}
