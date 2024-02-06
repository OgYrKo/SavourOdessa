namespace DataLayer.EfClasses;

public partial class Tablereservation
{
    public int Tablereservationid { get; set; }

    public int Tableid { get; set; }

    public DateTime Reservationtime { get; set; }

    public TimeSpan Duration { get; set; }

    public int Userid { get; set; }

    public virtual ICollection<Dishorder> Dishorders { get; set; } = new List<Dishorder>();

    public virtual Restauranttable Table { get; set; } = null!;

    public virtual Systemuser User { get; set; } = null!;
}
