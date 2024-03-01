using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.RestaurantServices
{
    public class RestaurantListCombinedDto
    {
        public RestaurantListCombinedDto(SortFilterPageOptions sortFilterPageData, IEnumerable<RestaurantListDto> restaurantList)
        {
            SortFilterPageData = sortFilterPageData;
            RestaurantList = restaurantList;
        }

        public SortFilterPageOptions SortFilterPageData { get; private set; }

        public IEnumerable<RestaurantListDto> RestaurantList { get; private set; }
    }
}
