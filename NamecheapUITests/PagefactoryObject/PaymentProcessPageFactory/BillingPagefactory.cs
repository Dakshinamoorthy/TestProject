using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NamecheapUITests.PagefactoryObject.PaymentProcessPageFactory
{
    public class BillingPagefactory
    {
        [FindsBy(How = How.XPath, Using = "(.//label[contains(@for,'po-r-1')])/input")]
        [CacheLookup]
        internal IWebElement CardPaymentOption { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//label[contains(@for,'po-r-2')])/input")]
        [CacheLookup]
        internal IWebElement PaypalPaymentOption { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//label[contains(@for,'po-r-3')])/input")]
        [CacheLookup]
        internal IWebElement FundsPaymentOption { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[contains(@class,'content-panel') and not(contains(@class,'error'))][contains(@style,'block')]//p[not(normalize-space(.)='')])")]
        [CacheLookup]
        internal IList<IWebElement> InfoText { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//label[contains(@for,'po-r-')])")]
        [CacheLookup]
        internal IList<IWebElement> PaymentOptions { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(@class,'cart spacer-bottom side-cart')]/p/a")]
        [CacheLookup]
        internal IWebElement PaymentContinueBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[contains(@class,'content-panel')][contains(@style,'block')]//p[not(normalize-space(.)='')])")]
        [CacheLookup]
        internal IList<IWebElement> InfoText1 { get; set; }
    }
}