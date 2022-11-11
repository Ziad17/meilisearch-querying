using System.Text;

namespace FluentSearchEngine.GenericEvaluators.Interfaces
{
    public interface IFilter
    {
        StringBuilder Filter { get; }
    }
}
