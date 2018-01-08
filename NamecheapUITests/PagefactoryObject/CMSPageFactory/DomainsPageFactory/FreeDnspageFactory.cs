using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PagefactoryObject.CMSPageFactory.DomainsPageFactory
{
    public class FreeDnspageFactory
    {
        [FindsBy(How = How.XPath, Using = ".//*[@nc-l10n='search.hero.bulkButton']", Priority = 0)]
        [FindsBy(How = How.LinkText, Using = "Bulk Search", Priority = 1)]
        [CacheLookup]
        internal IWebElement BulkDomainSearchTxtLink { get; set; }
        [FindsBy(How = How.Name, Using = "bulkDomains")]
        [CacheLookup]
        internal IWebElement BulkDomainSearchField { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(@nc-l10n,'search.bulk.getDnsButton')]")]
        [CacheLookup]
        internal IWebElement BulkDomainSearchBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@class='panel-body']/ul")]
        [CacheLookup]
        internal IWebElement FreeDnsTransferResults { get; set; }
        [FindsBy(How = How.Name, Using = "singleDomain")]
        [CacheLookup]
        internal IWebElement DomainSearch { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(@nc-l10n,'search.hero.getDnsButton')]")]
        [CacheLookup]
        internal IWebElement SearchBtn { get; set; }
        [FindsBy(How = How.PartialLinkText, Using = "manage this domain")]
        [CacheLookup]
        internal IWebElement ManageDomainLnk { get; set; }
    }
}