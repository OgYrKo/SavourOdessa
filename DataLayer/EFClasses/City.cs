namespace DataLayer.EfClasses;

public partial class City
{
    public int Cityid { get; set; }

    public string Cityname { get; set; } = null!;

    public virtual ICollection<Cityincountry> Cityincountries { get; set; } = new List<Cityincountry>();
}
