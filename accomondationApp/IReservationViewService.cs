using accomondationApp.Models;
using accomondationApp.ViewModel;

namespace accomondationApp
{
    public interface IReservationViewService
    {
        Task<ReservationViewWrapper> CallGetReservationViewWrapperNextPrev(DateTime currDate);
        Task<ReservationViewWrapper> CallGetReservationViewWrapper(DateTime currDate);
        Task<ReservationViewWrapper> CallRefreshReservationViewWrapper(Reservation reservation, DateTime pageReservationDate);
        Task<ReservationViewWrapper> CallInitializeReservationViewWrapper(DateTime currDate);
    }
}