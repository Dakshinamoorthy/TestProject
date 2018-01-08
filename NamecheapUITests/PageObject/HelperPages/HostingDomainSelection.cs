using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Gallio.Framework;
using NamecheapUITests.PagefactoryObject.HelpersPageFactory;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
namespace NamecheapUITests.PageObject.HelperPages
{
    public class PurchaseNewDomain : IDomainSelectOptions
    {
        public List<Tuple<string, string, string, decimal, decimal>> HostingDomainSelection(
            SortedDictionary<string, string> dicWhoisGuardProductDetailsDic)
        {
            PageInitHelper<PurchaseNewDomain>.PageInit.SecurityProductsPurchaseNewDomainBtn.Click();
            DomainName:
            var promoCode = string.Empty;
            var regPrice = 0.00M;
            var newDomainNames =
                PageInitHelper<RandomDomainNameGenerator>.PageInit.DomainName(UiConstantHelper.ReDomains);
            var newdomaintupleList =
                  new List<Tuple<string, string, string, decimal, decimal>>();
            
            foreach (var newdomain in newDomainNames)
            {
                PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.Clear();
                PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.SendKeys(newdomain);
                var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(5));
                Func<IWebDriver, bool> testCondition =
                    x =>
                        !PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainLoadingimg.GetAttribute(
                            UiConstantHelper.AttributeClass).Contains("loading");
                wait.Until(testCondition);
                var domainAvailiblityLbl =
                    PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainAvailabilityLbl.GetAttribute(
                        UiConstantHelper.AttributeClass);
                if (domainAvailiblityLbl.Contains("Not Available") || domainAvailiblityLbl.Contains("correct format") ||
                    domainAvailiblityLbl.Contains("unavailable") || domainAvailiblityLbl.Contains("not available"))
                {
                    newDomainNames.Clear();
                    goto DomainName;
                }
                var dname = PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.GetAttribute("value");
                var duration = BrowserInit.Driver.FindElement(By.XPath(".//*[@id='divRegisterDomainResult']/div[1]")).Text;
                foreach (
                    var para in
                        BrowserInit.Driver.FindElements(By.XPath(".//*[@id='divRegisterDomainResult']/div[3]/p")))
                {
                    var promocodes = para.FindElements(By.TagName("span")).Count > 0;
                    if (promocodes)
                    {
                        foreach (var promocode in para.FindElements(By.TagName("span")).Where(promocode => !(promocode.Text.Trim().Contains("NEW TLD!") || promocode.Text.Trim().Contains("NEW") ||
                                                                                                             promocode.Text.Trim().Equals(string.Empty))))
                        {
                            promoCode = promocode.Text.Trim();
                        }
                    }
                    if (!para.Text.Equals(string.Empty))
                    {
                        regPrice = decimal.Parse(Regex.Replace(para.Text, @"[^\d..][^\w\s]*", "").Trim());
                    }
                }
                var price = decimal.Parse(
                    Regex.Replace(
                        BrowserInit.Driver.FindElement(By.XPath(".//*[@id='divRegisterDomainResult']/div[3]/p[1]"))
                            .Text, @"[^\d..][^\w\s]*", "").Trim());
                newdomaintupleList.Add(Tuple.Create(dname, duration, promoCode, price, regPrice));
            }
            PageInitHelper<DomainSelectionPageFactory>.PageInit.AddDomainToCartBtn.Click();
            return newdomaintupleList;
        }
        #region Pagefactory
        [FindsBy(How = How.XPath, Using = "//a[contains(@href,'PurchaseNewDomain')]")]
        [CacheLookup]
        internal IWebElement SecurityProductsPurchaseNewDomainBtn { get; set; }
        #endregion
    }
    public class DomainInCart : IDomainSelectOptions
    {
        public List<Tuple<string, string, string, decimal, decimal>> HostingDomainSelection(
            SortedDictionary<string, string> dicWhoisGuardProductDetailsDic)
        {
            var domainincarttupleList = new List<Tuple<string, string, string, decimal, decimal>>();

            //var ddXpath = "//div[contains(@class,'domain-name-module')]//div[contains(@class,'buy-a-domain')]/div/div[contains(@class,'coreuiselect')]";
            //var dropdown = BrowserInit.Driver.FindElement(By.XPath(ddXpath));
            var dropdown = PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainsInCartDd;
            ClickDropDown:
            dropdown.Click();

            if (!dropdown.GetAttribute(UiConstantHelper.AttributeClass).Contains(UiConstantHelper.Open))
                goto ClickDropDown;

            //var domainListXpath = ddXpath + "/following-sibling::div[contains(@class,'show')]//li[not(contains(@class,'first'))]";
            var domainListXpath = "//div[contains(@class,'show')]//li[not(contains(@class,'first'))]";
            var domainList = BrowserInit.Driver.FindElements(By.XPath(domainListXpath));

            var dname = domainList.Count.Equals(1)
                ? domainList[0]
                : domainList[PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(domainList.Count, 0)];

            var domainName = dname.Text.Trim();
            dname.Click();
            //var dnAvailabilityXpath = "//div[contains(@class,'domain-name-module')]//div[contains(@class,'buy-a-domain')]/p[contains(@class,'availability')]";
            //var dnAvailability = BrowserInit.Driver.FindElement(By.XPath(dnAvailabilityXpath)).GetAttribute(UiConstantHelper.AttributeClass);
            var dnAvailability = PageInitHelper<DomainSelectionPageFactory>.PageInit.CartDomainAvailability.GetAttribute(UiConstantHelper.AttributeClass);

            if (!dnAvailability.Contains("available"))
                goto ClickDropDown;
            string domainPrice = "0.00";
            string domainRetailPrice = "0.00";
            string promotionCode = "";
            string domainDuration = "";
            foreach (var details in dicWhoisGuardProductDetailsDic)
            {
                if (details.Key.Equals(EnumHelper.DomainKeys.DomainPrice.ToString()))
                {
                    domainPrice = details.Value;
                    continue;
                }
                if (details.Key.Equals(EnumHelper.DomainKeys.DomainRetailPrice.ToString()))
                {
                    domainRetailPrice = details.Value;
                    continue;
                }
                if (details.Key.Equals(EnumHelper.DomainKeys.DomainNamePromotionCode.ToString()))
                {
                    promotionCode = details.Value;
                    continue;
                }
                if (details.Key.Equals(EnumHelper.DomainKeys.DomainDuration.ToString()))
                    domainDuration = details.Value;
            }
            domainincarttupleList.Add(Tuple.Create(domainName, domainDuration, promotionCode, decimal.Parse(domainPrice), decimal.Parse(domainRetailPrice)));
            return domainincarttupleList;
        }
    }

    public class FreeDomain : IDomainSelectOptions
    {
        public List<Tuple<string, string, string, decimal, decimal>> HostingDomainSelection(
            SortedDictionary<string, string> dicWhoisGuardProductDetailsDic)
        {
            var freedomaintupleList = new List<Tuple<string, string, string, decimal, decimal>>();
            var dicHosting = new SortedDictionary<string, string>(dicWhoisGuardProductDetailsDic);

            GenerateFreeDomain:
            var domainName = PageInitHelper<DataHelper>.PageInit.DomainName;
            PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.Clear();
            PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.SendKeys(domainName);

            var Wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(5));
            Func<IWebDriver, bool> testCondition =
                x =>
                    !PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainLoadingimg.GetAttribute(
                        UiConstantHelper.AttributeClass).Contains("loading");
            Wait.Until(testCondition);

            var domainAvailiblityLbl = PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainAvailabilityLbl.GetAttribute(UiConstantHelper.AttributeClass);

            if (domainAvailiblityLbl.Contains("Not Available") || domainAvailiblityLbl.Contains("correct format") || domainAvailiblityLbl.Contains("unavailable"))
                goto GenerateFreeDomain;

            //testCondition = x => PageInitHelper<DomainSelectionPageFactory>.PageInit.AddDomainToCartBtn.Enabled;
            //Wait.Until(testCondition);

            //string dname = domainName + ".website";
            string dname = PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.GetAttribute("value") + PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainLoadingimg.Text;
            dicHosting.Add(EnumHelper.DomainKeys.DomainName.ToString(), dname.ToLower());

            string duration = BrowserInit.Driver.FindElement(By.XPath(".//*[@id='freedomaincontent']/*[@id='divFreeDomainResult']/div[1]")).Text;
            dicHosting.Add(EnumHelper.DomainKeys.DomainDuration.ToString(), duration);

            string promoCode = BrowserInit.Driver.FindElement(By.XPath(".//*[@id='freedomaincontent']/*[@id='divFreeDomainResult']/div[3]/p[1]/span")).Text.Trim();
            dicHosting.Add(EnumHelper.DomainKeys.DomainNamePromotionCode.ToString(), promoCode);

            string cost = BrowserInit.Driver.FindElement(By.XPath(".//*[@id='freedomaincontent']/*[@id='divFreeDomainResult']/div[3]/p[1]")).Text;
            decimal price = decimal.Parse(Regex.Replace(cost, @"[^\d..][^\w\s]*", ""));
            //dicHosting.Add(EnumHelper.DicionaryKeys.DomainPrice.ToString(),Regex.Replace(price, @"[^\d..][^\w\s]*", ""));
            dicHosting.Add(EnumHelper.DomainKeys.DomainPrice.ToString(), price.ToString());

            string regCost = BrowserInit.Driver.FindElement(By.XPath(".//*[@id='freedomaincontent']/*[@id='divFreeDomainResult']/div[3]/p[2]")).Text;
            decimal regPrice = regCost.Equals(string.Empty) ? 0: decimal.Parse(Regex.Replace(regCost, @"[^\d..][^\w\s]*", ""));
            //decimal regPrice = decimal.Parse(Regex.Replace(regCost, @"[^\d..][^\w\s]*", ""));
            dicHosting.Add(EnumHelper.DomainKeys.DomainRetailPrice.ToString(), regPrice.ToString());

            BrowserInit.Driver.FindElement(By.XPath("//*[@class='btn domain-select-new-btn']")).Click();
            testCondition = x => PageInitHelper<DomainSelectionPageFactory>.PageInit.CartContinueWidgetBtn.Enabled;
            Wait.Until(testCondition);

            freedomaintupleList.Add(Tuple.Create(dname, duration, promoCode, price, regPrice));
            return freedomaintupleList;
        }

        /*
        public List<Tuple<SortedDictionary<Enum, string>>> HostingDomainSelection(SortedDictionary<string, string> dicWhoisGuardProductDetailsDic)
        {
            var freedomaintupleList = new List<Tuple<SortedDictionary<Enum, string>>>();

            var dicHosting = new SortedDictionary<string, string>(dicWhoisGuardProductDetailsDic);
            GenerateFreeDomain:
            var domainName = PageInitHelper<DataHelper>.PageInit.DomainName;
            PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.Clear();
            PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.SendKeys(domainName);
            var Wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(5));
            Func<IWebDriver, bool> testCondition = x => !PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainLoadingimg.GetAttribute(UiConstantHelper.AttributeClass).Contains("loading");
            Wait.Until(testCondition);
            var domainAvailiblityLbl = PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainAvailabilityLbl.GetAttribute(UiConstantHelper.AttributeClass);
            if (domainAvailiblityLbl.Contains("Not Available") || domainAvailiblityLbl.Contains("correct format"))
            {
                goto GenerateFreeDomain;
                //PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.Clear();
                //PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.SendKeys(PageInitHelper<CommonUtlitsWapper>.PageInit.DomainName);
            }
            //testCondition = x => PageInitHelper<DomainSelectionPageFactory>.PageInit.AddDomainToCartBtn.Enabled;
            //Wait.Until(testCondition);

            //string dname = domainName + ".website";
            string dname = PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.GetAttribute("value") + PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainLoadingimg.Text;
            dicHosting.Add(EnumHelper.DicionaryKeys.DomainName.ToString(), dname.ToLower());
            string duration = BrowserInit.Driver.FindElement(By.XPath(".//*[@id='freedomaincontent']/*[@id='divFreeDomainResult']/div[1]")).Text;
            dicHosting.Add(EnumHelper.DicionaryKeys.DomainDuration.ToString(), duration);
            string promoCode = BrowserInit.Driver.FindElement(By.XPath(".//*[@id='freedomaincontent']/*[@id='divFreeDomainResult']/div[3]/p[1]/span")).Text.Trim();
            dicHosting.Add(EnumHelper.DicionaryKeys.PromotionCode.ToString(), promoCode);
            string price = BrowserInit.Driver.FindElement(By.XPath(".//*[@id='freedomaincontent']/*[@id='divFreeDomainResult']/div[3]/p[1]")).Text;
            dicHosting.Add(EnumHelper.DicionaryKeys.DomainPrice.ToString(), Regex.Replace(price, @"[^\d..][^\w\s]*", ""));
            string regPrice = BrowserInit.Driver.FindElement(By.XPath(".//*[@id='freedomaincontent']/*[@id='divFreeDomainResult']/div[3]/p[2]")).Text;
            dicHosting.Add(EnumHelper.DicionaryKeys.DomainRetailPrice.ToString(), Regex.Replace(regPrice, @"[^\d..][^\w\s]*", ""));

            BrowserInit.Driver.FindElement(By.XPath("//*[@class='btn domain-select-new-btn']")).Click();
            testCondition = x => PageInitHelper<DomainSelectionPageFactory>.PageInit.CartContinueWidgetBtn.Enabled;
            Wait.Until(testCondition);


            //freedomaintupleList.Add(Tuple.Create(dname,duration,promoCode,price,regPrice));

            return freedomaintupleList;
        }
    }
    */
    }

    public class UseDomainWithNc : IDomainSelectOptions
    {
        public List<Tuple<string, string, string, decimal, decimal>> HostingDomainSelection(
            SortedDictionary<string, string> dicWhoisGuardProductDetailsDic)
        {
            var dname = string.Empty;
            PageInitHelper<UseDomainWithNc>.PageInit.SecurityProductsUseOwnDomainBtn.Click();
            var newdomaintupleList =
                new List<Tuple<string, string, string, decimal, decimal>>();
            GetTLD:
            var tld = "." + PageInitHelper<DataHelper>.PageInit.Tlds;
            var searchInputXpath = "//*[contains(@class,'search-input')]/input";
            var searchbox = BrowserInit.Driver.FindElements(By.XPath(searchInputXpath)).Count > 0;
            if (searchbox)
            {
                var searchInput = BrowserInit.Driver.FindElement(By.XPath(searchInputXpath));
                searchInput.Clear();
                searchInput.SendKeys(tld);
                searchInput.SendKeys(Keys.Enter);
                var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(5));
                Func<IWebDriver, bool> testCondition =
                    x =>
                        !BrowserInit.Driver.FindElement(By.XPath("//*[contains(@class,'search-input')]")).GetAttribute(
                            UiConstantHelper.AttributeClass).Contains("loading");
                wait.Until(testCondition);
                if (BrowserInit.Driver.FindElement(By.XPath("//*[contains(@class,'search-count')]/span[1]")).Text.Contains("NO"))
                    goto GetTLD;
            }
            else
            {
                if (
                    BrowserInit.Driver.FindElement(By.XPath("//*[contains(@class,'search-count')]/span[1]"))
                        .Text.Contains("NO"))
                    throw new TestFailedException("For product '" + BrowserInit.Driver.FindElement(By.XPath("//*[contains(@class,'side-cart')]//strong")).Text + "', an existing domain name search page there no domain name");
            }
            var existingDomainNameshownLstXpath = "//li[contains(@class,'result')]";
            IList<IWebElement> resultcount = BrowserInit.Driver.FindElements(By.XPath(existingDomainNameshownLstXpath));
            var option = 0;
            var domainname = string.Empty;
            foreach (var domainNamesShownLst in resultcount)
            {
                foreach (var singleDomainName in domainNamesShownLst.FindElements(By.TagName("div")))
                {
                    var divClassname = singleDomainName.GetAttribute(UiConstantHelper.AttributeClass);
                    if (divClassname.Equals("name"))
                    {
                        domainname = singleDomainName.Text;
                    }
                    if (divClassname.Equals("info"))
                    {
                        if (domainNamesShownLst.FindElement(By.TagName("a")).Text.Contains("Select"))
                            domainNamesShownLst.FindElement(By.TagName("a")).Click();
                        divClassname = singleDomainName.GetAttribute(UiConstantHelper.AttributeClass);
                        if (divClassname.Equals("info"))
                        {
                            newdomaintupleList.Add(Tuple.Create(domainname, string.Empty, string.Empty, 0.00M, 0.00M));
                            return newdomaintupleList;
                        }
                    }
                    if (divClassname.Equals("error-info"))
                        option = option + 1;
                }
                if (option == resultcount.Count)
                    goto GetTLD;
            }
            newdomaintupleList.Add(Tuple.Create(dname, string.Empty, string.Empty, 0.00M, 0.00M));
            return newdomaintupleList;
        }
        #region Pagefactory
        [FindsBy(How = How.CssSelector, Using = ".alert")]
        [CacheLookup]
        internal IWebElement AlertMessage { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[contains(@class,'domain-select-search-input')]//input")]
        [CacheLookup]
        internal IWebElement NcDomainNameTxt { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[contains(@href,'DomainIOwn')]")]
        [CacheLookup]
        internal IWebElement SecurityProductsUseOwnDomainBtn { get; set; }
        [FindsBy(How = How.XPath, Using = ".//cart-component//*[contains(@class,'cart-widget')]/p/button")]
        [FindsBy(How = How.XPath, Using = "//a[@class='btn btn-block']")]
        [CacheLookup]
        internal IWebElement ViewCartButton { get; set; }
        #endregion
    }
    public class TransferDomainFromanotherRegistrar : IDomainSelectOptions
    {
        public List<Tuple<string, string, string, decimal, decimal>> HostingDomainSelection(
            SortedDictionary<string, string> dicWhoisGuardProductDetailsDic = null)
        {
            var promoCode = string.Empty;
            var regPrice = 0.00M;
            PageInitHelper<TransferDomainFromanotherRegistrar>.PageInit.SecurityProductsTransferMyExistingDomainBtn.Click();
            var newdomaintupleList =
                new List<Tuple<string, string, string, decimal, decimal>>();
            DomainName:
            var newDomainNames =
                PageInitHelper<RandomDomainNameGenerator>.PageInit.DomainName(UiConstantHelper.SingleDomainTransfer);
            foreach (var newdomain in newDomainNames)
            {
                PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.Clear();
                PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.SendKeys(newdomain);
                var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(5));
                Func<IWebDriver, bool> testCondition =
                    x =>
                        !PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainLoadingimg.GetAttribute(
                            UiConstantHelper.AttributeClass).Contains("loading");
                wait.Until(testCondition);
                var domainAvailiblityLbl =
                    PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainAvailabilityLbl.GetAttribute(
                        UiConstantHelper.AttributeClass);
                if (domainAvailiblityLbl.Contains("Not Available") || domainAvailiblityLbl.Contains("correct format") ||
                    domainAvailiblityLbl.Contains("unavailable"))
                {
                    newDomainNames.Clear();
                    goto DomainName;
                }
                var dname = PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.GetAttribute("value");
                var duration = BrowserInit.Driver.FindElement(By.XPath(".//*[@id='divRegisterDomainResult']/div[1]")).Text;
                var price = decimal.Parse(
                    Regex.Replace(
                        BrowserInit.Driver.FindElement(By.XPath(".//*[@id='divRegisterDomainResult']/div[3]/p[1]"))
                            .Text, @"[^\d..][^\w\s]*", "").Trim());
                newdomaintupleList.Add(Tuple.Create(dname, duration, promoCode, price, regPrice));
                PageInitHelper<DomainSelectionPageFactory>.PageInit.AddDomainToCartBtn.Click();
            }
            return newdomaintupleList;
        }
        #region Pagefactory
        [FindsBy(How = How.XPath, Using = "//a[contains(@href,'TransferDomainName')]")]
        [CacheLookup]
        internal IWebElement SecurityProductsTransferMyExistingDomainBtn { get; set; }
        #endregion
    }
    public class UseDomainFromOtherRegister : IDomainSelectOptions
    {
        public List<Tuple<string, string, string, decimal, decimal>> HostingDomainSelection(SortedDictionary<string, string> dicWhoisGuardProductDetailsDic)
        {
            PageInitHelper<UseDomainFromOtherRegister>.PageInit.SecurityProductsAnotherRegistrarDomainBtn.Click();
            var duration = string.Empty;
            var promoCode = string.Empty;
            var price = 0.00M;
            var regPrice = 0.00M;
            var domainselectionClassName =
                PageInitHelper<UseDomainFromOtherRegister>.PageInit.DomainSelectionDiv.GetAttribute(
                    UiConstantHelper.AttributeClass);
            List<Tuple<string, string, string, decimal, decimal>> tupleListAnotherRegistar =
                 new List<Tuple<string, string, string, decimal, decimal>>();
            if (domainselectionClassName.Contains("pemiumDns"))
            {
                var dname = UsePremiumDnswithanydomain();
                tupleListAnotherRegistar.Add(Tuple.Create(dname, duration, promoCode, price, regPrice));
                return tupleListAnotherRegistar;
            }
            else
            {
                DomainName:
                var newDomainNames = PageInitHelper<RandomDomainNameGenerator>.PageInit.DomainName(UiConstantHelper.SingleDomainTransfer);

                foreach (var newdomain in newDomainNames)
                {
                    PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.Clear();
                    PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.SendKeys(newdomain);
                    var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(5));
                    Func<IWebDriver, bool> testCondition =
                        x =>
                            !PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainLoadingimg.GetAttribute(
                                UiConstantHelper.AttributeClass).Contains("loading");
                    wait.Until(testCondition);
                    var domainAvailiblityLbl = PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainAvailabilityLbl.GetAttribute(UiConstantHelper.AttributeClass);
                    if (domainAvailiblityLbl.Contains("Not Available") || domainAvailiblityLbl.Contains("correct format") ||
                        domainAvailiblityLbl.Contains("unavailable"))
                        goto DomainName;
                    tupleListAnotherRegistar.Add(Tuple.Create(newdomain, "", "", 0.0m, 0.0m));
                    break;
                }
                PageInitHelper<DomainSelectionPageFactory>.PageInit.AddDomainToCartBtn.Click();
            }
            return tupleListAnotherRegistar;
        }
        internal string UsePremiumDnswithanydomain()
        {
            if (
                BrowserInit.Driver.FindElement(By.XPath("//*[contains(@class,'search-count')]"))
                    .Text.Equals("NO DOMAIN NAMES SHOWN"))
                throw new TestFailedException("For product '" +
                                              BrowserInit.Driver.FindElement(
                                                  By.XPath("//*[contains(@class,'side-cart')]//strong")).Text +
                                              "', there are no existing domains");
            string externalDomainNameshownLstXpath = "//*/li[contains(@class,'result')]";
            IList<IWebElement> resultcount = BrowserInit.Driver.FindElements(By.XPath(externalDomainNameshownLstXpath));
            int option = 0;
            var domainname = string.Empty;
            GetTLD:
            foreach (var domainNamesShownLst in resultcount)
            {
                foreach (var singleDomainName in domainNamesShownLst.FindElements(By.TagName("div")))
                {
                    var divClassname = singleDomainName.GetAttribute(UiConstantHelper.AttributeClass);
                    if (divClassname.Equals("name"))
                    {
                        domainname = singleDomainName.Text;
                    }
                    if (divClassname.Equals("info"))
                    {
                        if (domainNamesShownLst.FindElement(By.TagName("a")).Text.Contains("Select"))
                            domainNamesShownLst.FindElement(By.TagName("a")).Click();
                        divClassname = singleDomainName.GetAttribute(UiConstantHelper.AttributeClass);
                        if (divClassname.Equals("info"))
                        {
                            return domainname;
                        }
                    }
                    if (divClassname.Equals("error-info"))
                        option = option + 1;
                }
                if (option == resultcount.Count)
                    goto GetTLD;
            }
            return domainname;
        }
        #region Pagefactory
        [FindsBy(How = How.XPath, Using = "//a[contains(@href,'ExternalDomain')]")]
        [CacheLookup]
        internal IWebElement SecurityProductsAnotherRegistrarDomainBtn { get; set; }
        [FindsBy(How = How.XPath, Using = "//*[@class='module domain-name-module']/ul/li/div")]
        [CacheLookup]
        internal IWebElement DomainSelectionDiv { get; set; }
        #endregion
    }

    public class DomainFromanotherRegistrar : IDomainSelectOptions
    {
        public List<Tuple<string, string, string, decimal, decimal>> HostingDomainSelection(
            SortedDictionary<string, string> dicWhoisGuardProductDetailsDic)
        {
            List<Tuple<string, string, string, decimal, decimal>> anotherregistrardomaintupleList =
                new List<Tuple<string, string, string, decimal, decimal>>();

            DomainName:
            var newDomainNames = PageInitHelper<RandomDomainNameGenerator>.PageInit.DomainName(UiConstantHelper.SingleDomainTransfer);

            foreach (var newdomain in newDomainNames)
            {
                PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.Clear();
                PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainNameTxt.SendKeys(newdomain);
                var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(5));
                Func<IWebDriver, bool> testCondition =
                    x =>
                        !PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainLoadingimg.GetAttribute(
                            UiConstantHelper.AttributeClass).Contains("loading");
                wait.Until(testCondition);
                var domainAvailiblityLbl = PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainAvailabilityLbl.GetAttribute(UiConstantHelper.AttributeClass);
                if (domainAvailiblityLbl.Contains("Not Available") || domainAvailiblityLbl.Contains("correct format") ||
                    domainAvailiblityLbl.Contains("unavailable"))
                    goto DomainName;
                anotherregistrardomaintupleList.Add(Tuple.Create(newdomain, "", "", 0.0m, 0.0m));
                break;
            }
            PageInitHelper<DomainSelectionPageFactory>.PageInit.AddDomainToCartBtn.Click();

            return anotherregistrardomaintupleList;
        }
    }
}