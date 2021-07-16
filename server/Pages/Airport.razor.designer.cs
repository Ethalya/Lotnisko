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
    public partial class AirportComponent : ComponentBase
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
        protected RadzenDataGrid<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Airport> grid0;

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
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddAirport>("Add Airport", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(AirlineReservationSystems.Models.AirlineReservationSystemsDb.Airport args)
        {
            var dialogResult = await DialogService.OpenAsync<EditAirport>("Edit Airport", new Dictionary<string, object>() { {"Id", args.Id} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var airlineReservationSystemsDbDeleteAirportResult = await AirlineReservationSystemsDb.DeleteAirport(data.Id);
                    if (airlineReservationSystemsDbDeleteAirportResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception airlineReservationSystemsDbDeleteAirportException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Airport" });
            }
        }
    }
}
