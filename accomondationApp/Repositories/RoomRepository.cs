using System.Runtime.Intrinsics.Arm;

using accomondationApp.Models;

using Microsoft.EntityFrameworkCore;

namespace accomondationApp.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private HotelAppDBContext db;

        public RoomRepository(HotelAppDBContext _db)
        {
            db = _db;
        }
        public Task<Room[]> ReturnRooms()
        {
            return Task.FromResult(db.Rooms.Include(c => c.RoomCapacity).ToArray<Room>());
        }
    }
}
