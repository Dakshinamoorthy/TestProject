using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NUnit.Framework;
using OpenQA.Selenium;
namespace NamecheapUITests.PageObject.ValidationPages
{
    public class ValidateHostingOrderInBillingPage : IOrdersValidation
    {
        public void VerifyPurchasedOrderInBillingHistoryPage(List<SortedDictionary<string, string>> listOfDicNameToBeVerified)
        {
            var purchasedItemNumber = PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.OrderSummaryPageVerification(listOfDicNameToBeVerified);
            var orderdetaipageList =
                  PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.AddOrderDetailPageItemsTodic(
                      purchasedItemNumber, listOfDicNameToBeVerified);
        }
        internal List<SortedDictionary<string, string>> AddOrderDetailPageItmesTodic(string purchasedItemNumber,
            List<SortedDictionary<string, string>> mergedScAndCartWidgetListWithOrderNum)
        {
            var orderDetailPageItemsList = new List<SortedDictionary<string, string>>();
            var orderDetailPageItemsDic = new SortedDictionary<string, string>();
            var productCount = BrowserInit.Driver.FindElements(By.XPath("(.//*[contains(@class,'item-start')])"));
            Assert.IsTrue(productCount.Count.ToString().Trim().Equals(purchasedItemNumber));
            foreach (var hostingProductName in BrowserInit.Driver.FindElements(By.XPath("(.//*[contains(@class,'details-start')]/preceding-sibling::tr[not(contains(@class,'details-start'))] //h3[not(contains(concat(' ',normalize-space(.),' '),'Domain'))])")))
            {
                orderDetailPageItemsDic.Add(EnumHelper.HostingKeys.ProductName.ToString(), hostingProductName.Text.Trim());
                orderDetailPageItemsList.Add(orderDetailPageItemsDic);
            }
            foreach (var dic in orderDetailPageItemsList)
            {
                var hostingPlan = dic[EnumHelper.HostingKeys.ProductName.ToString()].Trim();
                var xpath = ".//h3[.=" + hostingPlan + "']//following:td";
                var productDuration = BrowserInit.Driver.FindElement(By.XPath(xpath + "[2]//p")).Text.Trim();
                dic.Add(EnumHelper.HostingKeys.ProductDuration.ToString(), productDuration);
                var oldPrice = BrowserInit.Driver.FindElement(By.XPath(xpath + "[4]//p")).Text.Trim();
                dic.Add(EnumHelper.HostingKeys.ProductPrice.ToString(), oldPrice);
                var productStatus = BrowserInit.Driver.FindElement(By.XPath(xpath + "[5]/p/span")).Text.Trim();
                Assert.IsTrue(productStatus.Equals("Success", StringComparison.OrdinalIgnoreCase), hostingPlan + " Status is incorrect expected Success But Actual is " + productStatus);
            }
            AVerify verifingTwoListOfDic = new VerifyData();
            verifingTwoListOfDic.VerifyTwoListOfDic(orderDetailPageItemsList, mergedScAndCartWidgetListWithOrderNum);
            var domaincount =
                mergedScAndCartWidgetListWithOrderNum[Convert.ToInt32(EnumHelper.HostingKeys.ProductDomainName.ToString())].Count;
            var billingListDomainCount =
                BrowserInit.Driver.FindElements(
                    By.XPath("(.//*[contains(@class,'item-start')]//h3[contains(normalize-space(),'Domain')])")).Count;
            Assert.IsTrue(domaincount.Equals(billingListDomainCount), "Domain registration count should be equal Expected:- Domain Registration Count is " + domaincount + " But Actual In History Page  is " + billingListDomainCount);
            var whoisCountInDic = mergedScAndCartWidgetListWithOrderNum.Count(dicwhois => dicwhois.ContainsKey(EnumHelper.ShoppingCartKeys.WhoisGuardForDomainStatus.ToString()));
            Assert.IsTrue(PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.WhoisCountInProductPage.Count.Equals(whoisCountInDic), "Who is Gaurd count should be equal Expected:- Who is Gaurd Count is " + whoisCountInDic + " But Actual In History Page  is " + PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.WhoisCountInProductPage);
            var subtotalAmount =
               Regex.Replace(
                   BrowserInit.Driver.FindElement(
                       By.XPath("(.//*[contains(@class,'subtotal')]/td[contains(@class,'price')])[1]/p")).Text.Trim(),
                   @"[^\d..][^\w\s]*", "");
            var subtotalCharged =
               Regex.Replace(
                   BrowserInit.Driver.FindElement(
                       By.XPath("(.//*[contains(@class,'subtotal')]/td[contains(@class,'price')])[2]/p")).Text.Trim(),
                   @"[^\d..][^\w\s]*", "");
            var cultureInfo = new CultureInfo("en-US");
            Assert.IsTrue(string.Format(cultureInfo, "{0:C}", subtotalAmount).Equals(string.Format(cultureInfo, "{0:C}", subtotalCharged)));
            return orderDetailPageItemsList;
        }
    }
}