using accomondationApp.Models;
using accomondationApp.Repositories;
using accomondationApp.ViewModel;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace accomondationApp
{
    public class ReservationViewService : IReservationViewService
    {
        private readonly HotelAppDBContext context;
        private readonly IRoomRepository roomRepository;

        public ReservationViewService(HotelAppDBContext _context, IRoomRepository _roomRepository)
        {
            context = _context;
            roomRepository = _roomRepository;
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
        /*
        public bool[,] getSelectedDateArr(int months)
        {

        }
        */
        public Task<ReservationView[]> GetReservationView(DateTime currDate)
        {
            var reservationViewWrapper = new ReservationViewWrapper();
            var roomList = roomRepository.ReturnRooms().Result;
            var reservationView = new ReservationView[roomList.Count()];
            int months = context.DisplayedMonths.First().DisplayedMonths ?? 0;
            reservationViewWrapper.MonthArr = getMonthArr(currDate, months);
            reservationViewWrapper.SelectedDateArr = new bool[roomList.Length, reservationViewWrapper.MonthArr.Sum(c =>c) * months];
            reservationViewWrapper.ReservationViewHeader = new ReservationViewHeader();
            for (int i = 0; i < roomList.Length; i++)
            {
                reservationView[i] = new ReservationView();
                reservationView[i].room = roomList[i];              
                var dayDates = new List<DateTime>();
                var reservationName = new List<string>();
                var reservationStartSaved = new List<bool>();
                var reservationSaved = new List<bool>();
                int index = 0;
                for (int j = 0; j < months; j++)
                {
                    for (int m = 1; m <= reservationViewWrapper.MonthArr[j]; m++)
                    {
                        DateTime firstDayOfTheMonth = DateTime.Now.AddMonths(j).AddDays(-DateTime.Now.AddMonths(j).Day).Date;
                        dayDates.Add(firstDayOfTheMonth.AddDays(m).Date);
                        reservationName.Add(string.Empty);
                        reservationStartSaved.Add(false);
                        reservationSaved.Add(false);
                        reservationViewWrapper.SelectedDateArr[i,index] = false;
                        index++;
                    }
                }
                reservationView[i].dayDates = dayDates.ToArray<DateTime>();
                reservationView[i].reservationName = reservationName.ToArray<string>();
                reservationView[i].reservationStartSaved = reservationStartSaved.ToArray<bool>();
                reservationView[i].reservationSaved = reservationSaved.ToArray<bool>();
            }
          
            return Task.FromResult(reservationView);
        }
    }
}
