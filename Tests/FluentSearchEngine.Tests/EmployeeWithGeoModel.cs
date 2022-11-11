using FluentSearchEngine.Attributes;
using FluentSearchEngine.Model;

namespace FluentSearchEngine.Tests
{
    public class EmployeeWithGeoModel : GeoSearchModel<Guid>
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

        public int MonthlyTarget { get; set; }
    }
}
