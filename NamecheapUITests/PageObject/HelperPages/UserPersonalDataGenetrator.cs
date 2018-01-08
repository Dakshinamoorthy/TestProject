using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NamecheapUITests.PagefactoryObject.HelpersPageFactory;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using OpenQA.Selenium;
using System.Xml.Linq;
using NamecheapUITests.PageObject.HelperPages.WinForm.SecureCard;
using OpenQA.Selenium.Support.UI;
namespace NamecheapUITests.PageObject.HelperPages
{
    public class UserPersonalDataGenetrator : AGeoLocationFinder
    {
        private readonly Random _number = new Random();
        public Tuple<string, string, string, string, StringBuilder, string, string, Tuple<string>> CardDetails()
        {
            var userFirstNameTxt = PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                    EnumHelper.ExcelDataEnum.UserFirstName.ToString());
            var userLastNameTxt = PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                    EnumHelper.ExcelDataEnum.UserLastName.ToString());
            var nameoncard = userFirstNameTxt + " " + userLastNameTxt;
            var creditCardNumber = PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                EnumHelper.ExcelDataEnum.Creditcardnumbers.ToString());
            var now = DateTime.Now;
            var cardValidMonth = now.ToString("MM");
            var addYears = now.AddYears(UiConstantHelper.Ten).ToString("yy");
            var addyearupto = now.AddYears(UiConstantHelper.Twenty).ToString("yy");
            var cardValidYear = _number.Next(Convert.ToInt32(addYears), Convert.ToInt32(addyearupto)).ToString();
            StringBuilder cvcCode = new StringBuilder(3);
            for (var securityCode = UiConstantHelper.Zero; securityCode < UiConstantHelper.Three; securityCode++)
            {
                var securityCodeCount = _number.Next(UiConstantHelper.Zero, UiConstantHelper.Nine);
                cvcCode = cvcCode.Append(securityCodeCount);
            }
            var cardLast4Digits = creditCardNumber.Substring(creditCardNumber.Length - Convert.ToInt32(UiConstantHelper.FourNumber));
            var name4Dig = nameoncard.Substring(UiConstantHelper.Zero, Convert.ToInt32(UiConstantHelper.FourNumber));
            var namelase3Char = nameoncard.Substring(nameoncard.Length - 3);
            var cardName = name4Dig + UiConstantHelper.DashLine + namelase3Char +
                              UiConstantHelper.HypenSymbol + cardLast4Digits;
            var userFirstName = userFirstNameTxt;
            var userLastName = userLastNameTxt;
            CardInfo = Tuple.Create(nameoncard, creditCardNumber, cardValidMonth, cardValidYear, cvcCode, cardName, userFirstName, userLastName);
            UtilityHelper.CardDetails = CardInfo;
            return CardInfo;
        }
        public Tuple<string, string, string, string, string> RealCard()
        {
            var newCard = new LiveCardInputForm();
            newCard.ShowDialog();
            var livecardDetails = UtilityHelper.LivecardDetails;
            LiveCard = Tuple.Create(livecardDetails[EnumHelper.CardDetails.NameonCard], livecardDetails[EnumHelper.CardDetails.CardNumber], livecardDetails[EnumHelper.CardDetails.ExpMonth], livecardDetails[EnumHelper.CardDetails.ExpYear], livecardDetails[EnumHelper.CardDetails.CvvNumber]);
            return LiveCard;
        }
        public Tuple<string, string, string, string, string> UserBasicInfoDetails()
        {
            var userFirstNameTxt = PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                    EnumHelper.ExcelDataEnum.UserFirstName.ToString());
            var userLastNameTxt = PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                    EnumHelper.ExcelDataEnum.UserLastName.ToString());
            const string companyName = "Namecheap Web Services Pvt ltd";
            const string jobTitle = "QA Engineer";
            var addressIndex = _number.Next(UiConstantHelper.AddressName.Length);
            var addressName = UiConstantHelper.AddressName[addressIndex];
            var addressNameText = PageInitHelper<SldGenerator>.PageInit.GetRandomAlphabets(2) + addressName + PageInitHelper<SldGenerator>.PageInit.GetRandomAlphabets(2);
            UserbasicInfo = Tuple.Create(userFirstNameTxt, userLastNameTxt, companyName, jobTitle, addressNameText);
            UtilityHelper.UserBasicInfoDetails = UserbasicInfo;
            return UserbasicInfo;
        }
        public Tuple<Tuple<string, string, string, string, string, string>> StreetInformation()
        {
            PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CountryDd.Click();
            var countryLstCount = PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CountryLst.Count;
            var number = new Random();
            var countryNameListNumber = number.Next(2, countryLstCount);
            var countrynametextitem = BrowserInit.Driver.FindElement(By.XPath(".//*[contains(concat(' ',normalize-space(@role),' '),'listbox')]/li[" + countryNameListNumber + "]/div"));
            PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(countrynametextitem);
            Thread.Sleep(3000);
            countrynametextitem.Click();
            var countryName = PageInitHelper<UpdateUserInfoPageFactory>.PageInit.CountryDdtxt.Text;
            BillingInfo = GenerateStreetInfo(countryName);
            StreetInformations = Tuple.Create(BillingInfo);
            UtilityHelper.StreetInformation = StreetInformations;
            return StreetInformations;
        }
        public override Tuple<string, string, string, string, string, string> GenerateStreetInfo(string countryName)
        {
            Label:
            StreetInfo = new Tuple<string, string, string, string, string, string>(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            if (countryName.Contains(UiConstantHelper.Canada))
            {
                countryName = countryName.Replace(UiConstantHelper.Canada, UiConstantHelper.Whitehorse);
            }
            var locationUrl = "https://maps.google.com/maps/api/geocode/xml?address=" + Uri.EscapeDataString(countryName) + "&sensor=false&key=" + AppConfigHelper.APIKey;
            var document = XDocument.Load(locationUrl);
            var result = document.Root?.Element("result") ?? document.Root?.Element("Result");
            if (result == null)
                return StreetInfo;
            var location = result.Elements(UiConstantHelper.Geometry).Elements(UiConstantHelper.Location).FirstOrDefault();
            var latitude = location == null ? default(double?) : (double?)location.Element(UiConstantHelper.Latitude);
            var longitude = location == null ? default(double?) : (double?)location.Element(UiConstantHelper.Longitude);
            var locationAreaUrl = "https://maps.googleapis.com/maps/api/geocode/xml?latlng=" + latitude + "," + longitude + "&sensor=true&key=" + AppConfigHelper.APIKey;
            document = XDocument.Load(locationAreaUrl);
            result = document.Root?.Element("result") ?? document.Root?.Element("Result");
            if (result == null)
                return StreetInfo;
            var formattedAddress = (string)result.Element(UiConstantHelper.FormattedAddress);
            var cityName = (string)result.Elements(UiConstantHelper.AddressComponent).Where(ac => ac.Elements("type").Any(t => t.Value == UiConstantHelper.Locality)).Elements(UiConstantHelper.LongName).FirstOrDefault();
            if (cityName == null)
            {
                countryName = PageInitHelper<UserPersonalDataGenetrator>.PageInit.AlternativeCountryName();
                goto Label;
            }
            var state = (string)result.Elements(UiConstantHelper.AddressComponent).Where(ac => ac.Elements("type").Any(t => t.Value == UiConstantHelper.StateCode)).Elements(UiConstantHelper.LongName)
                        .FirstOrDefault();
            var stateSort = (string)result.Elements(UiConstantHelper.AddressComponent).Where(ac => ac.Elements("type").Any(t => t.Value == UiConstantHelper.StateCode)).Elements(UiConstantHelper.ShortName)
                        .FirstOrDefault();
            if (state == null)
            {
                countryName = PageInitHelper<UserPersonalDataGenetrator>.PageInit.AlternativeCountryName();
                goto Label;
            }
            var postalCode = (string)result.Elements(UiConstantHelper.AddressComponent).Where(ac => ac.Elements("type").Any(t => t.Value == UiConstantHelper.PostalCode)).Elements(UiConstantHelper.LongName)
                      .FirstOrDefault();
            if (postalCode == null)
            {
                countryName = PageInitHelper<UserPersonalDataGenetrator>.PageInit.AlternativeCountryName();
                goto Label;
            }
            var addressLine1 = Regex.Replace(formattedAddress.Split(cityName.First())[0], "[^0-9A-Za-z, ]", string.Empty);
            if (addressLine1 == string.Empty || addressLine1 == UiConstantHelper.NullValue)
            {
                addressLine1 = Regex.Replace(formattedAddress.Split(state.First())[0], "[^0-9A-Za-z, ]", string.Empty);
            }
            var addressLine2 = "Lovelace Road, Tampa Bay";
            StreetInfo = Tuple.Create(addressLine1, addressLine2, state, cityName, postalCode, stateSort);
            return StreetInfo;
        }
        public string AlternativeCountryName()
        {
            var countryName = PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                    EnumHelper.ExcelDataEnum.CountryNames.ToString());
            Thread.Sleep(2000);
            new SelectElement(BrowserInit.Driver.FindElement(By.XPath("//select[@id='addressCountry'] | //select[@id='billing_country']"))).SelectByText(countryName);
            return countryName;
        }
        public Tuple<StringBuilder, StringBuilder, string> ContactInformation()
        {
            var phoneNumber = new StringBuilder(UiConstantHelper.Twelve);
            for (var startTelcount = UiConstantHelper.Zero; startTelcount < UiConstantHelper.Eleven; startTelcount++)
            {
                var telCount = _number.Next(UiConstantHelper.Zero, UiConstantHelper.Nine);
                phoneNumber = phoneNumber.Append(telCount.ToString());
            }
            var faxNumber = new StringBuilder(UiConstantHelper.Six);
            for (var startfaxcount = UiConstantHelper.Zero; startfaxcount < UiConstantHelper.Six; startfaxcount++)
            {
                var faxCount = _number.Next(UiConstantHelper.Zero, UiConstantHelper.Nine);
                faxNumber = faxNumber.Append(faxCount.ToString());
            }
            var mailDomainName =
                PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                    EnumHelper.ExcelDataEnum.EmailServer.ToString());
            var emailAddress = UtilityHelper.UserBasicInfoDetails.Item1 + UtilityHelper.UserBasicInfoDetails.Item2 + mailDomainName;
            ContactInfo = Tuple.Create(phoneNumber, faxNumber, emailAddress);
            UtilityHelper.ContactInformation = ContactInfo;
            return ContactInfo;
        }
        public Tuple<string, string, string, string, string> UserNewAccount()
        {
            var env = AppConfigHelper.MainUrl.Contains("sandbox") ? "sb" : AppConfigHelper.MainUrl;
            var userName = Regex.Replace(new CultureInfo("en-US", false).TextInfo.ToTitleCase(env + " test user".ToLower()), " ", string.Empty) +
               PageInitHelper<PageValidationHelper>.PageInit.RandomGenratorstringBuilder(1);
            var userFirstNameTxt = new CultureInfo("en-US", false).TextInfo.ToTitleCase(PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                   EnumHelper.ExcelDataEnum.UserFirstName.ToString()).ToLower());
            var userLastNameTxt = new CultureInfo("en-US", false).TextInfo.ToTitleCase(PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                    EnumHelper.ExcelDataEnum.UserLastName.ToString()).ToLower());
            var newPassword = PageInitHelper<DataGenerator>.PageInit.GenerateNewPassword(10);
            var mailDomainName =
                 PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                     EnumHelper.ExcelDataEnum.EmailServer.ToString());
            var emailAddress = userFirstNameTxt.ToLowerInvariant() + userLastNameTxt.ToLowerInvariant() + mailDomainName;
            NewtoNamecheap = Tuple.Create(userName, userFirstNameTxt, userLastNameTxt, newPassword, emailAddress);
            UtilityHelper.NewtoNamecheap = UserbasicInfo;
            return NewtoNamecheap;
        }
        public Tuple<string, string, string, string, Tuple<string, string, string, string, string, string>> CardBillingAddress()
        {
            var userFirstNameTxt = PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                   EnumHelper.ExcelDataEnum.UserFirstName.ToString());
            var userLastNameTxt = PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                    EnumHelper.ExcelDataEnum.UserLastName.ToString());
            var companyName = UiConstantHelper.OrganizationName;
            var jobTitle = UiConstantHelper.JobTitle;
            BrowserInit.Driver.FindElement(By.XPath(".//*/div[contains(@class,'add-new-contact billing-option nc_address')]//label[contains(concat(' ',normalize-space(text()),' '),'Country')]/../div[1]")).Click();
            var countryLstCount = BrowserInit.Driver.FindElements(By.XPath("(.//*/div[contains(@class,'add-new-contact billing-option nc_address')]//li[contains(concat(' ',normalize-space(@class),' '),'ui-select__dropdown') and (not(contains(concat(' ',normalize-space(text()),' '), 'Select Your Country')))])"));
            var countryNameListNumber = PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(countryLstCount.Count, 2);
            var countrynametextitem = countryLstCount[countryNameListNumber];
            PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(countrynametextitem);
            countryLstCount[countryNameListNumber].Click();
            var countryName = BrowserInit.Driver.FindElement(By.XPath("//*[@id='CardAddressTopDiv']/div[7]/div[1]/span[1]")).Text;
            Thread.Sleep(2000);
            var generateStreetInfo = GenerateStreetInfo(countryName);
            BillingAddress = Tuple.Create(userFirstNameTxt, userLastNameTxt, companyName, jobTitle, Tuple.Create(generateStreetInfo.Item1, generateStreetInfo.Item2, generateStreetInfo.Item3, generateStreetInfo.Item4, generateStreetInfo.Item5, generateStreetInfo.Item6));
            UtilityHelper.BillingAddress = BillingAddress;
            return BillingAddress;
        }
        public Tuple<string, string, string, string, StringBuilder, string, string, Tuple<string>> CardInfo { get; set; }
        public Tuple<string, string, string, string, string> UserbasicInfo { get; set; }
        public Tuple<string, string, string, string, string, string> StreetInfo { get; set; }
        public Tuple<string, string, string, string, string, string> BillingInfo { get; set; }
        public Tuple<StringBuilder, StringBuilder, string> ContactInfo { get; set; }
        public Tuple<Tuple<string, string, string, string, string, string>> StreetInformations { get; set; }
        public Tuple<string, string, string, string, string> NewtoNamecheap { get; set; }
        public Tuple<string, string, string, string, Tuple<string, string, string, string, string, string>> BillingAddress { get; set; }
        public Tuple<string, string, string, string, string> LiveCard { get; set; }
    }
}