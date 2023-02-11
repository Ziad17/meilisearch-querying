using FluentSearchEngine.Model;
using Meilisearch;
using Index = Meilisearch.Index;

namespace FluentSearchEngine.Services.Abstractions
{
    public interface ISearchService<T, TKey>
        where T : SearchModel<TKey>
    {
        public Index GetIndex();

        public Task<bool> AddAsync(List<T> entities, CancellationToken cancellationToken);

        public Task<T> GetAsync(string id, CancellationToken cancellationToken);

        public Task<bool> AddAsync(T entity, CancellationToken cancellationToken);

        public Task<bool> DeleteAsync(CancellationToken cancellationToken, params string[] ids);

        public Task<ISearchable<T>> SearchAsync(string term, CancellationToken cancellationToken);

        public Task<ISearchable<T>> SearchAsync(SearchQuery searchQuery, CancellationToken cancellationToken);

        public Task<ISearchable<T>> SearchInBucketAsync(SearchQuery searchQuery, CancellationToken cancellationToken);
    }
}
