using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.SecurityPageFactory;
using NamecheapUITests.PagefactoryObject.HelpersPageFactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NamecheapUITests.PageObject.ValidationPages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
namespace NamecheapUITests.PageObject.CMSPages.SecurityPage
{
    public class WhoisGuardPage
    {
        public List<SortedDictionary<string, string>> AddingWhoisGuardWithDomainNamesToCart(string purchasingDomainFor)
        {
            var dicWhoisGuardProductDetailsList = new List<SortedDictionary<string, string>>();
            var dicWhoisGuardProductDetailsDic = new SortedDictionary<string, string>();
            var priceDurationTxt = Regex.Split(
                PageInitHelper<WhoisGuardPageFactory>.PageInit.WhoisGuardProductPriceTxt.Text, "/");
            var securityProductPrice = Regex.Replace(priceDurationTxt[0], @"[^\d..][^\w\s]*", "");
            var securityProductDuration = "1 " + priceDurationTxt[1].Replace(priceDurationTxt[1], "year");
            dicWhoisGuardProductDetailsDic.Add(EnumHelper.Ssl.WhoisGuardPrice.ToString(), securityProductPrice);
            dicWhoisGuardProductDetailsDic.Add(EnumHelper.Ssl.WhoisGuardDuration.ToString(),
                securityProductDuration);
            PageInitHelper<WhoisGuardPageFactory>.PageInit.WhoisGuardProducAddToCartBtn.Click();
            List<Tuple<string, string, string, decimal, decimal>> whoisGuardList;
            IDomainSelectOptions domainSelectOption;
            ICartValidation cartWidgetValidation;
            List<SortedDictionary<string, string>> mergedSearchdDomainAndCartWidgetList;
            switch (EnumHelper.ParseEnum<EnumHelper.DomainModuleSelection>(purchasingDomainFor))
            {
                case EnumHelper.DomainModuleSelection.NewDomain:
                    domainSelectOption = new PurchaseNewDomain();
                    whoisGuardList = domainSelectOption.HostingDomainSelection(dicWhoisGuardProductDetailsDic);
                    foreach (var newDomains in whoisGuardList)
                    {
                        dicWhoisGuardProductDetailsDic.Add(EnumHelper.DomainKeys.DomainName.ToString(),
                            newDomains.Item1);
                        dicWhoisGuardProductDetailsDic.Add(EnumHelper.DomainKeys.DomainDuration.ToString(),
                            newDomains.Item2);
                        dicWhoisGuardProductDetailsDic.Add(
                            EnumHelper.DomainKeys.DomainNamePromotionCode.ToString(), newDomains.Item3);
                        dicWhoisGuardProductDetailsDic.Add(EnumHelper.DomainKeys.DomainPrice.ToString(),
                            newDomains.Item4.ToString(CultureInfo.InvariantCulture));
                        dicWhoisGuardProductDetailsDic.Add(EnumHelper.DomainKeys.DomainRetailPrice.ToString(),
                            newDomains.Item5.ToString(CultureInfo.InvariantCulture));
                        dicWhoisGuardProductDetailsList.Add(dicWhoisGuardProductDetailsDic);
                    }
                    BrowserInit.Driver.FindElement(By.XPath("//*[@class='btn domain-select-new-btn']")).Click();
                    Func<IWebDriver, bool> testCondition =
                        x => PageInitHelper<DomainSelectionPageFactory>.PageInit.CartContinueWidgetBtn.Enabled;
                    var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(5));
                    wait.Until(testCondition);
                    cartWidgetValidation = new ProductListCartValidation();
                    mergedSearchdDomainAndCartWidgetList =
                        cartWidgetValidation.CartWidgetValidation(dicWhoisGuardProductDetailsList);
                    break;
                case EnumHelper.DomainModuleSelection.NcDomain:
                    domainSelectOption = new UseDomainWithNc();
                    whoisGuardList = domainSelectOption.HostingDomainSelection(dicWhoisGuardProductDetailsDic);
                    foreach (var newDomains in whoisGuardList)
                    {
                        dicWhoisGuardProductDetailsDic.Add(EnumHelper.DomainKeys.DomainName.ToString(),
                            newDomains.Item1);
                        dicWhoisGuardProductDetailsDic.Add(
                            EnumHelper.ShoppingCartKeys.WhoisGuardForDomainStatus.ToString(), "ON");
                        dicWhoisGuardProductDetailsList.Add(dicWhoisGuardProductDetailsDic);
                    }
                    cartWidgetValidation = new ProductListCartValidation();
                    mergedSearchdDomainAndCartWidgetList =
                        cartWidgetValidation.CartWidgetValidation(dicWhoisGuardProductDetailsList);
                    break;
                case EnumHelper.DomainModuleSelection.TransferDomain:
                    domainSelectOption = new TransferDomainFromanotherRegistrar();
                    whoisGuardList = domainSelectOption.HostingDomainSelection(dicWhoisGuardProductDetailsDic);
                    foreach (var newDomains in whoisGuardList)
                    {
                        dicWhoisGuardProductDetailsDic.Add(EnumHelper.DomainKeys.DomainName.ToString(),
                            newDomains.Item1);
                        dicWhoisGuardProductDetailsDic.Add(EnumHelper.DomainKeys.DomainDuration.ToString(),
                            newDomains.Item2);
                        dicWhoisGuardProductDetailsDic.Add(
                            EnumHelper.DomainKeys.DomainNamePromotionCode.ToString(), newDomains.Item3);
                        dicWhoisGuardProductDetailsDic.Add(EnumHelper.DomainKeys.DomainPrice.ToString(),
                            newDomains.Item4.ToString(CultureInfo.InvariantCulture));
                        dicWhoisGuardProductDetailsDic.Add(EnumHelper.DomainKeys.DomainRetailPrice.ToString(),
                            newDomains.Item5.ToString(CultureInfo.InvariantCulture));
                        dicWhoisGuardProductDetailsList.Add(dicWhoisGuardProductDetailsDic);
                    }
                    BrowserInit.Driver.FindElement(By.XPath("//*[@class='btn domain-select-new-btn']")).Click();
                    testCondition =
                        x => PageInitHelper<DomainSelectionPageFactory>.PageInit.CartContinueWidgetBtn.Enabled;
                    wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(5));
                    wait.Until(testCondition);
                    cartWidgetValidation = new ProductListCartValidation();
                    mergedSearchdDomainAndCartWidgetList =
                        cartWidgetValidation.CartWidgetValidation(dicWhoisGuardProductDetailsList);
                    break;
                default:
                    throw new IndexOutOfRangeException(purchasingDomainFor +
                                                       " Options is not available in a switch case statement on class name - 'WhoisGuardPage' under method name - 'AddingWhoisGuardWithDomainNamesToCart'");
            }
            return mergedSearchdDomainAndCartWidgetList;
        }
    }
}