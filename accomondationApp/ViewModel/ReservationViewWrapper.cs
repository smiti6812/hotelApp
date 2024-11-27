using System.Collections.Concurrent;
using System.Globalization;

using accomondationApp.Factory;
using accomondationApp.Interfaces;
using accomondationApp.Models;
using accomondationApp.Repositories;

using MoreLinq;

namespace accomondationApp.ViewModel
{
    public class ReservationViewWrapper : IReservationViewWrapper
    {
        private static ConcurrentDictionary<DateTime, ReservationViewWrapper>? reservationViewWrapperCache;
        private readonly IReservationService reservationService;
        private readonly IReservationViewProperties reservationViewProperties;
        public ReservationView[]? ReservationView { get; set; }
        public ReservationViewHeader? ReservationViewHeader { get; set; }
        public int[]? MonthArr { get; set; }
        public DateTime PageSelectedDate { get; set; }
        public Reservation[]? Reservations { get; set; }
        public ReservationViewWrapper(IReservationService _reservationService, IReservationViewProperties _reservationViewProperties)
        {
            reservationService = _reservationService;
            reservationViewProperties = _reservationViewProperties;
            if (reservationViewWrapperCache is null)
            {
                var wrapper = new List<ReservationViewWrapper>();
                wrapper.Add(InitializeReservationViewWrapper(DateTime.Now.AddDays(-DateTime.Now.Day + 1)).Result);
                reservationViewWrapperCache = new ConcurrentDictionary<DateTime, ReservationViewWrapper>(wrapper.ToDictionary(c => c.PageSelectedDate));
            }
        }
        private ReservationViewWrapper UpdateCache(DateTime pageSelectedDate, ReservationViewWrapper wrapper)
        {
            if (reservationViewWrapperCache != null)
            {
                if (reservationViewWrapperCache.TryGetValue(pageSelectedDate, out ReservationViewWrapper? old))
                {
                    if (reservationViewWrapperCache.TryUpdate(pageSelectedDate, wrapper, old))
                    {
                        return wrapper;
                    }
                }
            }
            return null!;
        }
        private int[] getMonthArr(DateTime currDate, int months)
        {
            var monthArr = new int[months];
            for (int i = 0; i < monthArr.Length; i++)
            {
                monthArr[i] = DateTime.DaysInMonth(currDate.AddMonths(i).Year, currDate.AddMonths(i).Month);
            }

            return monthArr;
        }

        public async Task<ReservationViewWrapper> GetReservationViewWrapperNextPrev(DateTime currDate)
        {
            var wrapper = reservationViewWrapperCache.Values.FirstOrDefault(c => c.PageSelectedDate.Date == currDate.Date);
            return wrapper is ReservationViewWrapper ? wrapper : reservationViewWrapperCache.AddOrUpdate(currDate, InitializeReservationViewWrapper(currDate).Result, UpdateCache);
        }       
        public async Task<ReservationViewWrapper> GetReservationViewWrapper(DateTime currDate) => reservationViewWrapperCache.Values.First(c => c.PageSelectedDate.Date == currDate.AddDays(-currDate.Day + 1).Date);

        public Task<ReservationViewWrapper> RefreshReservationViewWrapper(Reservation reservation, DateTime pageReservationDate)
        {
            var startDate = reservation.StartDate ?? DateTime.MinValue;
            var endDate = reservation.EndDate ?? DateTime.MinValue;
            var roomId = reservation.RommId ?? 0;
            var reservationViewWrapper = GetReservationViewWrapperNextPrev(pageReservationDate).Result;
            var reservationView = reservationViewWrapper?.ReservationView?.First(c => c.room.RoomId == roomId);
            var diffDays = (endDate - startDate).Days;
            for (int i = 0; i <= diffDays; i++)
            {
                var checkReservation = reservationViewProperties.ReturnReservationViewProperties(roomId, startDate.AddDays(i));
                int index = Array.IndexOf(reservationView.dayDates, startDate.AddDays(i));
                if (index > -1)
                {
                    reservationView.reservationName[index] = checkReservation.reservationName;
                    reservationView.reservationStartSaved[index] = checkReservation.startSaved;
                    reservationView.reservationSaved[index] = checkReservation.endSaved;
                    reservationView.selectedDateArr[index] = checkReservation.selectedDateArr;
                }
                else
                {
                    _ = RefreshReservationViewWrapper(reservation, pageReservationDate.AddMonths(-1));
                }
            }
            reservationViewWrapper.Reservations = reservationService.GetReservations(pageReservationDate).Result.ToArray();
            reservationViewWrapper.ReservationView.Where(c => c.room.RoomId == roomId).ForEach(rv => rv = reservationView);
            return Task.FromResult(reservationViewWrapper);
        }

        public Task<ReservationViewWrapper> InitializeReservationViewWrapper(DateTime currDate)
        {
            var reservationViewWrapper = this;
            reservationViewWrapper.PageSelectedDate = currDate.Date;
            var roomList = reservationService.GetAllRooms().Result.ToArray<Room>();
            var reservationView = new ReservationView[roomList.Count()];
            int months = reservationService.DisplayedMonth();
            reservationViewWrapper.MonthArr = getMonthArr(currDate, months);
            reservationViewWrapper.ReservationViewHeader = (ReservationViewHeader)ReservationViewHeaderFactory.GetReservationViewHeader(reservationViewWrapper.MonthArr, reservationViewWrapper.PageSelectedDate);
            var reservations = reservationService.GetReservations(currDate).Result;            
            for (int i = 0; i < roomList.Length; i++)
            {                
                reservationView[i] = (ReservationView)ReservationViewFactory.GetReservationView(roomList[i], reservationViewWrapper.PageSelectedDate, reservationViewProperties, reservationViewWrapper.MonthArr);
                reservationView[i].room = roomList[i];                
            }           
            reservationViewWrapper.Reservations = reservations.ToArray<Reservation>();
            reservationViewWrapper.ReservationView = reservationView;
            return Task.FromResult(reservationViewWrapper);
        }
    }
}