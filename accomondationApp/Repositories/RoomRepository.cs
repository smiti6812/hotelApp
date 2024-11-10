using accomondationApp.Models;

namespace accomondationApp.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private HotelAppDbContext db;
        
        public RoomRepository(HotelAppDbContext _db) 
        {
            db = _db;
        }
        public Task<IEnumerable<Room?>> ReturnRooms()
        {
            var result = db.Rooms;
            return Task.FromResult((IEnumerable<Room?>)result);
        }
    }
}
