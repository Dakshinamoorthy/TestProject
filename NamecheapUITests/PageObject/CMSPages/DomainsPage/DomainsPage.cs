using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.DomainsPageFactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.ValidationPages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Keys = OpenQA.Selenium.Keys;

namespace NamecheapUITests.PageObject.CMSPages.DomainsPage
{
    public class DomainsPage
    {
        internal List<SortedDictionary<string, string>> AddingDomainNamesToCart(string purchasingDomainFor, List<string> newDomainNames)
        {
            var domainInfoList = new List<SortedDictionary<string, string>>();
            if (purchasingDomainFor.Contains(UiConstantHelper.BulkDomains))
            {
                PageInitHelper<DomainsPageFactory>.PageInit.BulkOptionLnkTxt.Click();
                PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
                PageInitHelper<DomainsPageFactory>.PageInit.QualifieddomainnamesRdo.Click();
                PageInitHelper<DomainsPageFactory>.PageInit.MultipleDomainSearchInputTxt.Clear();
                foreach (var newDomain in newDomainNames)
                {
                    check:
                    var yesorno = PageInitHelper<DomainsPageFactory>.PageInit.QualifieddomainnamesRdo.Enabled;
                    if (yesorno)
                    {
                        PageInitHelper<DomainsPageFactory>.PageInit.MultipleDomainSearchInputTxt.SendKeys(newDomain);
                        PageInitHelper<DomainsPageFactory>.PageInit.MultipleDomainSearchInputTxt.SendKeys(Keys.Enter);
                    }
                    else
                    {
                        PageInitHelper<DomainsPageFactory>.PageInit.QualifieddomainnamesRdo.Click();
                        goto check;
                    }
                }
                PageInitHelper<DomainsPageFactory>.PageInit.BulkDomainSearchBtn.Click();
                Thread.Sleep(3000);
                PageInitHelper<PageNavigationHelper>.PageInit.WaitForPageLoad();
                var domaincount = PageInitHelper<DomainsPageFactory>.PageInit.BulkPopularDomainGridlst.Count;
                for (var dcount = 0; dcount < domaincount; dcount++)
                {
                    var dcount1 = dcount + 1;
                    Func<IWebDriver, bool> testCondition = x => BrowserInit.Driver.FindElement(By.XPath("(.//*[@class='tab-content']//li)[" + dcount1 + "]/span[1]")).Displayed;
                    var wait11 = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(3));
                    wait11.Until(testCondition);
                    if (BrowserInit.Driver.FindElement(By.XPath("(.//*[@class='tab-content']//li)[" + dcount1 + "]")).GetAttribute(UiConstantHelper.AttributeClass).Contains("unavailable make-offer"))
                        continue;
                    var ele = BrowserInit.Driver.FindElement(By.XPath("(.//*[@class='tab-content']//li)[" + dcount1 + "]/div/following-sibling::span[@class= 'reg-price']/following-sibling::*"));
                    PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(ele);
                    ele.Click();
                    testCondition = x =>
                          BrowserInit.Driver.FindElement
                              (By.XPath("(.//*[@class='tab-content']//li)[" + dcount1 +
                                        "]/div/following-sibling::span[@class= 'reg-price']/following-sibling::*"))
                              .GetCssValue(UiConstantHelper.LocatorBackgroundTxt).Contains(UiConstantHelper.AddedcartImg);
                    var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(80));
                    wait.Until(testCondition);
                    Thread.Sleep(2000);
                    var newDomain =
                       BrowserInit.Driver.FindElement(
                           By.XPath("(.//*[@class='tab-content']//li)[" + dcount1 + "]//span[@class='domain-name']")).Text;
                    var cartImgUrl = BrowserInit.Driver.FindElement(By.XPath("(.//*[@class='tab-content']//li)[" + dcount1 + "]/div/following-sibling::span[@class= 'reg-price']/following-sibling::*")).GetCssValue(UiConstantHelper.LocatorBackgroundTxt);
                    var domainName =
                        BrowserInit.Driver.FindElement(
                            By.XPath("(.//*[@class='tab-content']//li)[" + dcount1 + "]/div/span")).Text;
                    Assert.IsTrue(cartImgUrl.Contains(UiConstantHelper.AddedcartImg), domainName + " - Domain is not added to cart/time extends 1 mins to add domains to cart");
                    var domainInfoDic = PageInitHelper<DomainsPage>.PageInit.AddDomainInfoToDic(newDomain);
                    domainInfoList.Add(domainInfoDic);
                }
            }
            else
            {
                DomainAvailablityCheck:
                foreach (var newDomain in newDomainNames)
                {
                    PageInitHelper<DomainsPageFactory>.PageInit.DomainSearchTxt.Clear();
                    PageInitHelper<DomainsPageFactory>.PageInit.DomainSearchTxt.SendKeys(newDomain);
                    PageInitHelper<DomainsPageFactory>.PageInit.DomainSearchBtn.Click();
                    Func<IWebDriver, bool> testCondition = x =>
                          PageInitHelper<DomainsPageFactory>.PageInit.RegisterInfoTxt.Displayed;
                    var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(120));
                    wait.Until(testCondition);
                    Thread.Sleep(600);
                    if (purchasingDomainFor.Contains("Premium") && !purchasingDomainFor.Contains("PremiumDNS"))
                    {
                        Assert.IsTrue(PageInitHelper<DomainsPageFactory>.PageInit.RegisterInfoTxt.Text.Contains("premium domain"), newDomain + " Is not an " + purchasingDomainFor);
                    }
                    if (PageInitHelper<DomainsPageFactory>.PageInit.AvailableDomainInfo.Text.Contains(
                        UiConstantHelper.DomainAvailable)
                        && PageInitHelper<DomainsPageFactory>.PageInit.SuggestedDomainAddcartbtn.Text.Equals(
                            UiConstantHelper.Carttext) &&
                        !purchasingDomainFor.Contains(UiConstantHelper.BannedDomainName))
                    {
                        Assert.IsTrue(
                            PageInitHelper<PageValidationHelper>.PageInit.ElementIsAt(
                                PageInitHelper<DomainsPageFactory>.PageInit.SuggestedDomainHeading), newDomain + "  - domain name heading is missing at the top of the search box ");
                        PageInitHelper<PageValidationHelper>.PageInit.ElementIsAt(
                            PageInitHelper<DomainsPageFactory>.PageInit.SuggestedDomainVisible);
                        PageInitHelper<DomainsPageFactory>.PageInit.SuggestedDomainAddcartbtn.Click();
                        testCondition = x => !(PageInitHelper<DomainsPageFactory>.PageInit.SuggestedDomainAddcartbtn.GetAttribute(UiConstantHelper.AttributeClass).Contains("loading"));
                        wait.Until(testCondition);
                        Label:
                        if (!PageInitHelper<DomainsPageFactory>.PageInit.DomainSearchAddtoCartBtn.GetCssValue(UiConstantHelper.LocatorBackgroundTxt).Contains(UiConstantHelper.AddedcartImg))
                        {
                            Thread.Sleep(100);
                            goto Label;
                        }
                        Thread.Sleep(2000);
                        PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
                        var domainInfoList1 = PageInitHelper<DomainsPage>.PageInit.AddDomainInfoToDic(newDomain);
                        domainInfoList.Add(domainInfoList1);
                    }
                    else
                    {
                        if (purchasingDomainFor.Contains(UiConstantHelper.ReDomains))
                        {
                            newDomainNames.Clear();
                            newDomainNames = PageInitHelper<RandomDomainNameGenerator>.PageInit.DomainName(UiConstantHelper.ReDomains);
                            goto DomainAvailablityCheck;
                        }
                        if (purchasingDomainFor.Contains(UiConstantHelper.EnomDomains))
                        {
                            newDomainNames.Clear();
                            newDomainNames = PageInitHelper<RandomDomainNameGenerator>.PageInit.DomainName(UiConstantHelper.EnomDomains);
                            goto DomainAvailablityCheck;
                        }
                        if (purchasingDomainFor.Contains(UiConstantHelper.BannedDomainName))
                        {
                            PageInitHelper<DomainsPage>.PageInit.BannedDomains(newDomain);
                            return null;
                        }
                        if (purchasingDomainFor.Contains("Premium") && !purchasingDomainFor.Contains("PremiumDNS"))
                        {
                            throw new InconclusiveException(newDomain +
                                                            "This premium domain is unavailable to purchase in " +
                                                            AppConfigHelper.MainUrl);
                        }
                        var popularDomainListCount = PageInitHelper<DomainsPageFactory>.PageInit.SuggestedDomainsList.Count;
                        PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(popularDomainListCount);
                        newDomainNames.Clear();
                        newDomainNames = new List<string>
                        {
                            PageInitHelper<DomainsPageFactory>.PageInit.SuggestedDomainsList[PageValidationHelper.Number].Text
                        };
                        goto DomainAvailablityCheck;
                    }
                }
            }
            return domainInfoList;
        }
       internal List<string> BannedDomains(string newDomain)
        {
            Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.ElementIsAt(PageInitHelper<DomainsPageFactory>.PageInit.SuggestedDomainHeading), newDomain + " - domain name heading is missing at the top of the search box ");
            for (var index = 1; index < PageInitHelper<DomainsPageFactory>.PageInit.PopularDomainGridlst.Count; index++)
            {
                var liXpath = "(.//*[contains(concat(' ',normalize-space(@role),' '),'tabpanel')]/div/div//li";
                var xpathclass = liXpath + "/span[last()])[" + index + "]";
                var value = BrowserInit.Driver.FindElement(By.XPath(xpathclass));
                var domainName = liXpath + "/div[1]/span[@class='domain-name'])[" + index + "]";
                var spanClass = value.GetAttribute(UiConstantHelper.AttributeClass);
                if (spanClass.Contains("reg-price"))
                {
                    var xpath = xpathclass + "/following-sibling::a";
                    
                    BrowserInit.Driver.FindElement(By.XPath(xpath)).Click();
                    Thread.Sleep(2000);
                    Func<IWebDriver, bool> testCondition = x =>
           !BrowserInit.Driver.FindElement(By.XPath(xpathclass))
                                  .GetAttribute(UiConstantHelper.AttributeClass).Contains("loading");
                    var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(3));
                    wait.Until(testCondition);
                    Assert.IsTrue(BrowserInit.Driver.FindElement(By.XPath(xpathclass)).GetAttribute(UiConstantHelper.AttributeClass).Contains("contactSupport"), BrowserInit.Driver.FindElement(By.XPath(domainName)).Text.Trim() + "  - This domain is associated with banned domain category, but seems it is adding to cart");
                }
                else
                {
                    Assert.IsTrue(spanClass.Contains(UiConstantHelper.Disabled), BrowserInit.Driver.FindElement(By.XPath(domainName)).Text.Trim() + "  - This domain is associated with banned domain category, but seems it is adding to cart");
                }
            }
            return null;
        }
        internal SortedDictionary<string, string> AddDomainInfoToDic(string newDomain)
        {
            var domainDictionary = new SortedDictionary<string, string>
            {
                {EnumHelper.DomainKeys.DomainName.ToString(), newDomain}
            };
            var domainInfoXpath = "(//*[@data-attr-domain-id='" + newDomain + "'])[1]";
            var domainInfo = BrowserInit.Driver.FindElement(By.XPath(domainInfoXpath));
            var price = domainInfo.FindElement(By.ClassName("price")).Text;
            var priceDuration = Regex.Split(price, "/");
            var domainPrice = Convert.ToDecimal(Regex.Replace(priceDuration[0], @"[^\d..][^\w\s]*", "").Trim());
            domainDictionary.Add(EnumHelper.DomainKeys.DomainPrice.ToString(), domainPrice.ToString(CultureInfo.InvariantCulture));
            domainDictionary.Add(EnumHelper.DomainKeys.DomainDuration.ToString(), "1 " + priceDuration[1]);
            const decimal regprice0 = 0.00M;
            var regPrice = Regex.Replace(domainInfo.FindElement(By.ClassName("reg-price")).Text, @"[^\d..][^\w\s]*", "").Trim();
            domainDictionary.Add(EnumHelper.DomainKeys.DomainRetailPrice.ToString(), regPrice.ToString(CultureInfo.InvariantCulture).Equals(string.Empty) ? regprice0.ToString(CultureInfo.InvariantCulture) : Convert.ToDecimal(regPrice).ToString(CultureInfo.InvariantCulture));
            if (!domainInfo.GetAttribute("class")
                .Contains("has-promo")) return domainDictionary;
            var promocode = BrowserInit.Driver.FindElement(By.XPath("(.//li[@data-attr-domain-id='" + newDomain + "'])//strong")).Text.Trim();
            if (!(promocode.Trim().Contains("NEW TLD!") || promocode.Trim().Contains("NEW")))
            {
                domainDictionary.Add(EnumHelper.DomainKeys.DomainNamePromotionCode.ToString(), promocode);
            }
            return domainDictionary;
        }
    }
}