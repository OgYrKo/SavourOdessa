using DataLayer.EfClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SavourOdessa.Areas.Manager.Models.Restaurants;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SavourOdessa.Areas.Manager.Controllers
{
    [Area("Manager")]
    [CustomAuthorize("Manager")]
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
            string username = HttpContext.Session.GetString("Username")!;

            var restauranrs = await _context.Restaurants
                                            .Include(r => r.Owner)
                                            .Where(r => r.Owner.Username == username)
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
        public async Task<IActionResult> Stats(int id)
        {
            return await Stats(id, DateTime.Now.Year);
        }
        [HttpGet("Manager/Restaurants/Stats/{id}/{year}")]
        public async Task<IActionResult> Stats(int id, int year)
        {
            var monthlyReservations = await _context.GetMonthlyReservationChanges(id, year);
            StatsViewModel statsViewModel = new()
            {
                RestaurantId = id,
                Year = year,
                Month = -1,
                DailyReservationChanges = null,
                MonthlyReservationChanges = monthlyReservations
            };

            return View(statsViewModel);
        }
        [HttpGet("Manager/Restaurants/Stats/{id}/{year}/{month}")]
        public async Task<IActionResult> Stats(int id, int year, int month)
        {
            var dailyReservations = await _context.GetDailyReservationChanges(id, year, month);
            StatsViewModel statsViewModel = new()
            {
                RestaurantId = id,
                Year = year,
                Month = month,
                DailyReservationChanges = dailyReservations,
                MonthlyReservationChanges = null
            };

            return View(statsViewModel);
        }


        // GET: RestaurantsController/Details/5
        [HttpGet("Details/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            var reservations = await _context.Tablereservations
                                    .Include(t => t.Table)
                                    .Include(t => t.User)
                                    .OrderByDescending(t => t.Reservationtime)
                                    .Where(t => t.Table.Restaurantid == id && t.Reservationtime >= DateTime.Now.Date)
                                    .ToListAsync();
            List<ReservationItemViewModel> reservationItems = new();
            foreach (var reservation in reservations)
            {
                reservationItems.Add(new ReservationItemViewModel()
                {
                    ReservationId = reservation.Tablereservationid,
                    TableId = reservation.Tableid,
                    Time = reservation.Reservationtime,
                    Duration = reservation.Duration,
                    UserName = reservation.User.Username,
                    IsCompleted = reservation.Reservationtime <= DateTime.Now
                });
            }
            DetailsViewModel detailsViewModel = new()
            {
                RestaurantId = id,
                DayOffset = 0,
                Reservations = reservationItems
            };
            return View(detailsViewModel);
        }
        [HttpGet("Details/{id}/{dayOffset}")]
        public async Task<ActionResult> Details(int id, int dayOffset)
        {

            var reservations = await _context.Tablereservations
                                    .Include(t => t.Table)
                                    .Include(t => t.User)
                                    .OrderByDescending(t => t.Reservationtime)
                                    .Where(t => t.Table.Restaurantid == id && t.Reservationtime.Date == DateTime.Now.AddDays(dayOffset).Date)
                                    .ToListAsync();
            List<ReservationItemViewModel> reservationItems = new();
            foreach (var reservation in reservations)
            {
                reservationItems.Add(new ReservationItemViewModel()
                {
                    ReservationId = reservation.Tablereservationid,
                    TableId = reservation.Tableid,
                    Time = reservation.Reservationtime,
                    Duration = reservation.Duration,
                    UserName = reservation.User.Username,
                    IsCompleted = reservation.Reservationtime <= DateTime.Now
                });
            }
            DetailsViewModel detailsViewModel = new()
            {
                RestaurantId = id,
                DayOffset = dayOffset,
                Reservations = reservationItems
            };
            return View(detailsViewModel);
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
            var timeRules = await GetTimeRulesViewModel(id);
            var viewModel = new RestaurantEditViewModel()
            {
                RestaurantId = restaurant.Restaurantid,
                RestaurantName = restaurant.Restaurantname,
                Street = restaurant.Street,
                HouseNumber = restaurant.Housenum,
                SelectedCityId = restaurant.PostcodeNavigation.Cityincountry.City.Cityid,
                SelectedCountryId = restaurant.PostcodeNavigation.Cityincountry.Country.Countryid,
                TimeRules = timeRules
            };
            await AddLists(viewModel);

            return View(viewModel);
        }

        // POST: RestaurantsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, RestaurantEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var restaurant = await _context.Restaurants
                                        .Include(c => c.Openingrules)
                                        .FirstOrDefaultAsync(c => c.Restaurantid == id);
                    if (restaurant == null)
                        throw new Exception("Restaurant not found");


                    restaurant.Restaurantname = viewModel.RestaurantName;
                    restaurant.Street = viewModel.Street;
                    restaurant.Housenum = viewModel.HouseNumber;
                    restaurant.Postcode = await GetPostcodeAsync(viewModel.SelectedCountryId, viewModel.SelectedCityId);

                    await UpdateTimeRules(id, viewModel.TimeRules);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (PostgresException e)
                {
                    ModelState.AddModelError("", e.MessageText);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }

            }
            await AddLists(viewModel);
            return View(viewModel);
        }
        // GET: RestaurantsController/Delete/5
        public ActionResult Delete(int id)
        {
            var restaurant = _context.Restaurants
                            .FirstOrDefault(c => c.Restaurantid == id);
            if (restaurant == null)
            {
                return NotFound();
            }
            _context.Restaurants.Remove(restaurant);
            _context.SaveChanges();
            return View();
        }

        private List<TimeRuleViewModel> GetDefaultTimeRuleList()
        {
            List<TimeRuleViewModel> timeRules =
            [
                new TimeRuleViewModel()
                {
                    StartDate = DateOnly.FromDateTime(DateTime.Today),
                    StartTime = new TimeOnly(8, 0),
                    EndTime = new TimeOnly(20, 0),
                    SelectedRepeatRuleId = 2
                },
            ];
            return timeRules;
        }
        private async Task<List<TimeRuleViewModel>> GetTimeRulesViewModel(int restaurantId)
        {
            var openingRules = await _context.Openingrules
                                    .Include(o => o.Workhour)
                                    .Where(o => o.Restaurantid == restaurantId)
                                    .ToArrayAsync();
            var timeRules = new List<TimeRuleViewModel>();
            foreach (var openingRule in openingRules)
            {
                timeRules.Add(new TimeRuleViewModel()
                {
                    Id = openingRule.Openingrulesid,
                    StartDate = openingRule.Startday,
                    StartTime = openingRule.Workhour?.Openhours ?? new TimeOnly(0, 0),
                    EndTime = openingRule.Workhour?.Closehours ?? new TimeOnly(0, 0),
                    SelectedRepeatRuleId = openingRule.Repeatrulesid,
                    IsClosed = openingRule.Workhour == null
                });
            }
            return timeRules;
        }
        private async Task UpdateTimeRules(int restaurantId, List<TimeRuleViewModel>? timeRules)
        {
            var openingRules = await _context.Openingrules
                                    .Include(o => o.Workhour)
                                    .Where(o => o.Restaurantid == restaurantId)
                                    .ToDictionaryAsync(o => o.Openingrulesid);
            //remove deleted time rules
            foreach (var openingRule in openingRules.Values)
            {
                if (timeRules == null || !timeRules.Any(t => t.Id == openingRule.Openingrulesid))
                {
                    if (openingRule.Workhour != null)
                        _context.Remove(openingRule.Workhour);
                    _context.Remove(openingRule);
                }
            }
            if (timeRules == null)
                return;
            //update and add time rules
            foreach (var timeRule in timeRules)
            {
                if (timeRule.Id == 0)//add new time rule
                {

                    var openingRule = new Openingrule()
                    {
                        Startday = timeRule.StartDate,
                        Repeatrulesid = timeRule.SelectedRepeatRuleId,
                        Restaurantid = restaurantId,

                    };
                    if (!timeRule.IsClosed)
                    {
                        openingRule.Workhour = new Workhour()
                        {
                            Openhours = timeRule.StartTime,
                            Closehours = timeRule.EndTime
                        };
                    }
                    await _context.AddAsync(openingRule);
                }
                else//update existing time rule
                {
                    var openingRule = openingRules[timeRule.Id];
                    openingRule.Startday = timeRule.StartDate;
                    openingRule.Repeatrulesid = timeRule.SelectedRepeatRuleId;
                    if (timeRule.IsClosed)
                    {
                        if (openingRule.Workhour != null)
                            _context.Remove(openingRule.Workhour);
                    }
                    else
                    {
                        if (openingRule.Workhour == null)
                        {
                            openingRule.Workhour = new Workhour()
                            {
                                Openhours = timeRule.StartTime,
                                Closehours = timeRule.EndTime
                            };
                        }
                        else
                        {
                            openingRule.Workhour.Openhours = timeRule.StartTime;
                            openingRule.Workhour.Closehours = timeRule.EndTime;
                        }
                    }
                    _context.Update(openingRule);
                }

            }
        }
        private List<Openingrule> GetOpeningrules(List<TimeRuleViewModel>? timeRules)
        {
            List<Openingrule> openingRules = new();
            if (timeRules == null)
                return openingRules;
            foreach (var timeRule in timeRules)
            {
                var openingRule = new Openingrule()
                {
                    Startday = timeRule.StartDate,
                    Repeatrulesid = timeRule.SelectedRepeatRuleId
                };
                if (!timeRule.IsClosed)
                {
                    openingRule.Workhour = new Workhour()
                    {
                        Openhours = timeRule.StartTime,
                        Closehours = timeRule.EndTime
                    };
                }
                openingRules.Add(openingRule);
            }
            return openingRules;
        }
        private async Task AddLists(RestaurantEditViewModel viewModel)
        {
            if (viewModel.Cities == null)
                viewModel.Cities = await GetCityItemsAsync();
            if (viewModel.Countries == null)
                viewModel.Countries = await GetCountryItemsAsync();
            if (viewModel.TimeRules == null)
                viewModel.TimeRules = new List<TimeRuleViewModel>();
            if (viewModel.RepeatRules == null)
                viewModel.RepeatRules = await GetRepeatRuleItemsAsync();
        }
        private async Task<RepeatRuleViewModel[]> GetRepeatRuleItemsAsync()
        {
            var repeatRules = await _context.Repeatrules
                                    .ToArrayAsync();
            var repeatRuleViewModels = new RepeatRuleViewModel[repeatRules.Length];
            for (int i = 0; i < repeatRules.Length; i++)
            {
                repeatRuleViewModels[i] = new RepeatRuleViewModel()
                {
                    RepeatRuleId = repeatRules[i].Repeatrulesid,
                    RepeatRuleName = repeatRules[i].Repeatrulestype
                };
            }
            return repeatRuleViewModels;
        }
        private async Task<string> GetPostcodeAsync(int countryId, int cityId)
        {
            var cityincountry = await _context.Cityincountries
                                    .Include(c => c.Postcodes)
                                    .FirstOrDefaultAsync(c => c.Cityid == cityId && c.Countryid == countryId);
            var postcode = cityincountry?.Postcodes.FirstOrDefault()?.Postcode1;
            if (cityincountry == null || postcode == null)
                throw new Exception("Address not found");
            return postcode;
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
    }
}
