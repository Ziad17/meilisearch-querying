using FluentSearchEngine.Attributes;
using FluentSearchEngine.Configurations;
using FluentSearchEngine.Extensions;
using FluentSearchEngine.Services.Abstraction;
using FluentSearchEngine.Services.Implementations;
using Humanizer;
using Meilisearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FluentSearchEngine
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFluentSearchEngine(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();
            var searchEngineConfiguration = new SearchEngineConfiguration();
            configuration.Bind("SearchEngine", searchEngineConfiguration);
            MeilisearchClient client = new MeilisearchClient(searchEngineConfiguration.HostUrl, searchEngineConfiguration.ApiKey);
            services.AddSingleton(client);
            services.AddScoped(typeof(ISearchService<>), typeof(SearchService<>));

            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(x => x.BaseType != null && x.BaseType!.Name == "SearchModel`1" && x.Name != "GeoSearchModel`1");

            foreach (var type in types)
            {
                var sortableProperties = type.GetProperties().Where(x => x.IsDefined(typeof(Sortable), true)).Select(x => x.Name.FirstCharToLowerCase()).ToList();
                if (sortableProperties.Any())
                {
                    var index = client.Index(type.Name.Pluralize());
                    index.UpdateSortableAttributesAsync(sortableProperties);
                }
                var filterableProperties = type.GetProperties().Where(x => x.IsDefined(typeof(SearchFilter), true)).Select(x => x.Name.FirstCharToLowerCase()).ToList();
                if (filterableProperties.Any())
                {
                    var index = client.Index(type.Name.Pluralize());
                    index.UpdateSortableAttributesAsync(filterableProperties);
                }
            }

            return services;
        }
    }
}
