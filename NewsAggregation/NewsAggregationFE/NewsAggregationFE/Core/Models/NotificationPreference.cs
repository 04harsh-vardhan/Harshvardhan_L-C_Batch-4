namespace NewsAggregationFE.Core.Models
{
    public class NotificationPreference
    {
        public bool BusinessEnabled { get; set; }
        public bool EntertainmentEnabled { get; set; }
        public bool SportsEnabled { get; set; }
        public bool TechnologyEnabled { get; set; }
        public bool KeywordsEnabled { get; set; }
        public List<string> Keywords { get; set; } = new List<string>();
    }
} 