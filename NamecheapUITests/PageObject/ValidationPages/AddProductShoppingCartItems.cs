using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using NamecheapUITests.PagefactoryObject.ValidationPagefactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
namespace NamecheapUITests.PageObject.ValidationPages
{
    public class AddProductShoppingCartItems : IShoppingCartValidation
    {
        public List<SortedDictionary<string, string>> AddShoppingCartItemsToDic(List<SortedDictionary<string, string>> cartWidgetList, string whois, string premiumDns)
        {
          //  PageInitHelper<AddProductShoppingCartItems>.PageInit.MessageAndAlertVerification();
            var shoppingcartItemList =
                PageInitHelper<AddProductShoppingCartItems>.PageInit.AddShoppingCartItemsToDictionaries(cartWidgetList, whois, premiumDns);
            AVerify verifingTwoListOfDic = new VerifyData();
            verifingTwoListOfDic.VerifyTwoListOfDic(shoppingcartItemList, cartWidgetList);
            AMerge mergeTWoListOfDic = new MergeData();
            var mergedScAndCartWidgetDic = mergeTWoListOfDic.MergingTwoListOfDic(shoppingcartItemList, cartWidgetList);
            PageInitHelper<ShoppingCartPageFactory>.PageInit.ConfirmOrderBtn.Click();
            return mergedScAndCartWidgetDic;
        }
        private List<SortedDictionary<string, string>> AddShoppingCartItemsToDictionaries(List<SortedDictionary<string, string>> cartWidgetList, string whois, string premiumDns)
        {
            var domainNameinSc = string.Empty;
            var domainDuration = string.Empty;
            var domainAutoRenewStatus = string.Empty;
            var domainPrice = 0.00M;
            var icannFeePrice = 0.00M;
            var whoisGuardstatus = string.Empty;
            var whoisGuardDuration = string.Empty;
            var whoisGuardAutoRenewStatus = string.Empty;
            var whoisGuardPrice = 0.00M;
            var whoisGuardRentalPrice = 0.00M;
            var premiumDnsStatus = string.Empty;
            var premiumDnsDuration = string.Empty;
            var premiumDnsAutorenewStatus = string.Empty;
            var premiumDnsRentealPrice = 0.00M;
            var premiumDnsPrice = 0.00M;
            var domainRentalprice = 0.00M;
            var promotionCode = string.Empty;
            var productName = string.Empty;
            var productNameprice = 0.00M;
            var productDuration = string.Empty;
            var productDataCenter = string.Empty;
            var productDatacenterPrice = 0.00M;
            var shoppingcartItemList = new List<SortedDictionary<string, string>>();
            const string productGroupsXpath = "(.//*[@class='product-group'])";
            var productGroups = BrowserInit.Driver.FindElements(By.XPath(productGroupsXpath));
            for (var productGroup = 1; productGroup <= productGroups.Count; productGroup++)
            {
                PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(productGroups[productGroup-1]);

                var shoppingCartItemsDic = new SortedDictionary<string, string>();
                var cartItemdivCount =
                    BrowserInit.Driver.FindElements(
                        By.XPath("(" + productGroupsXpath + "[" + productGroup + "]/div[contains(@class,'cart-item')])"));
                for (var cartItem = 1; cartItem <= cartItemdivCount.Count; cartItem++)
                {
                    var cartItemdivCountXpath = "(" + productGroupsXpath + "[" + productGroup + "]/div[contains(@class,'cart-item')])[" +
                                cartItem + "]";
                    var cartItemClassName =
                        BrowserInit.Driver.FindElement(
                            By.XPath(cartItemdivCountXpath)).GetAttribute(UiConstantHelper.AttributeClass);
                    if (cartItemClassName.IndexOf("domain-name", StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        var cartElementsCount =
                            BrowserInit.Driver.FindElements(
                                By.XPath("((" + productGroupsXpath + "[" + productGroup +
                                         "]/div[contains(@class,'cart-item')])[" +
                                         cartItem + "])/*[not(self::a)]"));
                        foreach (var cartEle in cartElementsCount.Select((value, i) => new { i, value }))
                        {
                            var value = cartEle.value;
                            var index = cartEle.i;
                            var cartElementClassName = value.GetAttribute(UiConstantHelper.AttributeClass);
                            var cartElement1 = index + 1;
                            var cartEleXpath = "(((" + productGroupsXpath + "[" + productGroup +
                                                "]/div[contains(@class,'cart-item')])[" + cartItem +
                                                "])/*[not(self::a)])[" + cartElement1 + "]";
                            if (cartElementClassName.Equals(string.Empty))
                            {
                                var domainNameInScList = new List<string>(Regex.Replace(
                                    BrowserInit.Driver.FindElement(By.XPath(cartEleXpath)).Text.Trim(), "\r\n", " ")
                                    .Split(' '));
                                var result = domainNameInScList.Where(i => i.Contains(".")).ToList();
                                domainNameinSc = result[0];
                                domainDuration =
                                    BrowserInit.Driver.FindElement(
                                        By.XPath(cartEleXpath + "/div[contains(@class,'Duration')]/span[1]"))
                                        .Text.Trim();
                                var priceEleCount =
                                    BrowserInit.Driver.FindElements(
                                        By.XPath(cartEleXpath + "//div[contains(@class,'price')]/*"));
                                for (var price = 1; price <= priceEleCount.Count; price++)
                                {
                                    var priceElexpath = "(" + cartEleXpath + "//div[contains(@class,'price')]/*)[" +
                                                         price +
                                                         "]";
                                    var priceEleTagName =
                                        BrowserInit.Driver.FindElement(
                                            By.XPath(priceElexpath)).TagName;
                                    if (priceEleTagName.Equals("s", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        domainRentalprice = Convert.ToDecimal(Regex.Replace(
                                            BrowserInit.Driver.FindElement(By.XPath(priceElexpath)).Text,
                                            @"[^\d..][^\w\s]*", string.Empty).Trim());
                                    }
                                    else if (priceEleTagName.Equals("span", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        var priceSpanClassName =
                                            BrowserInit.Driver.FindElement(By.XPath(priceElexpath))
                                                .GetAttribute(UiConstantHelper.AttributeClass);
                                        if (priceSpanClassName.Equals("amount",
                                            StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            domainPrice = Convert.ToDecimal(Regex.Replace(
                                                BrowserInit.Driver.FindElement(By.XPath(priceElexpath)).Text,
                                                @"[^\d..][^\w\s]*", string.Empty).Trim());
                                        }
                                        else if (priceSpanClassName.Equals("label",
                                            StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            promotionCode =
                                                BrowserInit.Driver.FindElement(By.XPath(priceElexpath)).Text.Trim();
                                        }
                                    }
                                }
                            }
                            if (cartElementClassName.Equals("toggles", StringComparison.CurrentCultureIgnoreCase))
                            {
                                var domainrenewstatus =
                                    BrowserInit.Driver.FindElement(By.XPath(cartEleXpath + "/li/div/input")).Selected;
                                var autoRenewToggle = domainrenewstatus ? "ON" : "OFF";
                                domainAutoRenewStatus = autoRenewToggle;
                            }
                            else if (cartElementClassName.Equals("small", StringComparison.CurrentCultureIgnoreCase))
                            {
                                var icaandivCount =
                                    BrowserInit.Driver.FindElements(By.XPath(cartEleXpath + "//*")).Count;
                                if (icaandivCount <= 1) continue;
                                var icaanDivClassName =
                                    BrowserInit.Driver.FindElement(By.XPath(cartEleXpath + "//*/div"))
                                        .GetAttribute(UiConstantHelper.AttributeClass);
                                if (!icaanDivClassName.Equals("price", StringComparison.CurrentCultureIgnoreCase))
                                    continue;
                                if (BrowserInit.Driver.FindElement(By.XPath(cartEleXpath + "//*/div/span")).Text.Equals(string.Empty))
                                {
                                    continue;
                                }
                                icannFeePrice = Convert.ToDecimal(Regex.Replace(
                                    BrowserInit.Driver.FindElement(By.XPath(cartEleXpath + "//*/div/span")).Text, @"[^\d..][^\w\s]*", string.Empty).Trim());
                            }
                        }
                    }
                    if (cartItemClassName.IndexOf("add-on", StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        var addonDivCount =
                            BrowserInit.Driver.FindElements(
                                By.XPath("(" + productGroupsXpath + "[" + productGroup +
                                         "]/div[contains(@class,'cart-item')])[" +
                                         cartItem + "]/div")).Count;
                        for (var addonDiv = 1; addonDiv <= addonDivCount; addonDiv++)
                        {
                            var addonXpath = "((" + productGroupsXpath + "[" + productGroup +
                                             "]/div[contains(@class,'cart-item')])[" +
                                             cartItem + "]/div)[" + addonDiv + "]";
                            var addonName = Regex.Replace(Regex.Replace(BrowserInit.Driver.FindElement(
                                By.XPath(addonXpath + "//strong")).Text, "NEW", string.Empty), "UPDATE", string.Empty);
                            if (addonName.Equals("WhoisGuard", StringComparison.CurrentCultureIgnoreCase))
                            {
                                var divCount = BrowserInit.Driver.FindElements(By.XPath(addonXpath + "/div")).Count;
                                if (divCount < 2)
                                {
                                    var whoisName = Regex.Replace(Regex.Replace(
                                        Regex.Replace(
                                            BrowserInit.Driver.FindElement(By.XPath(addonXpath + "/div"))
                                                .GetAttribute("outerText")
                                                .Trim(), "\r\n", " "), "NEW", string.Empty), "UPDATE", string.Empty);
                                    string[] whoisNamesplit = whoisName.Split(' ');
                                    whoisNamesplit = whoisNamesplit.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                                    object[] whoisNamesplitDomain = whoisNamesplit.Where(x => x.Contains(".")).ToArray();
                                    var cultureInfoq = new CultureInfo("en-US");
                                    domainNameinSc = string.Format(cultureInfoq, "{0:C}", whoisNamesplitDomain);
                                    whoisGuardDuration =
                                        BrowserInit.Driver.FindElement(
                                            By.XPath(addonXpath + "//div[contains(@class,'updateCartDuration')]/span"))
                                            .Text.Trim()
                                            .ToLower();
                                    var ulcount = BrowserInit.Driver.FindElements(By.XPath(addonXpath + "/ul"));
                                    foreach (var whoisAutoRenewToggle1 in from li in ulcount where li.GetAttribute(UiConstantHelper.AttributeClass).Contains("toggles") select li.FindElement(By.TagName("input")).Selected into whoisAutoRenew select whoisAutoRenew ? "ON" : "OFF")
                                    {
                                        whoisGuardAutoRenewStatus = whoisAutoRenewToggle1.Trim();
                                    }
                                    whoisGuardPrice = Convert.ToDecimal(Regex.Replace(
                                                        BrowserInit.Driver.FindElement(By.XPath(addonXpath + "//div[@class='price']/span[contains(@class,'amount')]")).Text,
                                                        @"[^\d..][^\w\s]*", string.Empty).Trim());
                                }
                                else
                                {
                                    Thread.Sleep(5000);
                                    if (
                                        BrowserInit.Driver.FindElement(By.XPath(addonXpath + "/*[1]"))
                                            .TagName.Equals("div"))
                                    {
                                        Assert.IsTrue(
                                            BrowserInit.Driver.FindElement(By.XPath(addonXpath + "/*[1]"))
                                                .Text.Contains("."));
                                        shoppingCartItemsDic.Add(
                                            EnumHelper.HostingKeys.ProductDomainName.ToString(),
                                            BrowserInit.Driver.FindElement(By.XPath(addonXpath + "/*[1]"))
                                                .Text.Replace("for", "")
                                                .Trim());
                                    }
                                    var whoisStatusToggle =
                                             BrowserInit.Driver.FindElement(
                                                 By.XPath(addonXpath + "/ul/li[1]/div/input")).Selected;
                                    if (whois == "yes")
                                    {
                                        if (whoisStatusToggle)
                                            BrowserInit.Driver.FindElement(
                                                By.XPath(addonXpath + "/ul/li[1]/div")).Click();
                                        Func<IWebDriver, bool> testCondition = x =>
                                            BrowserInit.Driver.FindElement(
                                                By.XPath(addonXpath + "/ul/li[1]/div/input")).Selected == false;
                                        var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(100));
                                        wait.Until(testCondition);
                                        Assert.IsTrue(BrowserInit.Driver.FindElement(
                                            By.XPath(addonXpath + "/ul/li[1]/div/input")).Selected == false);
                                    }
                                    whoisStatusToggle =
                                        BrowserInit.Driver.FindElement(
                                            By.XPath(addonXpath + "/ul/li[1]/div/input")).Selected;
                                    var whoisToggle = whoisStatusToggle ? "ON" : "OFF";
                                    whoisGuardstatus = whoisToggle.Trim();
                                    if (whoisGuardstatus.Equals("OFF", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        Thread.Sleep(3000);
                                        var whoisAutorenewStatusToggle1 =
                                            BrowserInit.Driver.FindElement(
                                                By.XPath(addonXpath + "/ul/li[2]/div/input")).Selected;
                                        var whoisAutoRenewToggle1 = whoisAutorenewStatusToggle1 ? "ON" : "OFF";
                                        Assert.IsTrue(
                                            whoisAutoRenewToggle1.Equals("OFF",
                                                StringComparison.CurrentCultureIgnoreCase),
                                            "At shopping cart page for " + domainNameinSc +
                                            " WhoisGuard status Toggle is in Disable state, but WhoisGaurd Auto renews is in a " +
                                            whoisAutoRenewToggle1 + " state which is wrong behavior.");
                                    }
                                    whoisGuardDuration = Regex.Replace(BrowserInit.Driver.FindElement(
                                        By.XPath("(" + addonXpath + "//span)[1]")).Text, "subscription", string.Empty)
                                        .Trim();
                                    var whoisAutorenewStatusToggle =
                                        BrowserInit.Driver.FindElement(
                                            By.XPath(addonXpath + "/ul[@class='toggles']/li[2]/div/input")).Selected;
                                    var whoisAutoRenewToggle = whoisAutorenewStatusToggle ? "ON" : "OFF";
                                    whoisGuardAutoRenewStatus = whoisAutoRenewToggle;
                                    if (!whoisGuardstatus.Equals("ON", StringComparison.CurrentCultureIgnoreCase))
                                        continue;
                                    var whoisguardPriceDivCount =
                                        BrowserInit.Driver.FindElements(
                                            By.XPath(addonXpath + "/div[contains(@class,'price active_price')]/*"))
                                            .Count;
                                    for (int whoisEle = 1; whoisEle <= whoisguardPriceDivCount; whoisEle++)
                                    {
                                        var whoisEleXpath = "(" + addonXpath +
                                                            "/div[contains(@class,'price active_price')]/*)[" +
                                                            whoisEle +
                                                            "]";
                                        var whoisEleTagName =
                                            BrowserInit.Driver.FindElement(By.XPath(whoisEleXpath)).TagName;
                                        if (whoisEleTagName.Equals("s", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            whoisGuardRentalPrice = Convert.ToDecimal(Regex.Replace(
                                                BrowserInit.Driver.FindElement(By.XPath(whoisEleXpath)).Text,
                                                @"[^\d..][^\w\s]*", string.Empty).Trim());
                                        }
                                        else if (whoisEleTagName.Equals("span",
                                            StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            var priceSpanClassName =
                                                BrowserInit.Driver.FindElement(By.XPath(whoisEleXpath))
                                                    .GetAttribute(UiConstantHelper.AttributeClass);
                                            if (priceSpanClassName.Equals("amount",
                                                StringComparison.CurrentCultureIgnoreCase))
                                            {
                                                whoisGuardPrice = Convert.ToDecimal(Regex.Replace(
                                                    BrowserInit.Driver.FindElement(By.XPath(whoisEleXpath)).Text,
                                                    @"[^\d..][^\w\s]*", string.Empty).Trim());
                                            }
                                        }
                                    }
                                }
                            }
                            else if (addonName.IndexOf("PremiumDNS", StringComparison.CurrentCultureIgnoreCase) >= 0)
                            {
                                var divCount = BrowserInit.Driver.FindElements(By.XPath(addonXpath + "/div")).Count;
                                if (divCount < 2)
                                {
                                    var premiumDnsxpath = Regex.Replace(Regex.Replace(
                                        Regex.Replace(
                                            BrowserInit.Driver.FindElement(By.XPath(addonXpath + "/div"))
                                                .GetAttribute("outerText")
                                                .Trim(), "\r\n", " "), "NEW", string.Empty), "UPDATE", string.Empty);
                                    string[] premiumDnsSplit = premiumDnsxpath.Split(' ');
                                    premiumDnsSplit = premiumDnsSplit.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                                    object[] premiumDnsDomainName = premiumDnsSplit.Where(x => x.Contains(".")).ToArray();
                                    var cultureInfoq = new CultureInfo("en-US");
                                    domainNameinSc = string.Format(cultureInfoq, "{0:C}", premiumDnsDomainName);
                                    premiumDnsDuration =
                                      BrowserInit.Driver.FindElement(
                                          By.XPath(addonXpath + "//div[contains(@class,'updateCartDuration')]/span"))
                                          .Text.Trim()
                                          .ToLower();
                                    var ulcount = BrowserInit.Driver.FindElements(By.XPath(addonXpath + "/ul"));
                                    foreach (var premiumDnsAutorenewToggle in from li in ulcount where li.GetAttribute(UiConstantHelper.AttributeClass).Contains("toggles") select li.FindElement(By.TagName("input")).Selected into djjh select djjh ? "ON" : "OFF")
                                    {
                                        premiumDnsAutorenewStatus = premiumDnsAutorenewToggle.Trim();
                                    }
                                    premiumDnsPrice = Convert.ToDecimal(Regex.Replace(
                                                       BrowserInit.Driver.FindElement(By.XPath(addonXpath + "//div[@class='price']/span[@class='amount']")).Text,
                                                       @"[^\d..][^\w\s]*", string.Empty).Trim());
                                }
                                else
                                {
                                    var premiumDnsStatusToggle =
                                        BrowserInit.Driver.FindElement(
                                            By.XPath(addonXpath + "/ul/li[1]/div/input")).Selected;
                                    if (premiumDns == "yes")
                                    {
                                        if (premiumDnsStatusToggle == false)
                                            BrowserInit.Driver.FindElement(
                                                By.XPath(addonXpath + "/ul/li[1]/div")).Click();
                                        Func<IWebDriver, bool> testCondition = x =>
                                            BrowserInit.Driver.FindElement(
                                                By.XPath(addonXpath + "/ul/li[1]/div/input")).Selected;
                                        var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(100));
                                        wait.Until(testCondition);
                                        Assert.IsTrue(BrowserInit.Driver.FindElement(
                                            By.XPath(addonXpath + "/ul/li[1]/div/input")).Selected);
                                    }
                                    premiumDnsStatusToggle =
                                        BrowserInit.Driver.FindElement(
                                            By.XPath(addonXpath + "/ul/li[1]/div/input")).Selected;
                                    var premiumDnsToggle = premiumDnsStatusToggle ? "ON" : "OFF";
                                    premiumDnsStatus = premiumDnsToggle.Trim();
                                    if (premiumDnsStatus.Equals("OFF", StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        var premiumDnsAutorenewStatusToggle1 =
                                            BrowserInit.Driver.FindElement(
                                                By.XPath(addonXpath + "/ul/li[2]/div/input")).Selected;
                                        var premiumDnsAutoRenewToggle1 = premiumDnsAutorenewStatusToggle1 ? "ON" : "OFF";
                                        Assert.IsTrue(
                                            premiumDnsAutoRenewToggle1.Equals("OFF",
                                                StringComparison.CurrentCultureIgnoreCase),
                                            "At shopping cart page for " + domainNameinSc +
                                            " WhoisGuard status Toggle is in Disable state, but WhoisGaurd Auto renews is in " +
                                            premiumDnsAutoRenewToggle1 + " state which is wrong behavior.");
                                    }
                                    premiumDnsDuration = Regex.Replace(BrowserInit.Driver.FindElement(
                                        By.XPath(addonXpath + "/span")).Text, "subscription", string.Empty).Trim();
                                    var prmiumdnsAutorenewStatusToggle =
                                        BrowserInit.Driver.FindElement(
                                            By.XPath(addonXpath + "/ul/li[2]/div/input")).Selected;
                                    var premiumDnsAutoRenewToggle = prmiumdnsAutorenewStatusToggle ? "ON" : "OFF";
                                    premiumDnsAutorenewStatus = premiumDnsAutoRenewToggle;
                                    if (!premiumDnsStatus.Equals("ON", StringComparison.CurrentCultureIgnoreCase))
                                        continue;
                                    var premiumDnsPriceDivCount =
                                        BrowserInit.Driver.FindElements(
                                            By.XPath(addonXpath + "/div[contains(@class,'price active_price')]/*"))
                                            .Count;
                                    for (int dnsEle = 1; dnsEle <= premiumDnsPriceDivCount; dnsEle++)
                                    {
                                        var dnsEleXpath = addonXpath +
                                                          "/div[contains(@class,'price active_price')]"; //"[@xpath='" + dnsEle + "']";
                                        var dnsEleTagName =
                                            BrowserInit.Driver.FindElement(By.XPath(dnsEleXpath)).TagName;
                                        if (dnsEleTagName.Equals("s", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            premiumDnsRentealPrice = Convert.ToDecimal(Regex.Replace(
                                                BrowserInit.Driver.FindElement(By.XPath(dnsEleTagName)).Text,
                                                @"[^\d..][^\w\s]*", string.Empty).Trim());
                                        }
                                        else if (dnsEleTagName.Equals("span", StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            var priceSpanClassName =
                                                BrowserInit.Driver.FindElement(By.XPath(dnsEleXpath))
                                                    .GetAttribute(UiConstantHelper.AttributeClass);
                                            if (priceSpanClassName.Equals("amount",
                                                StringComparison.CurrentCultureIgnoreCase))
                                            {
                                                premiumDnsPrice = Convert.ToDecimal(Regex.Replace(
                                                    BrowserInit.Driver.FindElement(By.XPath(dnsEleXpath)).Text,
                                                    @"[^\d..][^\w\s]*", string.Empty).Trim());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!cartItemClassName.Trim().Equals("cart-item")) continue;
                    {
                        foreach (var className in BrowserInit.Driver.FindElements(By.XPath(cartItemdivCountXpath + "/*")))
                        {
                            if (className.GetAttribute(UiConstantHelper.AttributeClass).Equals(string.Empty))
                            {
                                var productstrongText = Regex.Replace(
                                    Regex.Replace(className.FindElement(By.TagName("strong")).Text, "UPDATE", string.Empty),
                                    "NEW", string.Empty).Trim();
                              if (BrowserInit.Driver.Url.Contains("marketplace") )
                                {
                                    var productDomainName1 = Regex.Replace(Regex.Replace(Regex.Replace(
                                 Regex.Replace(BrowserInit.Driver.FindElement(By.XPath(cartItemdivCountXpath + "/div")).GetAttribute("outerText").Trim(), "\r\n", " "), "NEW", string.Empty), "UPDATE", string.Empty), "Market Place", string.Empty);
                                    string[] productDomainNamesplit1 = productDomainName1.Split(' ');
                                    productDomainNamesplit1 = productDomainNamesplit1.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                                    object[] productDomainNameTxt1 = productDomainNamesplit1.Where(x => x.Contains(".")).ToArray();
                                    domainNameinSc = productDomainNameTxt1[0].ToString().Trim();
                                    domainPrice =
                                        Convert.ToDecimal(Regex.Replace(productDomainNameTxt1[1].ToString(),
                                            @"[^\d..][^\w\s]*", string.Empty).Trim());
                                    
                                    continue;
                                }
                                if (!productstrongText.Contains("WhoisGuard") || !productstrongText.Contains("PremiumDNS"))
                                {
                                    productName = productstrongText;
                                }
                                if (!className.Text.Contains("for")) continue;
                                var productDomainName = Regex.Replace(Regex.Replace(Regex.Replace(
                                    Regex.Replace(BrowserInit.Driver.FindElement(By.XPath(cartItemdivCountXpath + "/div")).GetAttribute("outerText").Trim(), "\r\n", " "), "NEW", string.Empty), "UPDATE", string.Empty), "Market Place", string.Empty);
                                string[] productDomainNamesplit = productDomainName.Split(' ');
                                productDomainNamesplit = productDomainNamesplit.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                                object[] productDomainNameTxt = productDomainNamesplit.Where(x => x.Contains(".")).ToArray();
                                var cultureInfoq = new CultureInfo("en-US");
                                domainNameinSc = string.Format(cultureInfoq, "{0:C}", productDomainNameTxt);
                                productDuration =
                                    BrowserInit.Driver.FindElement(
                                        By.XPath(cartItemdivCountXpath + "//div[contains(@class,'updateCartDuration')]/span"))
                                        .Text.Trim()
                                        .ToLower();
                                var price = className.FindElement(By.ClassName("price"));
                                {
                                    productNameprice =
                                        Convert.ToDecimal(Regex.Replace(price.FindElement(By.TagName("span")).Text,
                                            @"[^\d..][^\w\s]*", string.Empty).Trim());
                                }
                            }
                            else if (className.GetAttribute(UiConstantHelper.AttributeClass).Contains("small"))
                            {
                                string[] datacenter = className.FindElement(By.TagName("li")).Text.Trim().Split(new[] { "\r\n" }, StringSplitOptions.None);
                                productDataCenter = datacenter[0].Trim();
                                productDatacenterPrice = Convert.ToDecimal(Regex.Replace(datacenter[1],
                                    @"[^\d..][^\w\s]*", string.Empty).Trim());
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(domainNameinSc))
                    shoppingCartItemsDic.Add(EnumHelper.DomainKeys.DomainName.ToString(), domainNameinSc);
                if (!string.IsNullOrEmpty(domainDuration))
                    shoppingCartItemsDic.Add(EnumHelper.DomainKeys.DomainDuration.ToString(), domainDuration);
                if (!string.IsNullOrEmpty(domainAutoRenewStatus))
                    shoppingCartItemsDic.Add(EnumHelper.ShoppingCartKeys.DomainAutoRenewStatus.ToString(), domainAutoRenewStatus);
                if (!string.IsNullOrEmpty(domainPrice.ToString(CultureInfo.InvariantCulture)))
                    shoppingCartItemsDic.Add(EnumHelper.DomainKeys.DomainPrice.ToString(), domainPrice.ToString(CultureInfo.InvariantCulture));
                if (!string.IsNullOrEmpty(icannFeePrice.ToString(CultureInfo.InvariantCulture)))
                    shoppingCartItemsDic.Add(EnumHelper.CartWidget.IcanPrice.ToString(), icannFeePrice.ToString(CultureInfo.InvariantCulture));
                if (!string.IsNullOrEmpty(whoisGuardstatus))
                    shoppingCartItemsDic.Add(EnumHelper.ShoppingCartKeys.WhoisGuardForDomainStatus.ToString(), whoisGuardstatus);
                if (!string.IsNullOrEmpty(whoisGuardDuration))
                    shoppingCartItemsDic.Add(EnumHelper.ShoppingCartKeys.WhoisGuardForDomainDuration.ToString(), whoisGuardDuration);
                if (!string.IsNullOrEmpty(whoisGuardAutoRenewStatus))
                    shoppingCartItemsDic.Add(EnumHelper.ShoppingCartKeys.WhoisGuardForDomainAutoRenewStatus.ToString(), whoisGuardAutoRenewStatus);
                if (!string.IsNullOrEmpty(whoisGuardPrice.ToString(CultureInfo.InvariantCulture)))
                    shoppingCartItemsDic.Add(EnumHelper.ShoppingCartKeys.WhoisGuardForDomainPrice.ToString(), whoisGuardPrice.ToString(CultureInfo.InvariantCulture));
                if (whoisGuardRentalPrice != 0.00M)
                    shoppingCartItemsDic.Add(EnumHelper.ShoppingCartKeys.WhoisGuardForDomainRentalPrice.ToString(), whoisGuardRentalPrice.ToString(CultureInfo.InvariantCulture));
                if (!string.IsNullOrEmpty(premiumDnsStatus))
                    shoppingCartItemsDic.Add(EnumHelper.ShoppingCartKeys.PremiumDnsForDomainStatus.ToString(), premiumDnsStatus);
                if (!string.IsNullOrEmpty(premiumDnsDuration))
                    shoppingCartItemsDic.Add(EnumHelper.ShoppingCartKeys.PremiumDnsForDomainDuration.ToString(), premiumDnsDuration);
                if (!string.IsNullOrEmpty(premiumDnsAutorenewStatus))
                    shoppingCartItemsDic.Add(EnumHelper.ShoppingCartKeys.PremiumDnsForDomainAutoRenewStatus.ToString(), premiumDnsAutorenewStatus);
                if (premiumDnsRentealPrice != 0.00M)
                    shoppingCartItemsDic.Add(EnumHelper.ShoppingCartKeys.PremiumDnsForDomainRentalprice.ToString(), premiumDnsRentealPrice.ToString(CultureInfo.InvariantCulture));
                if (!string.IsNullOrEmpty(premiumDnsPrice.ToString(CultureInfo.InvariantCulture)))
                    if (premiumDnsPrice > 0.00M)
                    {
                        foreach (var cartdic in cartWidgetList)
                        {
                            if (cartdic.ContainsKey(EnumHelper.CartWidget.SubTotal.ToString()) && !cartdic.ContainsKey(EnumHelper.HostingKeys.ProductPrice.ToString()))
                            {
                                var oldprice = cartdic[EnumHelper.CartWidget.SubTotal.ToString()];
                                cartdic.Remove(EnumHelper.CartWidget.SubTotal.ToString());
                                cartdic.Add(EnumHelper.CartWidget.SubTotal.ToString(), (decimal.Parse(oldprice) + premiumDnsPrice).ToString(CultureInfo.InvariantCulture));
                            }
                        }
                    }
                shoppingCartItemsDic.Add(EnumHelper.ShoppingCartKeys.PremiumDnsForDomainPrice.ToString(), premiumDnsPrice.ToString(CultureInfo.InvariantCulture));
                if (domainRentalprice != 0.00M)
                    shoppingCartItemsDic.Add(EnumHelper.DomainKeys.DomainRetailPrice.ToString(), domainRentalprice.ToString(CultureInfo.InvariantCulture));
                if (!string.IsNullOrEmpty(promotionCode))
                    shoppingCartItemsDic.Add(EnumHelper.DomainKeys.DomainNamePromotionCode.ToString(), promotionCode);
                if (!string.IsNullOrEmpty(productName))
                    shoppingCartItemsDic.Add(EnumHelper.HostingKeys.ProductName.ToString(), productName);
                if (!string.IsNullOrEmpty(productNameprice.ToString(CultureInfo.InvariantCulture)))
                    shoppingCartItemsDic.Add(EnumHelper.HostingKeys.ProductPrice.ToString(), productNameprice.ToString(CultureInfo.InvariantCulture));
                if (!string.IsNullOrEmpty(productDuration.ToString(CultureInfo.InvariantCulture)))
                    shoppingCartItemsDic.Add(EnumHelper.HostingKeys.ProductDuration.ToString(), productDuration.ToString(CultureInfo.InvariantCulture));
                if (!string.IsNullOrEmpty(productDataCenter.ToString(CultureInfo.InvariantCulture)))
                    shoppingCartItemsDic.Add(EnumHelper.HostingKeys.Datacenter.ToString(), productDataCenter.ToString(CultureInfo.InvariantCulture));
                if (!string.IsNullOrEmpty(productDatacenterPrice.ToString(CultureInfo.InvariantCulture)))
                    shoppingCartItemsDic.Add(EnumHelper.HostingKeys.DatacenterPrice.ToString(), productDatacenterPrice.ToString(CultureInfo.InvariantCulture));
                shoppingcartItemList.Add(shoppingCartItemsDic);
                domainNameinSc = string.Empty;
                domainDuration = string.Empty;
                domainAutoRenewStatus = string.Empty;
                domainPrice = 0.00M;
                icannFeePrice = 0.00M;
                whoisGuardstatus = string.Empty;
                whoisGuardDuration = string.Empty;
                whoisGuardAutoRenewStatus = string.Empty;
                whoisGuardPrice = 0.00M;
                whoisGuardRentalPrice = 0.00M;
                premiumDnsStatus = string.Empty;
                premiumDnsDuration = string.Empty;
                premiumDnsAutorenewStatus = string.Empty;
                premiumDnsRentealPrice = 0.00M;
                premiumDnsPrice = 0.00M;
                domainRentalprice = 0.00M;
                promotionCode = string.Empty;
            }
            var subTotalInSc = Convert.ToDecimal(
                Regex.Replace(
                    PageInitHelper<ShoppingCartPageFactory>.PageInit.SubtotalTxt.Text,
                    @"[^\d..][^\w\s]*", string.Empty).Trim());
            return shoppingcartItemList;
        }
        private void MessageAndAlertVerification()
        {
            //Hero banner image Text
            var cartText = PageInitHelper<ShoppingCartPageFactory>.PageInit.ShoppingCartHeroBannerTxt.Text;
            Assert.AreSame(cartText, UiConstantHelper.ShoppingCartHeroBannerText, "At shopping cart page hero image headline text should be '" + UiConstantHelper.ShoppingCartHeroBannerText + "', but actual headline text shown as " + cartText);
            //Check the added items are present or missing in Cart
            StringAssert.Contains(PageInitHelper<ShoppingCartPageFactory>.PageInit.ProductPresent.GetAttribute("class"), "At shopping cart page cart items are grid shown as empty ");
            //Sucess Alert Box missing
            var alertMsgClassAttr = PageInitHelper<ShoppingCartPageFactory>.PageInit.AlertMessageDiv.GetAttribute("class");
            StringAssert.Contains(alertMsgClassAttr, "force-hide", "At shopping cart page 'The item was successfully added/updated to the cart.' alert message is not shown at the top of the page");
            //Verify Mesage contains in Alert box 
            var cartAlertMsg = PageInitHelper<ShoppingCartPageFactory>.PageInit.AlertMessage.FindElement(By.TagName("p")).Text;
            Assert.IsNotEmpty(cartAlertMsg, "At shopping cart page alert message is shown empty content at the top of the page");
        }
    }
}