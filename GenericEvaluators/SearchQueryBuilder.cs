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
      IDateTimeEvaluator<T>,
      ICollectionEvaluator<T>,
      IValue<T>
    {
        public SearchQueryBuilder(string term = "")
        {
            Term = term;
        }

        public List<string> Sort { get; set; } = new List<string>();

        public string Term { get; set; }

        public IStringsEvaluator<T> Value(Expression<Func<T, string>> action)
        {
            AppendLiteralText(action);
            return this;
        }

        public IDateTimeEvaluator<T> Value(Expression<Func<T, DateTime>> action)
        {
            AppendLiteralText(action);
            return this;
        }

        public ICollectionEvaluator<T> Value<TData>(Expression<Func<T, ICollection<TData>>> action)
        {
            AppendLiteralText(action);
            return this;
        }

        public IBooleanEvaluator<T> Value(Expression<Func<T, bool>> action)
        {
            AppendLiteralText(action);
            return this;
        }

        public INumbersEvaluator<T> Value(Expression<Func<T, int>> action)
        {
            AppendLiteralText(action);
            return this;
        }

        public INumbersEvaluator<T> Value(Expression<Func<T, double>> action)
        {
            AppendLiteralText(action);
            return this;
        }

        public SearchQuery Evaluate(PageCriteria pagingOptions)
        {
            var searchQuery = new SearchQuery
            {
                Limit = pagingOptions.PageSize,
                Offset = (pagingOptions.PageNumber - 1) * pagingOptions.PageSize,
                Filter = Filter.ToString().Trim(),
                Q = Term.Trim()
            };

            if (Sort.Any())
                searchQuery.Sort = Sort;
            return searchQuery;
        }

        public SearchQuery Evaluate(int pageNumber = 1, int pageSize = 10)
        {
            return Evaluate(new PageCriteria(pageNumber, pageSize));
        }

        public SearchQueryBuilder<T> OrderBy<TSort>(Expression<Func<T, TSort>> action)
        {
            var body = (MemberExpression)action.Body;
            var propertyName = body.Member.Name;
            Sort.Add($"{propertyName.FirstCharToLowerCase()}:asc");
            return this;
        }

        public SearchQueryBuilder<T> OrderByDesc<TSort>(Expression<Func<T, TSort>> action)
        {
            var body = (MemberExpression)action.Body;
            var propertyName = body.Member.Name;
            Sort.Add($"{propertyName.FirstCharToLowerCase()}:desc");
            return this;
        }

        public SearchQueryBuilder<T> WithinRadius(double centerLat, double centerLon, int radiusInMeters)
        {
            var hasCoordinate = typeof(T).GetProperties().Any(x => x.PropertyType == typeof(GeoCoordinates));

            if (!hasCoordinate)
                throw new GeoFilterException("must use the geo model");

            Filter.Append($"_geoRadius({centerLat:#.000000}, {centerLon:#.000000}, {radiusInMeters})");

            return this;
        }

        private void AppendLiteralText<TKey>(Expression<Func<T, TKey>> action)
        {
            var body = (MemberExpression)action.Body;
            var propertyName = body.Member.Name;
            if (Filter.Length != 0)
                Filter.Append(" AND ");
            Filter.Append($"{propertyName.FirstCharToLowerCase()}");
        }

    }
}
