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
    public partial class EditAirportComponent : ComponentBase
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
        public dynamic Id { get; set; }

        AirlineReservationSystems.Models.AirlineReservationSystemsDb.Airport _airport;
        protected AirlineReservationSystems.Models.AirlineReservationSystemsDb.Airport airport
        {
            get
            {
                return _airport;
            }
            set
            {
                if (!object.Equals(_airport, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "airport", NewValue = value, OldValue = _airport };
                    _airport = value;
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
            var airlineReservationSystemsDbGetAirportByIdResult = await AirlineReservationSystemsDb.GetAirportById(Id);
            airport = airlineReservationSystemsDbGetAirportByIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(AirlineReservationSystems.Models.AirlineReservationSystemsDb.Airport args)
        {
            try
            {
                var airlineReservationSystemsDbUpdateAirportResult = await AirlineReservationSystemsDb.UpdateAirport(Id, airport);
                DialogService.Close(airport);
            }
            catch (System.Exception airlineReservationSystemsDbUpdateAirportException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update Airport" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
