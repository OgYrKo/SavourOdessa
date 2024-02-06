using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.RestaurantServices.QueryObjects
{
    public enum RestaurantsFilterBy
    {
        [Display(Name = "All")] NoFilter = 0,
        [Display(Name = "By Rating...")] ByVotes,
        [Display(Name = "By Address...")] ByAddress
    }
    public static class RestaurantListDtoFilter
    {
        public static IQueryable<RestaurantListDto> FilterRestaurantsBy
        (this IQueryable<RestaurantListDto> restaurants,
                       RestaurantsFilterBy filterBy, string filterValue)
        {
            switch (filterBy)
            {
                case RestaurantsFilterBy.NoFilter:
                    return restaurants;
                case RestaurantsFilterBy.ByVotes:
                    return restaurants.Where(x => x.AverageRating >= Convert.ToDouble(filterValue));
                case RestaurantsFilterBy.ByAddress:
                    return restaurants.Where(x => x.Address.Contains(filterValue));
                default:
                    throw new ArgumentOutOfRangeException(nameof(filterBy), filterBy, null);
            }
        }
    }
}
