namespace ServiceLayer.DishServices
{
    public class DishListDto
    {
        public int Dishid { get; set; }
        public string Dishname { get; set; } = null!;
        public string Dishtype { get; set; } = null!;
        public TimeSpan Preparingtime { get; set; }
        public string Dishkitchen { get; set; } = null!;
        public string Photo { get; set; } = null!;
        public decimal AverageRating { get; set; }
    }
}
