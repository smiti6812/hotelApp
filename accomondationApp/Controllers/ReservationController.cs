using accomondationApp.Models;
using accomondationApp.Repositories;
using accomondationApp.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace accomondationApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationViewService reservationViewService;
        private readonly IRoomRepository roomRepository;
        
        public ReservationController(HotelAppDBContext context, IRoomRepository _roomRepository)
        {            
            reservationViewService = new ReservationViewService(context, _roomRepository);
            roomRepository = _roomRepository;
        }     
        
        [HttpGet]     
        public async Task<ReservationView[]> Get(DateTime currDate) => await reservationViewService.GetReservationView(currDate);
        
        /*
        [HttpGet]        
        public Room GetRooms()
        {   
            
            return Enumerable.Range(0, roomRepository.ReturnRooms().Length).Select( index =>
                roomRepository.ReturnRooms()[index]
            ).ToArray();  
            
            var room = new Room();
            room.RoomId = 1;
            room.RoomNumber = "Room1";
            room.RoomCapacityId = 1;
            room.RoomStatusId = 1;
            return room;

        }
    */

    }
}
