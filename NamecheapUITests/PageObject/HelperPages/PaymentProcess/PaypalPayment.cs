using System;
using System.Threading;
using NamecheapUITests.PageObject.HelperPages.WinForm.Paypal;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;

namespace NamecheapUITests.PageObject.HelperPages.PaymentProcess
{
    public class PaypalPayment : IPaymentMethod
    {
        public void PaymentMode()
        {
            string payPalEmailId;
            string payPalPwd;
            if (AppConfigHelper.LivePaypalPurchase.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
            {
                var newPaypal = new LivePaypalInputForm();
                newPaypal.ShowDialog();
                var livePaypalAccountDetails = UtilityHelper.LivePaypalDetails;
                payPalEmailId = livePaypalAccountDetails[EnumHelper.PayPalDetails.EmailAddress];
                payPalPwd = livePaypalAccountDetails[EnumHelper.PayPalDetails.Password];
            }
            else
            {
                payPalEmailId = UiConstantHelper.PaypalDummyEmailAddress;
                payPalPwd = UiConstantHelper.PaypalDummyPassword;
            }
            var paypalUrl = BrowserInit.Driver.Url;
            if (paypalUrl.Contains("sandbox.paypal.com"))
            {
                if (BrowserInit.Driver.Url.Contains("/checkout/login"))
                {
                    Thread.Sleep(5000);
                    BrowserInit.Driver.SwitchTo().Frame(BrowserInit.Driver.FindElement(By.TagName("iframe")));
                    PageInitHelper<PaypalPayment>.PageInit.PayPalEmailTxt.SendKeys(payPalEmailId);
                    PageInitHelper<PaypalPayment>.PageInit.PayPalPasswordTxt.SendKeys(payPalPwd);
                    PageInitHelper<PaypalPayment>.PageInit.PaypalLoginBtn.Click();
                    Thread.Sleep(5000);
                    var orderreviewPageTitle = BrowserInit.Driver.Title;
                    var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(5));
                    Func<IWebDriver, bool> searchtestCondition =
                        x =>
                            !orderreviewPageTitle.Equals("PayPal Checkout - Review your payment",
                                StringComparison.InvariantCultureIgnoreCase);
                    wait.Until(searchtestCondition);
                    Thread.Sleep(5000);
                    PageInitHelper<PaypalPayment>.PageInit.PayPalSubmitBtn.Click();

                }
                else if (BrowserInit.Driver.Url.Contains("/checkout/review"))
                {
                    BrowserInit.Driver.SwitchTo().Frame(BrowserInit.Driver.FindElement(By.TagName("iframe")));
                    BrowserInit.Driver.SwitchTo()
                        .Window(BrowserInit.Driver.SwitchTo().DefaultContent().CurrentWindowHandle);
                    var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(5));
                    Func<IWebDriver, bool> searchtestCondition =
                        x =>
                            ((IJavaScriptExecutor)BrowserInit.Driver).ExecuteScript("return document.readyState")
                                .Equals("complete");
                    wait.Until(searchtestCondition);
                    PageInitHelper<PaypalPayment>.PageInit.PayPalSubmitBtn.Click();
                }
            }
            else if (paypalUrl.Contains("www.paypal.com"))
            {
                if (BrowserInit.Driver.Url.Contains("/checkout/signup"))
                {
                    BrowserInit.Driver.FindElement(By.XPath(".//*[@id='loginSection']/div/div[2]/a")).Click();
                    PageInitHelper<TimeSpanHelper>.PageInit.AttributeIsDisplayed(
                        BrowserInit.Driver.FindElement(
                            By.XPath(".//*[contains(@class,'transitioning spinnerWithLockIcon')]")),
                        UiConstantHelper.AttributeClass, "hide");
                }
                PageInitHelper<PaypalPayment>.PageInit.PayPalEmailTxt.SendKeys(payPalEmailId);
                BrowserInit.Driver.FindElement(By.XPath("//*[@id='btnNext']")).Click();
                PageInitHelper<TimeSpanHelper>.PageInit.AttributeIsDisplayed(
                       BrowserInit.Driver.FindElement(
                           By.XPath(".//*[contains(@class,'transitioning spinnerWithLockIcon')]")),
                       UiConstantHelper.AttributeClass, "hide");
                PageInitHelper<PaypalPayment>.PageInit.PayPalPasswordTxt.SendKeys(payPalPwd);
                BrowserInit.Driver.FindElement(By.XPath("//*[@id='btnLogin']")).Click();

            }
            PageInitHelper<OrderProcessing>.PageInit.WaitForOrderProcessing();
        }
        #region PayPal PageFactory
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@type),' '),'email')]")]
        [CacheLookup]
        internal IWebElement PayPalEmailTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@type),' '),'password')]")]
        [CacheLookup]
        internal IWebElement PayPalPasswordTxt { get; set; }
        [FindsBy(How = How.Id, Using = "btnLogin")]
        [CacheLookup]
        internal IWebElement PaypalLoginBtn { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@type),' '),'submit')]")]
        [CacheLookup]
        internal IWebElement PayPalSubmitBtn { get; set; }
        #endregion
    }
}