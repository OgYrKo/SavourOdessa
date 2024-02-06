namespace DataLayer.EfClasses;

public partial class Rating
{
    public int Ratingid { get; set; }

    public int Restaurantid { get; set; }

    public int Userid { get; set; }

    public int Rate { get; set; }

    public virtual Restaurant Restaurant { get; set; } = null!;

    public virtual Systemuser User { get; set; } = null!;
}
