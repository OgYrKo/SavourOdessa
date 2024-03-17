
using NodaTime;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.EfClasses;

public partial class Repeatrule
{
    public int Repeatrulesid { get; set; }

    public string Repeatrulestype { get; set; } = null!;

    //public TimeSpan Repeatrulesduration { get; set; }
    //[NotMapped]
    //public Period Repeatrulesduration { get; set; } = null!;

    public virtual ICollection<Openingrule> Openingrules { get; set; } = new List<Openingrule>();
}
