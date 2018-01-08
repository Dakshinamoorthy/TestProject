using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NamecheapUITests.PagefactoryObject.PaymentProcessPageFactory
{
    public class AccountNavPageFactory
    {
        [FindsBy(How = How.CssSelector, Using = "#ctl00_ctl00_ctl00_ctl00_base_content_web_base_content_home_content_page_content_left_addressControl_Organization")]
        [CacheLookup]
        internal IWebElement AccountInfoOrganizationNametxt { get; set; }
        [FindsBy(How = How.CssSelector, Using = "#ctl00_ctl00_ctl00_ctl00_base_content_web_base_content_home_content_page_content_left_addressControl_JobTitle")]
        [CacheLookup]
        internal IWebElement AccountInfoJobTitletxt { get; set; }
        [FindsBy(How = How.CssSelector, Using = "#ctl00_ctl00_ctl00_ctl00_base_content_web_base_content_home_content_page_content_left_addressControl_Address1")]
        [CacheLookup]
        internal IWebElement AccountInfoAddress1Txt { get; set; }
        [FindsBy(How = How.CssSelector, Using = "#ctl00_ctl00_ctl00_ctl00_base_content_web_base_content_home_content_page_content_left_addressControl_Address2")]
        [CacheLookup]
        internal IWebElement AccountInfoAddress2Txt { get; set; }
        [FindsBy(How = How.CssSelector, Using = "#ctl00_ctl00_ctl00_ctl00_base_content_web_base_content_home_content_page_content_left_addressControl_City")]
        [CacheLookup]
        internal IWebElement AccountInfoCityTxt { get; set; }
        [FindsBy(How = How.CssSelector, Using = "#ctl00_ctl00_ctl00_ctl00_base_content_web_base_content_home_content_page_content_left_addressControl_StateProvince")]
        [CacheLookup]
        internal IWebElement AccountInfoStateTxt { get; set; }
        [FindsBy(How = How.CssSelector, Using = "#ctl00_ctl00_ctl00_ctl00_base_content_web_base_content_home_content_page_content_left_addressControl_PostalCode")]
        [CacheLookup]
        internal IWebElement AccountInfoZipCodeTxt { get; set; }
        [FindsBy(How = How.CssSelector, Using = "#content > div > div.grid-col.three-quarters > div.module.receipt-details-module.nc_address > ul > li > div:nth-child(7) > label")]
        [CacheLookup]
        internal IWebElement AccountInfoScrollText { get; set; }
        [FindsBy(How = How.CssSelector, Using = "#ctl00_ctl00_ctl00_ctl00_base_content_web_base_content_home_content_page_content_left_addressControl_Phone")]
        [CacheLookup]
        internal IWebElement AccountInfoPhonetxt { get; set; }
        [FindsBy(How = How.CssSelector, Using = "#ctl00_ctl00_ctl00_ctl00_base_content_web_base_content_home_content_page_content_left_addressControl_Fax")]
        [CacheLookup]
        internal IWebElement AccountInfoFaxtxt { get; set; }
        [FindsBy(How = How.CssSelector, Using = "#content > div > div.grid-col.three-quarters > div.module.receipt-details-module.nc_address > ul > li > div:nth-child(8) > div.four-cols.coreuiselect.b-core-ui-select.nc_country.nc_country_required")]
        [CacheLookup]
        internal IWebElement ContactPageCountLbl { get; set; }
        [FindsBy(How = How.XPath, Using = " //*[@id='content']/div/div[1]/div[2]/ul/li/div[7]/div[2]/div/ul/li[contains(.,'India')]")]
        [CacheLookup]
        internal IWebElement ContactPageCountIndiaDdl { get; set; }
        [FindsBy(How = How.Id, Using = "btn_Submit")]
        [CacheLookup]
        internal IWebElement ContinueBtn { get; set; }
    }
}