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

    }
}
