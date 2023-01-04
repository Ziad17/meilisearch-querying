using FluentSearchEngine.Attributes;
using FluentSearchEngine.Extensions;
using FluentSearchEngine.Services.Implementations;
using Humanizer;
using Meilisearch;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json.Serialization;
using FluentSearchEngine.Services.Abstractions;

namespace FluentSearchEngine
{
    public static class DependencyInjection
    {
        //TODO:: enable options based pattern to configure collective index
        //TODO:: enable pagination max hit modification
        public static void ResolveFluentFiltersFromAssembly(this MeilisearchClient client, params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(s => s.GetTypes())
                .Where(x => x.BaseType != null && x.BaseType!.Name is "SearchModel`1" or "GeoSearchModel`1" && x.Name != "GeoSearchModel`1");

            foreach (var type in types)
            {
                var sortableProperties = type.GetProperties()
                    .Where(x => x.IsDefined(typeof(Sortable), true))
                    .Select(x => x.IsDefined(typeof(JsonPropertyNameAttribute), true) ? x.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name : x.Name.FirstCharToLower()).ToList();

                var filterableProperties = type.GetProperties()
                    .Where(x => x.IsDefined(typeof(SearchFilter), true))
                    .Select(x => x.IsDefined(typeof(JsonPropertyNameAttribute), true) ? x.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name : x.Name.FirstCharToLower()).ToList();

                var index = client.Index(type.Name.Pluralize());

                if (sortableProperties.Any())
                    index.UpdateSortableAttributesAsync(sortableProperties);

                if (filterableProperties.Any())
                    index.UpdateFilterableAttributesAsync(filterableProperties);
            }
        }

        public static void AddGenericSearchService(this IServiceCollection services)
        {
            services.AddScoped(typeof(ISearchService<>), typeof(SearchService<>));
        }
    }
}
