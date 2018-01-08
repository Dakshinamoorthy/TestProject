using System;
using System.Configuration;
using System.IO;
using System.Linq;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.ValidationPages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace NamecheapUITests.PageObject.HelperPages
{
    public class UrlNavigationHelper
    {
        public void GoToUrl(string urlPart = "")
        {
            Url = "";
            if (AppConfigHelper.MainUrl.Equals("sandbox"))
                Url = "https://www." + AppConfigHelper.MainUrl + ".namecheap.com/";
            else if (AppConfigHelper.MainUrl.Equals("live"))
                Url = string.IsNullOrEmpty(AppConfigHelper.CMSZone) ? "https://www.namecheap.com/" : "http://" + AppConfigHelper.CMSZone + "." + "www.namecheap.com/";
            else if (AppConfigHelper.MainUrl.Contains("bsb"))
                Url = "https://phx01" + AppConfigHelper.MainUrl + ".sb.corp.namecheap.net/";
            BrowserInit.Driver.Navigate().GoToUrl(Url);
        }
        public string UrlGenerator(string testCoverage = "", string urlPart = "", string cmsZones = "")
        {
            string suplimentryurlkey = null;
            string zonestype = null;
            if (!string.IsNullOrEmpty(cmsZones))
            {
                 zonestype = cmsZones.ToLower() + ".";
            }
            if (testCoverage != null && (testCoverage.IndexOf("AccountPanel", StringComparison.OrdinalIgnoreCase) >= 0 || testCoverage.IndexOf("AP", StringComparison.OrdinalIgnoreCase) >= 0))
            {
                suplimentryurlkey = "ap.";
            }
            Url = "";
            if (AppConfigHelper.MainUrl.Equals("sandbox"))
                Url = "https://" + zonestype + suplimentryurlkey + "www." + AppConfigHelper.MainUrl + ".namecheap.com/";
            else if (AppConfigHelper.MainUrl.Equals("live"))
                Url = "https://" + zonestype + suplimentryurlkey + "www.namecheap.com/";
            else if (AppConfigHelper.MainUrl.Contains("bsb"))
                Url = "https://" + zonestype + suplimentryurlkey + "phx01" + AppConfigHelper.MainUrl + ".sb.corp.namecheap.net/";
            Url = Url + urlPart;
            return Url;
        }
        public string Url { get; set; }

        public void ValidateUrl()
        {
            if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
            {
                BrowserInit.Driver.FindElement(By.CssSelector("header > nav > .logo>a")).Click();
                if (!BrowserInit.Driver.Url.Contains(AppConfigHelper.CMSZone))
                    GoToUrl();
                new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(60.00)).Until(driver1 => ((IJavaScriptExecutor)BrowserInit.Driver).ExecuteScript("return document.readyState").Equals("complete"));
            }
        }
    }
}