namespace DataLayer.EfClasses;

public partial class Dishorder
{
    public int Dishorderid { get; set; }

    public int Dishrestaurantid { get; set; }

    public int Tablereservationid { get; set; }

    public int Count { get; set; }

    public TimeOnly Takeawaytime { get; set; }

    public virtual Dishinrestaurant Dishrestaurant { get; set; } = null!;

    public virtual Tablereservation Tablereservation { get; set; } = null!;
}
