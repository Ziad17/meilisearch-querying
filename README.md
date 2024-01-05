




### Introduction

A fluent API that serves as a string builder to provide searching, filtering and sorting capability with using the amazing [MeiliSearch]((https://github.com/meilisearch/meilisearch))

### Getting Started

This implementation provides a set of bases on which usage can be derived and extended from, the default behavior is described in the following few sections

#### Attributes

To sort or filter a model, two attributes must be used in order to index the required properties in MeiliSearch Client

##### SearchFilter

**[SearchFilter]** attribute to make a property filterable

##### Sortable

**[Sortable]** attribute to enable sorting using a property

#### Models

To be able to use ***SearchQueryBuilder***, your models must inherit one of these bases 

##### SearchModel

***SearchModel<TKey>***  the base class for any model 

##### GeoSearchModel

***GeoSearchModel<TKey> : SearchModel<TKey>***  is a search model with coordinates search capability

#### Pagination

***SearchQueryBuilder*** supports pagination of results on calling evaluate method by passing a ***PageCriteria*** object

```C#
Evaluate(new PageCriteria(1, 10))
```

#### Usage

##### Model Entity 

```C#
public class Employee : GeoSearchModel<Guid>
{	
    [SearchFilter]
    public string Name { get; set; }    

    [Sortable]
    [SearchFilter]
    public int Age { get; set; }

    [SearchFilter]
    public bool IsDeleted { get; set; }

    [Sortable]
    [SearchFilter]
    public int Salary { get; set; }

    [SearchFilter]
    public int MonthlyTarget { get; set; }
}

```



##### Simple  Client Usage

```c#
//initiate the client
var client = new MeilisearchClient("http://localhost:7700", "4ebc913989554d17acea2ee981287a26");

//resolve usage of both SearchFilter and Sortable attributes 
client.ResolveFluentFiltersFromAssembly(AppDomain.CurrentDomain.GetAssemblies());

//inject the client
builder.Services.AddSingleton(client);
var searchService = new SearchService<Employee>(client);

var searchQuery = new SearchQueryBuilder<Employee>("John") //can discard the search term
    .OrderByDesc(x => x.Age) //Sorting Fields
    .OrderBy(x => x.Salary)
    .WithinRadius(31.222813, 29.951325, 2000) //Filtering by geo models
    .Value(x => x.Salary).GreaterThan(2000) //Filtering by logic operators
    .Value(x => x.IsDeleted).IsTrue()
    .Value(x => x.Age).GreaterThan(29)
    .Evaluate(); //Evaluating to Meilisearch search model

var result = await searchService.SearchAsync(searchQuery);
```

##### Web  Application Usage

Nuget Installation

```
Install-Package FluentSearchEngine
```

Program.cs

```c#
//initiate the client
var client = new MeilisearchClient("http://localhost:7700", "4ebc913989554d17acea2ee981287a26");

//add usage for SearchService<T>
builder.Services.AddGenericSearchService();

//resolve usage of both SearchFilter and Sortable attributes 
client.ResolveFluentFiltersFromAssembly(AppDomain.CurrentDomain.GetAssemblies());

//inject the client
builder.Services.AddSingleton(client);
```

EmployeesController.cs

```c#
   [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {

        private readonly ISearchService<Employee> _searchService;

        public EmployeesController(ISearchService<Employee> searchService)
        {
            _searchService = searchService;
        }

        [HttpGet("GetModels")]
        public async Task<IActionResult> Get()
        {
            var searchQuery = new SearchQueryBuilder<Employee>()
                .OrderByDesc(x => x.Age)
                .Value(x => x.Salary).LowerThan(3000)
                .Value(x => x.IsDeleted).IsFalse()
                .Value(x => x.MonthlyTarget).GreaterThanOrEquals(20000)
                .Value(x => x.Age).GreaterThan(29)
                .Evaluate(new PageCriteria(1, 10));

            var result = await _searchService.SearchAsync(searchQuery);
            return Ok(result.Hits);
        }
    }
```



![image](https://user-images.githubusercontent.com/36865821/201401382-da52a451-228d-407b-aa44-1f27e76308ed.png)

