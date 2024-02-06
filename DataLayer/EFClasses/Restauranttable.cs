using NpgsqlTypes;

namespace DataLayer.EfClasses;

public partial class Restauranttable
{
    public int Tableid { get; set; }

    public NpgsqlPoint Tablelocation { get; set; }

    public int Restaurantid { get; set; }

    public int Sitscount { get; set; }

    public virtual Restaurant Restaurant { get; set; } = null!;

    public virtual ICollection<Tablereservation> Tablereservations { get; set; } = new List<Tablereservation>();
}
