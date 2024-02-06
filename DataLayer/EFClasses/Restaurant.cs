namespace DataLayer.EfClasses;

public partial class Restaurant
{
    public int Restaurantid { get; set; }

    public string Restaurantname { get; set; } = null!;

    public string Postcode { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string Housenum { get; set; } = null!;

    public bool Verified { get; set; }

    public int Ownerid { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Dishinrestaurant> Dishinrestaurants { get; set; } = new List<Dishinrestaurant>();

    public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

    public virtual ICollection<Openingrule> Openingrules { get; set; } = new List<Openingrule>();

    public virtual Systemuser Owner { get; set; } = null!;

    public virtual Postcode PostcodeNavigation { get; set; } = null!;

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Restaurantphoto> Restaurantphotos { get; set; } = new List<Restaurantphoto>();

    public virtual ICollection<Restaurantstaff> Restaurantstaffs { get; set; } = new List<Restaurantstaff>();

    public virtual ICollection<Restauranttable> Restauranttables { get; set; } = new List<Restauranttable>();
}
