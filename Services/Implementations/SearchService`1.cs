using FluentSearchEngine.Services.Abstractions;
using Humanizer;
using Meilisearch;
using Index = Meilisearch.Index;

namespace FluentSearchEngine.Services.Implementations
{
    public class SearchService<T> : ISearchService<T>
        where T : class
    {
        public readonly MeilisearchClient Client;
        public readonly Index Index;

        public SearchService(MeilisearchClient client)
        {
            Client = client;
            Index = client.Index(typeof(T).Name.Pluralize());
        }

        public async Task<bool> AddOrUpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            var result = await Index.AddDocumentsAsync(new List<T>() { entity }, cancellationToken: cancellationToken);
            return result.Status == TaskInfoStatus.Succeeded;
        }

        public Index GetIndex()
        {
            return Index;
        }

        public async Task<bool> AddOrUpdateAsync(List<T> entities, CancellationToken cancellationToken = default)
        {
            var result = await Index.AddDocumentsAsync(entities, cancellationToken: cancellationToken);
            return result.Status == TaskInfoStatus.Succeeded;
        }

        public async Task<bool> DeleteAsync(CancellationToken cancellationToken, params string[] ids)
        {
            var result = await Index.DeleteDocumentsAsync(ids, cancellationToken: cancellationToken);
            return result.Status == TaskInfoStatus.Succeeded;
        }

        public async Task<SearchResult<T>> SearchAsync(string term, CancellationToken cancellationToken = default)
        {
            var searchQuery = new SearchQuery();
            var result = await Index.SearchAsync<T>(term, searchQuery, cancellationToken);
            return result;
        }

        public async Task<SearchResult<T>> SearchAsync(SearchQuery searchQuery, CancellationToken cancellationToken = default)
        {
            var term = searchQuery.Q;
            searchQuery.Q = null;
            var result = await Index.SearchAsync<T>(term, searchQuery, cancellationToken);

            return result;
        }
    }
}
