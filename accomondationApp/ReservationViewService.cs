using accomondationApp.Models;
using accomondationApp.Repositories;
using accomondationApp.ViewModel;

namespace accomondationApp
{
    public class ReservationViewService : IReservationViewService
    {
        private ReservationView[] reservationView;
        private RoomRepository roomRepository;
        private IEnumerable<Room?> roomList;
        public ReservationViewService(HotelAppDbContext context)
        {
            roomRepository = new RoomRepository(context);
            roomList = roomRepository.ReturnRooms().Result;
            reservationView = new ReservationView[roomList.Count()];
        }

        public Task<ReservationView[]> GetReservationView(int months)
        {
            foreach (var room in roomList)
            {
                for (int i = 0; i < reservationView.Length; i++)
                {
                    reservationView[i] = new ReservationView();
                    reservationView[i].room = room;
                    for (int j = 0; j <= months; j++)
                    {
                        int daysInMonth = DateTime.DaysInMonth(DateTime.Now.AddMonths(j).Year, DateTime.Now.AddMonths(j).Month);
                        reservationView[i].dayDates = new DateTime[daysInMonth];
                        reservationView[i].reservationName = new string[daysInMonth];
                        reservationView[i].reservationStartSaved = new bool[daysInMonth];
                        DateTime firstDayOfTheMonth = DateTime.Now.AddMonths(j).AddDays(-DateTime.Now.AddMonths(j).Day + 1).Date;
                        for (int d = 0; d < daysInMonth; d++)
                        {
                            reservationView[i].dayDates[d] = firstDayOfTheMonth.AddDays(d).Date;
                            reservationView[i].reservationName[d] = string.Empty;
                            reservationView[i].reservationStartSaved[d] = false;
                        }
                    }
                }
            }

            return Task.FromResult(reservationView);
        }
    }
}
