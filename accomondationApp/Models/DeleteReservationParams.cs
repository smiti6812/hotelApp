namespace accomondationApp.Models
{
    public class DeleteReservationParams
    {
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
        public DateTime PageSelectedDate { get; set; }
    }
}
