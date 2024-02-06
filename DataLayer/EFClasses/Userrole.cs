namespace DataLayer.EfClasses;

public partial class Userrole
{
    public int Userroleid { get; set; }

    public string Userrolename { get; set; } = null!;

    public virtual ICollection<Systemuser> Systemusers { get; set; } = new List<Systemuser>();
}
