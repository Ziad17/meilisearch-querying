using FluentSearchEngine.Services.Abstraction;
using Humanizer;
using Meilisearch;
using Index = Meilisearch.Index;

namespace FluentSearchEngine.Services.Implementations
{
    public class SearchService<T> : ISearchService<T>
        where T : class
    {
        public readonly MeilisearchClient Client;
        public readonly Index _index;

        public SearchService(MeilisearchClient client)
        {
            Client = client;
            _index = client.Index(typeof(T).Name.Pluralize());
        }

        public async Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var result = await _index.AddDocumentsAsync(new List<T>() { entity }, cancellationToken: cancellationToken);
            return result.Status == TaskInfoStatus.Succeeded;
        }

        public Index GetIndex()
        {
            return _index;
        }

        public async Task<bool> AddAsync(List<T> entities, CancellationToken cancellationToken = default)
        {
            var result = await _index.AddDocumentsAsync(entities, cancellationToken: cancellationToken);
            return result.Status == TaskInfoStatus.Succeeded;
        }

        public async Task<bool> DeleteAsync(CancellationToken cancellationToken, params string[] ids)
        {
            var result = await _index.DeleteDocumentsAsync(ids, cancellationToken: cancellationToken);
            return result.Status == TaskInfoStatus.Succeeded;
        }

        public async Task<SearchResult<T>> SearchAsync(string term, CancellationToken cancellationToken = default)
        {
            var searchQuery = new SearchQuery()
            {
            };
            var result = await _index.SearchAsync<T>(term, searchQuery, cancellationToken);
            return result;
        }

        public async Task<SearchResult<T>> SearchAsync(SearchQuery searchQuery, CancellationToken cancellationToken = default)
        {
            var term = searchQuery.Q;
            searchQuery.Q = null;
            var result = await _index.SearchAsync<T>(term, searchQuery, cancellationToken);

            return result;
        }
    }
}
