using System.Collections.Generic;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
namespace NamecheapUITests.PageObject.HelperPages
{
    public class DataHelper
    {
        public string DomainName = PageInitHelper<SldGenerator>.PageInit.GetRandomString(15);
        public string TransferDomainName = PageInitHelper<SldGenerator>.PageInit.GetRandomString(3);
        public List<string> BulkTlds = new List<string>();
        public string Tlds = PageInitHelper<RandomDomainNameGenerator>.PageInit.Tlds();
    }
}