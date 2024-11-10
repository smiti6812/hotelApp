using accomondationApp.ViewModel;

namespace accomondationApp
{
    public interface IReservationViewService
    {
        Task<ReservationView[]> GetReservationView(int months);
    }
}