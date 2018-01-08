using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using NamecheapUITests.PagefactoryObject.ValidationPagefactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using OpenQA.Selenium;
namespace NamecheapUITests.PageObject.ValidationPages
{
    public class AddSslShoppingCartItems : IShoppingCartValidation
    {
        public List<SortedDictionary<string, string>> AddShoppingCartItemsToDic(List<SortedDictionary<string, string>> cartWidgetList, string whois, string premiumDns)
        {
            var certificateQtyinSc = string.Empty;
            var certificateDurationinSc = string.Empty;
            var certificatePriceinSc = 0.00M;
            var shoppingcartItemList = new List<SortedDictionary<string, string>>();
            const string productGroupsXpath = "(.//*[@class='product-group'])";
            var productGroups = BrowserInit.Driver.FindElements(By.XPath(productGroupsXpath));
            var i = 0;
            foreach (var productGroup in productGroups)
            {
                i = i + 1;
                var shoppingCartItemsDic = new SortedDictionary<string, string>();
                var certificateNameinSc = Regex.Replace(productGroup.FindElement(By.TagName("strong")).Text.Trim(), "UPDATE", string.Empty);
                var cerQty = productGroup.FindElements(By.ClassName("qty")).Count > 0;
                if (cerQty)
                {
                    certificateQtyinSc = productGroup.FindElement(By.XPath("//*[contains(@class,'qty')]/input")).GetAttribute(UiConstantHelper.AttributeValue).Trim();
                }
                var certificateDurationcount = productGroup.FindElement(By.XPath("//*[contains(@class,'Duration')]"));
                foreach (var certificateDuration in certificateDurationcount.FindElements(By.TagName("span")).Where(certificateDuration => certificateDuration.Text.Contains("Year")))
                {
                    certificateDurationinSc = certificateDuration.Text.Trim();
                    break;
                }
                if (BrowserInit.Driver.FindElements(By.XPath("(" + productGroupsXpath + "[" + i + "]/../div/div/*)")).Count == 4)
                {
                    var extraCount =
                        BrowserInit.Driver.FindElements(
                            By.XPath("(" + productGroupsXpath + "[" + i + "]/../div/div/*)[2]//*"));
                    foreach (var extradomainPrice in from extraDomain in extraCount where extraDomain.GetAttribute(UiConstantHelper.AttributeClass).Contains("price") select decimal.Parse(Regex.Replace(extraDomain.Text, @"[^\d..][^\w\s]*", string.Empty).Trim()))
                    {
                        shoppingCartItemsDic.Add("Extra domain price", extradomainPrice.ToString(CultureInfo.InvariantCulture));
                        break;
                    }
                }
                var cerPriceCount =
                    BrowserInit.Driver.FindElements(By.XPath("(" + productGroupsXpath + "[" + i + "]//div[3]/span)"));
                foreach (var cerPrice in cerPriceCount.Where(cerPrice => cerPrice.GetAttribute(UiConstantHelper.AttributeClass).Contains("amount")))
                {
                    certificatePriceinSc = decimal.Parse(Regex.Replace(cerPrice.Text, @"[^\d..][^\w\s]*", string.Empty).Trim());
                }
                shoppingCartItemsDic.Add(EnumHelper.Ssl.CertificateName.ToString(), certificateNameinSc);
                shoppingCartItemsDic.Add(EnumHelper.Ssl.CertificatePrice.ToString(), certificatePriceinSc.ToString(CultureInfo.InvariantCulture));
                shoppingCartItemsDic.Add(EnumHelper.Ssl.CertificateDuration.ToString(), certificateDurationinSc);
                if (certificateQtyinSc != string.Empty)
                {
                    shoppingCartItemsDic.Add("Certificate Qty", certificateQtyinSc);
                }
                shoppingcartItemList.Add(shoppingCartItemsDic);
            }
            AVerify verifingTwoListOfDic = new VerifyData();
            verifingTwoListOfDic.VerifyTwoListOfDic(shoppingcartItemList, cartWidgetList);
            AMerge mergeTWoListOfDic = new MergeData();
            var mergedScAndCartWidgetDic = mergeTWoListOfDic.MergingTwoListOfDic(shoppingcartItemList, cartWidgetList);
            PageInitHelper<ShoppingCartPageFactory>.PageInit.ConfirmOrderBtn.Click();
            return mergedScAndCartWidgetDic;
        }
    }
}