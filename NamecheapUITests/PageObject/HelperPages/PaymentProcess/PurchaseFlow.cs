using System;
using System.Collections.Generic;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using NamecheapUITests.PageObject.Interface;
namespace NamecheapUITests.PageObject.HelperPages.PaymentProcess
{
    public class PurchaseFlow
    {
        //  private delegate void DelMethod();
        public List<SortedDictionary<string, string>> PurchasingNcProducts(
            List<SortedDictionary<string, string>> mergedScAndCartWidgetList, string paymentOption = "",
            bool changePaymentMode = true)
        {
            var mergedPurchaseSummaryItemsAndScItemsList = new List<SortedDictionary<string, string>>();
            bool changePayment = false;
            ICheckoutNav checkout;
            Check:
            var checkoutNavTxt = PageInitHelper<PurchaseFlow>.PageInit.CheckoutNav.Text.Trim();
            if (checkoutNavTxt.Equals(UiConstantHelper.Account))
            {
                checkout = new AccountNav();
                checkout.PlaceOrderCheckoutFlow(paymentOption, changePaymentMode);
                goto Check;
            }
            if (checkoutNavTxt.Equals(UiConstantHelper.Setup))
            {
                checkout = new SetupNav();
                checkout.PlaceOrderCheckoutFlow(paymentOption, changePaymentMode);
                goto Check;
            }
            if (checkoutNavTxt.Equals(UiConstantHelper.Billing))
            {
                checkout = new BillingNav();
                changePayment = checkout.PlaceOrderCheckoutFlow(paymentOption, changePaymentMode);

                goto Check;
            }
            if (checkoutNavTxt.Equals(UiConstantHelper.Order))
            {
                if (!changePayment)
                {
                    //changePaymentMode = changePayment.Equals(false) ? false : true;
                    checkout = new OrderNav();
                    checkout.PlaceOrderCheckoutFlow(paymentOption, changePaymentMode);

                    // PageInitHelper<PurchaseFlow>.PageInit.PaymentContinueBtn.Click();
                }
                var elemTermsAgree = BrowserInit.Driver.PageSource.Contains("your-cart-terms");
                if (elemTermsAgree)
                {
                    var chkTerms = PageInitHelper<PurchaseFlow>.PageInit.TermsAgreementsChk.Selected;
                    if (chkTerms == false)
                        PageInitHelper<PurchaseFlow>.PageInit.TermsAgreementsChk.Click();
                }
                // PageInitHelper<PurchaseFlow>.PageInit.PaymentContinueBtn.Click();
                var paymentMethodText = PageInitHelper<PurchaseFlow>.PageInit.PaymentMethod.Text.Trim();
                PageInitHelper<PurchaseFlow>.PageInit.PayNowBtn.Click();
                PageInitHelper<PageNavigationHelper>.PageInit.WaitForPageLoad();
                var paymentMethods = paymentMethodText.IndexOf(EnumHelper.PaymentMethod.Paypal.ToString(),StringComparison.CurrentCultureIgnoreCase)>=0 || paymentMethodText.IndexOf(EnumHelper.PaymentMethod.Funds.ToString(), StringComparison.CurrentCultureIgnoreCase) >= 0
              ? paymentMethodText
              : EnumHelper.PaymentMethod.Card.ToString();
                if (paymentMethods.IndexOf(EnumHelper.PaymentMethod.Paypal.ToString(), StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    IPaymentMethod paypal = new PaypalPayment();
                    paypal.PaymentMode();
                }
                else
                {
                    PageInitHelper<OrderProcessing>.PageInit.WaitForOrderProcessing();
                }
                /* IPaymentMethod paypal = new PaypalPayment();
                 (paymentMethods.Equals("PAYPAL", StringComparison.InvariantCultureIgnoreCase)
                     ? paypal.PaymentMode
                     : (Action)PageInitHelper<OrderProcessing>.PageInit.WaitForOrderProcessing)();*/
                goto Check;
            }
            if (checkoutNavTxt.Equals("Done"))
            {
                mergedPurchaseSummaryItemsAndScItemsList =
                   PageInitHelper<DoneNav>.PageInit.ValidateInPurchaseSumarypage(mergedScAndCartWidgetList);

            }

            return mergedPurchaseSummaryItemsAndScItemsList;
        }


        #region PageFactory
        [FindsBy(How = How.XPath, Using = ".//*[@class='checkout-nav']//li[@class='selected']/a")]
        [CacheLookup]
        internal IWebElement CheckoutNav { get; set; }


        [FindsBy(How = How.XPath, Using = ".//*[@class='group checkout-terms']/label/span/input")]
        [CacheLookup]
        internal IWebElement TermsAgreementsChk { get; set; }
        [FindsBy(How = How.XPath,
            Using = ".//*[contains(@class,'product-group')]//*[text()[contains(.,'Payment Method')]]/following::div[1]")
        ]
        [CacheLookup]
        internal IWebElement PaymentMethod { get; set; }
        [FindsBy(How = How.LinkText, Using = "CHANGE")]
        [CacheLookup]
        internal IWebElement ChangeLnk { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(@class,'cart spacer-bottom side-cart')]/p/a")]
        [CacheLookup]
        internal IWebElement PaymentContinueBtn { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@class='grid-col one-quarter']/div/div/p[1]/input")]
        [CacheLookup]
        internal IWebElement PayNowBtn { get; set; }

        #endregion
    }
}