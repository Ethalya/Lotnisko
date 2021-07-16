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
    public partial class FlightBuyComponent : ComponentBase
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

        [Parameter]
        public dynamic FlightId { get; set; }

        AirlineReservationSystems.Models.AirlineReservationSystemsDb.Reservation _reservation;
        protected AirlineReservationSystems.Models.AirlineReservationSystemsDb.Reservation reservation
        {
            get
            {
                return _reservation;
            }
            set
            {
                if (!object.Equals(_reservation, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "reservation", NewValue = value, OldValue = _reservation };
                    _reservation = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight _places;
        protected AirlineReservationSystems.Models.AirlineReservationSystemsDb.Flight places
        {
            get
            {
                return _places;
            }
            set
            {
                if (!object.Equals(_places, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "places", NewValue = value, OldValue = _places };
                    _places = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        int _FreePlaces;
        protected int FreePlaces
        {
            get
            {
                return _FreePlaces;
            }
            set
            {
                if (!object.Equals(_FreePlaces, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "FreePlaces", NewValue = value, OldValue = _FreePlaces };
                    _FreePlaces = value;
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
            reservation = new AirlineReservationSystems.Models.AirlineReservationSystemsDb.Reservation();

            reservation.UserId = Security.User.Id;

            reservation.FlightId = FlightId;
        }

        protected async System.Threading.Tasks.Task Form0Submit(AirlineReservationSystems.Models.AirlineReservationSystemsDb.Reservation args)
        {
            var airlineReservationSystemsDbGetFlightByIdResult = await AirlineReservationSystemsDb.GetFlightById(FlightId);
            places = airlineReservationSystemsDbGetFlightByIdResult;

            FreePlaces = airlineReservationSystemsDbGetFlightByIdResult.FreePlaces;

            places.FreePlaces = places.FreePlaces - reservation.NoPlaces;

            try
            {
                if (places.FreePlaces >= 0)
                {
                    var airlineReservationSystemsDbCreateReservationResult = await AirlineReservationSystemsDb.CreateReservation(reservation);
                    var airlineReservationSystemsDbUpdateFlightResult = await AirlineReservationSystemsDb.UpdateFlight(places.Id, places);

                    DialogService.Close(reservation);
                }
            }
            catch (System.Exception airlineReservationSystemsDbCreateReservationException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update Reservation" });
            }

            if (FreePlaces < reservation.NoPlaces)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Info,Detail = $"We only have {FreePlaces} vacancies.",Duration = 2000 });
            }
        }

        protected async System.Threading.Tasks.Task Button1Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
