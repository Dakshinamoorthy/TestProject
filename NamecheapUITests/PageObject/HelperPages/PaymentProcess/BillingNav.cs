using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Gallio.Framework;
using NamecheapUITests.PagefactoryObject.PaymentProcessPageFactory;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NUnit.Framework;
using OpenQA.Selenium;

namespace NamecheapUITests.PageObject.HelperPages.PaymentProcess
{
    public class BillingNav : ICheckoutNav
    {
        string pmode = string.Empty;
        public bool PlaceOrderCheckoutFlow(string paymentOption = "", bool changePaymentMode = true)
        {
            string newPmode = string.Empty;
            var pMode = (AppConfigHelper.MainUrl.Equals("live") && changePaymentMode)
                ? EnumHelper.PaymentMethod.Paypal.ToString()
                : string.IsNullOrEmpty(paymentOption)
              ? AppConfigHelper.PaymentMethod
                : paymentOption;
            List<string> pModes = new List<string>();
            bool paymentChanged = false;
            PaymentMode:
            int paratextCount;
            pMode = newPmode.Equals(string.Empty) ? pMode : newPmode;
            switch (EnumHelper.ParseEnum<EnumHelper.PaymentMethod>(pMode))
            {
                case EnumHelper.PaymentMethod.Card:
                    PageInitHelper<BillingPagefactory>.PageInit.CardPaymentOption.Click();
                    var isCardPaymentOptionEnabled = PageInitHelper<BillingPagefactory>.PageInit.CardPaymentOption.Enabled;
                    if (!changePaymentMode && !isCardPaymentOptionEnabled)
                    {
                        var errorMsgTxt =
                            BrowserInit.Driver.FindElement(
                                By.XPath("(.//label[contains(@for,'po-r-1')])/../div[1]/div/p")).Text;
                        throw new TestFailedException("Card Payment Exception: " + errorMsgTxt);
                    }
                    if (changePaymentMode && !isCardPaymentOptionEnabled)
                    {
                        pModes.Add(pMode);
                        newPmode = ChangePaymentMode(pModes);
                        paymentChanged = true;
                        goto PaymentMode;
                    }
                    paratextCount = PageInitHelper<BillingPagefactory>.PageInit.InfoText.Count;
                    if (paratextCount == 2)
                    {
                        bool enumerable = PageInitHelper<BillingPagefactory>.PageInit.InfoText.Any(
                            message => message.Text.Contains("don't have"));
                        if (enumerable && !changePaymentMode)
                            throw new TestFailedException();
                        pModes.Add(pMode);
                        newPmode = ChangePaymentMode(pModes);
                        paymentChanged = true;
                        goto PaymentMode;
                    }
                    IPaymentMethod payment = new CardPayment();
                    payment.PaymentMode();
                    paymentChanged = false;
                    break;
                case EnumHelper.PaymentMethod.Paypal:
                    PageInitHelper<BillingPagefactory>.PageInit.PaypalPaymentOption.Click();
                    paratextCount = PageInitHelper<BillingPagefactory>.PageInit.InfoText.Count;
                    if (paratextCount == 2)
                    {
                        var enumerable = PageInitHelper<BillingPagefactory>.PageInit.InfoText.Any(
                            message => message.Text.Contains("don't have"));
                        if (enumerable && !changePaymentMode)
                            throw new TestFailedException();
                        pModes.Add(pMode);
                        newPmode = ChangePaymentMode(pModes);
                        paymentChanged = true;
                        goto PaymentMode;
                    }
                    break;
                case EnumHelper.PaymentMethod.Funds:
                    FundCheck:
                    PageInitHelper<BillingPagefactory>.PageInit.FundsPaymentOption.Click();
                    paratextCount = PageInitHelper<BillingPagefactory>.PageInit.InfoText1.Count;
                    if (paratextCount == 2)
                    {
                        var enumerable = PageInitHelper<BillingPagefactory>.PageInit.InfoText.Any(
                            message => message.Text.Contains("don't have"));
                        if (enumerable && !changePaymentMode && AppConfigHelper.MainUrl == "Live")
                            throw new InconclusiveException("Don't have sufficient funds in your account to cover purchase in Production Namecheap site");
                        if (enumerable && AppConfigHelper.MainUrl != "Live")
                        {
                            if (changePaymentMode)
                            {
                                pModes.Add(pMode);
                                newPmode = ChangePaymentMode(pModes);
                                paymentChanged = true;
                                goto PaymentMode;
                            }
                            IPaymentMethod accpayment = new AccountFundsPayment();
                            accpayment.PaymentMode();
                            goto FundCheck;
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            PageInitHelper<BillingPagefactory>.PageInit.PaymentContinueBtn.Click();
            Thread.Sleep(1000);
            PageInitHelper<PageNavigationHelper>.PageInit.WaitForPageLoad();
            return paymentChanged;
        }

        internal string ChangePaymentMode(List<string> paymentMode)
        {
            var newPaymentMode = new List<string>(paymentMode);
            foreach (var pmethod in PageInitHelper<BillingPagefactory>.PageInit.PaymentOptions)
            {
                var ptext = pmethod.Text;
                var spanText = pmethod.FindElement(By.ClassName("cc")).Text;
                var removespanTxt = ptext.Replace(spanText, string.Empty);
                var paymodeTxt = removespanTxt.Replace("\r\n", string.Empty);
                var paymode = paymodeTxt.Contains(EnumHelper.PaymentMethod.Card.ToString())
                    ? EnumHelper.PaymentMethod.Card.ToString()
                    : paymodeTxt.Contains(EnumHelper.PaymentMethod.Funds.ToString())
                        ? EnumHelper.PaymentMethod.Funds.ToString()
                        : paymodeTxt;
                if (newPaymentMode.Count.Equals(0))
                {
                    return paymode;
                }
                foreach (var payMode in newPaymentMode)
                {
                    if (pmethod.Text.Contains(payMode))
                    {
                        newPaymentMode.Remove(payMode);
                        break;
                    }

                }
            }
            Assert.Fail("We can't purchase in all three payment modes due to some issue.");
            return pmode;
        }


    }
}