namespace DataLayer.EfClasses;

public partial class Dishkitchen
{
    public int Dishkitchenid { get; set; }

    public string Dishkitchenname { get; set; } = null!;

    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
