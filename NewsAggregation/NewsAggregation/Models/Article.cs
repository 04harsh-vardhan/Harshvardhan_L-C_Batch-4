namespace NewsAggregation.Models
{
    public class Article
    {
        public int Article_Id { get; set; }
        public string Article_uuid { get; set; }
        public string Article_Title { get; set; }
        public string? Article_Description { get; set; }
        public string? Article_Source { get; set; }
        public string? Article_Url { get; set; }
        public int Category_Id { get; set; }
        public DateTime Created_At { get; set; }
    }
}
