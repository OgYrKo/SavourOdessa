using DataLayer.EfClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SavourOdessa.Models.Dishes;

namespace SavourOdessa.Controllers
{
    public class MenuController : Controller
    {
        private readonly DataContext _context;
        public MenuController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int id)
        {
            var dishes = await _context.Dishinrestaurants
                                        .Include(d => d.Dish)
                                        .Include(d => d.Dish.Dishbylanguages)
                                        .Include(d => d.Dish.Dishkitchen)
                                        .Include(d => d.Dish.Dishtype)
                                        .Where(d => d.Restaurantid == id)
                                        .Where(d => d.Dish.Dishbylanguages.Any(dbl => dbl.Languageid == 2))
                                        .ToListAsync();     
            Dictionary<string, List<DishListItemViewModel>> dishesByType = new();
            foreach (var dish in dishes)
            {
                if (!dishesByType.ContainsKey(dish.Dish.Dishtype.Dishtypename)) 
                    dishesByType[dish.Dish.Dishtype.Dishtypename] = new List<DishListItemViewModel>();

                var dishDescription = dish.Dish.Dishbylanguages.First();
                dishesByType[dish.Dish.Dishtype.Dishtypename].Add(
                    new DishListItemViewModel(dish.Dish.Dishid,
                                              dishDescription.Dishname,
                                              dishDescription.Dishcomposition,
                                              GetFirstImage(dish.Dish.Dishid.ToString()), 
                                              dish.Cost, 
                                              dish.Dish.Preparingtime.ToString()));
            }

            return View(new DishListViewModel(dishesByType));
        }

        //private IEnumerable<string> GetImages(string dishName)
        //{
        //    string dishesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Dishes");
        //    string dishPath = Path.Combine(dishesPath, dishName);
        //    if (!Directory.Exists(dishPath))
        //    {
        //        yield return "~/images/Dishes/default.jpeg";
        //        yield break;
        //    }

        //    string[] images = Directory.GetFiles(dishPath, "*.*", SearchOption.AllDirectories);
        //    foreach (var image in images)
        //    {
        //        yield return "~/" + Path.GetRelativePath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), image).Replace("\\", "/");
        //    }
        //}

        private string GetFirstImage(string dishName)
        {
            string dishesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Dishes");
            string dishPath = Path.Combine(dishesPath, dishName);
            string? firstImage = null;
            if (Directory.Exists(dishPath))
            {
                firstImage = Directory.GetFiles(dishPath, "*.*", SearchOption.AllDirectories)
                                     .FirstOrDefault();
            }
            if (firstImage != null)
            {
                return "~/" + Path.GetRelativePath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), firstImage).Replace("\\", "/");
            }
            return "~/images/Dishes/default.jpeg";
        }
    }
}
