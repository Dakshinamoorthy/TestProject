using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PagefactoryObject.CMSPageFactory.Header
{
    public class SupportLinksPageFactory
    {
        [FindsBy(How = How.XPath, Using = "//header/nav/*[contains(@class,'top-hat')]/div/ul[not(contains(@class,'commerce'))]/li[1]")]
        [CacheLookup]
        internal IWebElement SupportLink { get; set; }
        [FindsBy(How = How.XPath, Using = "//header/nav/*[contains(@class,'top-hat')]/div/ul[not(contains(@class,'commerce'))]/li[1]/div/ul/li")]
        [CacheLookup]
        internal IList<IWebElement> AllSupportLinks { get; set; }
        [FindsBy(How = How.XPath, Using = "//header/nav/*[contains(@class,'top-hat')]/div/ul[not(contains(@class,'commerce'))]/li[1]/div/ul")]
        [CacheLookup]
        internal IWebElement AccountLink { get; set; }
        
        [FindsBy(How = How.XPath, Using = ".//dl[1]/dd[1]")]   // .//*[contains(@class,'panel-body')]/div[1]/div[2]/small - Previous
        [CacheLookup]
        internal IWebElement CustomerName { get; set; } 
        
        [FindsBy(How = How.XPath, Using = ".//dl[1]/dd[2]")]     // .//*[contains(@class,'panel-body')]/div[2]/div[2]/small - Previous
        [CacheLookup]
        internal IWebElement UserName { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@nc-l10n='personalInfo.messages.name']/../following-sibling::div/p")]    
        [CacheLookup]
        internal IWebElement PersonalInfoCustomerName { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@nc-l10n='personalInfo.messages.username']/../following-sibling::div/ul/li[1]")]
        [CacheLookup]
        internal IWebElement PersonalInfoUserName { get; set; }
        
        [FindsBy(How = How.XPath, Using = ".//li[contains(@class,'gb-panel')]/a[contains(@href,'dashboard')]")]  // .//div[@class='gb-panel__body'][@data-reactid='33']//ul/li[1]/a/span - Previous
        [CacheLookup]
        internal IWebElement DashboardLink { get; set; }
        
        [FindsBy(How = How.XPath, Using = ".//li[contains(@class,'gb-panel')]/a[contains(@href,'Profile')]")]  // .//div[@class='gb-panel__body'][@data-reactid='33']//ul/li[2]/a/span - Previous
        [CacheLookup]
        internal IWebElement ProfileLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//li[contains(@class,'gb-panel')]/a[contains(@href,'signout')]")]  //  .//div[@class='gb-panel__body'][@data-reactid='33']//ul/li[3]/a/span - Previous
        [CacheLookup]
        internal IWebElement SignoutLink { get; set; }
        
        [FindsBy(How = How.XPath, Using = ".//dl[1]/dd[3]")]   // .//*[contains(@class,'panel-body')]/div[3]/div[2]/b - Previous
        [CacheLookup]
        internal IWebElement SupportPin { get; set; }
        
        [FindsBy(How = How.XPath, Using = ".//li[@class='list-group-item'][3]/div[1]/div[1]/div[1]/div[2]//li[1]/b")]      // //*[@id='maincontent']/div[4]/div[6]/div[2]/p[1]/strong - Previous
        [CacheLookup]
        internal IWebElement SecuritySupportPin { get; set; }
    }
}