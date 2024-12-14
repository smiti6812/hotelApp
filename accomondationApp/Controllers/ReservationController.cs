using accomondationApp.Interfaces;
using accomondationApp.Models;
using accomondationApp.Repositories;
using accomondationApp.ViewModel;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace accomondationApp.Controllers
{
    [ApiController]   
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationViewService reservationViewService;
        private readonly IReservationService reservationService;

        public ReservationController(IReservationViewService _reservationViewService, IReservationService reservationService)
        {
            reservationViewService = _reservationViewService;
            this.reservationService = reservationService;
        }
        [HttpPost("delete")]
        [Authorize(Roles ="Admin")]
        public async Task<ReservationViewWrapper> DeleteReservation([FromBody] DeleteReservationParams parameters)
        {
            var reservation = await reservationService.CheckReturnReservation(parameters.RoomId, parameters.Date);
            bool deleted = await reservationService.DeleteReservation(reservation.ReservationId) ?? false;
            DateTime pageSelecteddate = parameters.PageSelectedDate.AddDays(-parameters.PageSelectedDate.Day + 1).Date;
            return deleted ? await reservationViewService.CallRefreshReservationViewWrapper(reservation, pageSelecteddate) : await reservationViewService.CallGetReservationViewWrapperNextPrev(pageSelecteddate);
        }
        [HttpPost]
        public async Task<ReservationViewWrapper?> SaveReservation([FromBody] ReservationParams reservationParams)
        {
            var reservation = reservationService.SaveReservation(reservationParams.Reservation).Result;
            return await reservationViewService.CallRefreshReservationViewWrapper(reservation, reservationParams.PageSelectedDate.AddDays(-reservationParams.PageSelectedDate.Day + 1).Date);
        }

        [HttpGet("nextprev")]
        public async Task<ReservationViewWrapper> GetResViewWrapperPrevNext(DateTime currDate) => await reservationViewService.CallGetReservationViewWrapperNextPrev(currDate.AddDays(-currDate.Day + 1).Date);

        [HttpGet]    
        public async Task<ReservationViewWrapper> Get(DateTime currDate) => await reservationViewService.CallGetReservationViewWrapper(currDate.AddDays(-currDate.Day + 1).Date);

    }
}
