using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PagefactoryObject.CMSPageFactory.DomainsPageFactory
{
    public class NewTldsPageFactory
    {
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ', normalize-space(@class), ' '), 'tab-list')]/ul/li[1]")]
        [CacheLookup]
        internal IWebElement ExploreTabLbl { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[contains(concat(' ', normalize-space(@class), ' '),'module-green tlds-explorer')]/ul/li)")]
        [CacheLookup]
        internal IList<IWebElement> ListofTlds { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ', normalize-space(@class), ' '),'empty-tlds-list')]")]
        [CacheLookup]
        internal IWebElement EmptyTldListTxt { get; set; }
    }
}