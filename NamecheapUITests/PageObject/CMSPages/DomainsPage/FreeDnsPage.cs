using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.DomainsPageFactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NUnit.Framework;
using OpenQA.Selenium;
namespace NamecheapUITests.PageObject.CMSPages.DomainsPage
{
    public class FreeDnsPage
    {
        internal List<SortedDictionary<string, string>> AddingfreeDnsDomainNamesToCartAndPurchase(string purchasingDomainFor,
            List<string> newFreeDnsDomainNames)
        {
            var domainInfoList = new List<SortedDictionary<string, string>>();
            if (purchasingDomainFor.Contains(UiConstantHelper.BulkDomains))
            {
                PageInitHelper<FreeDnspageFactory>.PageInit.BulkDomainSearchTxtLink.Click();
                PageInitHelper<FreeDnspageFactory>.PageInit.BulkDomainSearchField.Clear();
                foreach (var freeDnsDomain in newFreeDnsDomainNames)
                {
                    PageInitHelper<FreeDnspageFactory>.PageInit.BulkDomainSearchField.SendKeys(freeDnsDomain);
                    PageInitHelper<FreeDnspageFactory>.PageInit.BulkDomainSearchField.SendKeys(Keys.Enter);
                }
                PageInitHelper<FreeDnspageFactory>.PageInit.BulkDomainSearchBtn.Click();
                Thread.Sleep(3000);
                PageInitHelper<PageNavigationHelper>.PageInit.WaitForPageLoad();
                var dnscount = BrowserInit.Driver.FindElements(By.XPath("(.//*/li[contains(@class,'list-group-item')]//*[contains(@class,'btn btn')])")).Count;
                for (var dcount = 1; dcount <= dnscount; dcount++)
                {
                    var dcount1 = dcount;
                    var ele = BrowserInit.Driver.FindElement(By.XPath("(.//*/li[contains(@class,'list-group-item')]//*[contains(@class,'btn btn')])[" + dcount1 + "]"));
                    PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(ele);
                    ele.Click();
                    var newDomain =
                     BrowserInit.Driver.FindElement(
                         By.XPath("(.//*/li[contains(@class,'list-group-item')]//p[contains(@class,'strong')])[" + dcount1 + "]")).Text;
                    var domainInfoDic = PageInitHelper<FreeDnsPage>.PageInit.AddDomainInfoToDic(newDomain);
                    domainInfoList.Add(domainInfoDic);
                }
            }
            else
            {
                foreach (var freeDnsDomain in newFreeDnsDomainNames)
                {
                    PageInitHelper<FreeDnspageFactory>.PageInit.DomainSearch.Clear();
                    PageInitHelper<FreeDnspageFactory>.PageInit.DomainSearch.SendKeys(freeDnsDomain);
                    PageInitHelper<FreeDnspageFactory>.PageInit.SearchBtn.Click();
                    PageInitHelper<PageNavigationHelper>.PageInit.WaitForPageLoad();
                    Assert.IsTrue(PageInitHelper<FreeDnspageFactory>.PageInit.FreeDnsTransferResults.Displayed, "In freedns seach result page 'Eligible for FreeDNS' result grid is not displayed");
                    var dlist = PageInitHelper<FreeDnspageFactory>.PageInit.FreeDnsTransferResults;
                    var domainList = dlist.FindElements(By.TagName("li"));
                    foreach (var dName in domainList.Select(domain => domain.FindElement(By.XPath("//p[contains(@class,'strong')]")).Text.Trim()))
                    {
                        Assert.IsTrue(dName.Equals(freeDnsDomain), "Given Domain name is differ from the resulted Domain name");
                        var dnsPrice = BrowserInit.Driver.FindElement(By.XPath(".//*/p[normalize-space(.)='" + dName + "']/../..//span[@nc-l10n='result.itemType']")).Text;
                        BrowserInit.Driver.FindElement(By.XPath(".//*/p[normalize-space(.)='" + dName + "']/../..//button")).Click();
                        var dicFreeDnsDic = new SortedDictionary<string, string>
                        {
                            {EnumHelper.DomainKeys.DomainName.ToString(), dName},
                            {"FreeDNSPrice", dnsPrice.Equals("FREE") ? "0.00" : dnsPrice}
                        };
                        domainInfoList.Add(dicFreeDnsDic);
                    }
                }
            }
            return domainInfoList;
        }
        internal SortedDictionary<string, string> AddDomainInfoToDic(string newDomain)
        {
            var domainDictionary = new SortedDictionary<string, string>
            {
                {EnumHelper.DomainKeys.DomainName.ToString(), newDomain}
            };
            var domainInfoXpath = ".//*/p[normalize-space(.)='" + newDomain + "']/../..//span[@nc-l10n='result.itemType']";
            var domainInfo = BrowserInit.Driver.FindElement(By.XPath(domainInfoXpath));
            var price = domainInfo.Text;
            if (!price.Equals("FREE", StringComparison.InvariantCultureIgnoreCase)) return domainDictionary;
            var domainprice = 0.00M;
            domainDictionary.Add(EnumHelper.DomainKeys.DomainPrice.ToString(),
                domainprice.ToString(CultureInfo.InvariantCulture));
            return domainDictionary;
        }
        internal void ManageDomain(List<SortedDictionary<string, string>> mergedSearchdDomainAndCartWidgetList)
        {
            foreach (var domainName in mergedSearchdDomainAndCartWidgetList.Select(dicDomainName => dicDomainName[EnumHelper.DomainKeys.DomainName.ToString()]))
            {
                PageInitHelper<FreeDnspageFactory>.PageInit.ManageDomainLnk.Click();
                Thread.Sleep(2000);
                Assert.IsTrue(BrowserInit.Driver.Url.Contains(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP",
                    "/domains/domaincontrolpanel/" + domainName + "/domain")),
                    "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " +
                    PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP",
                        "/domains/domaincontrolpanel/" + domainName + "/domain"));
                Assert.IsTrue(BrowserInit.Driver.FindElement(By.XPath(".//*/h1/strong")).Text.Trim().Equals(domainName.Trim()), "The FreeDns domain name mismatch in domains detail page expected domain name as " + domainName + ", but actual shown as " + BrowserInit.Driver.FindElement(By.XPath(".//*/h1/strong")).Text.Trim());
                Thread.Sleep(3000);
                var domainstatus =
                    BrowserInit.Driver.FindElement(By.XPath(".//*[@id='domain']/div/div[3]/div[1]/div/div/div[2]/p"))
                        .Text.Trim();
                Assert.IsTrue(domainstatus.Equals("Active", StringComparison.InvariantCultureIgnoreCase), "The domain status in the domain detail page should be 'Active' for the domain name - " + domainName + ", but status shown in domain list page as " + domainstatus);
                var validaity =
                    BrowserInit.Driver.FindElement(By.XPath(".//*[@id='domain']/div/div[3]/div[1]/div/div/div[3]/div/p"))
                        .Text.Trim();
                var splitString = validaity.Split('-');
                var fromDate = Convert.ToDateTime(DateTime.Parse(splitString[0])).ToString("MMM d, yyyy");
                var toDate = Convert.ToDateTime(DateTime.Parse(splitString[1])).ToString("MMM d, yyyy");
                var actualdate = fromDate + " - " + toDate;
                var validatitydate = DateTime.Now.ToString("MMM d, yyyy") + " - " + DateTime.Now.AddYears(+1).ToString("MMM d, yyyy").Trim();
                Assert.IsTrue(actualdate.Equals(validatitydate), "In Domain detail page freeDns domain validity mismatch for the domain name " + domainName + " expected validity date should be " + validatitydate + ", but actual date is shown in domain detail page as  " + actualdate);
            }
        }
    }
}