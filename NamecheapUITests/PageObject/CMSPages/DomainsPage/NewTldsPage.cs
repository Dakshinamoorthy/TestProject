using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.DomainsPageFactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
namespace NamecheapUITests.PageObject.CMSPages.DomainsPage
{
    public class NewTldsPage
    {
        internal List<SortedDictionary<string, string>> AddingDomainNamesToCart(List<string> newSld)
        {
            var newtldDomainInfoList = new List<SortedDictionary<string, string>>();
            Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.ElementIsAt(PageInitHelper<NewTldsPageFactory>.PageInit.ExploreTabLbl),
                "At New Tlds Page explore tab view is not visible at the top of the page");
            foreach (var newdomainSld in newSld)
            {
                Random:
                var listoftldCount = PageInitHelper<NewTldsPageFactory>.PageInit.ListofTlds.Count < 1;
                string domainName;
                decimal domainPrice;
                if (listoftldCount == false)
                {
                    var randomTldNumber =
                              PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(PageInitHelper<NewTldsPageFactory>.PageInit.ListofTlds.Count);
                    var tldele = PageInitHelper<NewTldsPageFactory>.PageInit.ListofTlds[randomTldNumber];
                    PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(tldele);
                    click:
                    tldele.FindElement(By.ClassName("search-domain-btn")).Click();
                    Thread.Sleep(1000);
                    if (tldele.GetAttribute(UiConstantHelper.AttributeClass).Contains(UiConstantHelper.Expanded))
                    {
                        tldele.FindElement(By.TagName("input")).Clear();
                        tldele.FindElement(By.TagName("input")).SendKeys(newdomainSld);
                        Func<IWebDriver, bool> testCondition = x => !tldele.FindElement(By.ClassName("fieldset")).GetAttribute(UiConstantHelper.AttributeClass).Contains("loading");
                        var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(3));
                        wait.Until(testCondition);
                        var actionMessage = tldele.FindElement(By.ClassName("message")).Text;
                        if (actionMessage.Contains("unavailable") || tldele.FindElement(By.XPath("//*[contains(@class,'preorder-btn register fromText')]")).GetAttribute(UiConstantHelper.AttributeClass).Contains(UiConstantHelper.Disabled))
                        {
                            goto Random;
                        }
                        domainName =
                            tldele.FindElement(By.TagName("input")).GetAttribute(UiConstantHelper.AttributeValue).Trim() + "." + tldele.FindElement(By.TagName("strong")).Text.Trim().ToLowerInvariant();
                        domainPrice = Convert.ToDecimal(Regex.Replace(tldele.FindElement(By.ClassName("price")).Text, @"[^\d..][^\w\s]*", "").Trim());
                        tldele.FindElement(By.XPath("//*[contains(@class,'preorder-btn register fromText')]")).Click();
                    }
                    else
                    {
                        goto click;
                    }
                }
                else
                {
                    Assert.IsTrue(PageInitHelper<NewTldsPageFactory>.PageInit.EmptyTldListTxt.Displayed,
                        "On new tlds landing page there is no tld list is available in this case warning message should be displayed in the grid is expected behavior, but the actual Result shown empty or no message are displayed");
                    throw new InconclusiveException("On new tlds landing page for user automatednc2 has no tlds listed in explorer tab grid to perform this function");
                }
                var newtldDic = new SortedDictionary<string, string>
                    {
                        {EnumHelper.DomainKeys.DomainName.ToString(), domainName.Trim()},
                        {
                            EnumHelper.DomainKeys.DomainPrice.ToString(),
                            domainPrice.ToString(CultureInfo.InvariantCulture).Trim()
                        },
                    };
                newtldDomainInfoList.Add(newtldDic);
            }
            return newtldDomainInfoList;
        }
    }
}