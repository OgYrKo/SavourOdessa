namespace SavourOdessa.Models.Dishes
{
    public class DishListItemViewModel(int id,string name,string description, string image,decimal price, string preparationTime)
    {
        public int Id { get; } = id;
        public string Name { get; } = name;
        public string Description { get; } = description;
        public string Image { get; } = image;
        public decimal Price { get; } = price;
        public string PreparationTime { get; } = preparationTime;
    }
}
