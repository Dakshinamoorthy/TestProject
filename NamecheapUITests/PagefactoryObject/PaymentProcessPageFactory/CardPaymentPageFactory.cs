using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NamecheapUITests.PagefactoryObject.PaymentProcessPageFactory
{
    public class CardPaymentPageFactory
    {
        [FindsBy(How = How.XPath, Using = "(.//*/h2)[1]/following::div[contains(@class,' card-option-select ')]  | ((.//*/h2)[1]/following::div[1]/div[2])/div")]
        [CacheLookup]
        internal IWebElement CardDetailsDropdownTxt { get; set; }

        [FindsBy(How = How.XPath, Using = "(.//*[@role='presentation']/div[(.='Add New Card')]|(.//*/h2)[1]/../div[2][//li[(.='Add new card')]])")]
        [CacheLookup]
        internal IWebElement AddNewCardLst { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='CardBillingAddressWithLabel']/div[1]/span[1]")]
        [CacheLookup]
        internal IWebElement CardBillingAddDdl { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='CardBillingAddressWithLabel']/div[2]/div/ul//li[.='Add new contact']")]
        [CacheLookup]
        internal IWebElement AddNewAddressLst { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='billing_register_company_checkbox']")]
        [CacheLookup]
        internal IWebElement BillingRegisterCompanyChk { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='billing_first_name']")]
        [CacheLookup]
        internal IWebElement BillingUserFirstNameTxt { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='billing_last_name']")]
        [CacheLookup]
        internal IWebElement BillingUserLastNameTxt { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='billing_company_name']")]
        [CacheLookup]
        internal IWebElement BillingUserCompanyNameTxt { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='billing_job_title']")]
        [CacheLookup]
        internal IWebElement BillingUserJobTitleTxt { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='billing_address1']")]
        [CacheLookup]
        internal IWebElement BillingUserAddress1Txt { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='billing_address2']")]
        [CacheLookup]
        internal IWebElement BillingUserAddress2Txt { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='billing_city']")]
        [CacheLookup]
        internal IWebElement BillingUserCityTxt { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='billing_state']")]
        [CacheLookup]
        internal IWebElement BillingUserStateTxt { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='billing_zip']")]
        [CacheLookup]
        internal IWebElement BillingUserZipcodeTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[header='Payment Method']")]
        [CacheLookup]
        internal IWebElement PaymentMethodHeadingTxt { get; set; }
        [FindsBy(How = How.Id, Using = "saveCreditCard")]
        [CacheLookup]
        internal IWebElement RememberCardChk { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='AddOrEditCardDiv']/div[contains(@class,'big-spacing remember-settings')]/div")]
        [CacheLookup]
        internal IWebElement ReCaptchaBlock { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*/div[contains(@class,'rc-anchor-alert')]/../div")]
        [CacheLookup]
        internal IList<IWebElement> ReCaptchaDivCount { get; set; }
        [FindsBy(How = How.XPath, Using = "/html/body/div[2]/div[3]/div[1]/div")]
        [CacheLookup]
        internal IWebElement ReCaptchaChk { get; set; }
    }
}