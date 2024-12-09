
using accomondationApp.Models;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace accomondationApp.Repositories
{
    public class SlideRepository : ISlideRepository
    {
        private readonly HotelAppDBContext dbContext;
        public SlideRepository(HotelAppDBContext _dbContext) => dbContext = _dbContext;      
        public Task<IEnumerable<Slide>> GetAllSlides() => Task.FromResult(dbContext.Slides.Include(pp => pp.PicturePaths).AsEnumerable<Slide>());
        public Task<Slide> GetSingleSlide(int roomId) => Task.FromResult(dbContext.Slides.Include(pp => pp.PicturePaths).First(f => f.PicturePaths.Any(p => p.RoomId == roomId)));
    }
}
