namespace NewsAggregation.Models
{
    public class Keyword
    {
        public int Keyword_Id { get; set; }
        public string Keyword_Name { get; set; }
        public int User_id { get; set; }
        public User User { get; private set; }
    }
}
