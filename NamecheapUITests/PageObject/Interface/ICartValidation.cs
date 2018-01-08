using System.Collections.Generic;
namespace NamecheapUITests.PageObject.Interface
{
    public interface ICartValidation
    {
        List<SortedDictionary<string, string>> CartWidgetValidation(List<SortedDictionary<string, string>> searchResultDomainsList);
        SortedDictionary<string, string> CartWidgetValidation(SortedDictionary<string, string> searchResultDomainsList);
    }
}