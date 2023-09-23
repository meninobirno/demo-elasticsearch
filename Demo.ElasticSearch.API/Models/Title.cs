using Nest;

namespace Demo.Elasticsearch.API.Models
{
    public class Title
    {
        public string TtConst { get; set; }
        [Keyword]
        public string TitleType { get; set; }
        [Text]
        public string PrimaryTitle { get; set; }
        [Text]
        public string OriginalTitle { get; set; }
        public bool IsAdult { get; set; }
        [Number]
        public int StartYear { get; set; }
        [Number]
        public int EndYear { get; set; }
        [Number]
        public int RuntimeMinutes { get; set; }
        public List<string> Genres { get; set; }
    }
}
