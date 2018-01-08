using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NUnit.Framework;
using OpenQA.Selenium;

namespace NamecheapUITests
{
    [SetUpFixture]
    public class BrowserInit
    {
        public static IWebDriver Driver;
        [SetUp]
        public void Initialize()
        {
            BrowserSetup.Initialize(EnumHelper.DriverOptions.Chrome);
            PageInitHelper<UrlNavigationHelper>.PageInit.GoToUrl();
            PageInitHelper<LoginPageHelper>.PageInit.NcLogin();
            PageInitHelper<UrlNavigationHelper>.PageInit.ValidateUrl();
        }

        [TearDown]
        public void TearDown()
        {
            PageInitHelper<BrowserSetup>.PageInit.Quit();
        }
    }
}