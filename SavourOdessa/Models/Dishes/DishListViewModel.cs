namespace SavourOdessa.Models.Dishes
{
    public class DishListViewModel
    {
        public DishListViewModel(Dictionary<string, List<DishListItemViewModel>> dishesByTypes)
        {
            DishesByTypes = dishesByTypes;
        }
        public Dictionary<string, List<DishListItemViewModel>> DishesByTypes { get; }
    }
}
