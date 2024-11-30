using accomondationApp.Models;

namespace accomondationApp.Repositories
{
    public interface ISlideRepository
    {
        Task<IEnumerable<Slide>> GetAllSlides();
        Task<Slide> GetSingleSlide(int roomId);
    }
}
