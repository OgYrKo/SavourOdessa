namespace SavourOdessa.Models.Restaurants
{
    public class CommentListViewModel(List<CommentListItemViewModel> comments)
    {
        public List<CommentListItemViewModel> Comments { get; set; } = comments;

        public void AddComment(CommentListItemViewModel comment)
        {
            Comments.Add(comment);
        }
    }
}
