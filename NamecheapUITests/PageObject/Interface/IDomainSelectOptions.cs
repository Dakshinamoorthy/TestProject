using System;
using System.Collections.Generic;
namespace NamecheapUITests.PageObject.Interface
{
    public interface IDomainSelectOptions
    {
        List<Tuple<string, string, string, decimal, decimal>> HostingDomainSelection(SortedDictionary<string, string> dicWhoisGuardProductDetailsDic = null);
        //List<Tuple<string, string, string, decimal, decimal>> HostingDomainSelection(SortedDictionary<string, string> dicWhoisGuardProductDetailsDic);
    }
}