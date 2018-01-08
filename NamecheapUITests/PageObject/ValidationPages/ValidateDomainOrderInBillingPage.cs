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
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PageObject.ValidationPages
{
    public class ValidateDomainOrderInBillingPage : IOrdersValidation
    {
        public void VerifyPurchasedOrderInBillingHistoryPage(
            List<SortedDictionary<string, string>> listOfDicNameToBeVerified)
        {
            var purchasedItemNumber = PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.OrderSummaryPageVerification(
                 listOfDicNameToBeVerified);
            var orderdetaipageList =
                    PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.AddOrderDetailPageItemsTodic(
                        purchasedItemNumber, listOfDicNameToBeVerified);
            AVerify verifingTwoListOfDic = new VerifyData();
            verifingTwoListOfDic.VerifyTwoListOfDic(orderdetaipageList, listOfDicNameToBeVerified);
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
                }
                PageInitHelper<PageNavigationHelper>.PageInit.MoveToElement(
                    BrowserInit.Driver.FindElement(
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
        internal List<SortedDictionary<string, string>> AddOrderDetailPageItemsTodic(string purchasedItemNumber, List<SortedDictionary<string, string>> mergedScAndCartWidgetListWithOrderNum)
        {
            var orderidtxt = string.Empty;
            var orderDetailPageDomainItemsDic = new List<KeyValuePair<string, String>>();
            var productCount = BrowserInit.Driver.FindElements(By.XPath("(.//*[contains(@class,'item-start')])"));
            foreach (var orderid in mergedScAndCartWidgetListWithOrderNum)
            {
                orderidtxt = orderid[EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString()];
                break;
            }
            Assert.IsTrue(productCount.Count.ToString().Trim().Equals(purchasedItemNumber), "The Purchased items count shown on order page for the order number " + orderidtxt + ", but Purchased items in the detail page are mismatched");
            var productDr =
                  BrowserInit.Driver.FindElements(
                      By.XPath(
                          "(.//*[contains(@class,'item-start')])/td/h3[contains(normalize-space(),'Domain') or contains(normalize-space(),'Market')][./following-sibling::p]/.."));
            for (var i = 0; i < productDr.Count; i++)
            {
                var increamentI = i + 1;
                var dname = Regex.Replace(BrowserInit.Driver.FindElement(
                    By.XPath(
                        "((.//*[contains(@class,'item-start')])/td/h3[contains(normalize-space(),'Domain') or contains(normalize-space(),'Market')][./following-sibling::p]/..)[" + increamentI + "]/p[1]")).Text, "for", string.Empty).Trim();
                orderDetailPageDomainItemsDic.Add(new KeyValuePair<string, string>(EnumHelper.DomainKeys.DomainName.ToString(), dname));
            }
            var orderDetailPageItemsList = (from orderDetailPageItemsDic in orderDetailPageDomainItemsDic
                                            let orderDetailPageItemsDicKey = string.Format(orderDetailPageItemsDic.Key)
                                            let orderDetailPageItemsDicValue = string.Format(orderDetailPageItemsDic.Value)
                                            select new SortedDictionary<string, string>
                {
                    {orderDetailPageItemsDicKey, orderDetailPageItemsDicValue}
                }).ToList();
            foreach (var dic in orderDetailPageItemsList)
            {
                var xpath = "(.//*[contains(@class,'item-start')]//p[contains(concat(' ',normalize-space(),' '),'" +
                               dic[EnumHelper.DomainKeys.DomainName.ToString()] + "')])";
                foreach (var productHeading in BrowserInit.Driver.FindElements(
                    By.XPath(
                        "(.//*[contains(@class,'item-start')]//p[contains(concat(' ',normalize-space(),' '),'" +
                        dic[EnumHelper.DomainKeys.DomainName.ToString()] + "')])"))
                    .Select(
                        dicitem =>
                            BrowserInit.Driver.FindElement(By.XPath(xpath + "//preceding-sibling::h3"))
                                .Text.Trim())
                    .Select((value, i) => new { i, value }).Select(productHeadings => productHeadings.i + 1).SelectMany(subitemindex => BrowserInit.Driver.FindElements(
                          By.XPath(
                              "((.//*[contains(@class,'item-start')]//p[contains(concat(' ',normalize-space(),' '),'" +
                              dic[EnumHelper.DomainKeys.DomainName.ToString()] + "')]))[" + subitemindex + "]"))
                          .Select(
                              dicitem =>
                                  BrowserInit.Driver.FindElement(By.XPath(xpath + "[" + subitemindex + "]//preceding-sibling::h3"))
                                      .Text.Trim())))
                {
                    if (productHeading.Trim().Contains("Domain"))
                    {
                        dic.Add(EnumHelper.DomainKeys.DomainDuration.ToString(),
                            BrowserInit.Driver.FindElement(
                                By.XPath(xpath + "/preceding-sibling::h3[normalize-space()='" + productHeading +
                                         "']/../following-sibling::td[2]/p")).Text.Trim());
                        var pcount =
                            BrowserInit.Driver.FindElements(
                                By.XPath(xpath + "/preceding-sibling::h3[normalize-space()='" + productHeading +
                                         "']/../following-sibling::td[3]/p"))
                                .Count;
                        if (pcount == 2)
                        {
                            dic.Add(EnumHelper.DomainKeys.DomainRetailPrice.ToString(),
                                Regex.Replace(BrowserInit.Driver.FindElement(
                                    By.XPath(xpath + "//preceding-sibling::h3[normalize-space()='" + productHeading +
                                             "']/../following-sibling::td[3]/p"))
                                    .Text.Trim(), @"[^\d..][^\w\s]*", ""));
                        }
                        dic.Add(EnumHelper.DomainKeys.DomainPrice.ToString(),
                            Regex.Replace(BrowserInit.Driver.FindElement(
                                By.XPath(xpath + "/preceding-sibling::h3[normalize-space()='" + productHeading +
                                         "']/../following-sibling::td[4]/p[1]"))
                                .Text.Trim(), @"[^\d..][^\w\s]*", ""));
                        var tdcount = BrowserInit.Driver.FindElements(By.XPath(xpath + "//preceding-sibling::h3/../*"));
                        foreach (
                            var tdcountText in
                                tdcount.Where(
                                    tdcountText =>
                                        tdcountText.Text.IndexOf("ICANN fee", StringComparison.CurrentCultureIgnoreCase) >=
                                        0))
                        {
                            dic.Add(EnumHelper.CartWidget.IcanPrice.ToString(), Regex.Replace(
                                BrowserInit.Driver.FindElement(
                                    By.XPath(xpath +
                                             "/preceding-sibling::h3[normalize-space()='" + productHeading +
                                             "']/../following-sibling::td[4]/p[3]"))
                                    .Text.Trim(), @"[^\d..][^\w\s]*", ""));
                        }
                    }
                    if (!productHeading.Trim().Equals("PremiumDNS")) continue;
                    var actualheading =
                        BrowserInit.Driver.FindElement(
                            By.XPath(xpath + "/preceding-sibling::h3[normalize-space()='" + productHeading +
                                     "']/../following-sibling::td[2]")).Text.Trim();
                    dic.Add(EnumHelper.ShoppingCartKeys.PremiumDnsForDomainDuration.ToString(), actualheading);
                    dic.Add(EnumHelper.ShoppingCartKeys.PremiumDnsForDomainPrice.ToString(), Regex.Replace(
                        BrowserInit.Driver.FindElement(
                            By.XPath(xpath +
                                     "/preceding-sibling::h3[normalize-space()='" + productHeading +
                                     "']/../following-sibling::td[4]/p[1]"))
                            .Text.Trim(), @"[^\d..][^\w\s]*", ""));
                }
            }
            var subtotalAmount =
                Regex.Replace(
                    PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.SubTotalPriceTxt.Text.Trim(),
                    @"[^\d..][^\w\s]*", "");
            var subtotalCharged =
               Regex.Replace(
                  PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.ChargerdPriceTxt.Text.Trim(),
                   @"[^\d..][^\w\s]*", "");
            var cultureInfo = new CultureInfo("en-US");
            Assert.IsTrue(string.Format(cultureInfo, "{0:C}", subtotalAmount).Equals(string.Format(cultureInfo, "{0:C}", subtotalCharged)), "The subtotal price of the product and charged an item price are mismatched in order page expected Result as " + subtotalAmount + ", but the actual charged price shown as " + ChargerdPriceTxt);
            var whoisCountInDic = mergedScAndCartWidgetListWithOrderNum.Where(dicwhois => dicwhois.Keys.Contains(EnumHelper.ShoppingCartKeys.WhoisGuardForDomainStatus.ToString())).Count(dicwhois => dicwhois[EnumHelper.ShoppingCartKeys.WhoisGuardForDomainStatus.ToString()].Equals("ON", StringComparison.CurrentCultureIgnoreCase));
            Assert.IsTrue(PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.WhoisCountInProductPage.Count.Equals(whoisCountInDic), "The Total number of WhoisGuard in  Order Page is a mismatch, expected Whoisguard count in order history should be " + whoisCountInDic + ", but the actual count shown on the order page as " + PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.WhoisCountInProductPage.Count);
            return orderDetailPageItemsList;
        }
        #region BillingPageFactory
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'row table-sticky-filters')]/div/div[contains(concat(' ',normalize-space(@class),' '),'search')]")]
        [CacheLookup]
        internal IWebElement BillingSearchTxt { get; set; }
        [FindsBy(How = How.Name, Using = "QuickSearchValue")]
        [CacheLookup]
        internal IWebElement BillingSearchInputTxt { get; set; }
        [FindsBy(How = How.ClassName, Using = "section-title")]
        [CacheLookup]
        internal IWebElement OrderIdTxt { get; set; }
        [FindsBy(How = How.ClassName, Using = "section-title-notes")]
        [CacheLookup]
        internal IWebElement OrderIdDateTxt { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[contains(@class,'item-start')])//h3[contains(concat(' ',normalize-space(.),' '),'WhoisGuard')]")]
        [CacheLookup]
        internal IList<IWebElement> WhoisCountInProductPage { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[contains(@class,'subtotal')]/td[contains(@class,'price')])[2]/p")]
        [CacheLookup]
        internal IWebElement ChargerdPriceTxt { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[contains(@class,'subtotal')]/td[contains(@class,'price')])[1]/p")]
        [CacheLookup]
        internal IWebElement SubTotalPriceTxt { get; set; }
        #endregion
    }
}