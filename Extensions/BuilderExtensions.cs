using FluentSearchEngine.Model;
using Meilisearch;

namespace FluentSearchEngine.Extensions
{
    public static class BuilderExtensions
    {
        public static SearchQuery AddFilter(this SearchQuery searchQuery, string field, string term, string filterOperation = "=")
        {
            if (term == null || field == null)
                return searchQuery;

            var filterString = $"{field} {filterOperation} {term}";

            searchQuery.Filter = new List<string> { filterString };

            return searchQuery;
        }

        public static SearchQuery AddSort(this SearchQuery searchQuery, string field, SortDirection sortDirection = SortDirection.Ascending)
        {
            if (field == null)
                return searchQuery;

            var direction = sortDirection == SortDirection.Descending ? "desc" : "asc";

            var sortingFields = new[] { $"{field.FirstCharToLowerCase()}:{direction}" };

            searchQuery.Sort = sortingFields;

            return searchQuery;
        }

        public static SearchQuery AddGeoSort(this SearchQuery searchQuery, string latitude, string longitude, SortDirection sortDirection = SortDirection.Ascending)
        {
            if (latitude == null || longitude == null)
                return searchQuery;

            var direction = sortDirection == SortDirection.Descending ? "desc" : "asc";

            var sortingFields = new List<string> { $"_geoPoint({latitude},{longitude}):{direction}" };

            sortingFields.AddRange(searchQuery.Sort ?? new List<string>());

            searchQuery.Sort = sortingFields;
            return searchQuery;
        }
    }
}
