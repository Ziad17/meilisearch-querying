using FluentSearchEngine.Model;

namespace FluentSearchEngine.Services.Abstractions
{
    public interface ICollectiveIndexService<T, TKey>
        where T : SearchModel<TKey>
    {
        public Task AddToCollectiveIndexAsync(List<T> entities, CancellationToken cancellationToken);

        public Task AddToCollectiveIndexAsync(T entity, CancellationToken cancellationToken);

        public Task DeleteFromCollectiveAsync(CancellationToken cancellationToken, params string[] ids);
    }
}
