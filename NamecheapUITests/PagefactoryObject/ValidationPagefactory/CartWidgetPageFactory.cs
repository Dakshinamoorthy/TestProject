using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PagefactoryObject.ValidationPagefactory
{
    public class CartWidgetPageFactory
    {
        [FindsBy(How = How.XPath,
         Using = "((.//*[contains(@class,'cart spacer-bottom')]//ul/li[@class='register'] | //ul/li[@class='transfer'] | .//*[@class='cart-widget']/ul/li|(//ul/li[not(@class)]/strong[contains(@class,'product')]/..)| .//*[@class='cart-widget']/ul/li)[not(contains(@class,'subtotal'))][not(contains(@class,'more'))])")]
        [CacheLookup]
        internal IList<IWebElement> CartWidgetWindowItems { get; set; }
        [FindsBy(How = How.XPath,
            Using = ".//*[contains(@class,'cart spacer-bottom side')]//ul/li[contains(@class,'subtotal')]/span")]
        [CacheLookup]
        internal IWebElement CartWidgetSubTotal { get; set; }
        [FindsBy(How = How.XPath,
            Using = ".//*[@class='cart spacer-bottom side-cart']/ul")]
        [CacheLookup]
        internal IWebElement CartContent { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@class='btn btn-block']|//button[@nc-l10n='cart.setupDns']")]
        [CacheLookup]
        internal IWebElement ViewCartButton { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@class='headline']/h1")]
        [CacheLookup]
        internal IWebElement ShoppingCartHeadingTxt { get; set; }
        [FindsBy(How = How.XPath, Using = "((.//*[contains(@class,'cart spacer-bottom')]//ul/li[@class='register'] | //ul/li[@class='transfer'] | .//*[@class='cart-widget']/ul/li|(//ul/li[not(@class)]/strong[contains(@class,'product')]/..)| .//*[@class='cart-widget']/ul/li)[not(contains(@class,'subtotal'))][not(contains(@class,'more'))])")]
        [CacheLookup]
        internal IList<IWebElement> ProductListCount { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[contains(@class,'cart spacer-bottom side')] | .//*[@class='cart-widget'])//ul/li[(@class='subtotal')]/span[@class='cart-dollar-value'] | //ul/li[(@class='subtotal')]/span[contains(@nc-l10n,'totalValue')]")]
        [CacheLookup]
        internal IWebElement SubTotal { get; set; }
    }
}