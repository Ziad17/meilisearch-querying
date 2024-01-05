namespace FluentSearchEngine.Configurations
{
    public class SearchServiceOptions
    {
        public string Host { get; set; }

        public string Key { get; set; }

        public bool UseCrossSearch { get; set; }

        public string CollectiveHubName { get; set; } = "CollectiveHub";

        public int MaxTotalHits { get; set; } = 999;
    }
}
