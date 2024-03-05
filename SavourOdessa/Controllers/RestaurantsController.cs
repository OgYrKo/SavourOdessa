using DataLayer.EfClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavourOdessa.Models.Restaurants;
using ServiceLayer.RestaurantServices;


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

        public async Task<ActionResult> Details(int id)
        {
            var restaurant = _context.Restaurants
                                    .Include(r => r.PostcodeNavigation)
                                    .Include(r => r.PostcodeNavigation.Cityincountry)
                                    .Include(r => r.PostcodeNavigation.Cityincountry.City)
                                    .Include(r => r.PostcodeNavigation.Cityincountry.Country)
                                    .FirstOrDefaultAsync(r => r.Restaurantid == id)
                                    .Result;
            if(restaurant == null) return NotFound();
            var comments = _context.Comments
                                    .Include(c => c.User)
                                    .Where(c => c.Restaurantid == id)
                                    .ToArrayAsync()
                                    .Result;
            var commentsDto = new CommentListItemViewModel[comments.Length];
            for (int i = 0; i < comments.Length; i++)
            {
                commentsDto[i] = new CommentListItemViewModel(comments[i].User.Username,comments[i].Commentdate, comments[i].Commenttext);
            }

            if (restaurant == null) return NotFound();
            

            var openingHours = GetOpeningHours(DateTime.Now);
            

            
            return View(new RestaurantDetailViewModel(restaurant.Restaurantid,
                                                            restaurant.Restaurantname,
                                                            GetAddress(restaurant),
                                                            GetImages(restaurant.Restaurantname).ToArray(),
                                                            await GetAverage(restaurant),
                                                            openingHours.Start<=DateTime.Now&&DateTime.Now<=openingHours.End,
                                                            openingHours.Start,
                                                            openingHours.End,
                                                            new CommentListViewModel(commentsDto)));
        }

        //TODO: create a stored procedure for this
        private (DateTime Start,DateTime End) GetOpeningHours(DateTime date)
        {
            DateTime start = DateTime.Now.Date.AddHours(6);
            DateTime end = DateTime.Now.Date.AddDays(1);
            return (start, end);
        }

        private IEnumerable<string> GetImages(string restaurantName)
        {
            string restaurantsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Restaurants");
            string restaurantPath = Path.Combine(restaurantsPath, restaurantName);
            if (!Directory.Exists(restaurantPath)) 
            { 
                yield return "~/images/Restaurants/default.png";
                yield break; 
            }
            
            string[] images = Directory.GetFiles(restaurantPath, "*.*", SearchOption.AllDirectories);
            foreach (var image in images)
            {
                yield return "~/" + Path.GetRelativePath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), image).Replace("\\", "/");
            }
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
