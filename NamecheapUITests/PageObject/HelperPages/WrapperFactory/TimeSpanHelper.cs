using System;
using System.Threading;
using OpenQA.Selenium;
namespace NamecheapUITests.PageObject.HelperPages.WrapperFactory
{
    public class TimeSpanHelper
    {
        #region Constants
        public readonly int PageLoadTimeout = 10;
        #endregion
        public bool WaitUntilElementIsDisplayed(IWebElement element, int timeoutInSeconds = 30)
        {
            for (var i = 0; i < timeoutInSeconds; i++)
            {
                if (ElementIsDisplayed(element))
                {
                    return true;
                }
                Thread.Sleep(1000);
            }
            return false;
        }

        private bool ElementIsDisplayed(IWebElement element)
        {
            bool present;
            BrowserInit.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
            try
            {
                present = element.Displayed;
            }
            catch (NoSuchElementException)
            {
                throw new NoSuchElementException(element + " is not present in " + BrowserInit.Driver.Title);
            }
            BrowserInit.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(PageLoadTimeout));
            return present;
        }
        public bool WaitUntilpageTiltleIsDisplayed(string pageTitle, int timeoutInSeconds)
        {
            for (var i = 0; i < timeoutInSeconds; i++)
            {
                if (PageTitleIsDisplayed(pageTitle))
                {
                    return true;
                }
                Thread.Sleep(1000);
            }
            return false;
        }

        private bool PageTitleIsDisplayed(string pageTitle)
        {
            bool present;
            BrowserInit.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
            try
            {
                present = BrowserInit.Driver.Title.IndexOf(pageTitle.Trim(), StringComparison.OrdinalIgnoreCase) >= 0;
            }
            catch (NoSuchElementException)
            {
                throw new NoSuchElementException();
            }
            BrowserInit.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(PageLoadTimeout));
            return present;
        }
        public bool VerifyElementPresentBasedonattribute(IWebElement element, string attributeName, string contains)
        {
            for (var i = 0; i < 30; i++)
            {
                if (AttributeIsDisplayed(element, attributeName, contains))
                {
                    return true;
                }
                Thread.Sleep(1000);
            }
            return false;
        }
        public bool AttributeIsDisplayed(IWebElement element, string attributeName, string contains)
        {
            bool present;
            BrowserInit.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(0));
            try
            {
                present = element.GetAttribute(attributeName).Contains(contains);
            }
            catch (NoSuchElementException)
            {
                throw new NoSuchElementException(element + " is not present in " + BrowserInit.Driver.Title);
            }
            BrowserInit.Driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(PageLoadTimeout));
            return present;
        }
        public bool VerifyElementPresentBasedonEleText(IWebElement element, string attributeName, string contains)
        {
            for (var i = 0; i < 130; i++)
            {
                var result = AttributeIsDisplayed(element, attributeName, contains);
                if (!result)
                {
                    return false;
                }
                Thread.Sleep(1000);
            }
            return true;
        }
    }
}