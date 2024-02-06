namespace DataLayer.EfClasses;

public partial class Comment
{
    public int Commentid { get; set; }

    public int Restaurantid { get; set; }

    public string Commenttext { get; set; } = null!;

    public DateTime Commentdate { get; set; }

    public int Userid { get; set; }

    public virtual Restaurant Restaurant { get; set; } = null!;

    public virtual Systemuser User { get; set; } = null!;
}
