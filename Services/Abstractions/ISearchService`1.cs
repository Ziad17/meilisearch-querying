using Meilisearch;

namespace FluentSearchEngine.Services.Abstraction
{
    public interface ISearchService<T>
        where T : class
    {
        public Task<bool> AddAsync(List<T> entities, CancellationToken cancellationToken = default);

        public Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default);

        public Task<bool> DeleteAsync(CancellationToken cancellationToken, params string[] ids);

        public Task<SearchResult<T>> SearchAsync(string term, CancellationToken cancellationToken = default);
        public Task<SearchResult<T>> SearchAsync(SearchQuery searchQuery, CancellationToken cancellationToken = default);
    }
}
