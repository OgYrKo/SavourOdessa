namespace DataLayer.EfClasses;

public partial class Dishphoto
{
    public int Dishphotoid { get; set; }

    public int Dishid { get; set; }

    public string Dishphotopath { get; set; } = null!;

    public virtual Dish Dish { get; set; } = null!;
}
