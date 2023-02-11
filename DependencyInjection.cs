using FluentSearchEngine.Attributes;
using FluentSearchEngine.Extensions;
using FluentSearchEngine.Services.Implementations;
using System.Reflection;
using System.Text.Json.Serialization;
using Humanizer;
using Meilisearch;
using Microsoft.Extensions.DependencyInjection;
using FluentSearchEngine.Configurations;
using FluentSearchEngine.Services.Abstractions;
using FluentSearchEngine.Model;
using Microsoft.Extensions.Options;

namespace FluentSearchEngine
{
    public static class DependencyInjection
    {
        //TODO:: enable options based pattern to configure collective index
        //TODO:: enable pagination max hit modification
        public static void ResolveFluentFiltersFromAssembly(this MeilisearchClient client, IOptions<SearchServiceOptions> options, params Assembly[] assemblies)
        {
            client.CreateIndexAsync(options.Value.CollectiveHubName).Wait();
            var types = assemblies.SelectMany(s => s.GetTypes())
                .Where(x => typeof(SearchModel<>).IsAssignableFrom(x) && !x.IsAbstract);

            foreach (var type in types)
            {
                var sortableProperties = type.GetProperties()
                    .Where(x => x.IsDefined(typeof(Sortable), true))
                    .Select(x => x.IsDefined(typeof(JsonPropertyNameAttribute), true) ? x.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name : x.Name.FirstCharToLowerCase()).ToList();

                var filterableProperties = type.GetProperties()
                    .Where(x => x.IsDefined(typeof(SearchFilter), true))
                    .Select(x => x.IsDefined(typeof(JsonPropertyNameAttribute), true) ? x.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name : x.Name.FirstCharToLowerCase()).ToList();

                var indexName = type.Name.Pluralize();
                client.CreateIndexAsync(indexName).Wait();
                var index = client.Index(indexName);

                var paginationSettings = new Pagination()
                {
                    MaxTotalHits = options.Value.MaxTotalHits
                };

                index.UpdatePaginationAsync(paginationSettings).Wait();

                if (sortableProperties.Any())
                    index.UpdateSortableAttributesAsync(sortableProperties).Wait();

                if (filterableProperties.Any())
                    index.UpdateFilterableAttributesAsync(filterableProperties).Wait();
            }
        }

        public static void AddGenericSearchService(this IServiceCollection services)
        {
            services.AddScoped(typeof(ISearchService<,>), typeof(SearchService<,>));
        }
    }
}
