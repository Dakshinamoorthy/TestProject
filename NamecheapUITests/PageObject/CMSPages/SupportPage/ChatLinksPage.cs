using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using MbUnit.Framework;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.ChatLinksPageFactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace NamecheapUITests.PageObject.CMSPages.ChatLinksPage
{
    class ChatLinksPage
    {
        public void CheckHomePageLiveChat(string page)
        {
            string supportPin;
            if (!BrowserInit.Driver.Title.Contains(UiConstantHelper.LiveChat))
            {
                PageInitHelper<UrlNavigationHelper>.PageInit.GoToUrl();
                PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(PageInitHelper<ChatLinksPageFactory>.PageInit.HomePageLiveChatLink);
                PageInitHelper<ChatLinksPageFactory>.PageInit.HomePageLiveChatLink.Click();
                var popupWindow = BrowserInit.Driver.WindowHandles.Count;
                if (popupWindow > 1)
                    BrowserInit.Driver.SwitchTo().Window(BrowserInit.Driver.WindowHandles[0]).Close();
                BrowserInit.Driver.SwitchTo().Window(BrowserInit.Driver.WindowHandles.Last());
            }
            Thread.Sleep(3000);
            if (!PageInitHelper<ChatLinksPageFactory>.PageInit.ViewSuportPin.Text.Contains(UiConstantHelper.Login))
            {
                PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(PageInitHelper<ChatLinksPageFactory>.PageInit.TopNavPanelUsername);
                supportPin = PageInitHelper<ChatLinksPageFactory>.PageInit.TopNavPanelUserSupportPin.Text;
                Assert.IsTrue(PageInitHelper<ChatLinksPageFactory>.PageInit.ViewSuportPin.Text.Contains(supportPin),"Mis Match Support Pin");
            }

            string pageTabXpath = ".//*[contains(@class,'tab-view')]/div[contains(@class,'tab-list')]/ul/li";
            for (int i = 1; i <= BrowserInit.Driver.FindElements(By.XPath(pageTabXpath)).Count; i++)
            {
                var pageChatLink = BrowserInit.Driver.FindElement(By.XPath(pageTabXpath + "[" + i + "]"));
                string pageChatLinkClass = pageChatLink.GetAttribute(UiConstantHelper.AttributeClass);

                if (!pageChatLink.Text.Contains(page))
                    continue;
                if (!pageChatLinkClass.Contains(UiConstantHelper.Selected))
                {
                    var pageLink = pageChatLink.FindElement(By.TagName("a"));
                    pageLink.Click();
                    break;
                }
            }  
            Thread.Sleep(3000);
            PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(PageInitHelper<ChatLinksPageFactory>.PageInit.PageChatLink);
            string parentWindowHandler = BrowserInit.Driver.CurrentWindowHandle;
            string subWindowHandler = null;
            PageInitHelper<ChatLinksPageFactory>.PageInit.PageChatLink.Click();
            IWait<IWebDriver> wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(30.00));
            wait.Until(driver1 => ((IJavaScriptExecutor)BrowserInit.Driver).ExecuteScript("return document.readyState").Equals("complete"));
            ReadOnlyCollection<string> handles = BrowserInit.Driver.WindowHandles; // get all window handles
            IEnumerator<string> iterator = handles.GetEnumerator();
            while (iterator.MoveNext())
            {
                subWindowHandler = iterator.Current;
            }
            BrowserInit.Driver.SwitchTo().Window(subWindowHandler);
            PageInitHelper<ChatLinksPageFactory>.PageInit.SelectDept.Click();
            if (PageInitHelper<ChatLinksPageFactory>.PageInit.Options.Count > 1)
            {
                for (int i = 1; i <= PageInitHelper<ChatLinksPageFactory>.PageInit.Options.Count; i++)
                {
                    BrowserInit.Driver.FindElement(By.XPath("//select[@class='swiftselect']/option[" + i + "]")).Click();
                    wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(30.00));
                    wait.Until(driver1 => ((IJavaScriptExecutor)BrowserInit.Driver).ExecuteScript("return document.readyState").Equals("complete"));
                    PageInitHelper<ChatLinksPageFactory>.PageInit.SelectDept.Click();
                }
            }
            else
                Assert.IsTrue(PageInitHelper<ChatLinksPageFactory>.PageInit.SelectDept.Text.Contains(page),"Invalid Page");
            BrowserInit.Driver.SwitchTo().Window(subWindowHandler).Close();
            BrowserInit.Driver.SwitchTo().Window(parentWindowHandler);
            if (BrowserInit.Driver.Title.Contains(UiConstantHelper.LiveChat) || BrowserInit.Driver.Title.Contains(UiConstantHelper.Namecheap))
                Assert.Contains(BrowserInit.Driver.Url, UiConstantHelper.LiveChat.Replace(" ", "-").ToLower());
        }
    }
}
