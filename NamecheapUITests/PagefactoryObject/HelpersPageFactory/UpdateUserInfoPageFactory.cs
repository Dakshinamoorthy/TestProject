using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PagefactoryObject.HelpersPageFactory
{
    public class UpdateUserInfoPageFactory
    {
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@id),' '),'Billing_name')]")]
        [CacheLookup]
        internal IWebElement NameonCardInputTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@name),' '),'privateStripeFrame')]")]
        [CacheLookup]
        internal IWebElement CardStripeFrame { get; set; }
        [FindsBy(How = How.Name, Using = "cardnumber")]
        [CacheLookup]
        internal IWebElement CardnumberInputTxt { get; set; }
        [FindsBy(How = How.Name, Using = "exp-date")]
        [CacheLookup]
        internal IWebElement CardValidityInputTxt { get; set; }
        [FindsBy(How = How.Name, Using = "cvc")]
        [CacheLookup]
        internal IWebElement CardCvcInputTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*/label[contains(concat(' ',normalize-space(@for),' '),'friendly_name')]/../following-sibling::div[1]/input")]
        [CacheLookup]
        internal IWebElement CardNameInputTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*/label[contains(concat(' ',normalize-space(@for),' '),'saveCreditCard')]")]
        [CacheLookup]
        internal IWebElement SaveCardForlaterUserchk { get; set; }
        [FindsBy(How = How.Name, Using = "cvc")]
        [CacheLookup]
        internal IWebElement CardType { get; set; }
        [FindsBy(How = How.XPath, Using = ".//div[contains(concat(' ',normalize-space(@id),' '),'CardUseAsOption')]")]
        [CacheLookup]
        internal IWebElement UseasDd { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='CardUseAsOption']/option")]
        [CacheLookup]
        internal IList<IWebElement> UseasDdl { get; set; }
        [FindsBy(How = How.Id, Using = "CardUseAsOption")]
        [CacheLookup]
        internal IWebElement UseasOptions { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@role='listbox']/li[1]/div")]
        [CacheLookup]
        internal IWebElement NoneCardOption { get; set; }
        //Basic Information
        [FindsBy(How = How.XPath, Using = ".//*/label[contains(concat(' ',normalize-space(text()),' '),'Address Name')]/../following-sibling::div[1]/p")]
        [CacheLookup]
        internal IWebElement AddressNameParaTxt { get; set; }
        [FindsBy(How = How.Id, Using = "addressName")]
        [CacheLookup]
        internal IWebElement AddressNameTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*/label[contains(concat(' ',normalize-space(text()),' '),'First Name')]/../following-sibling::div[1]/input")]
        [CacheLookup]
        internal IWebElement FirstNameTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*/label[contains(concat(' ',normalize-space(text()),' '),'Last Name')]/../following-sibling::div[1]/input")]
        [CacheLookup]
        internal IWebElement LastNameTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='maincontent']/form/div[5]|.//div[contains(@ng-show,'modalBehavior.model.isCompanyAddress')]")]
        [CacheLookup]
        internal IWebElement CompanyNameSection { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@id='maincontent']/form/div[5]/div[2]/div/div[1]/div/label|.//label[@for='company']")]
        [CacheLookup]
        internal IWebElement CompanyAddressChk { get; set; }
        [FindsBy(How = How.Name, Using = "Address.Organization")]
        [CacheLookup]
        internal IWebElement CompanyNameTxt { get; set; }
        [FindsBy(How = How.Name, Using = "Address.JobTitle")]
        [CacheLookup]
        internal IWebElement JobTitleeTxt { get; set; }
        //street Info
        //.//*/label[contains(concat(' ',normalize-space(text()),' '),'Country')]/../following-sibling::div[1]/div/a | .//*/div[contains(@class,'add-new-contact billing-option nc_address')]//label[contains(concat(' ',normalize-space(text()),' '),'Country')]/../div[1]
        [FindsBy(How = How.XPath, Using = ".//*/label[contains(concat(' ',normalize-space(text()),' '),'Country')]/../following-sibling::div[1]/div/a | .//*/div[contains(@class,'add-new-contact billing-option nc_address')]//label[contains(concat(' ',normalize-space(text()),' '),'Country')]/../div[1]")]
        [CacheLookup]
        internal IWebElement CountryDd { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@role),' '),'listbox')]/li/div[not(contains(concat(' ',normalize-space(text()),' '), 'Select Country'))]")]
        [CacheLookup]
        internal IList<IWebElement> CountryLst { get; set; }
        //contact Info
        [FindsBy(How = How.XPath, Using = ".//*/label[contains(concat(' ',normalize-space(text()),' '),' Phone Number')]/../following-sibling::div[1]/div/a")]
        [CacheLookup]
        internal IWebElement PhoneNumberDd { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[contains(concat(' ',normalize-space(@role),' '),'listbox')]/li/div[not(contains(concat(' ',normalize-space(text()),' '), 'Select'))])")]
        [CacheLookup]
        internal IList<IWebElement> PhoneNumberDdl { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='Address_Phone']|.//*[@id='tel']")]
        [CacheLookup]
        internal IWebElement PhoneNumberTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*/label[contains(concat(' ',normalize-space(text()),' '),' Fax Number')]/../following-sibling::div[1]/div/a")]
        [CacheLookup]
        internal IWebElement FaxNumberDd { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[contains(concat(' ',normalize-space(@role),' '),'listbox')]/li/div[not(contains(concat(' ',normalize-space(text()),' '), 'Select'))])")]
        [CacheLookup]
        internal IList<IWebElement> FaxNumberDdl { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='Address_Fax']|.//*[@id='fax']")]
        [CacheLookup]
        internal IWebElement FaxNumberTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*/label[contains(concat(' ',normalize-space(text()),' '),'Email ')]/../following-sibling::div[1]/p")]
        [CacheLookup]
        internal IWebElement EmailAddressParaTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='Address_EmailAddress']|.//*[@id='email']")]
        [CacheLookup]
        internal IWebElement EmailAddressTxt { get; set; }
        //Street Info
        [FindsBy(How = How.XPath, Using = ".//*/label[text()='Address']/../following-sibling::div/input[contains(@name,'ddress')]")]
        [CacheLookup]
        internal IWebElement Address1Txt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*/label[text()='Address']/../../following-sibling::div[1]/div/input[contains(@name,'ddress')]")]
        [CacheLookup]
        internal IWebElement Address2Txt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*/label[contains(concat(' ',normalize-space(text()),' '),'City')]/../following-sibling::div/input")]
        [CacheLookup]
        internal IWebElement CityTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*/label[contains(concat(' ',normalize-space(text()),' '),'State/Province')]/../following-sibling::div/input")]
        [CacheLookup]
        internal IWebElement StateTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*/label[contains(concat(' ',normalize-space(text()),' '),'ZIP')]/../following-sibling::div/input")]
        [CacheLookup]
        internal IWebElement ZipTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*/label[contains(concat(' ',normalize-space(text()),' '),'Country')]/../following-sibling::div[1]/div/a/span[1]")]
        [CacheLookup]
        internal IWebElement CountryDdtxt { get; set; }
        //Save
        [FindsBy(How = How.XPath, Using = ".//input[@type='submit' and contains(@value,'Save') or contains(@value,'Next')]")]
        [CacheLookup]
        internal IWebElement SaveBtn { get; set; }
        [FindsBy(How = How.XPath, Using = ".//p[contains(concat(' ',normalize-space(@class),' '),'cont-msg error')] | //div[@id='maincontent']/div[1]/div/h1")]
        [CacheLookup]
        internal IList<IWebElement> ErrorCount { get; set; }
        [FindsBy(How = How.XPath, Using = ".//form[contains(concat(' ',normalize-space(@class),' '),'ElementsApp')]")]
        [CacheLookup]
        internal IWebElement CardStripeError { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*/h1[contains(concat(' ',normalize-space(@class),' '),'section-title')]")]
        [CacheLookup]
        internal IWebElement SectiontitleLbl { get; set; }
    }
}