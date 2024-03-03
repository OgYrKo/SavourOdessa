namespace ServiceLayer.RestaurantServices
{
    public class RestaurantListDto
    {
        public int Restaurantid { get; set; }
        public string Restaurantname { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Photo { get; set; }
        public double AverageRating { get; set; }
        public bool IsVerified { get; set; }
        public RestaurantListDto()
        {
        }

        public RestaurantListDto(int restaurantid, string restaurantname, string address, string? photo, double averageRating, bool isVerified)
        {
            Restaurantid = restaurantid;
            Restaurantname = restaurantname;
            Address = address;
            Photo = photo;
            AverageRating = averageRating;
            IsVerified = isVerified;
        }
    }
}
