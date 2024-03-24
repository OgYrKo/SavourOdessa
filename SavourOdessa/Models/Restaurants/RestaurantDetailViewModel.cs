namespace SavourOdessa.Models.Restaurants
{
    public class RestaurantDetailViewModel
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string[]? Photos { get; set; }
        public double AverageRating { get; set; }
        public bool IsOpened { get; set; }
        public DateTime? OpenTime { get; set; }
        public DateTime? CloseTime { get; set; }
        public CommentViewModel CommentViewModel { get; set; } = null!;

        public RestaurantDetailViewModel(int restaurantId, 
                                         string restaurantName, 
                                            string address,
                                            string[]? photos, 
                                            double averageRating, 
                                            bool isOpened, 
                                            DateTime? openTime, 
                                            DateTime? closeTime,
                                            CommentViewModel commentViewModel)
        {
            RestaurantId = restaurantId;
            RestaurantName = restaurantName;
            Address = address;
            Photos = photos;
            AverageRating = averageRating;
            IsOpened = isOpened;
            OpenTime = openTime;
            CloseTime = closeTime;
            CommentViewModel = commentViewModel;
        }
    }
}
