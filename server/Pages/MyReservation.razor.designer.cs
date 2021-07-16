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
    public partial class MyReservationComponent : ComponentBase
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
        protected RadzenDataGrid<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Reservation> grid0;

        IEnumerable<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Reservation> _getReservationsResult;
        protected IEnumerable<AirlineReservationSystems.Models.AirlineReservationSystemsDb.Reservation> getReservationsResult
        {
            get
            {
                return _getReservationsResult;
            }
            set
            {
                if (!object.Equals(_getReservationsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getReservationsResult", NewValue = value, OldValue = _getReservationsResult };
                    _getReservationsResult = value;
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
            var airlineReservationSystemsDbGetReservationsResult = await AirlineReservationSystemsDb.GetReservations(new Query() { Filter = $@"i => i.UserId.Contains(@0)", FilterParameters = new object[] { Security.User.Id }, OrderBy = $"Id desc", Expand = "Flight" });
            getReservationsResult = airlineReservationSystemsDbGetReservationsResult;
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var airlineReservationSystemsDbDeleteReservationResult = await AirlineReservationSystemsDb.DeleteReservation(data.Id);
                    if (airlineReservationSystemsDbDeleteReservationResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception airlineReservationSystemsDbDeleteReservationException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Reservation" });
            }
        }
    }
}
