using System.Globalization;

using accomondationApp.Models;
using accomondationApp.Repositories;
using accomondationApp.ViewModel;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using accomondationApp.Interfaces;



namespace accomondationApp
{
    public class ReservationViewService : IReservationViewService
    {

        private readonly IReservationViewWrapper reservationViewWrapper;
        public ReservationViewService(IReservationViewWrapper _reservationViewWrapper) => reservationViewWrapper = _reservationViewWrapper;
        public async Task<ReservationViewWrapper> CallGetReservationViewWrapperNextPrev(DateTime currDate) => await reservationViewWrapper.GetReservationViewWrapperNextPrev(currDate);
        public async Task<ReservationViewWrapper> CallGetReservationViewWrapper(DateTime currDate) => await reservationViewWrapper.GetReservationViewWrapper(currDate);
        public async Task<ReservationViewWrapper> CallRefreshReservationViewWrapper(Reservation reservation, DateTime pageReservationDate) => await reservationViewWrapper.RefreshReservationViewWrapper(reservation, pageReservationDate);
        public async Task<ReservationViewWrapper> CallInitializeReservationViewWrapper(DateTime currDate) => await reservationViewWrapper.InitializeReservationViewWrapper(currDate);
    }
}
