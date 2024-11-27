using accomondationApp.Models;
using accomondationApp.ViewModel;

namespace accomondationApp.Interfaces
{
    public interface IReservationViewWrapper
    {
        int[]? MonthArr { get; set; }
        DateTime PageSelectedDate { get; set; }
        Reservation[]? Reservations { get; set; }
        ReservationView[]? ReservationView { get; set; }
        ReservationViewHeader? ReservationViewHeader { get; set; }
        Task<ReservationViewWrapper> GetReservationViewWrapper(DateTime currDate);
        Task<ReservationViewWrapper> GetReservationViewWrapperNextPrev(DateTime currDate);
        Task<ReservationViewWrapper> InitializeReservationViewWrapper(DateTime currDate);
        Task<ReservationViewWrapper> RefreshReservationViewWrapper(Reservation reservation, DateTime pageReservationDate);
    }
}
