namespace FluentSearchEngine.Configurations
{
    public class SearchServiceOptions
    {
        public bool UseCrossSearch { get; set; }

        public string CollectiveHubName { get; set; } = "CollectiveHub";

        public int MaxTotalHits { get; set; } = 999;
    }
}
