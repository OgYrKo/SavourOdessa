namespace DataLayer.EfClasses;

public partial class Postcode
{
    public string Postcode1 { get; set; } = null!;

    public int Cityincountryid { get; set; }

    public virtual Cityincountry Cityincountry { get; set; } = null!;

    public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
}
