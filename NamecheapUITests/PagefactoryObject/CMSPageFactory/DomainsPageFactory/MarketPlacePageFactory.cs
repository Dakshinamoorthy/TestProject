using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NamecheapUITests.PagefactoryObject.CMSPageFactory.DomainsPageFactory
{
    public class MarketPlacePageFactory
    {
        [FindsBy(How = How.XPath, Using = "(//mp-list//nc-grid[contains(@class,'nc-grid')]/div/nc-grid-table//table/tbody/tr/td)[1]")]
        [CacheLookup]
        internal IWebElement DomainItemsTxt { get; set; }

        [FindsBy(How = How.XPath, Using = "//mp-list//nc-grid-pager//li[not(@class='first')]")]
        [CacheLookup]
        internal IList<IWebElement> TotalPageNumberCountLst { get; set; }

        [FindsBy(How = How.XPath, Using = "(//mp-list//nc-grid[contains(@class,'nc-grid')]/div/nc-grid-table//table/tbody/tr/td/a[contains(@class,'link')])")]
        [CacheLookup]
        internal IList<IWebElement> DomainsNamesLst { get; set; }

        [FindsBy(How = How.Name, Using = "searchKeyword")]
        [CacheLookup]
        internal IWebElement DomainNameSearchInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[contains(concat(' ',normalize-space(@class),' '),'btn-hero-search')]")]
        [CacheLookup]
        internal IWebElement DomainNameSearchBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@id,'errorDisplayParentDiv')]//p")]
        [CacheLookup]
        internal IWebElement SuccessMessageTxt { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='filter1']/li[1]/a")]
        [CacheLookup]
        internal IWebElement CategoriesMoreLink { get; set; }

        [FindsBy(How = How.XPath, Using = "//marketplace-categories/div[contains(@class,'modal')]")]
        [CacheLookup]
        internal IWebElement SelectCategoriesModalpopup { get; set; }

        [FindsBy(How = How.Id, Using = "filter1")]
        [CacheLookup]
        internal IWebElement RefineSearchGrid { get; set; }

        [FindsBy(How = How.XPath, Using = ".//div[contains(@class,'form-group')]/div/a")]
        [CacheLookup]
        internal IWebElement UncheckAllLnk { get; set; }

        [FindsBy(How = How.XPath, Using = ".//div[contains(@class,'form-group')]/div/div")]
        [CacheLookup]
        internal IList<IWebElement> ModalCategoryList { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'modal-body')]/div/div/div/input[@type='submit']")]
        [CacheLookup]
        internal IWebElement DoneBtn { get; set; }


        [FindsBy(How = How.XPath, Using = "//div[contains(concat(' ',normalize-space(@class),' '),'module marketplace')]/ul/li/div/strong/a")]
        [CacheLookup]
        internal IList<IWebElement> DomainNameLst { get; set; }

        [FindsBy(How = How.XPath, Using = ".//div[contains(@class,'listing-details')]//h2[contains(@class,'h3')]")]
        [CacheLookup]
        internal IWebElement DNameinDomainListing { get; set; }

        [FindsBy(How = How.XPath, Using = "(.//div[contains(@class,'listing-details')]//ul/li[1]/strong/a[not(.='')])")]
        [CacheLookup]
        internal IList<IWebElement> CategoriesNamesWebElement { get; set; }

        [FindsBy(How = How.XPath, Using = "//nc-grid-table/div/table")]
        [CacheLookup]
        internal IWebElement TableLoading { get; set; }

    }
}