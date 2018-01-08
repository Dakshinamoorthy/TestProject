using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NamecheapUITests.PagefactoryObject.HelpersPageFactory
{
    public class DomainSelectionPageFactory
    {
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'four-cols')]//input | .//*[contains(concat(' ',normalize-space(@class),' '),'domain-select-search')]//input")]
        [CacheLookup]
        internal IWebElement DomainNameTxt { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'four-cols')]")]
        [CacheLookup]
        internal IWebElement DomainLoadingimg { get; set; }
        
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'four-cols')]/following-sibling::p[contains(concat(' ',normalize-space(@class),' '),'availability')]")]
        [CacheLookup]
        internal IWebElement DomainAvailabilityLbl { get; set; }

        [FindsBy(How = How.Id, Using = "btnViewCart")]
        [CacheLookup]
        internal IWebElement CartContinueWidgetBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'domain-name-module')]//div[contains(@class,'buy-a-domain')]/div/div[contains(@class,'coreuiselect')]")]
        [CacheLookup]
        internal IWebElement DomainsInCartDd { get; set; }

        [FindsBy(How = How.XPath, Using = "//p[contains(@class,'availability ')]")]
        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'domain-name-module')]//div[contains(@class,'buy-a-domain')]/p[contains(@class,'availability')]")]
        [CacheLookup]
        internal IWebElement CartDomainAvailability { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[contains(@class,'domain-select-button')]/a")]
        [CacheLookup]
        internal IWebElement AddDomainToCartBtn { get; set; }
    }
}