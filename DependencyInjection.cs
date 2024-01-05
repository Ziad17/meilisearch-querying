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

namespace FluentSearchEngine
{
    //TODO:: enable options based pattern to configure collective index
    //TODO:: enable pagination max hit modification

    public static class DependencyInjection
    {
        public static void ResolveFluentFiltersFromAssembly(this MeilisearchClient client, Assembly[] assemblies, SearchServiceOptions options = null)
        {
            if (options != null)
                client.CreateIndexAsync(options.CollectiveHubName).Wait();

            var types = assemblies.SelectMany(s => s.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract && x.BaseType is { IsGenericType: true } &&
                            (x.BaseType.GetGenericTypeDefinition() == typeof(SearchModel<>) || x.BaseType.GetGenericTypeDefinition() == typeof(GeoSearchModel<>)));

            EnrichAttributes(types, client, options);
        }

        public static void ResolveFluentFiltersFromAssembly(this MeilisearchClient client, SearchServiceOptions options = null)
        {
            var types = Assembly.GetCallingAssembly().GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.BaseType is { IsGenericType: true } &&
                            (x.BaseType.GetGenericTypeDefinition() == typeof(SearchModel<>) || x.BaseType.GetGenericTypeDefinition() == typeof(GeoSearchModel<>)));

            EnrichAttributes(types, client, options);
        }

        public static void AddGenericSearchService(this IServiceCollection services, MeilisearchClient client)
        {
            services.AddSingleton(client);
            services.AddScoped(typeof(ISearchService<,>), typeof(SearchService<,>));
        }

        private static void EnrichAttributes(IEnumerable<Type> types, MeilisearchClient client, SearchServiceOptions options = null)
        {
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

                if (options != null)
                {
                    var paginationSettings = new Pagination()
                    {
                        MaxTotalHits = options.MaxTotalHits
                    };

                    index.UpdatePaginationAsync(paginationSettings).Wait();
                }

                if (sortableProperties.Any())
                    index.UpdateSortableAttributesAsync(sortableProperties).Wait();

                if (filterableProperties.Any())
                    index.UpdateFilterableAttributesAsync(filterableProperties).Wait();
            }
        }

    }
}
