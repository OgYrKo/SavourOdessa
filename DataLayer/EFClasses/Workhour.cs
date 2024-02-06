namespace DataLayer.EfClasses;

public partial class Workhour
{
    public int Workhoursid { get; set; }

    public int Openingrulesid { get; set; }

    public TimeOnly Openhours { get; set; }

    public TimeOnly Closehours { get; set; }

    public virtual Openingrule Openingrules { get; set; } = null!;
}
