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
    public class PremiunDnsPage
    {
        public List<SortedDictionary<string, string>> AddingPremiumDnsWithDomainNamesToCart(string purchasingDomainFor)
        {
            var listPremiumDnsProductDetails = new List<SortedDictionary<string, string>>();
            var dicPremiumDnsProducttDetails = new SortedDictionary<string, string>();
            var priceDurationTxt = Regex.Split(
                PageInitHelper<PremiumDnsPageFactory>.PageInit.PremiunDnsProductPriceTxt.Text, "/");
            var premiunDnsProductPrice = Regex.Replace(priceDurationTxt[0], @"[^\d..][^\w\s]*", "");
            var premiunDnsProductDuration = "1 " + priceDurationTxt[1].Replace(priceDurationTxt[1], "year");
            dicPremiumDnsProducttDetails.Add(EnumHelper.Ssl.PremiumDnsPrice.ToString(), premiunDnsProductPrice);
            dicPremiumDnsProducttDetails.Add(EnumHelper.Ssl.PremiumDnsDuration.ToString(),
                premiunDnsProductDuration);
            PageInitHelper<PremiumDnsPageFactory>.PageInit.PremiunDnsProductAddToCartBtn.Click();
            List<Tuple<string, string, string, decimal, decimal>> premiumDnsTupleList;
            IDomainSelectOptions domainSelectOption;
            ICartValidation cartWidgetValidation;
            List<SortedDictionary<string, string>> mergedSearchdDomainAndCartWidgetList;
            switch (EnumHelper.ParseEnum<EnumHelper.DomainModuleSelection>(purchasingDomainFor))
            {
                case EnumHelper.DomainModuleSelection.NewDomain:
                    domainSelectOption = new PurchaseNewDomain();
                    premiumDnsTupleList = domainSelectOption.HostingDomainSelection(dicPremiumDnsProducttDetails);
                    foreach (var newDomains in premiumDnsTupleList)
                    {
                        dicPremiumDnsProducttDetails.Add(EnumHelper.DomainKeys.DomainName.ToString(),
                            newDomains.Item1);
                        dicPremiumDnsProducttDetails.Add(EnumHelper.DomainKeys.DomainDuration.ToString(),
                            newDomains.Item2);
                        dicPremiumDnsProducttDetails.Add(
                            EnumHelper.DomainKeys.DomainNamePromotionCode.ToString(), newDomains.Item3);
                        dicPremiumDnsProducttDetails.Add(EnumHelper.DomainKeys.DomainPrice.ToString(),
                            newDomains.Item4.ToString(CultureInfo.InvariantCulture));
                        dicPremiumDnsProducttDetails.Add(EnumHelper.DomainKeys.DomainRetailPrice.ToString(),
                            newDomains.Item5.ToString(CultureInfo.InvariantCulture));
                        listPremiumDnsProductDetails.Add(dicPremiumDnsProducttDetails);
                    }
                    BrowserInit.Driver.FindElement(By.XPath("//*[@class='btn domain-select-new-btn']")).Click();
                    Func<IWebDriver, bool> testCondition =
                        x => PageInitHelper<DomainSelectionPageFactory>.PageInit.CartContinueWidgetBtn.Enabled;
                    var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(5));
                    wait.Until(testCondition);
                    cartWidgetValidation = new ProductListCartValidation();
                    mergedSearchdDomainAndCartWidgetList =
                        cartWidgetValidation.CartWidgetValidation(listPremiumDnsProductDetails);
                    break;
                case EnumHelper.DomainModuleSelection.NcDomain:
                    domainSelectOption = new UseDomainWithNc();
                    premiumDnsTupleList = domainSelectOption.HostingDomainSelection(dicPremiumDnsProducttDetails);
                    foreach (var newDomains in premiumDnsTupleList)
                    {
                        dicPremiumDnsProducttDetails.Add(EnumHelper.DomainKeys.DomainName.ToString(),
                            newDomains.Item1);
                        dicPremiumDnsProducttDetails.Add(
                            EnumHelper.ShoppingCartKeys.PremiumDnsForDomainStatus.ToString(), "ON");
                        listPremiumDnsProductDetails.Add(dicPremiumDnsProducttDetails);
                    }
                    cartWidgetValidation = new ProductListCartValidation();
                    mergedSearchdDomainAndCartWidgetList =
                        cartWidgetValidation.CartWidgetValidation(listPremiumDnsProductDetails);
                    break;
                case EnumHelper.DomainModuleSelection.AnotherRegistrarDomain:
                    domainSelectOption = new UseDomainFromOtherRegister();
                    premiumDnsTupleList = domainSelectOption.HostingDomainSelection(dicPremiumDnsProducttDetails);
                    foreach (var newDomains in premiumDnsTupleList)
                    {
                        dicPremiumDnsProducttDetails.Add(EnumHelper.DomainKeys.DomainName.ToString(),
                            newDomains.Item1);
                        dicPremiumDnsProducttDetails.Add(EnumHelper.DomainKeys.DomainDuration.ToString(),
                            newDomains.Item2);
                        dicPremiumDnsProducttDetails.Add(
                            EnumHelper.DomainKeys.DomainNamePromotionCode.ToString(), newDomains.Item3);
                        dicPremiumDnsProducttDetails.Add(EnumHelper.DomainKeys.DomainPrice.ToString(),
                            newDomains.Item4.ToString(CultureInfo.InvariantCulture));
                        dicPremiumDnsProducttDetails.Add(EnumHelper.DomainKeys.DomainRetailPrice.ToString(),
                            newDomains.Item5.ToString(CultureInfo.InvariantCulture));
                        listPremiumDnsProductDetails.Add(dicPremiumDnsProducttDetails);
                    }
                    cartWidgetValidation = new ProductListCartValidation();
                    mergedSearchdDomainAndCartWidgetList =
                        cartWidgetValidation.CartWidgetValidation(listPremiumDnsProductDetails);
                    break;
                default:
                    throw new IndexOutOfRangeException(purchasingDomainFor +
                                                       " Options is not available in a switch case statement on class name - 'PremiumDns' under method name - 'AddingPremiumDnsWithDomainNamesToCart'");
            }
            return mergedSearchdDomainAndCartWidgetList;
        }
    }
}