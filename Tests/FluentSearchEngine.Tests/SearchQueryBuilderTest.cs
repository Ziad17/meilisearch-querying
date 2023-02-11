using FluentAssertions;
using FluentSearchEngine.Exceptions;
using FluentSearchEngine.Extensions;
using FluentSearchEngine.GenericEvaluators;
using FluentSearchEngine.Paging;
using Xunit;

namespace FluentSearchEngine.Tests
{
    public class SearchQueryBuilderTests
    {
        [Fact]
        public void Evaluate_WhenCalled_With_And_OperatorsFilterOptionsShouldMatchQuery()
        {
            //arrange
            var evaluator = new SearchQueryBuilder<EmployeeWithoutGeoModel>()
                .Value(x => x.Age).IsEqual(30)
                .Value(x => x.Salary).GreaterThan(2000)
                .Value(x => x.IsDeleted).IsFalse();

            //act
            var searchQuery = evaluator.Evaluate();
            var filter = (string)searchQuery.Filter;

            //assert
            searchQuery.Should().NotBeNull();
            filter.Should().Be("age = 30 AND salary > 2000 AND isDeleted = false");
        }

        [Fact]
        public void WithinRadius_WhenCalled_OnNonGeoModel_ShouldThrowGeoFilterException()
        {
            //arrange
            var queryBuilder = new SearchQueryBuilder<EmployeeWithoutGeoModel>();

            //act
            var act = () => queryBuilder.WithinRadius(21.65464, 21.2416354, 2000);

            //assert
            act.Should().ThrowExactly<GeoFilterException>()
                .WithMessage("must use the geo model");
        }

        [Fact]
        public void WithinRadius_WhenCalled_OnGeoModel_ShouldMatchQuery()
        {

            //arrange
            var evaluator = new SearchQueryBuilder<EmployeeWithGeoModel>()
                .WithinRadius(21.65464, 21.241, 2000);

            //act
            var searchQuery = evaluator.Evaluate();
            var filter = (string)searchQuery.Filter;

            //assert
            searchQuery.Should().NotBeNull();
            filter.Should().Be("_geoRadius(21.654640,21.241000,2000)");
        }

        [Fact]
        public void OrderBy_WhenCalled_ShouldMatchQuery()
        {
            //arrange
            var evaluator = new SearchQueryBuilder<EmployeeWithGeoModel>()
                .OrderBy(x => x.MonthlyTarget)
                .OrderBy(x => x.Age)
                .OrderByDesc(x => x.Name);

            var orderList = new List<string>()
            {
                "monthlyTarget:asc",
                "age:asc",
                "name:desc",
            };

            //act
            var searchQuery = evaluator.Evaluate();

            //assert
            searchQuery.Should().NotBeNull();
            for (var i = 0; i < searchQuery.Sort.Count(); i++)
            {
                searchQuery.Sort.ElementAt(i).Should().Be(orderList.ElementAt(i));
            }
        }

        [Fact]
        public void Evaluate_WhenCalled_With_PageCriteria_ShouldMatchQuery()
        {
            var pageSize = 30;
            var pageNumber = 5;
            var offset = (pageNumber - 1) * pageSize;

            //arrange
            var evaluator = new SearchQueryBuilder<EmployeeWithGeoModel>()
                .OrderBy(x => x.Age);

            //act
            var searchQuery = evaluator.Evaluate(new PageCriteria(pageNumber, pageSize));

            //assert
            searchQuery.Should().NotBeNull();
            searchQuery.Limit.Should().NotBeNull().And.Subject.Should().Be(pageSize);
            searchQuery.Offset.Should().NotBeNull().And.Subject.Should().Be(offset);
        }

    }

}