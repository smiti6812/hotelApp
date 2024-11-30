
using accomondationApp.Models;
using accomondationApp.Repositories;

namespace accomondationApp
{
    public class SlideService : ISlideService
    {
        private readonly ISlideRepository slideRepository;
        public SlideService(ISlideRepository _slideSRepository) => slideRepository = _slideSRepository;
        public async Task<IEnumerable<Slide>> GetAllSlides() => await slideRepository.GetAllSlides();
    }
}
