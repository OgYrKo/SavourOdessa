namespace SavourOdessa.Models.Restaurants
{
    public class CommentListItemViewModel(string userName,DateTime date,string text)
    {
        public string UserName { get; set; } = userName;
        public DateTime Date { get; set; } = date;
        public string Text { get; set; } = text;
    }
}
