using DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.RestaurantServices.QueryObjects;
using DataLayer.QueryObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.RestaurantServices.Concrete
{
    public class ListRestaurantsService
    {
        private readonly DataContext _context;

        public ListRestaurantsService(DataContext context)
        {
            _context = context;
        }

        public IQueryable<RestaurantListDto> SortFilterPage
            (SortFilterPageOptions options)
        {
            var booksQuery = _context.Restaurants 
                .AsNoTracking()
                .MapRestaurantToDto() 
                .OrderRestaurantsBy(options.OrderByOptions) 
                .FilterRestaurantsBy(options.FilterBy,
                    options.FilterValue); 

            options.SetupRestOfDto(booksQuery); 

            return booksQuery.Page(options.PageNum - 1, 
                options.PageSize); 
        }

    }
}
