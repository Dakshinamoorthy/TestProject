using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PagefactoryObject.CMSPageFactory.SecurityPageFactory
{
    public class SslCertificatePageFactory
    {
        [FindsBy(How = How.XPath, Using = "//*[contains(@class,'ssl-filters')]//*[@class='ssl-shop-filters']/ul/li/a[.='Validation']/..")]
        [CacheLookup]
        internal IWebElement ValidationFilter { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[contains(@class,'ssl-filters')]//*[@class='ssl-shop-filters']/ul/li/a[.='Validation']/../div/ul/li[1]//input")]
        [CacheLookup]
        internal IWebElement DvOption { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[contains(@class,'ssl-filters')]//*[@class='ssl-shop-filters']/ul/li/a[.='Validation']/../div/ul/li[2]//input")]
        [CacheLookup]
        internal IWebElement OvOption { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[contains(@class,'ssl-filters')]//*[@class='ssl-shop-filters']/ul/li/a[.='Validation']/../div/ul/li[3]//input")]
        [CacheLookup]
        internal IWebElement EvOption { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[contains(@class,'ssl-filters')]//*[@class='ssl-shop-filters']/ul/li/a[.='Domains']/..")]
        [CacheLookup]
        internal IWebElement DomainsFilter { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'ssl-certificates-module')]/ul/li/h2/span")]
        [CacheLookup]
        internal IWebElement AdditionalDomains { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='addonControls']/div//input")]
        [CacheLookup]
        internal IWebElement AdditionalDomainsTxt { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='addonControls']/div/div[contains(@class,'price-info')]//pr")]
        [CacheLookup]
        internal IWebElement AdditionalDomainsPrice { get; set; }
    }
}