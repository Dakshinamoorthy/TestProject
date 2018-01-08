using System;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace NamecheapUITests.PageObject.HelperPages
{
    public class ClearCart
    {
        public void ClearCartItems()
        {
            if (!BrowserInit.Driver.Url.Contains("/cart/cart.aspx"))
                BrowserInit.Driver.Navigate().GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("CMS", "/cart/cart.aspx"));
            new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(200.00)).Until(driver1 => ((IJavaScriptExecutor)BrowserInit.Driver).ExecuteScript("return document.readyState").Equals("complete"));
            var topAction = PageInitHelper<ClearCart>.PageInit.HeaderPanel.GetAttribute(UiConstantHelper.AttributeClass);
            if (!topAction.Contains("top-action")) return;
            PageInitHelper<ClearCart>.PageInit.DropdownEditCart.Click();
            PageInitHelper<ClearCart>.PageInit.DropDownItemClearAllItems.Click();
            Assert.IsTrue(PageInitHelper<ClearCart>.PageInit.AlertMessage.Text.IndexOf("Cart is cleared successfully!", StringComparison.OrdinalIgnoreCase) >= 0);
        }
        #region PageFactory
        [FindsBy(How = How.XPath, Using = ".//a[contains(text(),'Edit Cart')]")]
        [CacheLookup]
        internal IWebElement DropdownEditCart { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[text()='Clear all items']")]
        [CacheLookup]
        internal IWebElement DropDownItemClearAllItems { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[contains(@class, 'alert')]//p")]
        [CacheLookup]
        internal IWebElement AlertMessage { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[contains(@class,'tab-panel')]//*[contains(@class,'no-disable-force-hide')]/following-sibling::div[1]")]
        [CacheLookup]
        internal IWebElement HeaderPanel { get; set; }
        #endregion
    }
}