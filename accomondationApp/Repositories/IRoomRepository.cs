using accomondationApp.Models;

namespace accomondationApp.Repositories
{
    public interface IRoomRepository
    {
       Task<Room[]> ReturnRooms();
    }
}
