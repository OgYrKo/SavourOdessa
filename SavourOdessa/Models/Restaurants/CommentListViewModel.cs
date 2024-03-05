namespace SavourOdessa.Models.Restaurants
{
    public class CommentListViewModel(CommentListItemViewModel[] comments)
    {
        public CommentListItemViewModel[] Comments { get; set; } = comments;
    }
}
