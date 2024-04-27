namespace SavourOdessa.Areas.Manager.Models.Restaurants
{
    public class StatsViewModel
    {
        public int RestaurantId { get; set; }
        public int Year { get; set; } = -1;
        public int Month { get; set; } = -1;
        public IEnumerable<DataLayer.EFClasses.DailyReservationChanges>? DailyReservationChanges { get; set; }
        public IEnumerable<DataLayer.EFClasses.MonthlyReservationChanges>? MonthlyReservationChanges { get; set; }

    }
}
