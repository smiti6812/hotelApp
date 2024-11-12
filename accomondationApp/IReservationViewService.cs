using accomondationApp.Models;
using accomondationApp.ViewModel;

namespace accomondationApp
{
    public interface IReservationViewService
    {
        Task<ReservationView[]> GetReservationView(DateTime currDate);
    }
}