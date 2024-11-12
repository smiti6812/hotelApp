using accomondationApp.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;

namespace accomondationApp.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private HotelAppDBContext dbContext;

        private static ConcurrentDictionary<int, Reservation>? reservationCache;
        public ReservationRepository(HotelAppDBContext _dbContext)
        {
            dbContext = _dbContext;
            if (reservationCache == null)
            {
                reservationCache = new ConcurrentDictionary<int, Reservation>(dbContext.Reservations.ToDictionary(c => c.ReservationId));
            }
        }
        public async Task<Reservation?> AddReservationAsync(Reservation reservation)
        {
            EntityEntry<Reservation> added = await dbContext.Reservations.AddAsync(reservation);
            int affected = await dbContext.SaveChangesAsync();
            if (affected == 1)
            {
                if (reservationCache is null)
                {
                    return reservation;
                }

                return reservationCache.AddOrUpdate(reservation.ReservationId, reservation, UpdateCache);
            }

            return null;

        }

        private Reservation UpdateCache(int reservationId, Reservation reservation)
        {
            Reservation? old;
            if (reservationCache != null)
            {
                if (reservationCache.TryGetValue(reservationId, out old))
                {
                    if (reservationCache.TryUpdate(reservationId, reservation, old))
                    {
                        return reservation;
                    }
                }
            }
            return null!;
        }

        public async Task<bool?> DeleteReservationAsync(int reservationId)
        {
            Reservation? res = dbContext.Reservations.Find(reservationId);
            if (res == null)
            {
                return null;
            }
            dbContext.Reservations.Remove(res);
            int affected = await dbContext.SaveChangesAsync();
            if (affected == 1)
            {
                if (reservationCache is null)
                {
                    return null;
                }

                return reservationCache.TryRemove(reservationId, out res);
            }

            return null;
        }

        public Task<IEnumerable<Reservation?>> GetReservationsAsync(DateTime startDate, DateTime endDate)
        {
            return Task.FromResult(reservationCache is null ? Enumerable.Empty<Reservation>() : reservationCache.Values.Where(r => r.StartDate >= startDate && r.EndDate <= endDate));
        }

        public Task<Reservation?>? GetSingleReservationAsync(int reservationId)
        {
            if ( reservationCache is null)
            {
                return null;
            }

            reservationCache.TryGetValue(reservationId, out Reservation? res);
            return Task.FromResult(res);
        }
    }
}
