using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NamecheapUITests.PagefactoryObject.CMSPageFactory.ChatLinksPageFactory
{
    public class ChatLinksPageFactory
    {
        [FindsBy(How = How.TagName, Using = "footer")]
        [CacheLookup]
        internal IWebElement Footer { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(@class,'promo-chat')]//a[contains(@class,'btn')]")]
        [CacheLookup]
        internal IWebElement HomePageLiveChatLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(@class,'tab-view')]/div[contains(@class,'tab-list')]/ul/li")]
        [CacheLookup]
        internal IList<IWebElement> PageLiveChatLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(@class,'tab-view')]/div[contains(@class,'tab-list')]/ul/li")]
        [CacheLookup]
        internal IList<IWebElement> PageLiveChatLinks { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='DefaultLiveChat']")]
        [CacheLookup]
        internal IWebElement PageChatLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*/div[contains(@class,'promo-chat')]//p[1]")]
        [CacheLookup]
        internal IWebElement ViewSuportPin { get; set; }

        //[FindsBy(How = How.XPath, Using = "//*[@id='chatform']/table[1]/tbody/tr[1]/td[2]/select")]
        [FindsBy(How = How.XPath, Using = "//table/tbody/tr[1]/td[2]/select")]
        [CacheLookup]
        internal IWebElement SelectDept { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='header']/nav/div[4]/div/ul[1]/li[4]")]
        [CacheLookup]
        internal IWebElement TopNavPanelUsername { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(@class,'user-menu')]//p[contains(@class,'pin')]")]
        [CacheLookup]
        internal IWebElement TopNavPanelUserSupportPin { get; set; }

        [FindsBy(How = How.XPath, Using = "//select[@class='swiftselect']/option")]
        [CacheLookup]
        internal IList<IWebElement> Options { get; set; }
        
    }
}
