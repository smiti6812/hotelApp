
using accomondationApp.Interfaces;
using accomondationApp.Models;
using accomondationApp.Repositories;

namespace accomondationApp
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IRoomRepository roomRepository;
        public ReservationService(IReservationRepository _reservationRepository, IRoomRepository _roomRepository)
        {
            reservationRepository = _reservationRepository;
            roomRepository = _roomRepository;
        }            
        public async Task<bool?> DeleteReservation(int reservationId) => await reservationRepository.DeleteReservationAsync(reservationId);
        public int DisplayedMonth() => reservationRepository.DisplayMonths();     
        public Task<IEnumerable<Reservation>> GetReservations(DateTime date) => Task.FromResult(reservationRepository.GetReservations(date));
        public async Task<Reservation> SaveReservation(Reservation reservation) => await reservationRepository.AddReservationAsync(reservation);
        public async Task<IEnumerable<Room>> GetAllRooms() => await roomRepository.ReturnRooms();
        public async Task<Reservation> CheckReturnReservation(int roomId, DateTime date) => await reservationRepository.CheckReturnReservation(roomId, date);
    }
}
