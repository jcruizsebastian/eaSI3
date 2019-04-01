using System.Collections.Generic;

namespace SI3.Issues
{
    public interface IRepositoryXML<T> where T : IElement
    {
        List<T> elements { get; set; }
    }
}