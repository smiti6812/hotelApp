using accomondationApp.Models;

namespace accomondationApp.Repositories
{
    public interface ISlideService
    {
        Task<IEnumerable<Slide>> GetAllSlides();
    }
}