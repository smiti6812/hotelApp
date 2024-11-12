namespace accomondationApp.ViewModel
{
    public class ReservationViewWrapper
    {
        public ReservationView[] ReservationView { get; set; }
        public bool[,] SelectedDateArr { get; set; }
        public ReservationViewHeader ReservationViewHeader { get; set; }
        public int[] MonthArr { get; set; }
    }
}
