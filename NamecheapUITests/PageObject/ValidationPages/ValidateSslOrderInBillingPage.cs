using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NUnit.Framework;
using OpenQA.Selenium;
namespace NamecheapUITests.PageObject.ValidationPages
{
    public class ValidateSslOrderInBillingPage : IOrdersValidation
    {
        public void VerifyPurchasedOrderInBillingHistoryPage(
          List<SortedDictionary<string, string>> listOfDicNameToBeVerified)
        {
            var purchasedItemNumber = PageInitHelper<ValidateSslOrderInBillingPage>.PageInit.OrderSummaryPageVerification(
                 listOfDicNameToBeVerified);
            var orderdetaipageList =
                    PageInitHelper<ValidateSslOrderInBillingPage>.PageInit.AddOrderDetailPageItemsTodic(
                        purchasedItemNumber, listOfDicNameToBeVerified);
            AVerify verifingTwoListOfDic = new VerifyData();
            verifingTwoListOfDic.VerifyTwoListOfDic(orderdetaipageList, listOfDicNameToBeVerified);
        }
        internal List<SortedDictionary<string, string>> AddOrderDetailPageItemsTodic(string purchasedItemNumber, List<SortedDictionary<string, string>> mergedScAndCartWidgetListWithOrderNum)
        {
            var orderDetailPageDomainItemslist = new List<SortedDictionary<string, string>>();
            var orderidtxt = string.Empty;
            var productCount = BrowserInit.Driver.FindElements(By.XPath("(.//*[contains(@class,'item-start')])"));
            foreach (var orderid in mergedScAndCartWidgetListWithOrderNum)
            {
                orderidtxt = orderid[EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString()];
                break;
            }
            Assert.IsTrue(productCount.Count.ToString().Trim().Equals(purchasedItemNumber), "The Purchased items count shown on order page for the order number " + orderidtxt + ", but Purchased items in the detail page are mismatched");
            const string productDrXpath = "(.//*[contains(@class,'item-start')]/td/..)";
            var productDr =
                  BrowserInit.Driver.FindElements(
                      By.XPath(
                          productDrXpath));
            for (var pgroup = 1; pgroup <= productDr.Count; pgroup++)
            {
                var certificateName = Regex.Replace(BrowserInit.Driver.FindElement(By.XPath(productDrXpath + "[" + pgroup + "]//h3")).Text, "Comodo", string.Empty).Trim();
                var items = BrowserInit.Driver.FindElement(By.XPath(productDrXpath + "[" + pgroup + "]/td[2]/p")).Text.Trim();
                var certificateDuration = BrowserInit.Driver.FindElement(By.XPath(productDrXpath + "[" + pgroup + "]/td[3]/p")).Text.Trim();
                var price = BrowserInit.Driver.FindElement(By.XPath(productDrXpath + "[" + pgroup + "]/td[4]/p")).GetAttribute("innerHTML");
                var amount = price.Substring(price.LastIndexOf("$", StringComparison.Ordinal));
                var certificatePrice = decimal.Parse(Regex.Replace(amount, @"[^\d..][^\w\s]*", "").Trim());
                var chargedAmount = decimal.Parse(Regex.Replace(BrowserInit.Driver.FindElement(By.XPath(productDrXpath + "[" + pgroup + "]/td[5]/p")).Text, @"[^\d..][^\w\s]*", "").Trim());
                var orderDetailPageDomainItemsDic = new SortedDictionary<string, string>();
                if (
                  BrowserInit.Driver.FindElement(By.XPath("(.//*/tbody/tr)[2]"))
                      .GetAttribute(UiConstantHelper.AttributeClass)
                      .Contains("details"))
                {
                    var extradomainPrice = decimal.Parse(Regex.Replace(BrowserInit.Driver.FindElement(By.XPath("(.//*/tbody/tr)[2]/td[2]/p")).Text, @"[^\d..][^\w\s]*", "").Trim());
                    orderDetailPageDomainItemsDic.Add("Extra Domain Price", extradomainPrice.ToString(CultureInfo.InvariantCulture));
                }
                orderDetailPageDomainItemsDic.Add(EnumHelper.Ssl.CertificateName.ToString(), certificateName);
                orderDetailPageDomainItemsDic.Add("TotalItem", items);
                orderDetailPageDomainItemsDic.Add(EnumHelper.Ssl.CertificateDuration.ToString(), certificateDuration);
                orderDetailPageDomainItemsDic.Add(EnumHelper.Ssl.CertificatePrice.ToString(), certificatePrice.ToString(CultureInfo.InvariantCulture));
                Assert.AreEqual(certificatePrice, chargedAmount, "In order detail page for the ssl certificate " + certificateName + " total amount and charged amount is mismatching, the total amount in the grid shown as " + certificatePrice + ", but the charged price shown as " + chargedAmount);
                orderDetailPageDomainItemslist.Add(orderDetailPageDomainItemsDic);
            }
            foreach (var subtotal in orderDetailPageDomainItemslist)
            {
                var subtotalamount = 0.00M;
                subtotalamount =
                    subtotalamount + decimal.Parse(subtotal[EnumHelper.Ssl.CertificatePrice.ToString()]);
                if (subtotal.ContainsKey("Extra Domain Price"))
                    subtotalamount = subtotalamount + decimal.Parse(subtotal["Extra Domain Price"]);
                var subTotalAmountInOrderPage = decimal.Parse(Regex.Replace(
                    BrowserInit.Driver.FindElement(By.XPath("(.//*[contains(@class,'subtotal')]/td/..)/td[2]")).Text,
                    @"[^\d..][^\w\s]*", "").Trim());
                var subTotalChargedAmountInOrderPage = decimal.Parse(Regex.Replace(
                    BrowserInit.Driver.FindElement(By.XPath("(.//*[contains(@class,'subtotal')]/td/..)/td[3]")).Text,
                    @"[^\d..][^\w\s]*", "").Trim());
                Assert.AreEqual(subTotalChargedAmountInOrderPage, subtotalamount,
                    "In order detail page for the ssl certificate " + subtotal[EnumHelper.Ssl.CertificateName.ToString()]
                    + " subtotal charged amount is mismatching, the total charged amount in the grid shown as " +
                    subTotalChargedAmountInOrderPage + ", but the expeced charged amount should as " + subtotalamount);
                Assert.AreEqual(subTotalAmountInOrderPage, subtotalamount, "In order detail page for the ssl certificate " + subtotal[EnumHelper.Ssl.CertificateName.ToString()]
                    + " subtotal amount is mismatching, the total amount in the grid shown as " +
                    subTotalAmountInOrderPage + ", but the expeced subtotal should as " + subtotalamount);
            }
            return orderDetailPageDomainItemslist;
        }
        internal string OrderSummaryPageVerification(List<SortedDictionary<string, string>> listOfDicNameToBeVerified)
        {
            var purchasedItemNumber = string.Empty;
            BrowserInit.Driver.Navigate()
             .GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "/Profile/Billing/Orders"));
            foreach (var purchasedDict in listOfDicNameToBeVerified)
            {
                string orderId;
                if (purchasedDict.ContainsKey(EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString()))
                {
                    orderId = purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString()];
                }
                else
                {
                    continue;
                }
                if (
                    !PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.BillingSearchTxt.GetAttribute(
                        UiConstantHelper.AttributeClass).Contains(UiConstantHelper.NgHide))
                {
                    PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.BillingSearchInputTxt.Clear();
                    PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.BillingSearchInputTxt.SendKeys(orderId);
                    PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.BillingSearchInputTxt.SendKeys(Keys.Enter);
                    Thread.Sleep(2000);
                }
                PageInitHelper<PageNavigationHelper>.PageInit.MoveToElement(BrowserInit.Driver.FindElement(
                        By.XPath(
                            ".//*/table/tbody/tr[(contains(concat(' ',normalize-space(@class),' '),'with-extra-row'))]//td[1]//p[.='" +
                            orderId + "']")));
                var orderStatus =
                    BrowserInit.Driver.FindElement(
                        By.XPath(
                            ".//*/table/tbody/tr[(contains(concat(' ',normalize-space(@class),' '),'with-extra-row'))]//td[1]//p[.='" +
                            orderId + "']/../following-sibling::td[1]/p")).Text.Trim();
                Assert.IsTrue(orderStatus.IndexOf("Success", StringComparison.CurrentCultureIgnoreCase) >= 0,
                    "The order status shown in order history page for the order number " + purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString()] + " is mismatching expected status should be 'success', but status shown in the order history page as " + orderStatus);
                var orderPurchasedDate = DateTime.Parse(BrowserInit.Driver.FindElement(
                    By.XPath(
                        ".//*/table/tbody/tr[(contains(concat(' ',normalize-space(@class),' '),'with-extra-row'))]//td[1]//p[.='" +
                        orderId + "']/../following-sibling::td[2]/p")).Text.Trim()).ToString("MMM d, yyyy");
                Assert.IsTrue(orderPurchasedDate.Contains(DateTime.Parse(
                           purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderdateAndtime.ToString()]).ToString("MMM d, yyyy")),
                           "The purchased date and time of the items is missing in the order history page for the order number " + purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString()] + " Expected date and time should be " + purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderdateAndtime.ToString()] + ", but actual value shown as " + orderPurchasedDate);
                purchasedItemNumber = BrowserInit.Driver.FindElement(
                   By.XPath(
                       ".//*/table/tbody/tr[(contains(concat(' ',normalize-space(@class),' '),'with-extra-row'))]//td[1]//p[.='" +
                       orderId + "']/../following-sibling::td[3]/p")).Text.Trim();
                var purchasedOrderPrice = decimal.Parse(Regex.Replace(BrowserInit.Driver.FindElement(
                    By.XPath(
                        ".//*/table/tbody/tr[(contains(concat(' ',normalize-space(@class),' '),'with-extra-row'))]//td[1]//p[.='" +
                        orderId + "']/../following-sibling::td[4]/p")).Text.Trim(), @"[^\d..][^\w\s]*", ""));
                if (purchasedDict.ContainsKey(EnumHelper.CartWidget.SubTotal.ToString()))
                {
                    Assert.IsTrue(
                        purchasedOrderPrice.ToString(CultureInfo.InvariantCulture)
                            .Equals(purchasedDict[EnumHelper.CartWidget.SubTotal.ToString()]),
                        "The order total price of the items is missing in the order history page for the order number " + purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString()] + " Expected total price should be " + purchasedDict[EnumHelper.CartWidget.SubTotal.ToString()] + ", but actual value shown as " + purchasedOrderPrice);
                }
                BrowserInit.Driver.FindElement(
                    By.XPath(
                        ".//*/table/tbody/tr[(contains(concat(' ',normalize-space(@class),' '),'with-extra-row'))]//td[1]//p[.='" +
                        orderId + "']/../following-sibling::td[5]//div/a[1]")).Click();
                Thread.Sleep(3000);
                var orderDetailPageOrderId = Regex.Replace(
                    PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.OrderIdTxt.Text.Trim(),
                    @"[^\d..][^\w\s]*", "");
                Assert.IsTrue(
                    orderDetailPageOrderId.Equals(
                        purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString()]),
                    "The order id on order detail page is mismatched expected order id is " + purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString()] + ", but actual order number shown as " + orderDetailPageOrderId);
                var replaced = new List<string> { "EDT", "EST" };
                foreach (var orderDetailPageOrderDandT in from yes in replaced let time = PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.OrderIdDateTxt.Text where time.Contains(yes) select Regex.Replace(time, yes, string.Empty) into timeZone select DateTime.Parse(timeZone.Trim()).ToString("MMM d, yyyy,  hh:mm tt"))
                {
                    Assert.IsTrue(
                        orderDetailPageOrderDandT.Contains(
                            DateTime.Parse(purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderdateAndtime.ToString()].Trim()).ToString("MMM d, yyyy,  hh:mm tt")),
                        "The purchased ordered date and time at order detail page for order number " + purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString()] + "is mismatched expected order number should be " + purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderdateAndtime.ToString()] + ", but actual date and time shown as " + orderDetailPageOrderDandT);
                }
            }
            return purchasedItemNumber;
        }
    }
}