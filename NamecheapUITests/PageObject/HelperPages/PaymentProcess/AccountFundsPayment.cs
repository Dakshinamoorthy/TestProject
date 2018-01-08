using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using Gallio.Framework;
using NamecheapUITests.PagefactoryObject.HelpersPageFactory;
using NamecheapUITests.PagefactoryObject.PaymentProcessPageFactory;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
namespace NamecheapUITests.PageObject.HelperPages.PaymentProcess
{
    public class AccountFundsPayment : IPaymentMethod
    {
        public void PaymentMode()
        {
            var addFundsHref = PageInitHelper<AccountFundspagefactory>.PageInit.AddFundsLinkTxt.GetAttribute("href");
            BrowserInit.Driver.ExecuteJavaScript("window.open('" + addFundsHref + "','_blank');");
            IList<string> tabs = new List<string>(BrowserInit.Driver.WindowHandles);
            BrowserInit.Driver.SwitchTo().Window(tabs[1]);
            var waitForDocumentReady = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(10));
            waitForDocumentReady.Until(wdriver => ((IJavaScriptExecutor)BrowserInit.Driver).ExecuteScript("return document.readyState").Equals("complete"));
            /*  Assert.IsTrue(addFundsHref.Contains("profile/billing/Topup"),
                  "add funds link - href url is not updated for Account Funds 'in-sufficient' caution message text in Billing page Payment Method section expected :" +
                  PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "profile/billing/Topup") +
                  " But actual: " + addFundsHref);*/
            Assert.IsTrue(BrowserInit.Driver.Url.Equals(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "profile/billing/Topup")));
            PageInitHelper<AccountFundspagefactory>.PageInit.SecureCardRdo.Click();
            PageInitHelper<AccountFundspagefactory>.PageInit.PaymentMethodNextBtn.Click();
            var currentBalance = Regex.Replace(PageInitHelper<AccountFundspagefactory>.PageInit.TopUpPageCurrentBalLbl.Text, @"[^\d..][^\w\s]*", string.Empty).Trim();
            const double amountToCart = 1000.89;
            PageInitHelper<AccountFundspagefactory>.PageInit.TopUpAmountTxt.SendKeys(amountToCart.ToString(CultureInfo.InvariantCulture));
            PageInitHelper<AccountFundspagefactory>.PageInit.TopUppageNextBtn.Click();
            if (PageInitHelper<AccountFundspagefactory>.PageInit.AmountSelectionNavLst.GetAttribute(UiConstantHelper.AttributeClass).Contains(UiConstantHelper.Selected))
                throw new TestFailedException("Error: Failed to call payment gateway in " + BrowserInit.Driver.Url + "page");
            Assert.IsTrue(currentBalance.Equals(Regex.Replace(PageInitHelper<AccountFundspagefactory>.PageInit.CardNavListPageCurrentBalLbl.Text, @"[^\d..][^\w\s]*", string.Empty).Trim()));
            CultureInfo cultureInfo = new CultureInfo("en-US");
            string converttocurrency = string.Format(cultureInfo, "{0:C}", amountToCart);
            var amounttoAdd = Convert.ToDecimal(Regex.Replace(PageInitHelper<AccountFundspagefactory>.PageInit.CardNavListPageAmttoAddLbl.Text, @"[^\d..][^\w\s]*", string.Empty).Trim());
            Assert.IsTrue(converttocurrency.Equals(string.Format(cultureInfo, "{0:C}", amounttoAdd)), "In TopupWithCard Page Current Balace and Amount to Add text is not equal, Current Balance: " + converttocurrency + " Amount to Add: " + amounttoAdd);
            var cardSelection = PageInitHelper<AccountFundspagefactory>.PageInit.BillingNameTxt.GetAttribute(UiConstantHelper.AttributeValue).Equals(string.Empty);
            var userFirstNameTxt = string.Empty;
            AUserInfoUpdation addOrEditCreditCardInfo;
            if (AppConfigHelper.LiveCardPurchase.Equals("Y", StringComparison.CurrentCultureIgnoreCase))
            {
                if (PageInitHelper<AccountFundspagefactory>.PageInit.CarddetailsSelectionLbl.GetAttribute("for").Equals("carddetails"))
                {
                    new SelectElement(PageInitHelper<AccountFundspagefactory>.PageInit.CardOptionDdl).SelectByText("Add New Card");
                }
                PageInitHelper<UserInfoUpdationHelper>.PageInit.LiveCard();
                if (PageInitHelper<AccountFundspagefactory>.PageInit.SaveCardChk.Selected)
                    PageInitHelper<AccountFundspagefactory>.PageInit.SaveCardLbl.Click();
                userFirstNameTxt = UtilityHelper.LivecardDetails[EnumHelper.CardDetails.NameonCard];
                UtilityHelper.LivecardDetails.Clear();
            }
            else
            {
                if (!cardSelection)
                {
                    ExistingCardPayment();
                    PageInitHelper<AccountFundspagefactory>.PageInit.ExistingCardPageNextBtn.Click();
                    if (!PageInitHelper<AccountFundspagefactory>.PageInit.ConfrimNavList.GetAttribute(UiConstantHelper.AttributeClass).Contains(UiConstantHelper.Disabled))
                        PageInitHelper<AccountFundspagefactory>.PageInit.CardPageSubmitBtn.Click();
                }
                if (cardSelection)
                {
                    addOrEditCreditCardInfo = new UserInfoUpdationHelper();
                    addOrEditCreditCardInfo.DummyCardDetails();
                }
            }
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.FirstNameTxt.Clear();
            if (string.IsNullOrEmpty(userFirstNameTxt))
            {
                userFirstNameTxt = PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                    EnumHelper.ExcelDataEnum.UserFirstName.ToString());
            }
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.FirstNameTxt.SendKeys(userFirstNameTxt);
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.LastNameTxt.Clear();
            var userLastNameTxt = PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                EnumHelper.ExcelDataEnum.UserLastName.ToString());
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.LastNameTxt.SendKeys(userLastNameTxt);
            addOrEditCreditCardInfo = new UserInfoUpdationHelper();
            addOrEditCreditCardInfo.BillingAddressDetails();
            var pageUrlInSaveinfoPage = BrowserInit.Driver.Url;
            PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.SaveBtn);
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.SaveBtn.Click();
            Thread.Sleep(5000);
            PageInitHelper<PageNavigationHelper>.PageInit.WaitForPageLoad();
            if (BrowserInit.Driver.Url != pageUrlInSaveinfoPage)
            {
                PageInitHelper<AccountFundspagefactory>.PageInit.CardPageSubmitBtn.Click();
            }
            else
            {
                var errormsgdisplayed = PageInitHelper<AccountFundspagefactory>.PageInit.ErrorMessageLbl.Displayed;
                if (errormsgdisplayed)
                {
                    var paymethods = EnumHelper.PaymentMethod.Card.ToString();
                    var errormsg = PageInitHelper<AccountFundspagefactory>.PageInit.ErrorMessageLblTxt.Text;
                    BrowserInit.Driver.SwitchTo().Window(tabs[1]).Close();
                    BrowserInit.Driver.SwitchTo().Window(tabs[0]);
                    throw new TestFailedException("cant able to Top Up Account Balance with " + paymethods +
                                                  " throws error message as  " + errormsg);
                }
            }
            BrowserInit.Driver.SwitchTo().Window(tabs[1]).Close();
            BrowserInit.Driver.SwitchTo().Window(tabs[0]);
            PageInitHelper<BillingPagefactory>.PageInit.PaypalPaymentOption.Click();
            PageInitHelper<BillingPagefactory>.PageInit.PaymentContinueBtn.Click();
            PageInitHelper<PurchaseFlow>.PageInit.ChangeLnk.Click();
        }
        [Description("Card Selection in Dropdown list")]
        private void ExistingCardPayment()
        {
            PageInitHelper<AccountFundspagefactory>.PageInit.SelectedCardDdl.Click();
            var cardcount = BrowserInit.Driver.FindElements(By.XPath(".//*[contains(concat(' ',normalize-space(@class),' '),'select2-drop-active')]/ul/li"));
            var cardListCount = PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(cardcount.Count);
            var selectCardType = BrowserInit.Driver.FindElement(By.XPath("(.//*[contains(concat(' ',normalize-space(@class),' '),'select2-drop-active')]/ul/li)[" + cardListCount + "]"));
            selectCardType.Click();
            Thread.Sleep(2000);
            Assert.IsTrue(BrowserInit.Driver.FindElement(By.XPath(".//*[contains(concat(' ',normalize-space(@class),' '),'card-form existing')]/div[2]/div[1]/p[1]")).Displayed, "");
        }
    }
}