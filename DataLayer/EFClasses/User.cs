using DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EFClasses
{
    public partial class User
    {
        public int Usesysid { get; set; }
        public string Username { get; set; }
        public string Rolname { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();


        //public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();

        //public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();

        //public virtual ICollection<Restaurantstaff> Restaurantstaffs { get; set; } = new List<Restaurantstaff>();

        public virtual ICollection<Tablereservation> Tablereservations { get; set; } = new List<Tablereservation>();
    }
}
