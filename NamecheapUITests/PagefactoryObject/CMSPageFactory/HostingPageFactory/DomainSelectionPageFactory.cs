using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NamecheapUITests.PagefactoryObject.CMSPageFactory.HostingPageFactory
{
    class DomainSelectionPageFactory
    {
        [FindsBy(How = How.XPath, Using = "(.//*[contains(@class,'domain-select-options')]//a)")]
        [CacheLookup]
        internal IList<IWebElement> DomainOptionsLst { get; set; }
    }
}
