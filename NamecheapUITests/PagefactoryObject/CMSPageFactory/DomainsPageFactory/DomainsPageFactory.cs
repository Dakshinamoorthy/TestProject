using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PagefactoryObject.CMSPageFactory.DomainsPageFactory
{
    public class DomainsPageFactory
    {
        [FindsBy(How = How.Id,
          Using = "ctl00_ctl00_ctl00_ctl00_base_content_web_base_content_home_content_page_content_left_ctl00_inputSingleDomain",
          Priority = 0)]
        [FindsBy(How = How.XPath, Using = ".//*[@class='form-control handlereturn style__formInput___2t6Rt']",
          Priority = 1)]
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'multiple-domain-search')]", Priority = 2)]
        [CacheLookup]
        internal IWebElement DomainSearchTxt { get; set; }
        [FindsBy(How = How.Name, Using = "ctl00$ctl00$ctl00$ctl00$base_content$web_base_content$home_content$page_content_left$ctl00$searchButton$actionObjectPlaceHolder", Priority = 0)]
        [FindsBy(How = How.XPath, Using = ".//*[@type='submit']", Priority = 1)]
        [CacheLookup]
        internal IWebElement DomainSearchBtn { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@data-gaevent-action),' '),'Bulk search')]")]
        [CacheLookup]
        internal IWebElement BulkDomainSearchBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[contains(@class,'btn-adv-search')])[1]", Priority = 0)]
        [FindsBy(How = How.LinkText, Using = "Bulk Options", Priority = 1)]
        [CacheLookup]
        internal IWebElement BulkOptionLnkTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//label[.='I entered fully qualified domain names']")]
        [CacheLookup]
        internal IWebElement QualifieddomainnamesRdo { get; set; }
        [FindsBy(How = How.ClassName, Using = "info")]
        [CacheLookup]
        internal IWebElement AvailableDomainInfo { get; set; }
        [FindsBy(How = How.XPath,
             Using = ".//*/h2[contains(concat(' ',normalize-space(@class),' '),'domain-name')]/span")]
        [CacheLookup]
        internal IWebElement SuggestedDomainHeading { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'embedDomainSearchResults')]/div[contains(concat(' ',normalize-space(@class),' '),'domain-result')]/div/span[1]"
             )]
        [CacheLookup]
        internal IWebElement SuggestedDomainVisible { get; set; }
        [FindsBy(How = How.XPath,
            Using =
                ".//*[contains(concat(' ',normalize-space(@class),' '),'embedDomainSearchResults')]/div[contains(concat(' ',normalize-space(@class),' '),'domain-result')]/div/span[1][not(@style = '')]/following::a[1]"
            )]
        [CacheLookup]
        internal IWebElement SuggestedDomainAddcartbtn { get; set; }
        [FindsBy(How = How.XPath,
             Using =
                 ".//*[contains(concat(' ',normalize-space(@role),' '),'tabpanel')]/div/div//li[not(contains(concat(' ',normalize-space(@class),' '), 'unavailable make-offer')) and not(contains(concat(' ',normalize-space(@style),' '),'display: none;'))and ./div/span]/div/span[1]"
             )]
        [CacheLookup]
        internal IList<IWebElement> SuggestedDomainsList { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'multiple-domain-search')]")]
        [CacheLookup]
        internal IWebElement MultipleDomainSearchInputTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@class='tab-content']//li")]
        [CacheLookup]
        internal IList<IWebElement> PopularDomainGridlst { get; set; }
        [FindsBy(How = How.XPath, Using = ".//div[contains(@class,'Result')]//p[@class='info']")]
        [CacheLookup]
        internal IWebElement RegisterInfoTxt { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[@class='reg-price']/following-sibling::*)[1]")]
        [CacheLookup]
        internal IWebElement DomainSearchAddtoCartBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[contains(concat(' ',normalize-space(@role),' '),'tabpanel')]/div/div//li/span[last()])")]
        [CacheLookup]
        internal IList<IWebElement> BulkPopularDomainGridlst { get; set; }
    }
}