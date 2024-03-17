using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SavourOdessa.Areas.Admin.Models.Restaurants
{
    public class RestaurantEditViewModel
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = null!;
        public CityItemViewModel[]? Cities { get; set; } = null!;
        public int SelectedCityId { get; set; }
        public CountryItemViewModel[]? Countries { get; set; } = null!;
        public int SelectedCountryId { get; set; }
        //add regex for street with ukrainian and english letters, first letter should be capital
        [RegularExpression(@"^[A-ZА-Я][a-zA-Zа-яА-Я\s]*$", ErrorMessage = "Street should contain only letters (first letter should be capital)")]
        public string Street { get; set; } = null!;

        [RegularExpression(@"^\d+[a-zA-Zа-яА-Я]*$", ErrorMessage = "HouseNumber should start with a digit and can contain letters.")]
        public string HouseNumber { get; set; } = null!;
        public RepeatRuleViewModel[]? RepeatRules { get; set; } = null!;
        public List<TimeRuleViewModel>? TimeRules { get; set; } = null!;
    }
    public class CityItemViewModel
    {
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public string CityName { get; set; } = null!;
    }
    public class CountryItemViewModel
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; } = null!;
    }
}
