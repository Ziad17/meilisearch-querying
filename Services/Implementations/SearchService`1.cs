using System.Text.Json;
using FluentSearchEngine.Configurations;
using FluentSearchEngine.Model;
using FluentSearchEngine.Services.Abstractions;
using Humanizer;
using Meilisearch;
using Microsoft.Extensions.Options;
using Index = Meilisearch.Index;

namespace FluentSearchEngine.Services.Implementations
{
    public class SearchService<T, TKey> : ISearchService<T, TKey>
        where T : SearchModel<TKey>
    {
        private readonly Index _index;
        private readonly Index _collectiveIndex;
        private readonly SearchServiceOptions _settings;

        public SearchService(MeilisearchClient client, IOptions<SearchServiceOptions> settings)
        {
            _settings = settings.Value;
            _index = client.Index(typeof(T).Name.Pluralize());
            if (_settings.UseCrossSearch)
                _collectiveIndex = client.Index(settings.Value.CollectiveHubName);
        }

        public Index GetIndex()
        {
            return _index;
        }

        public async Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var result = await _index.AddDocumentsAsync(new List<T>() { entity }, cancellationToken: cancellationToken);

            if (_settings.UseCrossSearch)
                await AddToCollectiveIndexAsync(entity, cancellationToken);

            return result.Status == TaskInfoStatus.Succeeded;
        }

        public async Task<bool> AddAsync(List<T> entities, CancellationToken cancellationToken = default)
        {
            var result = await _index.AddDocumentsAsync(entities, cancellationToken: cancellationToken);

            if (_settings.UseCrossSearch)
                await AddToCollectiveIndexAsync(entities, cancellationToken);

            return result.Status == TaskInfoStatus.Succeeded;
        }

        public async Task<T> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _index.GetDocumentAsync<T>(id, cancellationToken: cancellationToken);
        }

        public async Task<bool> DeleteAsync(CancellationToken cancellationToken, params string[] ids)
        {
            var result = await _index.DeleteDocumentsAsync(ids, cancellationToken: cancellationToken);

            await DeleteFromCollectiveAsync(cancellationToken, ids);

            return result.Status == TaskInfoStatus.Succeeded;
        }

        public async Task<ISearchable<T>> SearchAsync(string term, CancellationToken cancellationToken = default)
        {
            var searchQuery = new SearchQuery();
            var result = await _index.SearchAsync<T>(term, searchQuery, cancellationToken);
            return result;
        }

        public async Task<ISearchable<T>> SearchAsync(SearchQuery searchQuery, CancellationToken cancellationToken = default)
        {
            var term = searchQuery.Q;
            searchQuery.Q = null;
            var result = await _index.SearchAsync<T>(term, searchQuery, cancellationToken);
            return result;
        }

        public async Task<ISearchable<T>> SearchInBucketAsync(SearchQuery searchQuery, CancellationToken cancellationToken = default)
        {
            if (!_settings.UseCrossSearch)
                throw new Exception("Cross search is not enabled");
            var term = searchQuery.Q;
            searchQuery.Q = null;
            var result = await _collectiveIndex.SearchAsync<T>(term, searchQuery, cancellationToken);
            return result;
        }

        public async Task AddToCollectiveIndexAsync(List<T> entities, CancellationToken cancellationToken)
        {
            var models = entities.Select(model =>
                new IndicesModel<TKey>(model.Id, JsonSerializer.Serialize(model), typeof(T).Name.Pluralize()));

            await _collectiveIndex.AddDocumentsAsync(models, cancellationToken: cancellationToken);
        }

        public async Task AddToCollectiveIndexAsync(T entity, CancellationToken cancellationToken)
        {
            var model = new IndicesModel<TKey>(entity.Id, JsonSerializer.Serialize(entity), typeof(T).Name.Pluralize());

            await _collectiveIndex.AddDocumentsAsync(new List<IndicesModel<TKey>>() { model }, cancellationToken: cancellationToken);
        }

        public async Task DeleteFromCollectiveAsync(CancellationToken cancellationToken, params string[] ids)
        {
            await _collectiveIndex.DeleteDocumentsAsync(ids, cancellationToken: cancellationToken);
        }
    }
}
