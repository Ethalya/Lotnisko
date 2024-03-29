﻿using System;
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
    public partial class EditReservationComponent : ComponentBase
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
            var airlineReservationSystemsDbGetReservationByIdResult = await AirlineReservationSystemsDb.GetReservationById(Id);
            reservation = airlineReservationSystemsDbGetReservationByIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(AirlineReservationSystems.Models.AirlineReservationSystemsDb.Reservation args)
        {
            try
            {
                var airlineReservationSystemsDbUpdateReservationResult = await AirlineReservationSystemsDb.UpdateReservation(Id, reservation);
                DialogService.Close(reservation);
            }
            catch (System.Exception airlineReservationSystemsDbUpdateReservationException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update Reservation" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
