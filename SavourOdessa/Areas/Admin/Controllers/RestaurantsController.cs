using DataLayer.EfClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SavourOdessa.Areas.Admin.Models.Restaurants;

namespace SavourOdessa.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RestaurantsController : Controller
    {
        private readonly DataContext _context;
        public RestaurantsController(DataContext context)
        {
            _context = context;
        }
        // GET: RestaurantsController
        public async Task<IActionResult> Index()
        {
            var restauranrs = await _context.Restaurants
                                            .ToListAsync();
            List<RestaurantListItemViewModel> restaurantListViewModel = new();
            foreach (var restaurant in restauranrs)
            {
                restaurantListViewModel.Add(new RestaurantListItemViewModel()
                {
                    RestaurantId = restaurant.Restaurantid,
                    RestaurantName = restaurant.Restaurantname
                });
            }

            return View(restaurantListViewModel);
        }

        // GET: RestaurantsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RestaurantsController/Create
        public async Task<ActionResult> Create()
        {
            var countries = await _context.Countries
                                    .ToListAsync();

            var citiesInCountries = await _context.Cityincountries
                                            .Include(c => c.City)
                                            .Include(c => c.Country)
                                            .ToListAsync();

            var cityItemsViewModel = new CityItemViewModel[citiesInCountries.Count];
            for (int i = 0; i < citiesInCountries.Count; i++)
            {
                cityItemsViewModel[i] = new CityItemViewModel()
                {
                    CityId = citiesInCountries[i].City.Cityid,
                    CityName = citiesInCountries[i].City.Cityname,
                    CountryId = citiesInCountries[i].Country.Countryid
                };
            }
            var countryItemsViewModel = new CountryItemViewModel[countries.Count];
            for (int i = 0; i < countries.Count; i++)
            {
                countryItemsViewModel[i] = new CountryItemViewModel()
                {
                    CountryId = countries[i].Countryid,
                    CountryName = countries[i].Countryname
                };
            }

            var viewModel = new RestaurantEditViewModel()
            {
                Cities = cityItemsViewModel,
                Countries = countryItemsViewModel
            };
            return View("Edit");
        }

        // POST: RestaurantsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RestaurantsController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var restaurant = await _context.Restaurants
                                    .Include(c => c.PostcodeNavigation)
                                    .Include(c => c.PostcodeNavigation.Cityincountry)
                                    .Include(c => c.PostcodeNavigation.Cityincountry.Country)
                                    .Include(c => c.PostcodeNavigation.Cityincountry.City)
                                    .FirstOrDefaultAsync(c => c.Restaurantid == id);

            var countries = await _context.Countries
                                    .ToListAsync();

            var citiesInCountries = await _context.Cityincountries
                                            .Include(c => c.City)
                                            .Include(c => c.Country)
                                            .ToListAsync();
            if (restaurant == null)
            {
                return NotFound();
            }
            var cityItemsViewModel = new CityItemViewModel[citiesInCountries.Count];
            for (int i = 0; i < citiesInCountries.Count;i++)
            {
                cityItemsViewModel[i]=new CityItemViewModel()
                {
                    CityId = citiesInCountries[i].City.Cityid,
                    CityName = citiesInCountries[i].City.Cityname,
                    CountryId = citiesInCountries[i].Country.Countryid
                };
            }
            var countryItemsViewModel = new CountryItemViewModel[countries.Count];
            for (int i = 0; i < countries.Count; i++)
            {
                countryItemsViewModel[i] = new CountryItemViewModel()
                {
                    CountryId = countries[i].Countryid,
                    CountryName = countries[i].Countryname
                };
            }

            var viewModel = new RestaurantEditViewModel()
            {
                RestaurantId = restaurant.Restaurantid,
                RestaurantName = restaurant.Restaurantname,
                Street = restaurant.Street,
                HouseNumber = restaurant.Housenum,
                Cities = cityItemsViewModel,
                Countries = countryItemsViewModel,
                SelectedCityId = restaurant.PostcodeNavigation.Cityincountry.City.Cityid,
                SelectedCountryId = restaurant.PostcodeNavigation.Cityincountry.Country.Countryid
            };

            return View(viewModel);
        }

        // POST: RestaurantsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RestaurantEditViewModel viewModel)
        {
            //try
            //{
            //    //var restaurant = await _context.Restaurants.SingleOrDefaultAsync()
            //}
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        // GET: RestaurantsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RestaurantsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
