using FluentAssertions;
using FluentSearchEngine.Extensions;
using FluentSearchEngine.GenericEvaluators;
using Xunit;

namespace FluentSearchEngine.Tests.Extensions;

public class CommonExtensionsTest
{
    [Fact]
    public void Exists()
    {
        //arrange
        var evaluator = new SearchQueryBuilder<EmployeeWithoutGeoModel>()
            .Value(x => x.Age).Exists()
            .Value(x => x.Name).Exists();

        //act
        var searchQuery = evaluator.Evaluate();
        var filter = (string)searchQuery.Filter;

        //assert
        searchQuery.Should().NotBeNull();
        filter.Should().Be("age EXISTS AND name EXISTS");
    }
}
