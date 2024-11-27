using accomondationApp.Interfaces;
using accomondationApp.Models;
using accomondationApp.Repositories;

namespace accomondationApp.ViewModel
{
    public class ReservationViewProperties : IReservationViewProperties
    {
        private readonly IReservationRepository reservationRepository;
        public ReservationViewProperties(IReservationRepository _reservationRepository) => reservationRepository = _reservationRepository;
        public (bool checkReservation, Reservation reservation) CheckReservation(int roomId, DateTime date) =>
        reservationRepository.CheckReturnReservation(roomId, date).Result is not Reservation r ? (false, null) : (true, r);
      

        public (bool checkReservation, bool startSaved, bool endSaved, string reservationName, bool selectedDateArr) ReturnReservationViewProperties(int roomId, DateTime date)
        {
            var checkReservation = CheckReservation(roomId, date);
            return checkReservation.checkReservation switch
            {
                true when checkReservation.reservation.StartDate == checkReservation.reservation.EndDate => (true, true, true, checkReservation.reservation.Customer.Name, true),
                true when checkReservation.reservation.StartDate == date => (true, true, false, string.Empty, true),
                true when checkReservation.reservation.StartDate == date.AddDays(-1) && date == checkReservation.reservation.EndDate => (true, false, true, checkReservation.reservation.Customer.Name, true),
                true when checkReservation.reservation.StartDate == date.AddDays(-1) => (true, false, false, checkReservation.reservation.Customer.Name, true),
                true when checkReservation.reservation.EndDate == date => (true, false, true, string.Empty, true),
                true => (true, false, false, string.Empty, true),
                _ => (false, false, false, string.Empty, false),
            };
        }
    }
}
