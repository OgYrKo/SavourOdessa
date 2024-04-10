namespace DataLayer.EfClasses;

public partial class Systemuser
{
    public int Userid { get; set; }

    public string Username { get; set; } = null!;

    public int Userroleid { get; set; }

    //public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    //public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();

    public virtual ICollection<Restaurantstaff> Restaurantstaffs { get; set; } = new List<Restaurantstaff>();

    //public virtual ICollection<Tablereservation> Tablereservations { get; set; } = new List<Tablereservation>();

    public virtual Userrole Userrole { get; set; } = null!;
}
