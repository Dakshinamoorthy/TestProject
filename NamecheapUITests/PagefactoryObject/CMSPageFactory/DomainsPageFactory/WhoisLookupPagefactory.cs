using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PagefactoryObject.CMSPageFactory.DomainsPageFactory
{
    public class WhoisLookupPagefactory
    {
        [FindsBy(How = How.XPath, Using = ".//form/*[contains(@class,'hero-search')]//fieldset/input[@type='search']")]
        [CacheLookup]
        internal IWebElement DomainSearchForWhoisTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//form/*[contains(@class,'hero-search')]//fieldset/button")]
        [CacheLookup]
        internal IWebElement WhoisLookuphBtn { get; set; }
        [FindsBy(How = How.Id, Using = "ctl00_page_content_left_ctl01_ctl00_ctl00_whoisForDomainLabel")]
        [CacheLookup]
        internal IWebElement WhoisDomainLabel { get; set; }
        [FindsBy(How = How.Id, Using = "ctl00_page_content_left_ctl01_ctl00_ctl00_whoisResultText")]
        [CacheLookup]
        internal IWebElement WhoisResult { get; set; }
    }
}