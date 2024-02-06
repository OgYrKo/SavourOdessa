namespace DataLayer.EfClasses;

public partial class Restaurantphoto
{
    public int Restaurantphotoid { get; set; }

    public int Restaurantid { get; set; }

    public string Restaurantphotopath { get; set; } = null!;

    public virtual Restaurant Restaurant { get; set; } = null!;
}
