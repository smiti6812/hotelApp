using accomondationApp.Models;
using accomondationApp.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace accomondationApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private HotelAppDbContext hotelAppDbContext;
        private ReservationViewService reservationViewService;
        private int months = 3;
        
        public ReservationController(HotelAppDbContext _hotelAppDbContext)
        {
            hotelAppDbContext = _hotelAppDbContext;
            reservationViewService = new ReservationViewService(hotelAppDbContext);
        }     

        [HttpGet]       
        public async Task<ReservationView[]> Get() => await reservationViewService.GetReservationView(months);
        

    }
}
