namespace DataLayer.EfClasses;

public partial class Openingrule
{
    public int Openingrulesid { get; set; }

    public int Restaurantid { get; set; }

    public DateOnly Startday { get; set; }

    public int Repeatrulesid { get; set; }

    public virtual Repeatrule Repeatrules { get; set; } = null!;

    public virtual Restaurant Restaurant { get; set; } = null!;

    public virtual Workhour? Workhour { get; set; }
}
