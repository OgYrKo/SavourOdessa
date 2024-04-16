using System.ComponentModel.DataAnnotations;

namespace SavourOdessa.Areas.Manager.Models.Restaurants
{
    public class DetailsViewModel
    {
        public int RestaurantId { get; set; }
        public int DayOffset { get; set; }
        public IEnumerable<ReservationItemViewModel> Reservations { get; set; } = null!;
        
    }

    public class ReservationItemViewModel
    {
        public int ReservationId { get; set; }
        public int TableId { get; set; }
        [DisplayFormat(DataFormatString = "{0:t}")]
        public DateTime Time { get; set; }
        public TimeSpan Duration { get; set; }
        public string UserName { get; set; } = null!;
        public bool IsCompleted { get; set; }
    }
}
