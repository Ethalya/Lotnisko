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
    public partial class AddFlightComponent : ComponentBase
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

        IEnumerable<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Airport> _getAirportsResult;
        protected IEnumerable<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Airport> getAirportsResult
        {
            get
            {
                return _getAirportsResult;
            }
            set
            {
                if (!object.Equals(_getAirportsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getAirportsResult", NewValue = value, OldValue = _getAirportsResult };
                    _getAirportsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight _flight;
        protected AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight flight
        {
            get
            {
                return _flight;
            }
            set
            {
                if (!object.Equals(_flight, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "flight", NewValue = value, OldValue = _flight };
                    _flight = value;
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
            var airlineReservationSystemsDbGetAirportsResult = await AirlineReservationSystemsDb.GetAirports();
            getAirportsResult = airlineReservationSystemsDbGetAirportsResult;

            flight = new AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight args)
        {
            try
            {
                var airlineReservationSystemsDbCreateFlightResult = await AirlineReservationSystemsDb.CreateFlight(flight);
                DialogService.Close(flight);
            }
            catch (System.Exception airlineReservationSystemsDbCreateFlightException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to create new Flight!" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
