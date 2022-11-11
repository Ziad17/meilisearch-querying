using FluentSearchEngine.GenericEvaluators.Interfaces;
using System.Text;

namespace FluentSearchEngine.GenericEvaluators
{
    public class FilterBase : IFilter
    {
        public StringBuilder Filter { get; protected set; } = new StringBuilder();
    }
}
