using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NamecheapUITests.PagefactoryObject.PaymentProcessPageFactory
{
    public class AccountFundspagefactory
    {
        [FindsBy(How = How.CssSelector, Using = "#s2id_SelectedCard > a")]
        [CacheLookup]
        internal IWebElement SelectedCardDdl { get; set; }
        [FindsBy(How = How.CssSelector, Using = ".btn.nc_btnSubmit")]
        [CacheLookup]
        internal IWebElement ExistingCardPageNextBtn { get; set; }
        [FindsBy(How = How.CssSelector, Using = ".btn.nc_btnSubmit")]
        [CacheLookup]
        internal IWebElement CardPageSubmitBtn { get; set; }

        [FindsBy(How = How.PartialLinkText, Using = "add funds")]
        [CacheLookup]
        internal IWebElement AddFundsLinkTxt { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='secure-card']/../label")]
        [CacheLookup]
        internal IWebElement SecureCardRdo { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@class='btn nc_billingTopup_submit']")]
        [CacheLookup]
        internal IWebElement PaymentMethodNextBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='maincontent']/div[5]/div/form/div[1]/div[2]/p")]
        [CacheLookup]
        internal IWebElement TopUpPageCurrentBalLbl { get; set; }

        [FindsBy(How = How.Name, Using = "AmountToTopUp")]
        [CacheLookup]
        internal IWebElement TopUpAmountTxt { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@value='Next']")]
        [CacheLookup]
        internal IWebElement TopUppageNextBtn { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='maincontent']/div[2]/div/div/ul/li[1]")]
        [CacheLookup]
        internal IWebElement AmountSelectionNavLst { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*/form/div[1]/div[2]/p[@class='no-input']")]
        [CacheLookup]
        internal IWebElement CardNavListPageCurrentBalLbl { get; set; }
        [FindsBy(How = How.CssSelector, Using = "#maincontent > div:nth-child(12) > div > form > div:nth-child(8) > div.small-9.xlarge-6.xxlarge-5.columns > p")]
        [CacheLookup]
        internal IWebElement CardNavListPageAmttoAddLbl { get; set; }
        [FindsBy(How = How.Id, Using = "Billing_name")]
        [CacheLookup]
        internal IWebElement BillingNameTxt { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*/form/div[4]//div[@class='xlarge-4 xxlarge-3 columns']/label)[1]")]
        [CacheLookup]
        internal IWebElement CarddetailsSelectionLbl { get; set; }

        [FindsBy(How = How.Id, Using = "carddetails")]
        [CacheLookup]
        internal IWebElement CardOptionDdl { get; set; }

        [FindsBy(How = How.Id, Using = "saveCreditCard")]
        [CacheLookup]
        internal IWebElement SaveCardChk { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='addEditCardSection']/div[3]/div[1]/div/div[1]/div/label")]
        [CacheLookup]
        internal IWebElement SaveCardLbl { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='maincontent']/div[2]/div/div/ul/li[3]")]
        [CacheLookup]
        internal IWebElement ConfrimNavList { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='errorDisplayParentDiv']/p")]
        [CacheLookup]
        internal IWebElement ErrorMessageLblTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='errorDisplayParentDiv']")]
        [CacheLookup]
        internal IWebElement ErrorMessageLbl { get; set; }

    }
}