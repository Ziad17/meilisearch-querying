using FluentSearchEngine.Configurations;
using Meilisearch;
using Microsoft.Extensions.DependencyInjection;

namespace FluentSearchEngine
{
    public static class PluginInstallation
    {
        public static void AddSearchQueryBuilder(this IServiceCollection services, string host, string key)
        {
            var client = new MeilisearchClient(host, key);

            if (!client.IsHealthyAsync().Result)
                throw new HttpRequestException("could not connect to Meilisearch host");

            client.ResolveFluentFiltersFromAssembly();

            services.AddGenericSearchService(client);
        }

        public static void AddSearchQueryBuilder(this IServiceCollection services, Action<SearchServiceOptions> optionsAction)
        {
            var options = new SearchServiceOptions();
            optionsAction.Invoke(options);

            var client = new MeilisearchClient(options.Host, options.Key);

            if (!client.IsHealthyAsync().Result)
                throw new HttpRequestException("could not connect to Meilisearch host");

            client.ResolveFluentFiltersFromAssembly(options);

            services.AddGenericSearchService(client);
        }
    }
}
