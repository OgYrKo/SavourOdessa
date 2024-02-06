namespace DataLayer.EfClasses;

public partial class Restaurantstaff
{
    public int Staffid { get; set; }

    public int Restaurantid { get; set; }

    public int Userid { get; set; }

    public bool Verified { get; set; }

    public virtual Restaurant Restaurant { get; set; } = null!;

    public virtual Systemuser User { get; set; } = null!;
}
