using Meilisearch;
using Index = Meilisearch.Index;

namespace FluentSearchEngine.Services.Abstractions
{
    public interface ISearchService<T>
        where T : class
    {
        public Index GetIndex();
        public Task<bool> AddOrUpdateAsync(List<T> entities, CancellationToken cancellationToken = default);

        public Task<bool> AddOrUpdateAsync(T entity, CancellationToken cancellationToken = default);

        public Task<bool> DeleteAsync(CancellationToken cancellationToken, params string[] ids);

        public Task<SearchResult<T>> SearchAsync(string term, CancellationToken cancellationToken = default);
        public Task<SearchResult<T>> SearchAsync(SearchQuery searchQuery, CancellationToken cancellationToken = default);
    }
}
