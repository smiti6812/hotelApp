using accomondationApp.Models;
using accomondationApp.Repositories;
using accomondationApp.ViewModel;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace accomondationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SlidesController : ControllerBase
    {
        private readonly ISlideService slideService;
        public SlidesController(ISlideService _slideService) => slideService = _slideService;

        [HttpGet]
        public Task<Slide[]> GetSlides() => Task.FromResult(slideService.GetAllSlides().Result.ToArray());
       

    }
}
