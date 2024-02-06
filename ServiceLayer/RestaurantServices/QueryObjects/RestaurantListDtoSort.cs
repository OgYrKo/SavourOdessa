using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.RestaurantServices.QueryObjects
{
    public enum OrderByOptions
    {
        [Display(Name = "sort by...")] SimpleOrder = 0,
        [Display(Name = "Rating ↓")] ByRatingLowestFirst,
        [Display(Name = "Rating ↑")] ByRatingHigestFirst
    }
    public static class RestaurantListDtoSort
    {
        public static IQueryable<RestaurantListDto> OrderRestaurantsBy
        (this IQueryable<RestaurantListDto> restaurants,
            OrderByOptions orderByOptions)
        {
            switch (orderByOptions)
            {
                case OrderByOptions.SimpleOrder:
                    return restaurants.OrderByDescending(x => x.Restaurantid);
                case OrderByOptions.ByRatingLowestFirst: 
                    return restaurants.OrderBy(x => x.AverageRating); 
                case OrderByOptions.ByRatingHigestFirst: 
                    return restaurants.OrderByDescending(x => x.AverageRating);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(orderByOptions), orderByOptions, null);
            }
        }
    }
}
