using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.DomainsPageFactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.ValidationPages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
namespace NamecheapUITests.PageObject.CMSPages.DomainsPage
{
    public class TransferDomainPage
    {
        internal List<SortedDictionary<string, string>> AddingDomainNamesToCart(string purchasingDomainFor,
            List<string> transferDomainNames)
        {
            var transferDomainsList = new List<SortedDictionary<string, string>>();
            if (purchasingDomainFor.Contains(UiConstantHelper.BulkDomainsTransfer))
            {
                PageInitHelper<TransferDomainPageFactory>.PageInit.BulkOptionLnkTxt.Click();
                PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
                PageInitHelper<TransferDomainPageFactory>.PageInit.MultipleDomainSearchInputTxt.Clear();
                foreach (var newDomain in transferDomainNames)
                {
                    PageInitHelper<TransferDomainPageFactory>.PageInit.MultipleDomainSearchInputTxt.SendKeys(newDomain);
                    PageInitHelper<TransferDomainPageFactory>.PageInit.MultipleDomainSearchInputTxt.SendKeys(Keys.Enter);
                }
                PageInitHelper<TransferDomainPageFactory>.PageInit.BulkTransferSearchBtn.Click();
                Thread.Sleep(3000);
                PageInitHelper<PageNavigationHelper>.PageInit.WaitForPageLoad();
                var domaincount = PageInitHelper<TransferDomainPageFactory>.PageInit.EligibleTransferList.Count;
                for (var dcount = 0; dcount < domaincount; dcount++)
                {
                    var dcount1 = dcount + 1;
                    var ele = BrowserInit.Driver.FindElement(By.XPath("(.//*[contains(@class,'eligible')]/ul/li)[" + dcount1 + "]/a"));
                    Func<IWebDriver, bool> testCondition = x => ele.Displayed;
                    var wait11 = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(3));
                    wait11.Until(testCondition);
                    PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(ele);
                    ele.Click();
                    testCondition = x =>
                             BrowserInit.Driver.FindElement(By.XPath("(.//*[contains(@class,'eligible')]/ul/li)[" + dcount1 + "]/span[@class= 'price']/following-sibling::*")).GetCssValue(UiConstantHelper.LocatorBackgroundTxt).Contains(UiConstantHelper.AddedcartImg);
                    var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(80));
                    wait.Until(testCondition);
                    Thread.Sleep(2000);
                    var domainName =
                       BrowserInit.Driver.FindElement(
                           By.XPath("(.//*[contains(@class,'eligible')]/ul/li)[" + dcount1 + "]//span[@class='domain-name']")).Text;
                    var domainPrice = Convert.ToDecimal(Regex.Replace(
                        BrowserInit.Driver.FindElement(
                            By.XPath("(.//*[contains(@class,'eligible')]/ul/li)[" + dcount1 + "]//span[@class='price']/span[2]")).Text, @"[^\d..][^\w\s]*", "").Trim());
                    var adminEmail = Regex.Replace(
                        BrowserInit.Driver.FindElement(
                            By.XPath("(.//*[contains(@class,'eligible')]/ul/li)[" + dcount1 + "]/div[@class='small']/p[3]")).Text, "ADMIN EMAIL", string.Empty).Trim();
                    var transferDomainDic = new SortedDictionary<string, string>
                    {
                        {EnumHelper.DomainKeys.DomainName.ToString(), domainName},
                        {
                            EnumHelper.DomainKeys.DomainPrice.ToString(),
                            domainPrice.ToString(CultureInfo.InvariantCulture)
                        },
                        {"Admin email", adminEmail}
                    };
                    transferDomainsList.Add(transferDomainDic);
                }
            }
            if (!purchasingDomainFor.Contains(UiConstantHelper.SingleDomainTransfer)) return transferDomainsList;
            {
                DomainAvailablityCheck:
                foreach (var newTransferDomain in transferDomainNames)
                {
                    PageInitHelper<TransferDomainPageFactory>.PageInit.DomainSearchTxt.Clear();
                    PageInitHelper<TransferDomainPageFactory>.PageInit.DomainSearchTxt.SendKeys(newTransferDomain);
                    PageInitHelper<TransferDomainPageFactory>.PageInit.TransferSearchBtn.Click();
                    Func<IWebDriver, bool> testCondition = x =>
                        PageInitHelper<TransferDomainPageFactory>.PageInit.SearchResultDiv.Displayed;
                    var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(100));
                    wait.Until(testCondition);
                    Thread.Sleep(600);
                    if (PageInitHelper<TransferDomainPageFactory>.PageInit.SearchResultDiv.GetAttribute(UiConstantHelper.AttributeClass).Contains("eligible"))
                    {
                        var authCode =
                            PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(int.Parse(PageInitHelper<SldGenerator>.PageInit.GetRandomDigits(4).ToString()),
                                int.Parse(PageInitHelper<SldGenerator>.PageInit.GetRandomDigits(4).ToString()));
                        if (PageInitHelper<TransferDomainPageFactory>.PageInit.UnKnownRegistrar.Count > 4)
                            PageInitHelper<TransferDomainPageFactory>.PageInit.AuthCodeTxtBox.Clear();
                        PageInitHelper<TransferDomainPageFactory>.PageInit.AuthCodeTxtBox.SendKeys(authCode.ToString());
                        PageInitHelper<TransferDomainPageFactory>.PageInit.YesToAllChkBox.Click();
                        PageInitHelper<TransferDomainPageFactory>.PageInit.TransferDomainToCartBtn.Click();
                        testCondition = x =>
                            !BrowserInit.Driver.FindElement(By.XPath("//span[contains(@class,'price')]/following-sibling::*"))
                                .GetAttribute(UiConstantHelper.AttributeClass).Contains("loading");
                        wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(3));
                        wait.Until(testCondition);
                        PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
                        var transferDomainDic = new SortedDictionary<string, string>();
                        var domainName = newTransferDomain;
                        var authorizationCode =
                            PageInitHelper<TransferDomainPageFactory>.PageInit.AuthCodeTxtBox.GetAttribute("value");
                        var adminEmailId = Regex.Replace(BrowserInit.Driver.FindElement(
                            By.XPath("(//ul[contains(@class,'transfer-results')]/li//p/strong)[3]/..")).Text, "ADMIN EMAIL", string.Empty).Trim();
                        var domainPrice = Convert.ToDecimal(Regex.Replace(
                            BrowserInit.Driver.FindElement(
                                By.XPath("(.//*[contains(@class,'eligible')]/ul/li)//span[@class='price']/span[2]")).Text, @"[^\d..][^\w\s]*", "").Trim());
                        transferDomainDic.Add(EnumHelper.DomainKeys.DomainName.ToString(), domainName);
                        transferDomainDic.Add("Authorization Code", authorizationCode);
                        transferDomainDic.Add("Admin email", adminEmailId);
                        transferDomainDic.Add(EnumHelper.DomainKeys.DomainPrice.ToString(), domainPrice.ToString(CultureInfo.InvariantCulture));
                        transferDomainsList.Add(transferDomainDic);
                    }
                    else
                    {
                        transferDomainNames.Clear();
                        transferDomainNames = PageInitHelper<RandomDomainNameGenerator>.PageInit.DomainName(UiConstantHelper.SingleDomainTransfer);
                        goto DomainAvailablityCheck;
                    }
                }
            }
            return transferDomainsList;
        }
    }
}