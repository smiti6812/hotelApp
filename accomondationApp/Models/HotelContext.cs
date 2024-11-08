using Microsoft.EntityFrameworkCore;



namespace accomondationApp.Models
{
    public class HotelContext: Microsoft.EntityFrameworkCore.DbContext
    {        
        public HotelContext(DbContextOptions<HotelContext> options)
           : base(options)
        {
        }
    }
}
