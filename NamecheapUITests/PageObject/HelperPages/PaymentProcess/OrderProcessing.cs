using System;
using System.Linq;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace NamecheapUITests.PageObject.HelperPages.PaymentProcess
{
    public class OrderProcessing
    {
        public void WaitForOrderProcessing()
        {
            PageInitHelper<PageNavigationHelper>.PageInit.WaitForPageLoad();
            var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(30));
            Func<IWebDriver, bool> searchtestCondition = x => BrowserInit.Driver.Url.IndexOf("confirmation", StringComparison.InvariantCultureIgnoreCase) >= 0;
            wait.Until(searchtestCondition);
            if (!BrowserInit.Driver.FindElement(By.XPath("//html[contains(@class,'no-js')]")).GetAttribute(UiConstantHelper.AttributeClass).Contains("modal-open")) return;
            BrowserInit.Driver.SwitchTo().Window(BrowserInit.Driver.WindowHandles.Last());
            BrowserInit.Driver.FindElement(By.CssSelector("#shopper-approved-modal > header > a")).Click();
            BrowserInit.Driver.SwitchTo().Window(BrowserInit.Driver.WindowHandles.FirstOrDefault());
        }
    }
}