namespace DataLayer.EfClasses;

public partial class Favourite
{
    public int Favouriteid { get; set; }

    public int Restaurantid { get; set; }

    public int Userid { get; set; }

    public virtual Restaurant Restaurant { get; set; } = null!;

    public virtual Systemuser User { get; set; } = null!;
}
