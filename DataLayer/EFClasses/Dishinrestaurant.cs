namespace DataLayer.EfClasses;

public partial class Dishinrestaurant
{
    public int Dishinrestaurantid { get; set; }

    public int Restaurantid { get; set; }

    public int Dishid { get; set; }

    public decimal Cost { get; set; }

    public virtual Dish Dish { get; set; } = null!;

    public virtual ICollection<Dishorder> Dishorders { get; set; } = new List<Dishorder>();

    public virtual Restaurant Restaurant { get; set; } = null!;
}
