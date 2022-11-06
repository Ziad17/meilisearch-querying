using FluentSearchEngine.Exceptions;
using FluentSearchEngine.Extensions;
using FluentSearchEngine.GenericEvaluators.Interfaces;
using FluentSearchEngine.Model;
using FluentSearchEngine.Paging;
using Meilisearch;
using System.Linq.Expressions;
using System.Text;

namespace FluentSearchEngine.GenericEvaluators
{
    public class SearchQueryBuilder<T> :
        INumbersEvaluator<T>,
        IStringsEvaluator<T>,
        IBooleanEvaluator<T>,
        IEvaluator<T>,
        IValue<T>
    {
        public StringBuilder Filter { get; set; } = new StringBuilder();
        public List<string> Sort { get; set; } = new List<string>();
        public string Term { get; set; }

        public SearchQueryBuilder(string term)
        {
            Term = term;
        }
        public IBooleanEvaluator<T> Value(Expression<Func<T, bool>> action)
        {
            var body = (MemberExpression)action.Body;
            var propertyName = body.Member.Name;

            Filter.Append(StringExtensions.AddWhiteSpaceBeforeToLower(propertyName));
            return this;
        }

        public INumbersEvaluator<T> Value(Expression<Func<T, int>> action)
        {
            var body = (MemberExpression)action.Body;
            var propertyName = body.Member.Name;

            Filter.Append(StringExtensions.AddWhiteSpaceBeforeToLower(propertyName));
            return this;
        }
        public IStringsEvaluator<T> Value(Expression<Func<T, string>> action)
        {
            var body = (MemberExpression)action.Body;
            var propertyName = body.Member.Name;

            Filter.Append(StringExtensions.AddWhiteSpaceBeforeToLower(propertyName));

            return this;
        }

        public SearchQuery Evaluate(PageCriteria? criteria = default)
        {
            var searchQuery = new SearchQuery();

            if (criteria != null)
            {
                searchQuery.Limit = criteria.PageSize;
                searchQuery.Offset = (criteria.PageNumber - 1) * criteria.PageSize;
            }

            searchQuery.Filter = Filter.ToString().Trim();
            searchQuery.Q = Term.Trim();
            if (Sort.Any())
                searchQuery.Sort = Sort;


            return searchQuery;
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

        public IValue<T> And()
        {
            this.Filter.Append((" AND"));
            return this;

        }

        public IValue<T> Or()
        {
            this.Filter.Append((" OR"));

            return this;
        }


        public SearchQueryBuilder<T> WithinRadius(double centerLat, double centerLon, int radiusInMeters)
        {
            var hasCoordinate = typeof(T).GetProperties().Any(x => x.PropertyType == typeof(GeoCoordinates));

            if (!hasCoordinate)
                throw new GeoFilterException("must use the geo model");

            this.Filter.Append(StringExtensions.AddWhiteSpaceBeforeToLower($"_geoRadius({centerLat:#.000000},{centerLon:#.000000},{radiusInMeters})"));

            return this;


        }


    }
}
