using DataLayer.EfClasses;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.RestaurantServices;
using ServiceLayer.RestaurantServices.Concrete;

namespace SavourOdessa.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly DataContext _context;
        public RestaurantController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Index(SortFilterPageOptions options)
        {
            var listService = new ListRestaurantsService(_context);
            var restaurantList = listService.SortFilterPage(options).ToList();
            return View(new RestaurantListCombinedDto(options,restaurantList));
        }
    }
}
