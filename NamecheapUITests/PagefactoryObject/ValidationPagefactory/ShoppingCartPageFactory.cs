using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PagefactoryObject.ValidationPagefactory
{
    public class ShoppingCartPageFactory
    {
        [FindsBy(How = How.XPath, Using = ".//*[@id='buynow']/div/div[2]/div/div[1]/p[1]/a")]
        [CacheLookup]
        internal IWebElement ConfirmOrderBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[contains(@class,'headline')]/h1")]
        [CacheLookup]
        internal IWebElement ShoppingCartHeroBannerTxt { get; set; }
        [FindsBy(How = How.CssSelector, Using = ".alert")]
        [CacheLookup]
        internal IWebElement AlertMessage { get; set; }
        [FindsBy(How = How.Id,
            Using =
                "ctl00_ctl00_ctl00_ctl00_base_content_web_base_content_home_content_page_content_left_errorDisplayControl_errorDisplayParentDiv"
            )]
        [CacheLookup]
        internal IWebElement AlertMessageDiv { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='grid-col three-quarters']/*[2]")]
        [CacheLookup]
        internal IWebElement ProductPresent { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='buynow']/div/div[2]/div/div[1]/ul/li")]
        [CacheLookup]
        internal IWebElement SubtotalTxt { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[contains(@class,'product-group')]")]
        [CacheLookup]
        internal IList<IWebElement> ProductGroup { get; set; }
    }
}