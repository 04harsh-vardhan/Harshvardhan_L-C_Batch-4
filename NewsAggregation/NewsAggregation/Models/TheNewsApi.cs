using Newtonsoft.Json;

public class TheNewsApi
{
    [JsonProperty("meta")]
    public Meta Meta { get; set; }

    [JsonProperty("data")]
    public List<Article1> Articles { get; set; }
}

public class Meta
{
    [JsonProperty("found")]
    public int TotalFound { get; set; }

    [JsonProperty("returned")]
    public int Returned { get; set; }

    [JsonProperty("limit")]
    public int Limit { get; set; }

    [JsonProperty("page")]
    public int Page { get; set; }
}

public class Article1
{
    [JsonProperty("uuid")]
    public string Uuid { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("keywords")]
    public string Keywords { get; set; }

    [JsonProperty("snippet")]
    public string Snippet { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("image_url")]
    public string ImageUrl { get; set; }

    [JsonProperty("language")]
    public string Language { get; set; }

    [JsonProperty("published_at")]
    public DateTime PublishedAt { get; set; }

    [JsonProperty("source")]
    public string Source { get; set; }

    [JsonProperty("categories")]
    public List<string> Categories { get; set; }

    [JsonProperty("relevance_score")]
    public double? RelevanceScore { get; set; }

    [JsonProperty("locale")]
    public string Locale { get; set; }
}
