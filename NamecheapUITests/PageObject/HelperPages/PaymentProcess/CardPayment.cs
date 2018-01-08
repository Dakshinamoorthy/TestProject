using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using NamecheapUITests.PagefactoryObject.PaymentProcessPageFactory;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Keys = OpenQA.Selenium.Keys;

namespace NamecheapUITests.PageObject.HelperPages.PaymentProcess
{
    public class CardPayment : IPaymentMethod
    {
        public void PaymentMode()
        {
            if (AppConfigHelper.LiveCardPurchase.Equals("y", StringComparison.CurrentCultureIgnoreCase))
            {
                PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementAndClick(PageInitHelper<CardPaymentPageFactory>.PageInit.CardDetailsDropdownTxt, PageInitHelper<CardPaymentPageFactory>.PageInit.AddNewCardLst);
                PageInitHelper<UserInfoUpdationHelper>.PageInit.LiveCard();
                UtilityHelper.LivecardDetails.Clear();
                PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementAndClick(PageInitHelper<CardPaymentPageFactory>.PageInit.CardBillingAddDdl , PageInitHelper<CardPaymentPageFactory>.PageInit.AddNewAddressLst);
            }
            else
            {
                if (PageInitHelper<CardPaymentPageFactory>.PageInit.CardDetailsDropdownTxt.Text.Equals("Add new card",
               StringComparison.CurrentCultureIgnoreCase))
                    PageInitHelper<UserInfoUpdationHelper>.PageInit.NewCardDetails(string.Empty);
            }
            if (PageInitHelper<CardPaymentPageFactory>.PageInit.CardBillingAddDdl.Text.Contains("Add new contact"))
            {
                var userDetails = PageInitHelper<UserPersonalDataGenetrator>.PageInit.CardBillingAddress();
                if (!PageInitHelper<CardPaymentPageFactory>.PageInit.BillingRegisterCompanyChk.Selected)
                    PageInitHelper<CardPaymentPageFactory>.PageInit.BillingRegisterCompanyChk.Click();
                PageInitHelper<CardPaymentPageFactory>.PageInit.BillingUserFirstNameTxt.SendKeys(userDetails.Item1);
                PageInitHelper<CardPaymentPageFactory>.PageInit.BillingUserLastNameTxt.SendKeys(userDetails.Item2);
                PageInitHelper<CardPaymentPageFactory>.PageInit.BillingUserCompanyNameTxt.SendKeys(userDetails.Item3);
                PageInitHelper<CardPaymentPageFactory>.PageInit.BillingUserJobTitleTxt.SendKeys(userDetails.Item4);
                PageInitHelper<CardPaymentPageFactory>.PageInit.BillingUserAddress1Txt.SendKeys(userDetails.Item5.Item1);
                PageInitHelper<CardPaymentPageFactory>.PageInit.BillingUserAddress2Txt.SendKeys(userDetails.Item5.Item2);
                PageInitHelper<CardPaymentPageFactory>.PageInit.BillingUserCityTxt.SendKeys(userDetails.Item5.Item4);
                PageInitHelper<CardPaymentPageFactory>.PageInit.BillingUserStateTxt.SendKeys(userDetails.Item5.Item3);
                PageInitHelper<CardPaymentPageFactory>.PageInit.BillingUserZipcodeTxt.SendKeys(userDetails.Item5.Item5);
                var paymentMethodDiv = PageInitHelper<CardPaymentPageFactory>.PageInit.PaymentMethodHeadingTxt;
                var action = new Actions(BrowserInit.Driver);
                action.SendKeys(paymentMethodDiv, Keys.Tab).Build().Perform();
                Thread.Sleep(1000);
                action.Click();
                var rememberThisCardisPresent = BrowserInit.Driver.FindElements(By.Id("saveCreditCard")).Count > 0;
                if (rememberThisCardisPresent)
                {
                    if (PageInitHelper<CardPaymentPageFactory>.PageInit.RememberCardChk.Enabled)
                        PageInitHelper<CardPaymentPageFactory>.PageInit.RememberCardChk.Click();
                }
            }
            var reCaptcha = PageInitHelper<CardPaymentPageFactory>.PageInit.ReCaptchaBlock.GetCssValue("display").Contains("block");
            if (!reCaptcha) return;
            BrowserInit.Driver.SwitchTo()
                .Frame(BrowserInit.Driver.FindElement(By.XPath("//*[@id='reCaptcha']/div/div/iframe")));
            var divCount = PageInitHelper<CardPaymentPageFactory>.PageInit.ReCaptchaDivCount;
            foreach (
                var enumerable in
                    divCount.Select(message => message.GetAttribute(UiConstantHelper.AttributeClass).Contains("rc-anchor rc-anchor-normal rc-anchor-light"))
                        .Where(contains => contains))
                if (enumerable)
                    PageInitHelper<CardPaymentPageFactory>.PageInit.ReCaptchaChk.Click();
            PageInitHelper<CardPaymentPageFactory>.PageInit.ReCaptchaChk.Click();
            MessageBox.Show(@"Recaptcha" + Environment.NewLine + @"Select the captcha image wich mataches: ",
                @"Important Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            Thread.Sleep(10 * 1000);
            MessageBox.Show(@"Once Completed Captcha click Ok button To further Automation process",
                @"Important Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            BrowserInit.Driver.SwitchTo().ParentFrame();
        }
      
    }
}