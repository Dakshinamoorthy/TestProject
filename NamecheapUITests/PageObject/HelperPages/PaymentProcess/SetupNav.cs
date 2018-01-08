using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NamecheapUITests.PageObject.HelperPages.PaymentProcess
{
    public class SetupNav : ICheckoutNav
    {
        public bool PlaceOrderCheckoutFlow(string paymentOption = "", bool changePaymentMode = true)
        {
            PageInitHelper<SetupNav>.PageInit.PaymentContinueBtn.Click();
            return changePaymentMode;
        }
        #region
        [FindsBy(How = How.XPath, Using = ".//*[contains(@class,'cart spacer-bottom side-cart')]/p/a")]
        [CacheLookup]
        internal IWebElement PaymentContinueBtn { get; set; }
        #endregion
    }

}