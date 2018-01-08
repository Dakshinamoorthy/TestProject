using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PagefactoryObject.CMSPageFactory.DomainsPageFactory
{
    public class TransferDomainPageFactory
    {
        [FindsBy(How = How.Id,
            Using = "ctl00_ctl00_ctl00_ctl00_base_content_web_base_content_home_content_page_content_left_ctl00_inputTransferDomain",
            Priority = 0)]
        [CacheLookup]
        internal IWebElement DomainSearchTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//button[@type='button'][normalize-space(@class)='ncRedirectButton ga-event']", Priority = 1)]
        [CacheLookup]
        internal IWebElement TransferSearchBtn { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@data-gaevent-action='Bulk transfer']", Priority = 0)]
        [CacheLookup]
        internal IWebElement BulkTransferSearchBtn { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(@class,'search-results')]")]
        [CacheLookup]
        internal IWebElement SearchResultDiv { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[contains(@class,'btn cart-add')]")]
        [CacheLookup]
        internal IWebElement TransferDomainToCartBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "//p[@class='checkbox check-all']/input")]
        [CacheLookup]
        internal IWebElement YesToAllChkBox { get; set; }
        [FindsBy(How = How.Id, Using = "authcode1")]
        [CacheLookup]
        internal IWebElement AuthCodeTxtBox { get; set; }
        [FindsBy(How = How.XPath, Using = "//ul[contains(@class,'transfer-results')]/li")]
        [CacheLookup]
        internal IList<IWebElement> UnKnownRegistrar { get; set; }
        [FindsBy(How = How.LinkText, Using = "Bulk Options", Priority = 0)]
        [FindsBy(How = How.LinkText, Using = "Bulk Search", Priority = 1)]
        [CacheLookup]
        internal IWebElement BulkOptionLnkTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'multiple-domain-search')]")]
        [CacheLookup]
        internal IWebElement MultipleDomainSearchInputTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(@class,'eligible')]/ul/li")]
        [CacheLookup]
        internal IList<IWebElement> EligibleTransferList { get; set; }
    }
}