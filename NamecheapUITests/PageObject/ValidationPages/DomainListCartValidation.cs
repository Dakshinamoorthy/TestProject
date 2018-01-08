using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using NamecheapUITests.PagefactoryObject.ValidationPagefactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NUnit.Framework;
using OpenQA.Selenium;
namespace NamecheapUITests.PageObject.ValidationPages
{
    public class DomainListCartValidation : ICartValidation
    {
        public SortedDictionary<string, string> CartWidgetValidation(SortedDictionary<string, string> domainListValidation)
        {
            return null;
        }
        public List<SortedDictionary<string, string>> CartWidgetValidation(List<SortedDictionary<string, string>> searchResultDomainsList)
        {
            var subTotal = 0.00M;
            var icannfees = 0.00M;
            var domainValidity = string.Empty;
            var domainPrice = 0.00M;
            var domainName = string.Empty;
            var moreclass =
                BrowserInit.Driver.FindElement(
                    By.XPath(
                        "((.//*[contains(@class,'cart spacer-bottom')]//ul/li[@class='subtotal'] | //ul/li[@class='transfer'] | .//*[@class='cart-widget']/ul/li)/preceding-sibling::li)[last()]"))
                    .GetAttribute(UiConstantHelper.AttributeClass);
            if (moreclass.Contains("more"))
            {
                var moretext =
                 Regex.Replace(BrowserInit.Driver.FindElement(By.XPath(".//*[(contains(@class,'more'))]/a")).Text,
                     "[^0-9.]", string.Empty).Trim();
                if (!moretext.Equals(string.Empty))
                {
                    BrowserInit.Driver.FindElement(By.XPath(".//*[(contains(@class,'more'))]/a")).Click();
                }
            }
            var cartWidgetList = new List<SortedDictionary<string, string>>();
            var cartWidgetWindowItems = PageInitHelper<CartWidgetPageFactory>.PageInit.ProductListCount;
            SortedDictionary<string, string> cartWidgetDic;
            for (var itemIndex = cartWidgetWindowItems.Count + 1; itemIndex-- > 1;)
            {
                var xpath =
                    "((.//*[contains(@class,'cart spacer-bottom')]//ul/li[@class='register'] | //ul/li[@class='transfer'] | .//*[@class='cart-widget']/ul/li)[not(contains(@class,'subtotal'))][not(contains(@class,'more'))])[" +
                    itemIndex + "]";
                var ele = BrowserInit.Driver.FindElement(By.XPath(xpath));
                PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(ele);
                cartWidgetDic = new SortedDictionary<string, string>();
                var cItemClass = ele.GetAttribute("class");
                if (cItemClass.Contains(string.Empty) | cItemClass.IndexOf("register", StringComparison.CurrentCultureIgnoreCase) >= 0 | cItemClass.IndexOf("transfer", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    var widgetItemDomainName = BrowserInit.Driver.FindElement(By.XPath(xpath + "//strong")).Text;
                    if (widgetItemDomainName.Contains("."))
                    {
                        domainName = widgetItemDomainName.Trim();
                    }
                    var domainValidityDetails = BrowserInit.Driver.FindElements(By.XPath(xpath + "/ul/li/p"));
                    for (var i = 1; i <= domainValidityDetails.Count; i++)
                    {
                        var spanCount = BrowserInit.Driver.FindElements(By.XPath(xpath + "/ul/li/p/span"));
                        for (var j = 1; j <= spanCount.Count; j++)
                        {
                            var spanclass =
                                BrowserInit.Driver.FindElement(By.XPath("(" + xpath + "/ul/li/p/span)[" + j + "]"))
                                    .GetAttribute(UiConstantHelper.AttributeClass);
                            if (spanclass.Contains("item"))
                            {
                                var spantext =
                                    BrowserInit.Driver.FindElement(By.XPath("(" + xpath + "/ul/li/p/span)[" + j + "]"))
                                        .Text;
                                if (spantext.IndexOf("registration", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                {
                                    domainValidity = Regex.Replace(spantext, "registration", string.Empty).Trim();
                                }
                                else if (spantext.IndexOf("transfer", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                {
                                    domainValidity = Regex.Replace(spantext, "transfer", string.Empty).Trim();
                                }
                                else if (spantext.IndexOf("FreeDNS", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                {
                                    cartWidgetDic.Add("Domaintype", spantext.Trim());
                                }
                            }
                            else if (spanclass.Contains("price"))
                            {
                                var spantext = BrowserInit.Driver.FindElement(By.XPath("(" + xpath + "/ul/li/p/span)[" + j + "]/../span[1]")).Text;
                                var spanPrice = BrowserInit.Driver.FindElement(By.XPath("(" + xpath + "/ul/li/p/span)[" + j + "]")).Text;
                                if (spantext.IndexOf("registration", StringComparison.CurrentCultureIgnoreCase) >= 0 | spantext.IndexOf("transfer", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                {
                                    domainPrice = Convert.ToDecimal(Regex.Replace(spanPrice, @"[^\d..][^\w\s]*", string.Empty).Trim());
                                }
                                else if (spantext.Contains("ICANN fee"))
                                {
                                    icannfees = Convert.ToDecimal(Regex.Replace(spanPrice, @"[^\d..][^\w\s]*", string.Empty).Trim());
                                }
                                else if (spantext.IndexOf("FreeDNS", StringComparison.CurrentCultureIgnoreCase) >= 0)
                                {
                                    domainPrice = Convert.ToDecimal(0.00M);
                                }
                            }
                        }
                    }
                }
                cartWidgetDic.Add(EnumHelper.DomainKeys.DomainName.ToString(), domainName.Trim());
                if (domainValidity != string.Empty)
                {
                    cartWidgetDic.Add(EnumHelper.DomainKeys.DomainDuration.ToString(), domainValidity);
                }
                cartWidgetDic.Add(EnumHelper.DomainKeys.DomainPrice.ToString(), domainPrice.ToString(CultureInfo.InvariantCulture));
                cartWidgetDic.Add(EnumHelper.CartWidget.IcanPrice.ToString(), icannfees.ToString(CultureInfo.InvariantCulture));
                subTotal = subTotal + domainPrice + icannfees;
                cartWidgetList.Add(cartWidgetDic);
                domainName = string.Empty;
                domainValidity = string.Empty;
                domainPrice = 0.00M;
                icannfees = 0.00M;
            }
            cartWidgetDic = new SortedDictionary<string, string>
            {
                {EnumHelper.CartWidget.SubTotal.ToString(), subTotal.ToString(CultureInfo.InvariantCulture)}
            };
            cartWidgetList.Add(cartWidgetDic);
            var subtotaldiv = PageInitHelper<CartWidgetPageFactory>.PageInit.SubTotal;
            PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(subtotaldiv);
            var widgetSubTotalText = subtotaldiv.Text;
            var widgetSubTotal = Convert.ToDecimal(Regex.Replace(widgetSubTotalText, @"[^\d..][^\w\s]*", string.Empty).Trim());
            Assert.IsTrue(widgetSubTotal.Equals(Convert.ToDecimal(cartWidgetDic[EnumHelper.CartWidget.SubTotal.ToString()])), "Cart Widget domain subtotal mismatching with subtotal values Expected - " + Convert.ToDecimal(cartWidgetDic[EnumHelper.CartWidget.SubTotal.ToString()]) + ", but actual subtotal shown as - " + widgetSubTotal);
            AVerify verifingTwoListOfDic = new VerifyData();
            verifingTwoListOfDic.VerifyTwoListOfDic(searchResultDomainsList, cartWidgetList);
            AMerge mergeTWoListOfDic = new MergeData();
            mergeTWoListOfDic.MergingTwoListOfDic(searchResultDomainsList, cartWidgetList);
            PageInitHelper<CartWidgetPageFactory>.PageInit.ViewCartButton.Click();
            return cartWidgetList;
        }
    }
}