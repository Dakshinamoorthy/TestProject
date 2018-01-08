using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PageObject.HelperPages.WrapperFactory
{
    public class PageInitHelper<TPi> where TPi : new()
    {
        public static TPi PageInit => GetPage();
        private static TPi GetPage()
        {
            var page = new TPi();
            PageFactory.InitElements(BrowserInit.Driver, page);
            return page;
        }
    }
}