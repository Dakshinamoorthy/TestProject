using System.Collections.Generic;
namespace NamecheapUITests.PageObject.Interface
{
    public interface IShoppingCartValidation
    {
        List<SortedDictionary<string, string>> AddShoppingCartItemsToDic(List<SortedDictionary<string, string>> cartWidgetList, string whois = "", string premiumDns = "");
    }
}