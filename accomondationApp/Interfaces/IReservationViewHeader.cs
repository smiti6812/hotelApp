namespace accomondationApp.Interfaces
{
    public interface IReservationViewHeader
    {
        DateTime[] DayDates { get; set; }
        int[] Days { get; set; }
        string[] WeekDays { get; set; }
    }
}