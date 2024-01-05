using FluentSearchEngine.Model;
using Meilisearch;
using Index = Meilisearch.Index;

namespace FluentSearchEngine.Services.Abstractions
{
    public interface ISearchService<T, TKey>
        where T : SearchModel<TKey>
    {
        public Index GetIndex();

        public Task<bool> AddAsync(List<T> entities, CancellationToken cancellationToken = default);

        public Task<T> GetAsync(string id, CancellationToken cancellationToken = default);

        public Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

        public Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default);

        public Task<bool> DeleteAsync(CancellationToken cancellationToken = default, params string[] ids);

        public Task<bool> DeleteAllAsync(CancellationToken cancellationToken = default);

        public Task<ISearchable<T>> SearchAsync(string term, CancellationToken cancellationToken = default);

        public Task<ISearchable<T>> SearchAsync(SearchQuery searchQuery, CancellationToken cancellationToken = default);

        public Task<ISearchable<T>> SearchInBucketAsync(SearchQuery searchQuery, CancellationToken cancellationToken = default);
    }
}
