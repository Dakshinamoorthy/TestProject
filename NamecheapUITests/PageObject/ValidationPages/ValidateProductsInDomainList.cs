using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
namespace NamecheapUITests.PageObject.ValidationPages
{
    public class ValidateProductsInDomainList : IDomainListValidation
    {
        void IDomainListValidation.DomainListValidation(List<SortedDictionary<string, string>> mergedScAndCartWidgetListWithOrderNum, string viewTab)
        {
            BrowserInit.Driver.Navigate().GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "Domains/DomainList"));
            //Thread.Sleep(3000);
            Func<IWebDriver, bool> tablesearchtestCondition = x => !PageInitHelper<ValidateProductsInDomainList>.PageInit.DomainListFirstGrid.GetAttribute(UiConstantHelper.AttributeClass).Equals("async-processing");
            IWait<IWebDriver> wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(100.00));
            wait.Until(tablesearchtestCondition);
            Thread.Sleep(3000);
            if (viewTab.Trim().Equals(UiConstantHelper.AllProducts.Trim()))
            {
                PageInitHelper<ValidateProductsInDomainList>.PageInit.AllProductDdl.Click();
                PageInitHelper<ValidateProductsInDomainList>.PageInit.AllProductLst.Click();
                Thread.Sleep(500);
                tablesearchtestCondition = x => !PageInitHelper<ValidateProductsInDomainList>.PageInit.DomainListFirstGrid.GetAttribute(UiConstantHelper.AttributeClass).Equals("async-processing");
                wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(100.00));
                wait.Until(tablesearchtestCondition);
            }
            var domainmissing = new StringBuilder();
            var whoismissing = new StringBuilder();
            foreach (var purchasedDict in mergedScAndCartWidgetListWithOrderNum)
            {
                var domainName = string.Empty;
                if (purchasedDict.ContainsKey(EnumHelper.DomainKeys.DomainName.ToString()))
                {
                    domainName = purchasedDict[EnumHelper.DomainKeys.DomainName.ToString()];
                }
                else if (!purchasedDict.ContainsKey(EnumHelper.DomainKeys.DomainName.ToString()))
                {
                    continue;
                }
                if (!PageInitHelper<ValidateProductsInDomainList>.PageInit.DomainListSearchTxt.GetAttribute(UiConstantHelper.AttributeClass).Contains(UiConstantHelper.NgHide))
                {
                    PageInitHelper<ValidateProductsInDomainList>.PageInit.DomainListSearchInputTxt.Clear();
                    PageInitHelper<ValidateProductsInDomainList>.PageInit.DomainListSearchInputTxt.SendKeys(domainName);
                    PageInitHelper<ValidateProductsInDomainList>.PageInit.DomainListSearchInputTxt.SendKeys(Keys.Enter);
                    Thread.Sleep(200);
                    tablesearchtestCondition = x => !PageInitHelper<ValidateProductsInDomainList>.PageInit.GridItem1List.GetAttribute(UiConstantHelper.AttributeClass).Equals("async-processing");
                    wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(60.00));
                    wait.Until(tablesearchtestCondition);
                }
                if (PageInitHelper<ValidateProductsInDomainList>.PageInit.DomainListTable.GetAttribute(UiConstantHelper.AttributeClass).Equals(string.Empty))
                {
                    domainmissing.Append(domainName + " , ");
                    continue;
                }

                PageInitHelper<PageNavigationHelper>.PageInit.MoveToElement(BrowserInit.Driver.FindElement(By.XPath(".//*/table/tbody/tr[not(contains(concat(' ',normalize-space(@class),' '),'ng-hide'))]//td[1]//p[@title='" + domainName + "']")));  // // label[@for=
                BrowserInit.Driver.FindElement(By.XPath(".//*/table/tbody/tr[not(contains(concat(' ',normalize-space(@class),' '),'ng-hide'))]//td[1]//p[@title='" + domainName + "']/following::td[@class='expander'][1]")).Click();  // //p[@title
                Thread.Sleep(1000);
                CheckExpanded:
                var expanded = BrowserInit.Driver.FindElement(By.XPath(".//*/table/tbody/tr[not(contains(concat(' ',normalize-space(@class),' '),'ng-hide'))]//td[1]//p[@title='" + domainName + "']/../../self::tr")).GetAttribute(UiConstantHelper.AttributeClass).Contains(UiConstantHelper.Expanded);
                if (expanded == false)
                {
                    BrowserInit.Driver.FindElement(By.XPath(".//*/table/tbody/tr[not(contains(concat(' ',normalize-space(@class),' '),'ng-hide'))]//td[1]//p[@title='" + domainName + "']/following::td[@class='expander'][1]/a")).Click();
                    goto CheckExpanded;
                }
                var subItemXpath =
                    ".//*/table/tbody/tr[not(contains(concat(' ',normalize-space(@class),' '),'ng-hide'))]//td[1]//p[@title='" +
                    domainName +
                    "']/../../../tr//tbody[contains(concat(' ',normalize-space(@data-ng-click),' '),'listItem.closeBuble($event)')]/tr";
                foreach (var subitem in BrowserInit.Driver.FindElements(By.XPath(subItemXpath)).Select((value, i) => new { i, value }))
                {
                    var subitemvalue = subitem.value;
                    var subitemindex = subitem.i + 1;
                    var dataNgClass = string.Empty;
                    var dataNgClassisPresent = BrowserInit.Driver.FindElements(By.XPath(subItemXpath + "[" + subitemindex + "][(@data-ng-class)]")).Count > 0;
                    if (dataNgClassisPresent)
                    {
                        dataNgClass = subitemvalue.GetAttribute("data-ng-class");
                    }
                    if (subitemvalue.GetAttribute(UiConstantHelper.AttributeClass).Contains("domain") || dataNgClass.Contains("domain"))
                    {
                        var domainNameInItemList = BrowserInit.Driver.FindElement(By.XPath(subItemXpath + "[" + subitemindex + "]//strong")).Text.Trim();
                        Assert.IsTrue(domainNameInItemList.Equals(purchasedDict[EnumHelper.DomainKeys.DomainName.ToString()]),
                            "The domain name in the domain list page mismatching, domain name shown on the domain list page as " + domainNameInItemList + " , but the expected domain name should be " + purchasedDict[EnumHelper.DomainKeys.DomainName.ToString()]);
                        if (BrowserInit.Driver.FindElements(By.XPath(".//*/table/tbody/tr[not(contains(concat(' ',normalize-space(@class),' '),'ng-hide'))]//td[1]//p[@title='" + purchasedDict[EnumHelper.DomainKeys.DomainName.ToString()] + "']/../../../tr[1]/td[1]/*")).Count > 4)
                        {
                            var paracount =
                                  BrowserInit.Driver.FindElements(By.XPath(subItemXpath + "[1]//td[1]//p")).Count > 2;
                            if (paracount)
                            {
                                if (
                                 BrowserInit.Driver.FindElement(By.XPath(subItemXpath + "[1]//td[1]//p[3]"))
                                     .Text.Trim()
                                     .Contains("FreeDNS"))
                                {
                                    var freeDnsStatus =
                                        BrowserInit.Driver.FindElement(By.XPath(subItemXpath + "//td[2]//p")).Text.Trim();
                                    Assert.IsTrue(
                                        freeDnsStatus.IndexOf("Active", StringComparison.CurrentCultureIgnoreCase) >= 0,
                                        "The domain status in the domain list page should be 'Active' for the domain name - " +
                                        purchasedDict[EnumHelper.DomainKeys.DomainName.ToString()] +
                                        ", but status shown in domain list page as " + freeDnsStatus);
                                    continue;
                                }
                            }
                            var transferStatus =
                            BrowserInit.Driver.FindElement(By.XPath(subItemXpath + "//td[2]//p")).Text.Trim();
                            Assert.IsTrue(transferStatus.Equals("Alert",StringComparison.InvariantCultureIgnoreCase)|| transferStatus.Equals("Active", StringComparison.InvariantCultureIgnoreCase),
                                "The domain status in the domain list page should be 'Alert or Active' for the domain name - " + purchasedDict[EnumHelper.DomainKeys.DomainName.ToString()] + ", but status shown in domain list page as " + transferStatus);
                            continue;
                        }
                        var domainbasedon =
                            BrowserInit.Driver.FindElement(
                                By.XPath(
                                    "//*[@id='maincontent']/div/div[5]/div/div[2]/div/table/tbody[2]/tr[1]/td[1]/span"))
                                .Text;
                        if (domainbasedon.Contains("Marketplace"))
                        {
                            var mdomainStatus =
                           BrowserInit.Driver.FindElement(By.XPath(subItemXpath + "//td[2]//p")).Text.Trim();
                            Assert.IsTrue(mdomainStatus.IndexOf("Alert", StringComparison.CurrentCultureIgnoreCase) >= 0,
                                "The domain status in the domain list page should be 'Alert' for the domain name - " + purchasedDict[EnumHelper.DomainKeys.DomainName.ToString()] + ", but status shown in domain list page as " + mdomainStatus);
                            continue;
                        }
                        var domainStatus =
                            BrowserInit.Driver.FindElement(By.XPath(subItemXpath + "//td[2]//p")).Text.Trim();
                        Assert.IsTrue(domainStatus.IndexOf("Active", StringComparison.CurrentCultureIgnoreCase) >= 0,
                            "The domain status in the domain list page should be 'Active' for the domain name - " + purchasedDict[EnumHelper.DomainKeys.DomainName.ToString()] + ", but status shown in domain list page as " + domainStatus);
                        var domainAutoRenewtoggle = subitemvalue.FindElement(By.XPath("//td[3]/div/input[contains(@id,'domain-autorenew')]")).Selected;
                        var domainAutoRenewtoggleStatus = domainAutoRenewtoggle ? "ON" : "OFF";
                        Assert.IsTrue(
                            domainAutoRenewtoggleStatus.Equals(
                                purchasedDict[EnumHelper.ShoppingCartKeys.DomainAutoRenewStatus.ToString()]),
                            "The domain autorenew in the domain list page mismatching, domain autorenew shows on the domain list page as - " +
                            domainAutoRenewtoggleStatus + " , But the expected domain name should be" +
                            purchasedDict[EnumHelper.ShoppingCartKeys.DomainAutoRenewStatus.ToString()] +
                            " for the domain name " + purchasedDict[EnumHelper.DomainKeys.DomainName.ToString()]);
                        var domainExpiration = DateTime.Parse(BrowserInit.Driver.FindElement(By.XPath(subItemXpath + "//td[4]//p")).Text.Trim()).ToString("MMM d, yyyy");
                        var domainExpirationshouldbe = DateTime.Now.AddYears(+Convert.ToInt32(Regex.Replace(purchasedDict[EnumHelper.DomainKeys.DomainDuration.ToString()], "[^0-9]+", string.Empty).Trim())).ToString("MMM d, yyyy");
                        Assert.IsTrue(domainExpiration.Equals(domainExpirationshouldbe),
                             "The domain Expiration date in the domain list should be greater than " + purchasedDict[EnumHelper.DomainKeys.DomainDuration.ToString()] + " from the date of domain purchased, but in the domain list page expriation date shown as - " + domainExpiration + " for the domain name " + purchasedDict[EnumHelper.DomainKeys.DomainName.ToString()]);
                    }
                    if (subitemvalue.GetAttribute(UiConstantHelper.AttributeClass).Contains("whoisguard"))
                    {
                        bool whoisindic =
                            purchasedDict.ContainsKey(EnumHelper.ShoppingCartKeys.WhoisGuardForDomainStatus.ToString());
                        if (whoisindic)
                        {
                           if (purchasedDict[EnumHelper.ShoppingCartKeys.WhoisGuardForDomainStatus.ToString()].Equals("ON", StringComparison.InvariantCultureIgnoreCase) )
                        {
                            var whois =
                                BrowserInit.Driver.FindElement(
                                    By.XPath(subItemXpath + "[" + subitemindex + "]/td[1]/p"))
                                    .GetAttribute(UiConstantHelper.AttributeClass);
                            if (whois != string.Empty)
                            {
                                whoismissing.Append(domainName + " , ");
                                continue;
                            }
                        }
                        if (purchasedDict[EnumHelper.ShoppingCartKeys.WhoisGuardForDomainStatus.ToString()].Equals(string.Empty))
                        {
                            continue;
                        }
                        string whoisGaurdStatus;
                        if (BrowserInit.Driver.FindElements(
                                By.XPath(subItemXpath + "/td[2]/div")).Count > 1)
                        {
                            whoisGaurdStatus =
                          BrowserInit.Driver.FindElement(
                              By.XPath(subItemXpath + "[" + subitemindex + "]//td[2]//p")).Text.Trim();
                            var whoisGuardAutoRenewtoggle = BrowserInit.Driver.FindElement(By.XPath(subItemXpath + "[" + subitemindex + "]//td[3]/div/input")).Selected;
                            var whoisGuardExpiration = BrowserInit.Driver.FindElement(By.XPath(subItemXpath + "[" + subitemindex + "]//td[4]//p")).Text.Trim();
                            var whoisGuardAutoRenewtoggleStatus = whoisGuardAutoRenewtoggle ? "ON" : "OFF";
                            Assert.IsTrue(whoisGuardAutoRenewtoggleStatus.Equals(purchasedDict[EnumHelper.ShoppingCartKeys.WhoisGuardForDomainAutoRenewStatus.ToString()]),
                                "The WhoisGuard auto renew status for domain name - " + purchasedDict[EnumHelper.DomainKeys.DomainName.ToString()] + " in domain list page is mismatches expected Who is Guard Auto Renew Status should be " + purchasedDict[EnumHelper.ShoppingCartKeys.WhoisGuardForDomainAutoRenewStatus.ToString()] + ", but who is guard autorenew status shown in the domain list page as " + whoisGuardAutoRenewtoggleStatus);
                            Assert.IsTrue(whoisGuardExpiration.Equals(DateTime.Now.AddYears(+Convert.ToInt32(Regex.Replace(purchasedDict[EnumHelper.DomainKeys.DomainDuration.ToString()], "[^0-9]+", string.Empty).Trim())).ToString("MMM d, yyyy")),
                                "The WhoisGuard Expiration date in domain list page should be greater than " + purchasedDict[EnumHelper.DomainKeys.DomainDuration.ToString()] + " from the date of domain purchased, but in domain list page expriation date shown as - " + whoisGuardExpiration + " for the domain name " + purchasedDict[EnumHelper.DomainKeys.DomainName.ToString()]);
                        }
                        else
                        {
                            whoisGaurdStatus = "OFF";
                        }
                        Assert.IsTrue(whoisGaurdStatus.Equals(purchasedDict[EnumHelper.ShoppingCartKeys.WhoisGuardForDomainStatus.ToString()]),
                           "The who is guard status for the domain name " + purchasedDict[EnumHelper.DomainKeys.DomainName.ToString()] + " in the domain list page is mismatching expected should be " + purchasedDict[EnumHelper.ShoppingCartKeys.WhoisGuardForDomainStatus.ToString()] + ", but who is guard status shown in the domain list page as " + whoisGaurdStatus);
                    }
                    }
                    if (subitemvalue.GetAttribute(UiConstantHelper.AttributeClass).Contains("hosting"))
                    {
                        var hostingName = subitemvalue.Text.Trim();
                        Assert.IsTrue(hostingName.Equals(purchasedDict[EnumHelper.HostingKeys.ProductName.ToString()]),
                            "The hosting product name in domain list page is missmatches, Expected Product name as " + purchasedDict[EnumHelper.HostingKeys.ProductName.ToString()] + ", but product name shown in domain list page as " + hostingName);
                        var hostingStatus =
                          BrowserInit.Driver.FindElement(
                              By.XPath(subItemXpath + "[" + subitemindex + "]//td[2]//p[2]")).Text.Trim();
                        Assert.IsTrue(hostingStatus.Equals("Alert", StringComparison.CurrentCultureIgnoreCase) | hostingStatus.Equals("Active", StringComparison.CurrentCultureIgnoreCase),
                            "The hosting product status should be neither 'Active or Alert' for the product " + purchasedDict[EnumHelper.HostingKeys.ProductName.ToString()] + ", but product status shown in domain list page as " + hostingStatus);
                        var subTitle =
                     BrowserInit.Driver.FindElement(
                         By.XPath(subItemXpath + "[" + subitemindex + "]/td[1]//p[2]")).Text.Trim();
                        Assert.IsTrue(subTitle.Equals("Hosting"),
                            "For the hosting product info sub tile should be 'Hosting' for the product " + purchasedDict[EnumHelper.HostingKeys.ProductName.ToString()] + ", but in domain list page product info displays as" + subTitle);
                    }
                    if (!subitemvalue.GetAttribute(UiConstantHelper.AttributeClass).Equals("pdns")) continue;
                    {
                        var premiumDnsStatus =
                            BrowserInit.Driver.FindElement(By.XPath(subItemXpath + "[" + subitemindex + "]//td[2]//p"))
                                .Text.Trim();
                        Assert.IsTrue(premiumDnsStatus.Equals(purchasedDict[EnumHelper.ShoppingCartKeys.PremiumDnsForDomainStatus.ToString()]),
                            "The PremiumDNS status for the domain name " + purchasedDict[EnumHelper.DomainKeys.DomainName.ToString()] + " in the domain list page is mismatching expected should be " + purchasedDict[EnumHelper.ShoppingCartKeys.PremiumDnsForDomainStatus.ToString()] + ", but PremiumDNS status shown in the domain list page as " + premiumDnsStatus);
                        var premiumDnsAutoRenewtoggle = BrowserInit.Driver.FindElement(By.XPath(subItemXpath + "[" + subitemindex + "]//td[3]/div/input")).Selected;
                        var premiumDnsAutoRenewtoggleStatus = premiumDnsAutoRenewtoggle ? "ON" : "OFF";
                        Assert.IsTrue(premiumDnsAutoRenewtoggleStatus.Equals(purchasedDict[EnumHelper.ShoppingCartKeys.PremiumDnsForDomainAutoRenewStatus.ToString()]),
                            "The PremiumDNS auto renew status for domain name - " + purchasedDict[EnumHelper.DomainKeys.DomainName.ToString()] + " in domain list page is mismatches expected PremiumDNS Auto Renew Status should be " + purchasedDict[EnumHelper.ShoppingCartKeys.PremiumDnsForDomainAutoRenewStatus.ToString()] + ", but PremiumDNS autorenew status shown in the domain list page as " + premiumDnsAutoRenewtoggleStatus);
                        var premiumDnsExpiration = BrowserInit.Driver.FindElement(By.XPath(subItemXpath + "[" + subitemindex + "]//td[4]//p")).Text.Trim();
                        Assert.IsTrue(premiumDnsExpiration.Equals(DateTime.Now.AddYears(+Convert.ToInt32(Regex.Replace(purchasedDict[EnumHelper.DomainKeys.DomainDuration.ToString()], "[^0-9]+", string.Empty).Trim())).ToString("MMM d, yyyy")),
                            "The PremiumDNS Expiration date in domain list page should be greater than " + purchasedDict[EnumHelper.DomainKeys.DomainDuration.ToString()] + " from the date of domain purchased, but in domain list page expriation date shown as - " + premiumDnsExpiration + " for the domain name " + purchasedDict[EnumHelper.DomainKeys.DomainName.ToString()]);
                    }
                }
            }
            if (domainmissing.Length != 0)
            {
                Assert.Inconclusive(domainmissing + " for these purchased domain names are missing the domain list page");
            }
            if (whoismissing.Length != 0)
            {
                Assert.Inconclusive(whoismissing + " for these purchased domain names who is guard info shows as 'WhoisGuard protection has never been turned ON' in domain list page, but who is guard is purchased for these domains along with domains purchase");
            }
        }
        #region DomainListPageFactory
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'row table-sticky-filters')]/div/div[contains(concat(' ',normalize-space(@class),' '),'search')]")]
        [CacheLookup]
        internal IWebElement DomainListSearchTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@data-ng-model),' '),'$parameters.search.value')]")]
        [CacheLookup]
        internal IWebElement DomainListSearchInputTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*/table/tbody/tr[@class='gb-row'][1] | .//table/tbody[2]/tr[@class='item'][1]")]
        [CacheLookup]
        internal IWebElement DomainListFirstGrid { get; set; }
        [FindsBy(How = How.XPath, Using = ".//div[contains(@class,'menu')]/span[contains(.,'Domains')]|.//a[contains(@class,'select2')]/span[contains(.,'Products')]")]  //  .//*[@class='dl-view']/div/a
        [CacheLookup]
        internal IWebElement AllProductDdl { get; set; }
        [FindsBy(How = How.XPath, Using = ".//div[contains(@class,'menu')]/span[contains(.,'All Products')]|.//*[@role='option'][contains(.,'Products')]")]  // .//div[@class='dl-view']/select/option[@value='All']
        [CacheLookup]
        internal IWebElement AllProductLst { get; set; }
        [FindsBy(How = How.XPath, Using = "//*/table/tbody[2]/tr")]
        [CacheLookup]
        internal IWebElement GridItem1List { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*/table/tbody/tr[not(contains(concat(' ',normalize-space(@class),' '),'ng-hide'))]//td[1])[1]")]
        [CacheLookup]
        internal IWebElement DomainListTable { get; set; }
        #endregion
    }
}