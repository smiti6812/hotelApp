using accomondationApp.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace accomondationApp.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly HotelAppDBContext dbContext;
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
            try
            {
                var cust = dbContext.Customers.Where(c => c.Name == reservation.Customer.Name && c.Email == reservation.Customer.Email).FirstOrDefault();
                if (cust is Customer)
                {
                    reservation.Customer = null;
                    reservation.CustomerId = cust.CustomerId;
                }
                EntityEntry<Reservation> added = await dbContext.Reservations.AddAsync(reservation);
                int affected = await dbContext.SaveChangesAsync();
                if (affected > 0)
                {
                    if (reservationCache is null)
                    {
                        return reservation;
                    }

                    return reservationCache.AddOrUpdate(reservation.ReservationId, reservation, UpdateCache);
                }
            }
            catch(Exception ex)
            {
                string err = ex.Message;
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

        public int DisplayMonths() => dbContext.DisplayedMonths.First().DisplayedMonths ?? 0;

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

        public Task<Reservation> CheckReturnReservation(int roomId, DateTime date) => Task.FromResult(dbContext.Reservations.FirstOrDefault(c => c.RommId == roomId && c.StartDate <= date && date <= c.EndDate));
        public IEnumerable<Reservation> GetReservations(DateTime date) => dbContext.Reservations.Include(cust => cust.Customer).Include(room => room.Romm).Where(c => c.EndDate >= date.AddDays(-date.Day) && date.AddDays(-date.Day) <= c.StartDate).ToList();
        public Task<IEnumerable<Reservation>> GetReservationsAsync(DateTime startDate, DateTime endDate)
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
