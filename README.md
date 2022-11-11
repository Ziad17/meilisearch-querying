# FluentSearchEngine

## A fluent API that serves as a string builder to provide searching, filtering and sorting capability with using the amazing [MeiliSearch]((https://github.com/meilisearch/meilisearch))

- ### Attributes

  To sort or filter a model, two attributes must be used in order to index the required properties in MeiliSearch Client

  **[SearchFilter]** 

  to make a property filterable

  

  **[Sortable]** 

  to enable sorting using a property

- ### Model Base

  To be able to use ***SearchQueryBuilder***, your models must inherit one of these bases 

  ### ***SearchModel<TKey>*** 

  the base class for any model 

  

  ### ***GeoSearchModel<TKey>*** 

  is a search model with coordinates search capability

- ### Pagination

  ***SearchQueryBuilder*** supports pagination of results on calling evaluate method by passing a ***PageCriteria*** object

  ```C#
  Evaluate(new PageCriteria(1, 10))
  ```



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



## Usage

- ### Simple  Client Usage

  ```c#
  MeilisearchClient client = new MeilisearchClient("http://host_name:7700", "4ebc913589554d17acea2ee981287a26");
  
  var searchService = new SearchService<Employee>(client);
  
  
  var searchQuery = new SearchQueryBuilder<Employee>("John") //can discard the search term
      .OrderByDesc(x => x.Age) //Sorting Fields
      .OrderBy(x => x.Salary)
      .WithinRadius(31.222813, 29.951325, 2000) //Filtering by geo models
      .And(x => x.Salary).GreaterThan(2000) //Filtering by logic operators
      .And(x => x.IsDeleted).IsTrue()
      .Or(x => x.Age).GreaterThan(29)
      .Evaluate(); //Evaluating to Meilisearch search model
  
  var result = await searchService.SearchAsync(searchQuery);
  ```

- ### Web  Application Usage

  Nuget Installation

  ```
  Install-Package FluentSearchEngine
  ```

  appSettings.json

  ```json
  "SearchEngine": {
      "HostUrl": "http://host_name:7700",
      "ApiKey": "4ebc913589554d17acea2ee981287a26"
    }
  ```

  Program.cs

  ```c#
  services.AddFluentSearchEngine();
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
                  .And(x => x.IsDeleted).IsFalse()
                  .And(x => x.MonthlyTarget).GreaterThanOrEquals(20000)
                  .Or(x => x.Age).GreaterThan(29)
                  .Evaluate(new PageCriteria(1, 10));
  
              var result = await _searchService.SearchAsync(searchQuery);
              return Ok(result.Hits);
          }
      }
  ```
  
  ![image](https://user-images.githubusercontent.com/36865821/201401382-da52a451-228d-407b-aa44-1f27e76308ed.png)


  

