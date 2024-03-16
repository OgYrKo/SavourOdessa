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
                                            .OrderBy(r => r.Restaurantid)
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

            var locale = citiesInCountries.First();

            var viewModel = new RestaurantEditViewModel()
            {
                SelectedCityId = locale.City.Cityid,
                SelectedCountryId = locale.Country.Countryid,
                Cities = cityItemsViewModel,
                Countries = countryItemsViewModel
            };
            return View("Edit", viewModel);
        }

        // POST: RestaurantsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RestaurantEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var cityincountry = await _context.Cityincountries
                                            .Include(c => c.Postcodes)
                                            .FirstOrDefaultAsync(c => c.Cityid == viewModel.SelectedCityId && c.Countryid == viewModel.SelectedCountryId);
                    var owner = await _context.Systemusers.FirstAsync();
                    var postcode = cityincountry?.Postcodes.FirstOrDefault()?.Postcode1;
                    if (cityincountry == null || postcode == null)
                        throw new Exception("Address not found");
                    var restaurant = new Restaurant()
                    {
                        Restaurantname = viewModel.RestaurantName,
                        Street = viewModel.Street,
                        Housenum = viewModel.HouseNumber,
                        Postcode = postcode,
                        Ownerid = owner.Userid
                    };
                    await _context.AddAsync(restaurant);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            if(viewModel.Countries.Length == 0 || viewModel.Cities.Length == 0)
            {
                viewModel.Cities = await GetCityItemsAsync();
                viewModel.Countries = await GetCountryItemsAsync();
            }
            return View("Edit", viewModel);
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

            
            if (restaurant == null)
            {
                return NotFound();
            }
            

            var viewModel = new RestaurantEditViewModel()
            {
                RestaurantId = restaurant.Restaurantid,
                RestaurantName = restaurant.Restaurantname,
                Street = restaurant.Street,
                HouseNumber = restaurant.Housenum,
                Cities = await GetCityItemsAsync(),
                Countries = await GetCountryItemsAsync(),
                SelectedCityId = restaurant.PostcodeNavigation.Cityincountry.City.Cityid,
                SelectedCountryId = restaurant.PostcodeNavigation.Cityincountry.Country.Countryid
            };

            return View(viewModel);
        }

        // POST: RestaurantsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id,RestaurantEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var restaurant = await _context.Restaurants
                                        .FirstOrDefaultAsync(c => c.Restaurantid == id);
                    if (restaurant == null)
                        throw new Exception("Restaurant not found");

                    restaurant.Restaurantname = viewModel.RestaurantName;
                    restaurant.Street = viewModel.Street;
                    restaurant.Housenum = viewModel.HouseNumber;
                    var cityincountry = await _context.Cityincountries
                                    .Include(c => c.Postcodes)
                                    .FirstOrDefaultAsync(c => c.Cityid == viewModel.SelectedCityId && c.Countryid == viewModel.SelectedCountryId);
                    var postcode = cityincountry?.Postcodes.FirstOrDefault()?.Postcode1;
                    if (cityincountry == null || postcode == null)
                        throw new Exception("Address not found");
                    restaurant.Postcode = postcode;

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }

            }
            if (viewModel.Countries.Length == 0 || viewModel.Cities.Length == 0)
            {
                viewModel.Cities = await GetCityItemsAsync();
                viewModel.Countries = await GetCountryItemsAsync();
            }
            return View(viewModel);
        }

        private async Task<CityItemViewModel[]> GetCityItemsAsync()
        {
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
            return cityItemsViewModel;
        }

        private async Task<CountryItemViewModel[]> GetCountryItemsAsync()
        {
            var countries = await _context.Countries
                                    .ToListAsync();
            var countryItemsViewModel = new CountryItemViewModel[countries.Count];
            for (int i = 0; i < countries.Count; i++)
            {
                countryItemsViewModel[i] = new CountryItemViewModel()
                {
                    CountryId = countries[i].Countryid,
                    CountryName = countries[i].Countryname
                };
            }
            return countryItemsViewModel;
        }

        // GET: RestaurantsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
