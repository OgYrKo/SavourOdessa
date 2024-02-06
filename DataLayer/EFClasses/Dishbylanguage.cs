namespace DataLayer.EfClasses;

public partial class Dishbylanguage
{
    public int Dishbylanguageid { get; set; }

    public int Dishid { get; set; }

    public string Dishname { get; set; } = null!;

    public string Dishcomposition { get; set; } = null!;

    public int Languageid { get; set; }

    public virtual Dish Dish { get; set; } = null!;

    public virtual Language Language { get; set; } = null!;
}
