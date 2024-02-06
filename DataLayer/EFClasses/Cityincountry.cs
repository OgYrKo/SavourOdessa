namespace DataLayer.EfClasses;

public partial class Cityincountry
{
    public int Cityincountryid { get; set; }

    public int Countryid { get; set; }

    public int Cityid { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Postcode> Postcodes { get; set; } = new List<Postcode>();
}
