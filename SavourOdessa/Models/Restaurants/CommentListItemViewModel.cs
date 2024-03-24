namespace SavourOdessa.Models.Restaurants
{
    public class CommentListItemViewModel
    {
        public CommentListItemViewModel() { }
        public CommentListItemViewModel(string userName, DateTime date, string text) 
        {
            UserName = userName;
            Date = date;
            Text = text;
        }

        public string? UserName { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
