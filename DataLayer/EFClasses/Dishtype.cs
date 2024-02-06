namespace DataLayer.EfClasses;

public partial class Dishtype
{
    public int Dishtypeid { get; set; }

    public string Dishtypename { get; set; } = null!;

    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
