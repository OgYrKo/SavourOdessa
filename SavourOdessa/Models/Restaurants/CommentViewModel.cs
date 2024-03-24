using Npgsql.Replication;

namespace SavourOdessa.Models.Restaurants
{
    public class CommentViewModel
    {
        public int RestaurantId { get; set; }
        public CommentListViewModel CommentList { get; set; }

        public void AddComment(CommentListItemViewModel listItem)
        {
            if (!string.IsNullOrWhiteSpace(listItem.Text))
            {
                CommentList.AddComment(listItem);
            }
        }

    }
}
