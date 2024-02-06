namespace DataLayer.EfClasses;

public partial class Currency
{
    public int Currencyid { get; set; }

    public string Currencyname { get; set; } = null!;

    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();
}
