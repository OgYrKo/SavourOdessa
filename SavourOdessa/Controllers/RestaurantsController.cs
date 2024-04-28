using DataLayer.EfClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
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
            if (restaurant == null) return NotFound();
            var comments = _context.Comments
                                    .Include(c => c.User)
                                    .Where(c => c.Restaurantid == id)
                                    .ToArrayAsync()
                                    .Result;

            var commentsList = new List<CommentListItemViewModel>(comments.Length);
            for (int i = 0; i < comments.Length; i++)
            {
                commentsList.Add(new CommentListItemViewModel(comments[i].User.Username, comments[i].Commentdate, comments[i].Commenttext));
            }
            var commentViewModel = new CommentViewModel() { RestaurantId = id, CommentList = new CommentListViewModel(commentsList)};


            var openingHours = _context.GetSchedule(restaurant.Restaurantid,DateTime.Now);

            var viewModel = new RestaurantDetailViewModel(restaurant.Restaurantid,
                                                            restaurant.Restaurantname,
                                                            GetAddress(restaurant),
                                                            GetImages(restaurant.Restaurantname).ToArray(),
                                                            await GetAverage(restaurant),
                                                            openingHours.Start,
                                                            openingHours.End,
                                                            commentViewModel);

            var restaurantDetailViewModelJson = JsonConvert.SerializeObject(viewModel);
            TempData["RestaurantDetailViewModel"] = restaurantDetailViewModelJson;
            //TempData["ViewModel"] = viewModel;
            return View(viewModel);
        }
        [HttpPost]
        [CustomAuthorize("Client")]
        public async Task<ActionResult> AddComment(string text)
        {
            var restaurantDetailViewModelJson = (string)TempData["RestaurantDetailViewModel"]!;
            var viewModel = JsonConvert.DeserializeObject<RestaurantDetailViewModel>(restaurantDetailViewModelJson)!;
            if (ModelState.IsValid)
            {
                try
                {
                    //var viewModel = TempData["ViewModel"] as RestaurantDetailViewModel;

                    

                    var date = DateTime.Now;
                    var userName = HttpContext.Session.GetString("Username");
                    var user = await _context.Users.Where(u => u.Username == userName).FirstAsync();
                    await AddCommentToDb(text, user.Usesysid, viewModel.RestaurantId);
                    viewModel.CommentViewModel.AddComment(new CommentListItemViewModel() { UserName = userName, Date = date,Text = text }) ;
                }
                catch(PostgresException e) 
                {
                    ModelState.AddModelError("", e.MessageText);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            restaurantDetailViewModelJson = JsonConvert.SerializeObject(viewModel);
            TempData["RestaurantDetailViewModel"] = restaurantDetailViewModelJson;
            return View("Details", viewModel);
        }


        private async Task AddCommentToDb(string text, int userId, int restaurantId)
        {
            var comment = new Comment
            {
                Commentdate = DateTime.Now,
                Commenttext = text,
                Userid = userId,
                Restaurantid = restaurantId
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
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
