using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.Header;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.ValidationPages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
namespace NamecheapUITests.PageObject.CMSPages.Header
{
    public class SupportLinksPage
    {
        public void PerformSupportLinkValidations(string supportPage = "")
        {
            PageInitHelper<SupportLinksPageFactory>.PageInit.AllSupportLinks.ToList().ForEach(page =>
            {
                IWebElement supportLinkElement =
                    BrowserInit.Driver.FindElement(By.XPath("//li[normalize-space(.)='" + supportPage + "']"));
                if (!supportLinkElement.Text.ToLower().Trim().Contains(supportPage.ToLower().Trim())) return;
                supportLinkElement.Click();
                PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
                BrowserInit.Driver.Navigate().GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator());
                
            });
        }
        internal void PerformUserAccountValidations(string testAction)
        {
            int SupportPin, SecuritySupportPin;
            switch (EnumHelper.ParseEnum<EnumHelper.UserAccountKeys>(testAction))
            {
                case EnumHelper.UserAccountKeys.CustomerName:
                    BrowserInit.Driver.Navigate().GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "settings/personal-info"));
                    Thread.Sleep(2000);
                    PageInitHelper<SupportLinksPage>.PageInit.GoToHeader(testAction);
                    string CustName = PageInitHelper<SupportLinksPageFactory>.PageInit.CustomerName.Text.Trim();
                    string PersonalInfoCustName = PageInitHelper<SupportLinksPageFactory>.PageInit.PersonalInfoCustomerName.Text.Trim();
                    Assert.IsTrue(CustName.Equals(PersonalInfoCustName, StringComparison.InvariantCultureIgnoreCase),
                        "On user menu Customer name showns as " + CustName + ", but in personal info page Customer name shown as " + PersonalInfoCustName + " both the customer name should be equal");
                    break;
                case EnumHelper.UserAccountKeys.UserName:
                    BrowserInit.Driver.Navigate().GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "settings/personal-info"));
                    Thread.Sleep(2000);
                    PageInitHelper<SupportLinksPage>.PageInit.GoToHeader(testAction);
                    string UserName = PageInitHelper<SupportLinksPageFactory>.PageInit.UserName.Text.Trim();
                    string PersonalInfoUserName = PageInitHelper<SupportLinksPageFactory>.PageInit.PersonalInfoUserName.Text.Trim();
                    PageInitHelper<SupportLinksPage>.PageInit.GoToHeader(testAction);
                    Assert.IsTrue(UserName.Equals(PersonalInfoUserName, StringComparison.InvariantCultureIgnoreCase),
                        "On user menu User name showns as " + UserName + ", but in personal info page User name shown as " + PersonalInfoUserName + " both the User name should be equal");
                    break;
                case EnumHelper.UserAccountKeys.SupportPin:
                    BrowserInit.Driver.Navigate().GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "settings/personal-info"));
                    Thread.Sleep(2000);
                    PageInitHelper<SupportLinksPage>.PageInit.GoToHeader(testAction);
                    int.TryParse(PageInitHelper<SupportLinksPageFactory>.PageInit.SupportPin.Text.Trim(), out SupportPin);
                    BrowserInit.Driver.Navigate().GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "Profile/Security"));
                    int.TryParse(PageInitHelper<SupportLinksPageFactory>.PageInit.SecuritySupportPin.Text.Trim(), out SecuritySupportPin);
                    Assert.IsTrue(SupportPin.Equals(SecuritySupportPin),
                       "On user menu Support pin showns as " + Convert.ToInt32(SupportPin) + ", but in Security page Support Pin shown as " + Convert.ToInt32(SecuritySupportPin) + " both the Support Pins should be equal");
                    break;
                case EnumHelper.UserAccountKeys.Dashboard:
                    BrowserInit.Driver.Navigate().GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "settings/personal-info"));
                    Thread.Sleep(2000);
                    PageInitHelper<SupportLinksPage>.PageInit.GoToHeader(testAction);
                    Assert.IsTrue(PageInitHelper<SupportLinksPageFactory>.PageInit.DashboardLink.Displayed, "On user menu dropdown list Dashboardlink is not visible or missing");
                    PageInitHelper<SupportLinksPageFactory>.PageInit.DashboardLink.Click();
                    Thread.Sleep(2000);
                    Assert.IsTrue(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "dashboard").Equals(BrowserInit.Driver.Url), "The Page Redirect to some other page - " + BrowserInit.Driver.Url + " instead of " + PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "/dashboard"));
                    break;
                case EnumHelper.UserAccountKeys.Profile:
                    BrowserInit.Driver.Navigate().GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "settings/personal-info"));
                    Thread.Sleep(2000);
                    PageInitHelper<SupportLinksPage>.PageInit.GoToHeader(testAction);
                    Assert.IsTrue(PageInitHelper<SupportLinksPageFactory>.PageInit.ProfileLink.Displayed, "On user menu dropdown list Profilelink is not visible or missing");
                    PageInitHelper<SupportLinksPageFactory>.PageInit.ProfileLink.Click();
                    Thread.Sleep(2000);
                    Assert.IsTrue(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "settings/personal-info").Equals(BrowserInit.Driver.Url), "The Page Redirect to some other page - " + BrowserInit.Driver.Url + " instead of " + PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "/settings/personal-info"));
                    break;
                case EnumHelper.UserAccountKeys.Signout:
                    BrowserInit.Driver.Navigate().GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "settings/personal-info"));
                    Thread.Sleep(2000);
                    PageInitHelper<SupportLinksPage>.PageInit.GoToHeader(testAction);
                    Assert.IsTrue(PageInitHelper<SupportLinksPageFactory>.PageInit.SignoutLink.Displayed, "On user menu dropdown list Signoutlink is not visible or missing");
                    PageInitHelper<SupportLinksPageFactory>.PageInit.SignoutLink.Click();
                    Thread.Sleep(2000);
                    Assert.IsTrue(BrowserInit.Driver.Url.Contains(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("/myaccount/signout")), "The Page Redirect to some other page - " + BrowserInit.Driver.Url + " instead of " + PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("/myaccount/signout"));
                    PageInitHelper<LoginPageHelper>.PageInit.Login();
                    break;
            }
        }
        public void GoToHeader(string headerMenu)
        {
            string username = ConfigurationManager.AppSettings["UserName"];
            var headerLink = new Actions(BrowserInit.Driver);
            if (headerMenu.ToLower().Trim().Equals(UiConstantHelper.Support.ToLower().Trim()))
                headerLink.ClickAndHold(PageInitHelper<SupportLinksPageFactory>.PageInit.SupportLink).Build().Perform();
            else
                headerLink.ClickAndHold(BrowserInit.Driver.FindElement(By.LinkText(username))).Build().Perform();
        }
    }
}