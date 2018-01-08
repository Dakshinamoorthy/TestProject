using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NamecheapUITests.PagefactoryObject.HelpersPageFactory;
using NamecheapUITests.PageObject.Interface;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
namespace NamecheapUITests.PageObject.HelperPages.WrapperFactory
{
    public class UserInfoUpdationHelper : AUserInfoUpdation
    {
        public string AddressNameText { get; set; }
        public int FlagBreak { get; set; }
        public override void ManagingCardInfo(string cardUseAsOption)
        {
            var flag = Pageherderselection();
            if (flag == 1)
            {
                NewCardDetails(cardUseAsOption);
                BillingAddressDetails();
                SaveInfo();
            }
            else
            {
                BillingAddressDetails();
                SaveInfo();
            }
        }
        public void ManagingAddressInfo()
        {
            BasicInformation();
            BillingAddressDetails();
            ContactInformation();
            SaveInfo();
        }
        public virtual int Pageherderselection()
        {
            var pageheading = Regex.Replace(PageInitHelper<UpdateUserInfoPageFactory>.PageInit.SectiontitleLbl.Text, "Address" + "|" + "Card", "").Trim();
            var flagNumber = 0;
            FlagBreak = 0;
            if (BrowserInit.Driver.Url.IndexOf("/creditcardtopup", StringComparison.CurrentCultureIgnoreCase) != -1 && AppConfigHelper.LiveCardPurchase.Equals("N", StringComparison.InvariantCultureIgnoreCase))
            {
                if (PageInitHelper<UpdateUserInfoPageFactory>.PageInit.NameonCardInputTxt.GetAttribute(UiConstantHelper.AttributeValue).Equals(string.Empty))
                {
                    FlagBreak = 1;
                }
                else if (PageInitHelper<UpdateUserInfoPageFactory>.PageInit.NameonCardInputTxt.GetAttribute(UiConstantHelper.AttributeValue) != string.Empty)
                {
                    FlagBreak = 2;
                }
            }
            if ((pageheading.IndexOf(UiConstantHelper.AddText, StringComparison.InvariantCultureIgnoreCase) !=
              -1 ||
                pageheading.IndexOf(UiConstantHelper.New, StringComparison.InvariantCultureIgnoreCase) !=
              -1 ||
                pageheading.IndexOf("Top Up", StringComparison.InvariantCultureIgnoreCase) !=
              -1 ||
                pageheading.IndexOf(UiConstantHelper.Add, StringComparison.InvariantCultureIgnoreCase) !=
              -1 || FlagBreak == 1))
            {
                flagNumber = 1;
            }
            else if (pageheading.IndexOf(UiConstantHelper.Edit, StringComparison.InvariantCultureIgnoreCase) !=
                   -1 ||
                     pageheading.IndexOf(UiConstantHelper.Manage, StringComparison.InvariantCultureIgnoreCase) !=
                   -1 | FlagBreak == 2 || AppConfigHelper.LiveCardPurchase.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
            {
                flagNumber = 2;
            }
            return flagNumber;
        }

        public override void LiveCard()
        {
            var liveCard = PageInitHelper<UserPersonalDataGenetrator>.PageInit.RealCard();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.NameonCardInputTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.NameonCardInputTxt.SendKeys(liveCard.Item1);
            BrowserInit.Driver.SwitchTo().Frame(PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardStripeFrame);
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardnumberInputTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardnumberInputTxt.SendKeys(liveCard.Item2);
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardValidityInputTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardValidityInputTxt.SendKeys(liveCard.Item3 + liveCard.Item4);
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardCvcInputTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardCvcInputTxt.SendKeys(liveCard.Item5);
            BrowserInit.Driver.SwitchTo().ParentFrame();

        }

        public override void DummyCardDetails()
        {
            var cardDetails = PageInitHelper<UserPersonalDataGenetrator>.PageInit.CardDetails();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.NameonCardInputTxt.SendKeys(cardDetails.Item1);
            BrowserInit.Driver.SwitchTo().Frame(PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardStripeFrame);
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardnumberInputTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardnumberInputTxt.SendKeys(cardDetails.Item2);
            VerifyCardNumber:
            if (PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardStripeError.GetAttribute(UiConstantHelper.AttributeClass).Contains("is-invalid"))
            {
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardnumberInputTxt.Clear();
                var creditCardNumber = PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                EnumHelper.ExcelDataEnum.Creditcardnumbers.ToString());
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardnumberInputTxt.SendKeys(creditCardNumber);
                goto VerifyCardNumber;
            }
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardValidityInputTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardValidityInputTxt.SendKeys(cardDetails.Item3 + cardDetails.Item4);
            VerifyCardvalidity:
            if (PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardStripeError.GetAttribute(UiConstantHelper.AttributeClass).Contains("is-invalid"))
            {
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardValidityInputTxt.Clear();
                var number = new Random();
                var now = DateTime.Now;
                var cardValidMonth = now.ToString("MM");
                var addYears = now.AddYears(UiConstantHelper.Ten).ToString("yy");
                var addyearupto = now.AddYears(UiConstantHelper.Twenty).ToString("yy");
                var cardValidYear = number.Next(Convert.ToInt32(addYears), Convert.ToInt32(addyearupto)).ToString();
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardValidityInputTxt.SendKeys(cardValidMonth + cardValidYear);
                goto VerifyCardvalidity;
            }
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardCvcInputTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardCvcInputTxt.SendKeys(cardDetails.Item5.ToString());
            BrowserInit.Driver.SwitchTo().ParentFrame();
            if (FlagBreak == 1)
            {
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.SaveCardForlaterUserchk.Click();
            }
            if (BrowserInit.Driver.Url.Contains("cart/checkout/payment")) return;
            BrowserInit.Driver.SwitchTo().Frame(PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardStripeFrame);
            var cardstring = BrowserInit.Driver.FindElement(By.XPath("//*[@id='root']/form/div/div[1]/div/div[1]/img")).GetAttribute("src");
            string s = cardstring.Substring(cardstring.IndexOf("img", StringComparison.Ordinal));
            string s1 = s.Substring(0, s.IndexOf("-", StringComparison.Ordinal));
            string cardType = s1.Substring(s1.IndexOf("/", StringComparison.Ordinal) + 1);

            BrowserInit.Driver.SwitchTo().ParentFrame();
        }
        public override void NewCardDetails(string cardUseAsOption)
        {
            var cardDetails = PageInitHelper<UserPersonalDataGenetrator>.PageInit.CardDetails();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.NameonCardInputTxt.SendKeys(cardDetails.Item1);
            BrowserInit.Driver.SwitchTo().Frame(PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardStripeFrame);
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardnumberInputTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardnumberInputTxt.SendKeys(cardDetails.Item2);
            VerifyCardNumber:
            if (PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardStripeError.GetAttribute(UiConstantHelper.AttributeClass).Contains("is-invalid"))
            {
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardnumberInputTxt.Clear();
                var creditCardNumber = PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                EnumHelper.ExcelDataEnum.Creditcardnumbers.ToString());
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardnumberInputTxt.SendKeys(creditCardNumber);
                goto VerifyCardNumber;
            }
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardValidityInputTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardValidityInputTxt.SendKeys(cardDetails.Item3 + cardDetails.Item4);
            VerifyCardvalidity:
            if (PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardStripeError.GetAttribute(UiConstantHelper.AttributeClass).Contains("is-invalid"))
            {
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardValidityInputTxt.Clear();
                var number = new Random();
                var now = DateTime.Now;
                var cardValidMonth = now.ToString("MM");
                var addYears = now.AddYears(UiConstantHelper.Ten).ToString("yy");
                var addyearupto = now.AddYears(UiConstantHelper.Twenty).ToString("yy");
                var cardValidYear = number.Next(Convert.ToInt32(addYears), Convert.ToInt32(addyearupto)).ToString();
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardValidityInputTxt.SendKeys(cardValidMonth + cardValidYear);
                goto VerifyCardvalidity;
            }
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardCvcInputTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardCvcInputTxt.SendKeys(cardDetails.Item5.ToString());
            BrowserInit.Driver.SwitchTo().ParentFrame();
            if (FlagBreak == 1)
            {
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.SaveCardForlaterUserchk.Click();
            }
            if (BrowserInit.Driver.Url.Contains("cart/checkout/payment")) return;
            BrowserInit.Driver.SwitchTo().Frame(PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardStripeFrame);
            var cardstring = BrowserInit.Driver.FindElement(By.XPath("//*[@id='root']/form/div/div[1]/div/div[1]/img")).GetAttribute("src");
            string s = cardstring.Substring(cardstring.IndexOf("img", StringComparison.Ordinal));
            string s1 = s.Substring(0, s.IndexOf("-", StringComparison.Ordinal));
            string cardType = s1.Substring(s1.IndexOf("/", StringComparison.Ordinal) + 1);

            BrowserInit.Driver.SwitchTo().ParentFrame();
            var cardName = cardType + UiConstantHelper.HypenSymbol + cardDetails.Item6;
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardNameInputTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CardNameInputTxt.SendKeys(cardName);
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.FirstNameTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.FirstNameTxt.SendKeys(cardDetails.Item7);
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.LastNameTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.LastNameTxt.SendKeys(cardDetails.Rest.Item1);
            if (PageInitHelper<UpdateUserInfoPageFactory>.PageInit.UseasDd.GetAttribute(UiConstantHelper.AttributeClass).Contains(UiConstantHelper.Disabled))
                return;
            if (cardUseAsOption.Contains("Default".Trim()))
            {
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.UseasDd.Click();
                var cardUseAsListOfOption = CardUseAsOptionList();
                if (!cardUseAsListOfOption.Contains("Default", StringComparer.OrdinalIgnoreCase))
                    throw new InconclusiveException(AppConfigHelper.UserName + "dont have" + "Default" + "option in Dropdown list");
                if (!cardUseAsListOfOption.Select(useas => Regex.Replace(useas, @"[\s+]", ""))
                    .Any(trimmedTxt => trimmedTxt.ToUpper().Equals("Default".ToUpper()))) return;
                Thread.Sleep(500);
                new SelectElement(PageInitHelper<UpdateUserInfoPageFactory>.PageInit.UseasOptions).SelectByIndex(1);
            }
            else if (cardUseAsOption.Contains("None".Trim()) || cardUseAsOption.Equals(string.Empty))
            {
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.UseasDd.Click();
                var cardUseAsListOfOption = CardUseAsOptionList();
                if (!cardUseAsListOfOption.Contains("None", StringComparer.OrdinalIgnoreCase))
                    throw new InconclusiveException(AppConfigHelper.UserName + "dont have" + "None" + "option in Dropdown list");
                if (!cardUseAsListOfOption.Select(useas => Regex.Replace(useas, @"[\s+]", ""))
                    .Any(trimmedTxt => trimmedTxt.ToUpper().Equals("None".ToUpper()))) return;
                Thread.Sleep(500);
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.NoneCardOption.Click();
            }
            else if (cardUseAsOption.Contains("FirstBackup".Trim()))
            {
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.UseasDd.Click();
                var cardUseAsListOfOption = CardUseAsOptionList();
                if (!cardUseAsListOfOption.Any(x => x.Contains("First Backup")))
                    throw new InconclusiveException(AppConfigHelper.UserName + " dont have" + " First Backup" + " option in Dropdown list");
                new SelectElement(PageInitHelper<UpdateUserInfoPageFactory>.PageInit.UseasOptions).SelectByIndex(2);
            }
            else if (cardUseAsOption.Contains("SecondBackup".Trim()))
            {
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.UseasDd.Click();
                var cardUseAsListOfOption = CardUseAsOptionList();
                if (!cardUseAsListOfOption.Any(x => x.Contains("Second Backup")))
                    throw new InconclusiveException(AppConfigHelper.UserName + " dont have" + " Second Backup" + " option in Dropdown list");
                Thread.Sleep(500);
                new SelectElement(PageInitHelper<UpdateUserInfoPageFactory>.PageInit.UseasOptions).SelectByIndex(3);
            }
            UtilityHelper.CardDetails = new Tuple<string, string, string, string, StringBuilder, string, string, Tuple<string>>(cardDetails.Item1, cardDetails.Item2, cardDetails.Item3, cardDetails.Item4,
                        cardDetails.Item5, cardDetails.Item6.Replace(cardDetails.Item6, cardName), cardDetails.Item7, new Tuple<string>(string.Empty));
        }
        private static List<string> CardUseAsOptionList()
        {
            var useasList = new List<string>();
            var listcount = PageInitHelper<UpdateUserInfoPageFactory>.PageInit.UseasDdl.Count;
            for (var i = 1; i <= listcount; i++)
            {
                var addlist = BrowserInit.Driver.FindElement(By.XPath(".//*[@role='listbox']/li[" + i + "]/div")).Text;
                useasList.Add(addlist);
            }
            useasList = useasList.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            return useasList;
        }
        public override string BasicInformation()
        {
            var userInfoDetails = PageInitHelper<UserPersonalDataGenetrator>.PageInit.UserBasicInfoDetails();
            var pageheading = Regex.Replace(PageInitHelper<UpdateUserInfoPageFactory>.PageInit.SectiontitleLbl.Text, "Address" + "|" + "Card", "").Trim();
            if (pageheading.IndexOf("Primary".Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (
                    PageInitHelper<UpdateUserInfoPageFactory>.PageInit.AddressNameParaTxt.GetAttribute(UiConstantHelper.AttributeClass)
                        .IndexOf("no-input", StringComparison.InvariantCultureIgnoreCase) == -1)
                {
                    PageInitHelper<UpdateUserInfoPageFactory>.PageInit.AddressNameTxt.Clear();
                    PageInitHelper<UpdateUserInfoPageFactory>.PageInit.AddressNameTxt.SendKeys(userInfoDetails.Item5);
                }
            }
            else
            {
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.AddressNameTxt.Clear();
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.AddressNameTxt.SendKeys(userInfoDetails.Item5);
            }
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.FirstNameTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.FirstNameTxt.SendKeys(userInfoDetails.Item1);
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.LastNameTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.LastNameTxt.SendKeys(userInfoDetails.Item2);
            if (
               !PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CompanyNameSection.GetAttribute(
                    UiConstantHelper.AttributeClass).Contains(UiConstantHelper.Expanded) ||
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CompanyNameSection.GetAttribute(
                    UiConstantHelper.AttributeClass).Contains(UiConstantHelper.NgHide))
            {
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CompanyAddressChk.Click();
            }
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CompanyNameTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CompanyNameTxt.SendKeys(userInfoDetails.Item3);
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.JobTitleeTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.JobTitleeTxt.SendKeys(userInfoDetails.Item4);
            return AddressNameText;
        }
        public override void BillingAddressDetails()
        {
            var streetInfo = PageInitHelper<UserPersonalDataGenetrator>.PageInit.StreetInformation();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.Address1Txt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.Address1Txt.SendKeys(streetInfo.Item1.Item1);
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.Address2Txt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.Address2Txt.SendKeys(streetInfo.Item1.Item2);
            if (PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CountryDdtxt.Text.Contains(UiConstantHelper.Canada))
            {
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.StateTxt.Clear();
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.StateTxt.SendKeys(streetInfo.Item1.Item3.Replace(streetInfo.Item1.Item3, UiConstantHelper.Yukon));
            }
            else
            {
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.StateTxt.Clear();
                PageInitHelper<UpdateUserInfoPageFactory>.PageInit.StateTxt.SendKeys(streetInfo.Item1.Item3);
            }
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CityTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CityTxt.SendKeys(streetInfo.Item1.Item4);
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.ZipTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.ZipTxt.SendKeys(streetInfo.Item1.Item5);
        }
        public override void ContactInformation()
        {
            var contactInfo = PageInitHelper<UserPersonalDataGenetrator>.PageInit.ContactInformation();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.PhoneNumberDd.Click();
            var phoneNumberListCount = PageInitHelper<UpdateUserInfoPageFactory>.PageInit.PhoneNumberDdl.Count;
            var number = new Random();
            var randomNumber = number.Next(1, phoneNumberListCount);
            var countryCode = PageInitHelper<UpdateUserInfoPageFactory>.PageInit.PhoneNumberDdl[randomNumber];
            PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(countryCode);
            countryCode.Click();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.PhoneNumberTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.PhoneNumberTxt.SendKeys(contactInfo.Item1.ToString());
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.FaxNumberDd.Click();
            var faxNumberListCount = PageInitHelper<UpdateUserInfoPageFactory>.PageInit.FaxNumberDdl.Count;
            randomNumber = number.Next(1, faxNumberListCount);
            countryCode = PageInitHelper<UpdateUserInfoPageFactory>.PageInit.FaxNumberDdl[randomNumber];
            PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(countryCode);
            countryCode.Click();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.FaxNumberTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.FaxNumberTxt.SendKeys(contactInfo.Item2.ToString());
            if (PageInitHelper<UpdateUserInfoPageFactory>.PageInit.EmailAddressParaTxt.GetAttribute(UiConstantHelper.AttributeClass).Contains("no-input")) return;
            if (!PageInitHelper<UpdateUserInfoPageFactory>.PageInit.EmailAddressTxt.GetAttribute(UiConstantHelper.AttributeValue).Contains(string.Empty)) return;
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.EmailAddressTxt.Clear();
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.EmailAddressTxt.SendKeys(contactInfo.Item3.ToLower());
        }
        public virtual void SaveInfo()
        {
            RepeatFun:
            var pageUrlInSaveinfoPage = BrowserInit.Driver.Url;
            PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(PageInitHelper<UpdateUserInfoPageFactory>.PageInit.SaveBtn);
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.SaveBtn.Click();
            Thread.Sleep(5000);
            PageInitHelper<PageNavigationHelper>.PageInit.WaitForPageLoad();
            if (BrowserInit.Driver.Url.Equals("https://ap.www.namecheap.com/profile/billing/creditcardtopup")) return;
            if (BrowserInit.Driver.Url != pageUrlInSaveinfoPage) return;
            var pageHeading = PageInitHelper<UpdateUserInfoPageFactory>.PageInit.SectiontitleLbl.Text;
            if (!(pageHeading.Contains(UiConstantHelper.AddNewAddress) || BrowserInit.Driver.Url.Contains(UiConstantHelper.EditRegistrantContacts) || pageHeading.Contains(UiConstantHelper.EditAddress) || pageHeading.Contains(UiConstantHelper.EditCard) || pageHeading.Contains(UiConstantHelper.AddNewCard) || pageHeading.Contains(UiConstantHelper.EditPrimaryAddress) || BrowserInit.Driver.Url.Contains(UiConstantHelper.Topup)))
                return;
            errorcheck:
            var errorCount = PageInitHelper<UpdateUserInfoPageFactory>.PageInit.ErrorCount.Count;
            if (errorCount <= 1) goto RepeatFun;
            for (var i = 0; i < errorCount;)
            {
                var errorLabelTxt = BrowserInit.Driver.FindElement(By.XPath("(.//p[contains(concat(' ',normalize-space(@class),' '),'cont-msg error')])[" + UiConstantHelper.OneNumber + "]/../../div[1]/label")).Text;
                if (errorLabelTxt.IndexOf(UiConstantHelper.LastNameTxt, StringComparison.CurrentCultureIgnoreCase) != -1 || errorLabelTxt.IndexOf(UiConstantHelper.AddressName, StringComparison.InvariantCultureIgnoreCase) != -1 || errorLabelTxt.IndexOf("Job Title", StringComparison.InvariantCultureIgnoreCase) != -1 || errorLabelTxt.IndexOf("Company Name", StringComparison.InvariantCultureIgnoreCase) != -1 || errorLabelTxt.IndexOf("First Name", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    UtilityHelper.UserBasicInfoDetails = new Tuple<string, string, string, string, string>(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    UtilityHelper.UserBasicInfoDetails = PageInitHelper<UserPersonalDataGenetrator>.PageInit.UserBasicInfoDetails();
                    BasicInformation();
                    goto errorcheck;
                }
                if (errorLabelTxt.IndexOf(UiConstantHelper.EmailAddress, StringComparison.InvariantCultureIgnoreCase) !=
                       -1)
                {
                    UtilityHelper.ContactInformation = new Tuple<StringBuilder, StringBuilder, string>(
                        new StringBuilder(), new StringBuilder(), string.Empty);
                    UtilityHelper.ContactInformation = PageInitHelper<UserPersonalDataGenetrator>.PageInit.ContactInformation();
                    ContactInformation();
                    goto errorcheck;
                }
                UtilityHelper.StreetInformation = new Tuple<Tuple<string, string, string, string, string, string>>(new Tuple<string, string, string, string, string, string>(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));
                UtilityHelper.StreetInformation = PageInitHelper<UserPersonalDataGenetrator>.PageInit.StreetInformation();
                BillingAddressDetails();
                goto errorcheck;
            }
        }
    }
}