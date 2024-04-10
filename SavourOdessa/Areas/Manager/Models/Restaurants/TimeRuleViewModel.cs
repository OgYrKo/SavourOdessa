namespace SavourOdessa.Areas.Manager.Models.Restaurants
{
    public class TimeRuleViewModel
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsClosed { get; set; }
        public int SelectedRepeatRuleId { get; set; }
    }
    public class RepeatRuleViewModel
    {
        public int RepeatRuleId { get; set; }
        public string RepeatRuleName { get; set; } = null!;
    }
}
