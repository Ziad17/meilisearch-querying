using FluentSearchEngine.Attributes;
using FluentSearchEngine.Extensions;
using FluentSearchEngine.Services.Abstraction;
using FluentSearchEngine.Services.Implementations;
using Humanizer;
using Meilisearch;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json.Serialization;

namespace FluentSearchEngine
{
    public static class DependencyInjection
    {
        public static void ResolveFluentFiltersFromAssembly(this MeilisearchClient client, params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(s => s.GetTypes()).Where(x => x.BaseType != null && (x.BaseType!.Name == "SearchModel`1" || x.BaseType!.Name == "GeoSearchModel`1") && x.Name != "GeoSearchModel`1");

            foreach (var type in types)
            {
                var sortableProperties = type.GetProperties().Where(x => x.IsDefined(typeof(Sortable), true)).Select(
                    x =>
                    {
                        if (x.IsDefined(typeof(JsonPropertyNameAttribute), true))
                        {
                            return x.GetCustomAttribute<JsonPropertyNameAttribute>().Name;
                        }
                        else
                        {
                            return x.Name.FirstCharToLowerCase();
                        }

                    }).ToList();
                if (sortableProperties.Any())
                {
                    var index = client.Index(type.Name.Pluralize());
                    index.UpdateSortableAttributesAsync(sortableProperties);
                }
                var filterableProperties = type.GetProperties().Where(x => x.IsDefined(typeof(SearchFilter), true)).Select(
                    x =>
                    {
                        if (x.IsDefined(typeof(JsonPropertyNameAttribute), true))
                        {
                            return x.GetCustomAttribute<JsonPropertyNameAttribute>().Name;
                        }
                        else
                        {
                            return x.Name.FirstCharToLowerCase();
                        }

                    }).ToList();
                if (filterableProperties.Any())
                {
                    var index = client.Index(type.Name.Pluralize());
                    index.UpdateFilterableAttributesAsync(filterableProperties);
                }
            }
        }

        public static void AddGenericSearchService(this IServiceCollection services)
        {
            services.AddScoped(typeof(ISearchService<>), typeof(SearchService<>));
        }
    }
}
