namespace DataLayer.EfClasses;

public partial class Country
{
    public int Countryid { get; set; }

    public string Countryname { get; set; } = null!;

    public int Currencyid { get; set; }

    public virtual ICollection<Cityincountry> Cityincountries { get; set; } = new List<Cityincountry>();

    public virtual Currency Currency { get; set; } = null!;
}
