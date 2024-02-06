using DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.RestaurantServices.QueryObjects
{
    public static class RestaurantListDtoSelect
    {
        public static IQueryable<RestaurantListDto> MapRestaurantToDto(this IQueryable<Restaurant> restaurants)
        {
            return restaurants.Select(r => new RestaurantListDto
            {
                Restaurantid = r.Restaurantid,
                Restaurantname = r.Restaurantname,
                Address = r.PostcodeNavigation.Cityincountry.Country.Countryname + ", " + r.PostcodeNavigation.Cityincountry.City.Cityname + ", " + r.Street + ", " + r.Housenum,
                Photo = r.Restaurantphotos.Select(photo => photo.Restaurantphotopath).FirstOrDefault(),
                AverageRating = r.Ratings.Select(rating => rating.Rate).Average(),
                IsVerified = r.Verified
            });
        }
    }
}
