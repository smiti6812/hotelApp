using accomondationApp.Models;

namespace accomondationApp.Repositories
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room?>>ReturnRooms();
    }
}
