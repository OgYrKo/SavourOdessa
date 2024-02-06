namespace DataLayer.EfClasses;

public partial class Language
{
    public int Languageid { get; set; }

    public string Languagename { get; set; } = null!;

    public virtual ICollection<Dishbylanguage> Dishbylanguages { get; set; } = new List<Dishbylanguage>();
}
