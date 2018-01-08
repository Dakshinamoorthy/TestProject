using System;
using System.Collections.Generic;
using Gallio.Framework;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PageObject.ValidationPages
{
    public class ValidatePurchaseSummary : APurchaseSummaryPageValidation
    {
        public override List<SortedDictionary<string, string>> PurchaseSummaryPage()
        {
            List<SortedDictionary<string, string>> purchaseOrderNumberList = new List<SortedDictionary<string, string>>();
            SortedDictionary<string, string> purchaseOrderNumberDic = new SortedDictionary<string, string>();
            Assert.IsTrue(
                PageInitHelper<ValidatePurchaseSummary>.PageInit.PurchaseSummaryHeader.Text.Contains(
                    UiConstantHelper.PurchaseSummary), "At order summary page page heading should be " + UiConstantHelper.PurchaseSummary + ", but it shown as" + PageInitHelper<ValidatePurchaseSummary>.PageInit.PurchaseSummaryHeader.Text);
            var productGroups =
                BrowserInit.Driver.FindElements(
                    By.XPath(".//*[contains(@class,'product-group')][not(contains(@class,'subtotal'))]")).Count;
            for (var productGroup = 1; productGroup <= productGroups; productGroup++)
            {
                var harErrorOrNot =
                    BrowserInit.Driver.FindElements(
                        By.XPath(".//*[contains(@class,'product-group')][not(contains(@class,'subtotal'))][" +
                                 productGroup + "]/*[contains(@class,'cart-item')]")).Count;
                for (var harError = 1; harError <= harErrorOrNot; harError++)
                {
                    var hasErrorOrNot =
                        BrowserInit.Driver.FindElement(By.XPath(".//*[contains(@class,'cart-item')][" + harError + "]"))
                            .GetAttribute("class");
                    if (hasErrorOrNot.Contains("has-error"))
                        throw new TestFailedException("In order summary page product or domain is getting failed");
                }
            }
            purchaseOrderNumberDic.Add(EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString(), PageInitHelper<ValidatePurchaseSummary>.PageInit.OrderNumber.Text.Trim());
            var para =
                BrowserInit.Driver.FindElement(
                    By.XPath(".//*[contains(@class,'your-cart summary')]/div[contains(@class,'thank-you')]/p[1]")).Text;
            var dandT = para.Substring(para.Remove(para.LastIndexOf("completed.", StringComparison.Ordinal)).IndexOf("on", StringComparison.Ordinal));
            var convertedDandT = DateTime.Parse(dandT.Remove(dandT.LastIndexOf("is", StringComparison.Ordinal)).Replace("on", string.Empty).Trim()).ToString("MMM d, yyyy,  hh:mm tt");
            purchaseOrderNumberDic.Add(EnumHelper.OrderSummaryKeys.PurchaseOrderdateAndtime.ToString(), convertedDandT);
            purchaseOrderNumberDic.Add(EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString(), PageInitHelper<ValidatePurchaseSummary>.PageInit.ProductTransactionId.Text.Trim());
            var paymentMethod = PageInitHelper<ValidatePurchaseSummary>.PageInit.PaymentMethodTxt.Text.Trim();
            if (paymentMethod.Replace("Payment Method", string.Empty).Trim().IndexOf("Funds", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                paymentMethod = "Account Balance".Trim();
            }
            if (paymentMethod.Replace("Payment Method", string.Empty).Trim().IndexOf("Credit Card", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                paymentMethod = "Secure Card Payment".Trim();
                var cardNumber = BrowserInit.Driver.FindElement(By.ClassName("cc-number")).Text.Trim();
                // var last4Digits = CardNumber.Substring(CardNumber.Length - 4);
                purchaseOrderNumberDic.Add(EnumHelper.OrderSummaryKeys.CardEndNumber.ToString(), cardNumber.Substring(Math.Max(0, cardNumber.Length - 4)));
            }
            if (paymentMethod.Replace("Payment Method", string.Empty).Trim().IndexOf("Paypal", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                paymentMethod = "Paypal".Trim();
                var paypalUserName =
                    PageInitHelper<ValidatePurchaseSummary>.PageInit.PayPalUserName.Text;
                purchaseOrderNumberDic.Add(EnumHelper.OrderSummaryKeys.PayPalUserName.ToString(), paypalUserName);
            }
            purchaseOrderNumberDic.Add(EnumHelper.OrderSummaryKeys.PaymentMethod.ToString(), paymentMethod);
            purchaseOrderNumberList.Add(purchaseOrderNumberDic);
            return purchaseOrderNumberList;
        }
        #region
        [FindsBy(How = How.XPath, Using = "//div[@class='your-cart summary group']/div[2]/h3")]
        [CacheLookup]
        internal IWebElement PurchaseSummaryHeader { get; set; }
        [FindsBy(How = How.XPath,
          Using = ".//*[contains(@class,'your-cart summary')]/div[contains(@class,'thank-you')]/p/strong")]
        [CacheLookup]
        internal IWebElement OrderNumber { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@class='order-details']//strong[normalize-space()='Transaction']/following-sibling::span")]
        [CacheLookup]
        internal IWebElement ProductTransactionId { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@class='order-detail-item']//strong[normalize-space()='Payment Method']/..")]
        [CacheLookup]
        internal IWebElement PaymentMethodTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@class='order-detail-item']//strong[normalize-space()='Payment Method']/../span")]
        [CacheLookup]
        internal IWebElement PayPalUserName { get; set; }
        #endregion
    }
}