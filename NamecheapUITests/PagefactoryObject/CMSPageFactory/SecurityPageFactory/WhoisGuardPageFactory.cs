using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PagefactoryObject.CMSPageFactory.SecurityPageFactory
{
    public class WhoisGuardPageFactory
    {
        [FindsBy(How = How.XPath, Using = "//span[@class='amount']/..")]
        [CacheLookup]
        internal IWebElement WhoisGuardProductPriceTxt { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[contains(@id,'productAddToCartFieldset')]/a")]
        [CacheLookup]
        internal IWebElement WhoisGuardProducAddToCartBtn { get; set; }
    }
}