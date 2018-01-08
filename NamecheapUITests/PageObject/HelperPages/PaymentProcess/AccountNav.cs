using NamecheapUITests.PagefactoryObject.PaymentProcessPageFactory;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NamecheapUITests.PageObject.ValidationPages;
using OpenQA.Selenium;

namespace NamecheapUITests.PageObject.HelperPages.PaymentProcess
{
    public class AccountNav : ICheckoutNav
    {
        public bool PlaceOrderCheckoutFlow(string paymentOption = "", bool changePaymentMode = true)
        {
            PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();

            PageInitHelper<AccountNavPageFactory>.PageInit.AccountInfoOrganizationNametxt.SendKeys(UiConstantHelper.OrganizationName);
            PageInitHelper<AccountNavPageFactory>.PageInit.AccountInfoJobTitletxt.SendKeys(UiConstantHelper.JobTitle);
            PageInitHelper<AccountNavPageFactory>.PageInit.AccountInfoAddress1Txt.SendKeys(UiConstantHelper.Address1);
            PageInitHelper<AccountNavPageFactory>.PageInit.AccountInfoAddress2Txt.SendKeys(UiConstantHelper.Address2);
            PageInitHelper<AccountNavPageFactory>.PageInit.AccountInfoCityTxt.SendKeys(UiConstantHelper.Cityname);
            PageInitHelper<AccountNavPageFactory>.PageInit.AccountInfoStateTxt.SendKeys(UiConstantHelper.StateName);

            PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(PageInitHelper<AccountNavPageFactory>.PageInit.AccountInfoScrollText);
            PageInitHelper<AccountNavPageFactory>.PageInit.AccountInfoZipCodeTxt.SendKeys(UiConstantHelper.Zipcode);
            var element = PageInitHelper<AccountNavPageFactory>.PageInit.ContactPageCountLbl;
            var executor = (IJavaScriptExecutor)BrowserInit.Driver;
            executor.ExecuteScript("arguments[0].click();", element);
            PageInitHelper<AccountNavPageFactory>.PageInit.ContactPageCountIndiaDdl.Click();
            PageInitHelper<AccountNavPageFactory>.PageInit.AccountInfoPhonetxt.Clear();
            PageInitHelper<PageValidationHelper>.PageInit.RandomGenratorstringBuilder(3);
            PageInitHelper<AccountNavPageFactory>.PageInit.AccountInfoPhonetxt.SendKeys(PageInitHelper<PageValidationHelper>.PageInit.RandomGenratorstringBuilder(5).ToString());
            PageInitHelper<AccountNavPageFactory>.PageInit.AccountInfoFaxtxt.SendKeys(PageInitHelper<PageValidationHelper>.PageInit.RandomGenratorstringBuilder(4).ToString());
            PageInitHelper<AccountNavPageFactory>.PageInit.ContinueBtn.Click();
            return changePaymentMode;
        }
      
    }
}