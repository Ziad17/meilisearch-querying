using FluentSearchEngine.Exceptions;
using FluentSearchEngine.Extensions;
using FluentSearchEngine.GenericEvaluators.Interfaces;
using FluentSearchEngine.Model;
using FluentSearchEngine.Paging;
using Meilisearch;
using System.Linq.Expressions;

namespace FluentSearchEngine.GenericEvaluators
{
    public class SearchQueryBuilder<T> :
        FilterBase,
        INumbersEvaluator<T>,
        IStringsEvaluator<T>,
        IBooleanEvaluator<T>,
        ICollectionEvaluator<T>,
        IValue<T>
    {
        public List<string> Sort { get; set; } = new();
        public string Term { get; set; }

        public SearchQueryBuilder(string term = "")
        {
            Term = term;
        }

        public IStringsEvaluator<T> Value(Expression<Func<T, string>> action)
        {
            AppendFilterLiteralText(action);
            return this;
        }

        public ICollectionEvaluator<T> Value<TData>(Expression<Func<T, ICollection<TData>>> action)
        {
            AppendFilterLiteralText(action);
            return this;
        }

        public IBooleanEvaluator<T> Value(Expression<Func<T, bool>> action)
        {
            AppendFilterLiteralText(action);
            return this;
        }

        public INumbersEvaluator<T> Value(Expression<Func<T, int>> action)
        {
            AppendFilterLiteralText(action);
            return this;
        }

        public INumbersEvaluator<T> Value(Expression<Func<T, double>> action)
        {
            AppendFilterLiteralText(action);
            return this;
        }

        public INumbersEvaluator<T> Value(Expression<Func<T, decimal>> action)
        {
            AppendFilterLiteralText(action);
            return this;
        }

        public SearchQuery Evaluate(PageCriteria criteria)
        {
            var searchQuery = new SearchQuery
            {
                Limit = criteria.PageSize,
                Offset = (criteria.PageNumber - 1) * criteria.PageSize,
                Filter = Filter.ToString().Trim(),
                Q = Term.Trim()
            };

            if (Sort.Any())
                searchQuery.Sort = Sort;

            return searchQuery;
        }

        public SearchQuery Evaluate(int pageNumber = 1, int pageSize = 10)
        {
            return Evaluate(new PageCriteria(1, 10));
        }

        public SearchQueryBuilder<T> OrderBy<TSort>(Expression<Func<T, TSort>> action)
        {
            var body = (MemberExpression)action.Body;
            var propertyName = body.Member.Name;
            Sort.Add($"{propertyName.FirstCharToLower()}:asc");
            return this;
        }

        public SearchQueryBuilder<T> OrderByDesc<TSort>(Expression<Func<T, TSort>> action)
        {
            var body = (MemberExpression)action.Body;
            var propertyName = body.Member.Name;
            Sort.Add($"{propertyName.FirstCharToLower()}:desc");
            return this;
        }

        public SearchQueryBuilder<T> OrderBy(string propertyName)
        {
            Sort.Add($"{propertyName.FirstCharToLower()}:asc");
            return this;
        }

        public SearchQueryBuilder<T> OrderByDesc(string propertyName)
        {
            Sort.Add($"{propertyName.FirstCharToLower()}:desc");
            return this;
        }

        public SearchQueryBuilder<T> Where(string clause)
        {
            AppendFilterLiteralText(clause);
            return this;
        }

        public SearchQueryBuilder<T> Order(string order)
        {
            Sort.Add(order);
            return this;
        }

        public SearchQueryBuilder<T> WithinRadius(double centerLat, double centerLon, int radiusInMeters)
        {
            var hasCoordinate = typeof(T).GetProperties().Any(x => x.PropertyType == typeof(GeoCoordinates));

            if (!hasCoordinate)
                throw new GeoFilterException("must use the geo model");

            this.Filter.Append($"_geoRadius({centerLat:#.000000},{centerLon:#.000000},{radiusInMeters})");

            return this;
        }

        public SearchQueryBuilder<T> WithGeoSort(string latitude, string longitude, bool ascending = true)
        {
            if (latitude == null || longitude == null)
                return this;

            var hasCoordinate = typeof(T).GetProperties().Any(x => x.PropertyType == typeof(GeoCoordinates));

            if (!hasCoordinate)
                throw new GeoFilterException("must use the geo model");

            var direction = ascending ? "asc" : "desc";

            Sort.Add($"_geoPoint({latitude},{longitude}):{direction}");

            return this;
        }

        private void AppendFilterLiteralText<TKey>(Expression<Func<T, TKey>> action)
        {
            var body = (MemberExpression)action.Body;
            var propertyName = body.Member.Name;
            if (Filter.Length != 0)
                Filter.Append(" AND ");
            Filter.Append($"{propertyName.FirstCharToLower()}");
        }

        private void AppendFilterLiteralText(string clause)
        {
            if (Filter.Length != 0)
                Filter.Append(" AND ");
            Filter.Append($"{clause.FirstCharToLower()}");
        }

    }
}
