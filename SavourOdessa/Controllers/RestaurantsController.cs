using DataLayer.EfClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.RestaurantServices;
using ServiceLayer.RestaurantServices.Concrete;
using System.ComponentModel;

namespace SavourOdessa.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly DataContext _context;
        public RestaurantsController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var restauranrs = await _context.Restaurants
                                            .Include(r => r.PostcodeNavigation)
                                            .Include(r => r.PostcodeNavigation.Cityincountry)
                                            .Include(r => r.PostcodeNavigation.Cityincountry.City)
                                            .Include(r => r.PostcodeNavigation.Cityincountry.Country)
                                            .ToListAsync();
            List<RestaurantListDto> restaurantListDto = new();
            foreach (var restaurant in restauranrs)
            {
                restaurantListDto.Add(new RestaurantListDto(restaurant.Restaurantid,
                                                            restaurant.Restaurantname,
                                                            GetAddress(restaurant),
                                                            GetFirstImage(restaurant.Restaurantname),
                                                            await GetAverage(restaurant),
                                                            restaurant.Verified));
            }

            return View(restaurantListDto);
        }

        private string GetFirstImage(string restaurantName)
        {
            string restaurantsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Restaurants");
            string restaurantPath = Path.Combine(restaurantsPath, restaurantName);
            string? firstImage = null;
            if (Directory.Exists(restaurantPath))
            {
                firstImage = Directory.GetFiles(restaurantPath, "*.*", SearchOption.AllDirectories)
                                     .FirstOrDefault();
            }
            if (firstImage != null)
            {
                return "~/" + Path.GetRelativePath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), firstImage).Replace("\\", "/");
            }
            return "~/images/Restaurants/default.png";
        }

        private string GetAddress(Restaurant restaurant)
        {
            string city = restaurant.PostcodeNavigation.Cityincountry.City.Cityname;
            string country = restaurant.PostcodeNavigation.Cityincountry.Country.Countryname;
            string street = restaurant.Street;
            string housenum = restaurant.Housenum;
            return $"{country}, {city}, {street}, {housenum}";
        }

        //TODO convert it into a stored procedure
        private async Task<double> GetAverage(Restaurant restaurant)
        {
            var ratings = await _context.Ratings.Where(r => r.Restaurantid == restaurant.Restaurantid).ToArrayAsync();
            if (ratings.Length == 0)
            {
                return 0;
            }
            else
            {
                return ratings.Average(r => r.Rate);
            }
        }
    }
}
