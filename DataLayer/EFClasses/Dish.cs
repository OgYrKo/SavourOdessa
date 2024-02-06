namespace DataLayer.EfClasses;

public partial class Dish
{
    public int Dishid { get; set; }

    public int Dishtypeid { get; set; }

    public int Dishkitchenid { get; set; }

    public TimeSpan Preparingtime { get; set; }

    public virtual ICollection<Dishbylanguage> Dishbylanguages { get; set; } = new List<Dishbylanguage>();

    public virtual ICollection<Dishinrestaurant> Dishinrestaurants { get; set; } = new List<Dishinrestaurant>();

    public virtual Dishkitchen Dishkitchen { get; set; } = null!;

    public virtual ICollection<Dishphoto> Dishphotos { get; set; } = new List<Dishphoto>();

    public virtual Dishtype Dishtype { get; set; } = null!;
}
