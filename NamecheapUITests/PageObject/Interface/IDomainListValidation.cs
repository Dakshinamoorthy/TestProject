using System.Collections.Generic;
namespace NamecheapUITests.PageObject.Interface
{
    public interface IDomainListValidation
    {
        void DomainListValidation(List<SortedDictionary<string, string>> mergedScAndCartWidgetListWithOrderNum, string viewTab = "All Products");
    }
}