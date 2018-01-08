using System;
using System.Collections.Generic;
using System.Globalization;
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
namespace NamecheapUITests.PageObject.ValidationPages
{
    public class ValidatingDomainTransationDetails : ITransactionValidation
    {
        public void VerifyPurchasedTransactionDetailsInBillingTransactionPage(
            List<SortedDictionary<string, string>> listOfDicNameToBeVerified)
        {
            var transcList = new List<SortedDictionary<string, string>>();
            var transctDic = new SortedDictionary<string, string>();
            foreach (var tracdetails in listOfDicNameToBeVerified)
            {
                if (tracdetails.Keys.Contains(EnumHelper.OrderSummaryKeys.PurchaseOrderdateAndtime.ToString()))
                {
                    transctDic.Add(EnumHelper.OrderSummaryKeys.PurchaseOrderdateAndtime.ToString(), tracdetails[EnumHelper.OrderSummaryKeys.PurchaseOrderdateAndtime.ToString()]);
                }
                if (tracdetails.Keys.Contains(EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString()))
                {
                    transctDic.Add(EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString(), tracdetails[EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString()]);
                }
                if (tracdetails.Keys.Contains(EnumHelper.OrderSummaryKeys.PaymentMethod.ToString()))
                {
                    transctDic.Add(EnumHelper.OrderSummaryKeys.PaymentMethod.ToString(), tracdetails[EnumHelper.OrderSummaryKeys.PaymentMethod.ToString()]);
                }
                if (tracdetails.Keys.Contains(EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString()))
                {
                    transctDic.Add(EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString(), tracdetails[EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString()]);
                }
                if (tracdetails.Keys.Contains(EnumHelper.OrderSummaryKeys.CardEndNumber.ToString()))
                {
                    transctDic.Add(EnumHelper.OrderSummaryKeys.CardEndNumber.ToString(), tracdetails[EnumHelper.OrderSummaryKeys.CardEndNumber.ToString()]);
                }
                if (tracdetails.Keys.Contains(EnumHelper.CartWidget.SubTotal.ToString()))
                {
                    transctDic.Add(EnumHelper.CartWidget.SubTotal.ToString(), tracdetails[EnumHelper.CartWidget.SubTotal.ToString()]);
                }
            }
            transcList.Add(transctDic);
            BrowserInit.Driver.Navigate()
                .GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "/Profile/Billing/Transactions"));
            foreach (var purchasedDict in transcList)
            {
                var transactionId = purchasedDict[EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString()];
                if (
                    !PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.BillingSearchTxt.GetAttribute(
                        UiConstantHelper.AttributeClass).Contains(UiConstantHelper.NgHide))
                {
                    PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.BillingSearchInputTxt.Clear();
                    PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.BillingSearchInputTxt.SendKeys(
                        transactionId);
                    PageInitHelper<ValidateDomainOrderInBillingPage>.PageInit.BillingSearchInputTxt.SendKeys(Keys.Enter);
                    Thread.Sleep(2000);
                }
                PageInitHelper<PageNavigationHelper>.PageInit.MoveToElement(BrowserInit.Driver.FindElement(
                        By.XPath(
                            ".//*[@id='transactions']/div[2]/div/div/table/tbody/tr//p[.='" +
                            transactionId + "']")));
                var transactionStatus =
                    BrowserInit.Driver.FindElement(
                        By.XPath(
                            ".//*[@id='transactions']/div[2]/div/div/table/tbody/tr//p[.='" +
                            transactionId + "']/../following-sibling::td[1]/p")).Text.Trim();
                Assert.IsTrue(transactionStatus.IndexOf("Success", StringComparison.CurrentCultureIgnoreCase) >= 0,
                    "The payment transaction status shown in transaction history page for the transaction id " +
                    purchasedDict[EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString()] +
                    " is mismatching expected status should be 'success', but status shown in the transaction history page as " +
                    transactionStatus);
                var transactionType = BrowserInit.Driver.FindElement(By.XPath(".//*[@id='transactions']/div[2]/div/div/table/tbody/tr//p[.='" +
                            transactionId + "']/../following-sibling::td[2]/p")).Text.Trim();
                Assert.IsTrue(transactionType.IndexOf("Order", StringComparison.CurrentCultureIgnoreCase) >= 0,
                "The order status shown in transaction history page for the transaction id " +
                    purchasedDict[EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString()] +
                    " is mismatching expected status should be 'Order', but status shown in the transaction history page as " +
                    transactionType);
                var paymentMethod =
                     BrowserInit.Driver.FindElement(
                         By.XPath(".//*[@id='transactions']/div[2]/div/div/table/tbody/tr//p[.='" + transactionId +
                                  "']/../following-sibling::td[3]/p")).Text.Trim();
                Assert.IsTrue(
                    paymentMethod.Equals(purchasedDict[EnumHelper.OrderSummaryKeys.PaymentMethod.ToString()],
                        StringComparison.OrdinalIgnoreCase),
                    "The payment mode shown in transaction history page for the transaction id " +
                    purchasedDict[EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString()] +
                    " is mismatching expected payment mode should be" + purchasedDict[EnumHelper.OrderSummaryKeys.PaymentMethod.ToString()] + ", but payment mode shown in the transaction history page as " +
                    paymentMethod);
                var transactionDate = DateTime.Parse(BrowserInit.Driver.FindElement(
                        By.XPath(".//*[@id='transactions']/div[2]/div/div/table/tbody/tr//p[.='" + transactionId +
                                 "']/../following-sibling::td[4]/p")).Text.Trim()).ToString("MMM d, yyyy");
                Assert.IsTrue(transactionDate.IndexOf(DateTime.Parse(purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderdateAndtime.ToString()]).ToString("MMM d, yyyy"), StringComparison.OrdinalIgnoreCase) >= 0,
                "The payment transaction date and time shown in transaction history page for the transaction id " + purchasedDict[EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString()] + " is mismatching Expected date and time should be " + purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderdateAndtime.ToString()] + ", but actual value shown as " + transactionDate);
                var transactionAmount = BrowserInit.Driver.FindElement(
                        By.XPath(".//*[@id='transactions']/div[2]/div/div/table/tbody/tr//p[.='" + transactionId +
                                 "']/../following-sibling::td[5]/p")).Text.Trim();
                var cultureInfo = new CultureInfo("en-US");
                if (purchasedDict.Keys.Contains(EnumHelper.CartWidget.SubTotal.ToString()))
                {
                    decimal convertFormatSubtotal =
                   decimal.Parse(purchasedDict[EnumHelper.CartWidget.SubTotal.ToString()].Trim());
                    Assert.IsTrue(convertFormatSubtotal.ToString("C", new CultureInfo("en-US")).Equals(transactionAmount, StringComparison.OrdinalIgnoreCase),
                     "The transaction amount on transaction history page for the transaction id " + purchasedDict[EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString()] + " is mismatching Expected transaction price should be " + string.Format(cultureInfo, "{0:C}", purchasedDict[EnumHelper.CartWidget.SubTotal.ToString()]) + ", but actual value shown as " + convertFormatSubtotal.ToString("C", new CultureInfo("en-US")));
                }
                BrowserInit.Driver.FindElement(
                        By.XPath(".//*[@id='transactions']/div[2]/div/div/table/tbody/tr//p[.='" + transactionId +
                                 "']/../following-sibling::td[6]//a")).Click();
                Assert.IsTrue(
                    BrowserInit.Driver.Title.Trim().IndexOf("Transaction Details", StringComparison.OrdinalIgnoreCase) >=
                    0,
                    "On Click on Trancation detail button for the transaction id " +
                    purchasedDict[EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString()] +
                    ", page navigate to some orther page as " + BrowserInit.Driver.Title.Trim());
                Assert.IsTrue(
                    BrowserInit.Driver.FindElement(By.TagName("h1"))
                        .Text.Trim()
                        .Equals("Transaction Details".Trim(), StringComparison.OrdinalIgnoreCase),
                    "At Transaction detail page for transaction id " +
                    purchasedDict[EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString()] +
                    " Expected page heading as 'Transaction Details', but the actual heading shown as " +
                    BrowserInit.Driver.FindElement(By.TagName("h1")).Text.Trim());
                var replaced = new List<string> { "EDT", "EST" };
                foreach (var transactionDetailPageOrderDandT in from yes in replaced let time = PageInitHelper<ValidatingDomainTransationDetails>.PageInit.TransactionDtAndTimeTxt.Text where time.Contains(yes) select Regex.Replace(time, yes, string.Empty) into timeZone select DateTime.Parse(timeZone.Trim()).ToString("MMM d, yyyy,  hh:mm tt"))
                {
                    Assert.IsTrue(
                        transactionDetailPageOrderDandT.Contains(
                            DateTime.Parse(purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderdateAndtime.ToString()].Trim()).ToString("MMM d, yyyy,  hh:mm tt")),
                        "The payment transaction detail page date and time shown in transaction history page for the transaction id " + purchasedDict[EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString()] + " is mismatching Expected date and time should be " + purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderdateAndtime.ToString()] + ", but actual value shown as " + transactionDetailPageOrderDandT);
                }
                var transactionTypeInDetailPage =
                    PageInitHelper<ValidatingDomainTransationDetails>.PageInit.TransactionType
                        .Text.Trim();
                Assert.IsTrue(transactionTypeInDetailPage.Equals("Order"),
                   "The order status shown in transaction detail history page for the transaction id " +
                    purchasedDict[EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString()] +
                    " is mismatching expected status should be 'Order', but status shown in the transaction history page as " +
                    transactionTypeInDetailPage);
                var purchaseOrderNumberForTransaction =
                     PageInitHelper<ValidatingDomainTransationDetails>.PageInit.PurchaseOrderNumber
                        .Text.Trim();
                Assert.IsTrue(purchaseOrderNumberForTransaction.Equals(purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString()], StringComparison.OrdinalIgnoreCase),
                 "The order id on transaction detail page is mismatched expected order id is " + purchasedDict[EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString()] + ", but actual order number shown as " + purchaseOrderNumberForTransaction);
                var paymentMethodInTransactionDetailPage =
                    PageInitHelper<ValidatingDomainTransationDetails>.PageInit.PaymentMethod.Text.Trim();
                if (paymentMethodInTransactionDetailPage.Equals("Secure Card Payment", StringComparison.CurrentCultureIgnoreCase))
                {
                    var paymentCardnumInTransactionDetailPage =
                    PageInitHelper<ValidatingDomainTransationDetails>.PageInit.PaymentCardnum.Text.Trim();
                    Assert.IsTrue(paymentCardnumInTransactionDetailPage.Substring(paymentCardnumInTransactionDetailPage.Length - 4).Equals(purchasedDict[EnumHelper.OrderSummaryKeys.CardEndNumber.ToString()]),
                        "The credit card last four digits number in transaction detail page for the transaction id " + purchasedDict[EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString()] + " is mismatching expected last four digits card number should be " + purchasedDict[EnumHelper.OrderSummaryKeys.CardEndNumber.ToString()] + ", but actual Result shown on transaction detail page as "
                        + paymentCardnumInTransactionDetailPage.Substring(paymentCardnumInTransactionDetailPage.Length - 4));
                }
                if (paymentMethodInTransactionDetailPage.IndexOf("Account Balance", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    var builder = new StringBuilder();
                    var sent = paymentMethodInTransactionDetailPage.Split(' ').Distinct();
                    foreach (var subitemvalue in sent.Select((value, i) => new { i, value }).Select(subitem => subitem.value))
                    {
                        builder.Append(subitemvalue).Append(" ");
                    }
                    paymentMethodInTransactionDetailPage = builder.ToString().Trim();
                }
                if (paymentMethodInTransactionDetailPage.IndexOf("PayPal", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    var builder = new StringBuilder();
                    var sent = paymentMethodInTransactionDetailPage.Split(' ').Distinct();
                    foreach (var subitemvalue in sent.Select((value, i) => new { i, value }).Select(subitem => subitem.value))
                    {
                        builder.Append(subitemvalue).Append(" ");
                    }
                    paymentMethodInTransactionDetailPage = builder.ToString().Trim();
                }
                Assert.IsTrue(paymentMethodInTransactionDetailPage.Equals(purchasedDict[EnumHelper.OrderSummaryKeys.PaymentMethod.ToString()], StringComparison.OrdinalIgnoreCase),
                   "The payment mode shown in transaction detail page for the transaction id " +
                    purchasedDict[EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString()] +
                    " is mismatching expected payment mode should be" + purchasedDict[EnumHelper.OrderSummaryKeys.PaymentMethod.ToString()] + ", but payment mode shown in the transaction detail page as " +
                    paymentMethodInTransactionDetailPage);
                if (purchasedDict.Keys.Contains(EnumHelper.CartWidget.SubTotal.ToString()))
                {
                    var chargedAmount =
                 PageInitHelper<ValidatingDomainTransationDetails>.PageInit.ChargedAmount
                     .Text.Trim();
                    var convertFormatDetailSubtotal =
                      decimal.Parse(purchasedDict[EnumHelper.CartWidget.SubTotal.ToString()].Trim());
                    Assert.IsTrue(convertFormatDetailSubtotal.ToString("C", new CultureInfo("en-US")).Equals(chargedAmount, StringComparison.OrdinalIgnoreCase),
                    "The transaction amount on transaction detail page for the transaction id " + purchasedDict[EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString()] + " is mismatching Expected transaction price should be " + string.Format(cultureInfo, "{0:C}", purchasedDict[EnumHelper.CartWidget.SubTotal.ToString()]) + ", but actual value shown as " + chargedAmount);
                }
                var statusOfTransactionInDetailPage =
                    PageInitHelper<ValidatingDomainTransationDetails>.PageInit.TransactionStatus
                        .Text.Trim();
                Assert.IsTrue(statusOfTransactionInDetailPage.Equals("Success", StringComparison.OrdinalIgnoreCase),
                     "The payment transaction status shown in transaction detail page for the transaction id " +
                    purchasedDict[EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString()] +
                    " is mismatching expected status should be 'success', but status shown in the transaction history page as " +
                    statusOfTransactionInDetailPage);
            }
        }
        #region TransactionPagefactory
        [FindsBy(How = How.XPath, Using = ".//*[@id='maincontent']/div[5]/div/div[1]/div[2]/p")]
        [CacheLookup]
        internal IWebElement TransactionDtAndTimeTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='maincontent']/div[5]/div/div[3]/div[2]/p")]
        [CacheLookup]
        internal IWebElement TransactionType { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='maincontent']/div[5]/div/div[4]/div[2]/p/a")]
        [CacheLookup]
        internal IWebElement PurchaseOrderNumber { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='maincontent']/div[5]/div/div[5]/div[2]/p[1]")]
        [CacheLookup]
        internal IWebElement PaymentMethod { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='maincontent']/div[5]/div/div[5]/div[2]//p[2]")]
        [CacheLookup]
        internal IWebElement PaymentCardnum { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='maincontent']/div[5]/div/div/div[contains(normalize-space(.),'Amount')]/following-sibling::div[1]/p")]
        [CacheLookup]
        internal IWebElement ChargedAmount { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='maincontent']/div[5]/div/div/div[contains(normalize-space(.),'Status')]/following-sibling::div[1]/p")]
        [CacheLookup]
        internal IWebElement TransactionStatus { get; set; }
        #endregion
    }
}