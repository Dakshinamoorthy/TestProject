using System;
using System.Threading;
using NamecheapUITests.PageObject.ValidationPages;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
namespace NamecheapUITests.PageObject.HelperPages.WrapperFactory
{
    public class PageNavigationHelper
    {
        public void NavigationTo(string productNavLbl, string selectProduct)
        {
            PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
            var navXpath = "//header[@id='header']//li//a[normalize-space(text())='" + productNavLbl + "']";
            var navItem = BrowserInit.Driver.FindElement(By.XPath(navXpath));
            var navMenu = navItem.FindElement(By.XPath(navXpath + "/..//li//a[normalize-space(text()) = '" + selectProduct + "']"));
            var clickandHoldDomainNavMenu = new Actions(BrowserInit.Driver);
            clickandHoldDomainNavMenu.ClickAndHold(navItem).Build().Perform();
            navMenu.Click();
            PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
        }
        public void MoveToElementClickHoldAndVerifyPageResponse(IWebElement moveToElement, IWebElement clickElement)
        {
            var clickandHoldDomainNavMenu = new Actions(BrowserInit.Driver);
            clickandHoldDomainNavMenu.ClickAndHold(moveToElement).Build().Perform();
            BrowserInit.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
            Thread.Sleep(500);
            clickElement.Click();
            PageInitHelper<PageNavigationHelper>.PageInit.WaitForPageLoad();
            PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
            BrowserInit.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
        }

        public void MoveToElement(IWebElement moveToElement)
        {
            var clickandHoldDomainNavMenu = new Actions(BrowserInit.Driver);
            clickandHoldDomainNavMenu.MoveToElement(moveToElement).Build().Perform();
            BrowserInit.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
            Thread.Sleep(500);
        }

        public void MoveToElementAndClick(IWebElement moveToElement, IWebElement clickElement)
        {
            var clickandHoldDomainNavMenu = new Actions(BrowserInit.Driver);
            clickandHoldDomainNavMenu.Click(moveToElement).Build().Perform();
            BrowserInit.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
            Thread.Sleep(500);
            clickElement.Click();
            PageInitHelper<PageNavigationHelper>.PageInit.WaitForPageLoad();
            PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
            BrowserInit.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
        }

        public void WaitForPageLoad()
        {
            new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(200.00)).Until(
                  driver1 =>
                      ((IJavaScriptExecutor)BrowserInit.Driver).ExecuteScript("return document.readyState")
                          .Equals("complete"));
        }
        public void ScrollToElement(IWebElement moveToElement)
        {
            ((IJavaScriptExecutor)BrowserInit.Driver).ExecuteScript("arguments[0].scrollIntoView(true);", moveToElement);
            Thread.Sleep(500);
        }
    }
}
