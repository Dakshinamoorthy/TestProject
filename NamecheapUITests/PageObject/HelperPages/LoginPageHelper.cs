using System;
using System.Threading;
using System.Windows.Forms;
using Gallio.Framework;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.ValidationPages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PageObject.HelperPages
{
    public class LoginPageHelper
    {
        public void NcLogin()
        {
            Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt("Domain"),
                "A valid Website was not Opened.");
            PageInitHelper<LoginPageHelper>.PageInit.Login();
            PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
            Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt("Dashboard"),
                "Invalid User Name or Password.");

        }
        public void Login(string username = "", string password = "")
        {
            if (string.IsNullOrEmpty(username))
            {
                username = AppConfigHelper.UserName;
                password = AppConfigHelper.Password;
            }
            ChangeToUsd();
            PageInitHelper<PageNavigationHelper>.PageInit.MoveToElement(PageInitHelper<LoginPageHelper>.PageInit.SignIn);
            PageInitHelper<LoginPageHelper>.PageInit.SignInUsername.SendKeys(username);
            PageInitHelper<LoginPageHelper>.PageInit.SignInPassword.SendKeys(password);
            PageInitHelper<LoginPageHelper>.PageInit.SignInTopNavLoginlink.Click();
        }
        public void ChangeToUsd()
        {
            PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<LoginPageHelper>.PageInit.CurrencyLnk, PageInitHelper<LoginPageHelper>.PageInit.UsdLst);
        }
        public void NcLogout()
        {
            PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<LoginPageHelper>.PageInit.UserNameHeaderMenu, PageInitHelper<LoginPageHelper>.PageInit.LogOutLink);
            Assert.IsTrue(BrowserInit.Driver.Url.Contains(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("CMS", "myaccount/signout")), "Url Navigation Wrong: expected:- " + PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("CMS", "myaccount/signout?loggedout=yes") + ", but actal Navigate to " + BrowserInit.Driver.Url);
            ChangeToUsd();
        }
        public void UserSignUpPage()
        {
            PageInitHelper<UrlNavigationHelper>.PageInit.GoToUrl();
            PageInitHelper<PageNavigationHelper>.PageInit.WaitForPageLoad();
            PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
            Assert.IsTrue(PageInitHelper<LoginPageHelper>.PageInit.UserSignUp.Displayed, "In Namecheap home page User Signup Link is missing");
            PageInitHelper<LoginPageHelper>.PageInit.UserSignUp.Click();
            PageInitHelper<PageNavigationHelper>.PageInit.WaitForPageLoad();
            Assert.IsTrue(BrowserInit.Driver.Url.Contains(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("CMS", "myaccount/signup.aspx")), "Url Navigation Wrong: expected:- " + PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("CMS", "myaccount/signup.aspx") + ", but actal Navigate to " + BrowserInit.Driver.Url);
            PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
            var userDetails = PageInitHelper<UserPersonalDataGenetrator>.PageInit.UserNewAccount();
            PageInitHelper<LoginPageHelper>.PageInit.SignupUserNameTxt.SendKeys(userDetails.Item1);
            PageInitHelper<LoginPageHelper>.PageInit.SignupPassword.SendKeys(userDetails.Item4);
            PageInitHelper<LoginPageHelper>.PageInit.SignupConfirmPassword.SendKeys(userDetails.Item4);
            PageInitHelper<LoginPageHelper>.PageInit.SignupFirstname.SendKeys(userDetails.Item2);
            PageInitHelper<LoginPageHelper>.PageInit.SignupLastname.SendKeys(userDetails.Item3);
            PageInitHelper<LoginPageHelper>.PageInit.SignupEmail.SendKeys(userDetails.Item5);
            if (PageInitHelper<LoginPageHelper>.PageInit.NewsLetterchk.Selected)
                PageInitHelper<LoginPageHelper>.PageInit.NewsLetterchk.Click();
            PageInitHelper<LoginPageHelper>.PageInit.SignupCreateUser.Click();
            PageInitHelper<LoginPageHelper>.PageInit.HandleErrorMessageValidation(userDetails);

        }
        public void HandleErrorMessageValidation(Tuple<string, string, string, string, string> userDetails)
        {
            if (BrowserInit.Driver.Url.Contains("signup.aspx") && PageInitHelper<LoginPageHelper>.PageInit.ErrorMessageTitleTxt.Text.Contains("SECURITY CHECK"))
            {
                ValidateCaptchaImage(userDetails);
            }
            else if (BrowserInit.Driver.Url.Contains("signup.aspx"))
            {
                VerifyErrorMessageValidation();
            }
            else if (BrowserInit.Driver.Title.Contains(UiConstantHelper.Dashboard) || BrowserInit.Driver.Url.Contains("addresspage.aspx"))
            {
                PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
            }
            else
            {
                throw new TestFailedException("USER EXCEPTION : Page Redirect to Some other Source : " + BrowserInit.Driver.Title);
            }
        }
        public void ValidateCaptchaImage(Tuple<string, string, string, string, string> userDetails)
        {
            LabelRecaptcha:
            const string script = "return document.querySelectorAll('#reCaptchaSignUp>div').length;";
            var count = ((IJavaScriptExecutor)BrowserInit.Driver).ExecuteScript(script);
            if (count.ToString().Equals("1"))
            {
                var captchaFrame = BrowserInit.Driver.FindElement(By.CssSelector("iframe[src*='api2/anchor']"));
                BrowserInit.Driver.SwitchTo().Frame(captchaFrame);
                PageInitHelper<LoginPageHelper>.PageInit.Captchaframechk.Click();
                BrowserInit.Driver.SwitchTo().DefaultContent();
                var imageFrame = BrowserInit.Driver.FindElement(By.CssSelector("iframe[src*='api2/frame']"));
                Thread.Sleep(2000);
                BrowserInit.Driver.SwitchTo().Frame(imageFrame);
                var captchatitleimage = PageInitHelper<LoginPageHelper>.PageInit.SelectImgTitleTxt.Text;
                MessageBox.Show(@"Recaptcha" + Environment.NewLine + @"Select the captcha image wich mataches: " + captchatitleimage,
                    @"Important Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                Thread.Sleep(10 * 1000);
                MessageBox.Show(@"Once Completed Captcha click Ok button To further Automation process",
                    @"Important Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                BrowserInit.Driver.SwitchTo().ParentFrame();
                PageInitHelper<LoginPageHelper>.PageInit.SignupPassword.Clear();
                PageInitHelper<LoginPageHelper>.PageInit.SignupPassword.SendKeys(userDetails.Item2);
                PageInitHelper<LoginPageHelper>.PageInit.SignupConfirmPassword.Clear();
                PageInitHelper<LoginPageHelper>.PageInit.SignupConfirmPassword.SendKeys(userDetails.Item2);
                PageInitHelper<LoginPageHelper>.PageInit.SignupCreateUser.Click();
            }
            else
            {
                PageInitHelper<LoginPageHelper>.PageInit.SignupPassword.Clear();
                PageInitHelper<LoginPageHelper>.PageInit.SignupPassword.SendKeys(userDetails.Item2);
                PageInitHelper<LoginPageHelper>.PageInit.SignupConfirmPassword.Clear();
                PageInitHelper<LoginPageHelper>.PageInit.SignupConfirmPassword.SendKeys(userDetails.Item2);
                PageInitHelper<LoginPageHelper>.PageInit.SignupCreateUser.Click();
                goto LabelRecaptcha;
            }
            HandleErrorMessageValidation(userDetails);
        }

        public void VerifyErrorMessageValidation()
        {
            LabelGenerateUserName:
            var userDetails = PageInitHelper<UserPersonalDataGenetrator>.PageInit.UserNewAccount();
            var getusername = PageInitHelper<LoginPageHelper>.PageInit.SignupUserNameTxt.GetAttribute(UiConstantHelper.AttributeValue);
            PageInitHelper<LoginPageHelper>.PageInit.SignupUserNameTxt.Clear();
            PageInitHelper<LoginPageHelper>.PageInit.SignupUserNameTxt.SendKeys(userDetails.Item1);
            if (PageInitHelper<LoginPageHelper>.PageInit.SignupUserNameTxt.GetAttribute(UiConstantHelper.AttributeValue).Equals(getusername))
            {
                goto LabelGenerateUserName;
            }
            PageInitHelper<LoginPageHelper>.PageInit.SignupPassword.Clear();
            PageInitHelper<LoginPageHelper>.PageInit.SignupPassword.SendKeys(userDetails.Item2);
            PageInitHelper<LoginPageHelper>.PageInit.SignupConfirmPassword.Clear();
            PageInitHelper<LoginPageHelper>.PageInit.SignupConfirmPassword.SendKeys(userDetails.Item2);
            PageInitHelper<LoginPageHelper>.PageInit.SignupFirstname.Clear();
            PageInitHelper<LoginPageHelper>.PageInit.SignupFirstname.SendKeys(userDetails.Item3);
            PageInitHelper<LoginPageHelper>.PageInit.SignupLastname.Clear();
            PageInitHelper<LoginPageHelper>.PageInit.SignupLastname.SendKeys(userDetails.Item4);
            PageInitHelper<LoginPageHelper>.PageInit.SignupEmail.Clear();
            PageInitHelper<LoginPageHelper>.PageInit.SignupEmail.SendKeys(userDetails.Item5);
            PageInitHelper<LoginPageHelper>.PageInit.SignupCreateUser.Click();
            HandleErrorMessageValidation(userDetails);
        }
        public void LoginPage()
        {
            PageInitHelper<LoginPageHelper>.PageInit.LoginFormUserName.SendKeys(AppConfigHelper.UserName);
            PageInitHelper<LoginPageHelper>.PageInit.LoginFormPassword.SendKeys(AppConfigHelper.Password);
            PageInitHelper<LoginPageHelper>.PageInit.LoginFormSignInBtn.Click();
            Thread.Sleep(3000);
        }
        #region LoginPageFactory
        [FindsBy(How = How.ClassName, Using = "current-currency")]
        [CacheLookup]
        internal IWebElement CurrencyLnk { get; set; }
        [FindsBy(How = How.ClassName, Using = "usd")]
        [CacheLookup]
        internal IWebElement UsdLst { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[contains(@class, 'toggle') and contains(., 'Sign In')]")]
        [CacheLookup]
        internal IWebElement SignIn { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@title='Username']")]
        [CacheLookup]
        internal IWebElement SignInUsername { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@title='Password']")]
        [CacheLookup]
        internal IWebElement SignInPassword { get; set; }
        [FindsBy(How = How.Id, Using = "ctl00_ctl00_ctl00_ctl00_base_content_web_base_content_topNavLoginLink")]
        [CacheLookup]
        internal IWebElement SignInTopNavLoginlink { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@id),' '),'header')]//li[contains(@class,'user-menu')] | .//a[contains(@class,'gb-dropdown')][contains(@href,'/dashboard')]")]
        [CacheLookup]
        internal IWebElement UserNameHeaderMenu { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@id),' '),'header')]//li[contains(@class,'user-menu')]/div/div/div[2]/ul/li[3]")]
        [CacheLookup]
        internal IWebElement LogOutLink { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'top-hat expandable-group')]/div/Ul[1]/li[3]")]
        [CacheLookup]
        public IWebElement UserSignUp { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'create-account-module')]//strong[contains(concat(' ',normalize-space(@class),' '),'title')]")]
        [CacheLookup]
        internal IWebElement ErrorMessageTitleTxt { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".recaptcha-checkbox-checkmark")]
        [CacheLookup]
        internal IWebElement Captchaframechk { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@id),' '),'rc-imageselect')]/div[2]/div[1]/div[1]/div[1]")]
        [CacheLookup]
        internal IWebElement SelectImgTitleTxt { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'create-account-module')]//input[contains(concat(' ',normalize-space(@class),' '),'nc_username nc_username_required')]")]
        [CacheLookup]
        internal IWebElement SignupUserNameTxt { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'create-account-module')]//input[contains(concat(' ',normalize-space(@class),' '),'nc_password nc_password_required')]")]
        [CacheLookup]
        internal IWebElement SignupPassword { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'create-account-module')]//input[contains(concat(' ',normalize-space(@class),' '),'nc_password_confirm nc_password_confirm_required')]")]
        [CacheLookup]
        internal IWebElement SignupConfirmPassword { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'create-account-module')]//input[contains(concat(' ',normalize-space(@class),' '),'Email nc_email nc_email_required')]")]
        [CacheLookup]
        internal IWebElement SignupEmail { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'create-account-module')]//input[contains(concat(' ',normalize-space(@class),' '),'nc_firstname nc_firstname_required')]")]
        [CacheLookup]
        internal IWebElement SignupFirstname { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'create-account-module')]//input[contains(concat(' ',normalize-space(@class),' '),'nc_lastname nc_lastname_required')]")]
        [CacheLookup]
        internal IWebElement SignupLastname { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(concat(' ',normalize-space(@class),' '),'create-account-module')]//input[contains(concat(' ',normalize-space(@class),' '),'btn normal-btn nc_signup_submit')]")]
        [CacheLookup]
        internal IWebElement SignupCreateUser { get; set; }

        [FindsBy(How = How.XPath, Using = ".//label[contains(concat(' ',normalize-space(@for),' '),'Newsletter')]/input")]
        [CacheLookup]
        internal IWebElement NewsLetterchk { get; set; }

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

        [FindsBy(How = How.CssSelector, Using = "#content > div > div.grid-col.three-quarters > div.module.receipt-details-module.nc_address > ul > li > div:nth-child(8) > div.b-core-ui-select__dropdown.show > div > ul > li:nth-child(103)")]
        [CacheLookup]
        internal IWebElement ContactPageCountIndiaDdl { get; set; }

        [FindsBy(How = How.Id, Using = "btn_Submit")]
        [CacheLookup]
        internal IWebElement ContinueBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "//fieldset[contains(concat(' ',normalize-space(@class),' '),'loginForm')]//input[contains(concat(' ',normalize-space(@class),' '),'nc_username')]")]
        [CacheLookup]
        public IWebElement LoginFormUserName { get; set; }

        [FindsBy(How = How.XPath, Using = "//fieldset[contains(concat(' ',normalize-space(@class),' '),'loginForm')]//input[contains(concat(' ',normalize-space(@class),' '),'nc_password')]")]
        [CacheLookup]
        public IWebElement LoginFormPassword { get; set; }

        [FindsBy(How = How.XPath, Using = "//fieldset[contains(concat(' ',normalize-space(@class),' '),'loginForm')]//input[contains(concat(' ',normalize-space(@class),' '),'nc_login_submit')]")]
        [CacheLookup]
        public IWebElement LoginFormSignInBtn { get; set; }

        #endregion
    }
}