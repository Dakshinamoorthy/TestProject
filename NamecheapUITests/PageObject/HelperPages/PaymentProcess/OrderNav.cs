using System;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;

namespace NamecheapUITests.PageObject.HelperPages.PaymentProcess
{
    public class OrderNav : ICheckoutNav
    {
        public bool PlaceOrderCheckoutFlow(string paymentOption = "", bool changePaymentMode = true)
        {
            var changePayment = changePaymentMode;
            var url = AppConfigHelper.MainUrl.Equals("live");
            var payPalStatus = AppConfigHelper.LivePaypalPurchase.Equals("N", StringComparison.CurrentCultureIgnoreCase);
            var cardStatus = AppConfigHelper.LiveCardPurchase.Equals("N", StringComparison.CurrentCultureIgnoreCase);
            if (url)
            {
                if (paymentOption.Equals("Paypal") && payPalStatus && !changePaymentMode && !string.IsNullOrEmpty(paymentOption))
                {
                    throw new CustomException("But, for namecheap production environment auto test verifies until payment confirmation page. until this page all streams works fine, because for production we don't have access for purchasing");
                }
                if (paymentOption.Equals("Card") && cardStatus && !changePaymentMode && !string.IsNullOrEmpty(paymentOption))
                {
                    throw new CustomException("But, for namecheap production environment auto test verifies until payment confirmation page. until this page all streams works fine, because for production we don't have access for purchasing");
                }
                if (string.IsNullOrEmpty(paymentOption))
                {
                    throw new CustomException("But, for namecheap production environment auto test verifies until payment confirmation page. until this page all streams works fine, because for production we don't have access for purchasing");
                }


            }
            var pMode = string.IsNullOrEmpty(paymentOption)
                ? AppConfigHelper.PaymentMethod
                : paymentOption;
            var paymentMethodText = PageInitHelper<PurchaseFlow>.PageInit.PaymentMethod.Text;
            var paymentMethods = paymentMethodText.IndexOf(EnumHelper.PaymentMethod.Paypal.ToString(), StringComparison.CurrentCultureIgnoreCase) >= 0 || paymentMethodText.IndexOf(EnumHelper.PaymentMethod.Funds.ToString(), StringComparison.CurrentCultureIgnoreCase) >= 0
              ? paymentMethodText
              : EnumHelper.PaymentMethod.Card.ToString();

            var paymentMethod = paymentMethods.IndexOf(pMode, StringComparison.OrdinalIgnoreCase) != -1;
            if (paymentMethod) return changePayment;
            PageInitHelper<PurchaseFlow>.PageInit.ChangeLnk.Click();
            ICheckoutNav checkout = new BillingNav();
            changePayment = checkout.PlaceOrderCheckoutFlow(paymentOption, changePaymentMode);
            return changePayment;
        }
        [Serializable]
        private class CustomException : Exception
        {
            public CustomException(string message)
                : base(message) { }

        }
    }
}