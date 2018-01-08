using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Gallio.Framework;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NamecheapUITests.PageObject.ValidationPages;
using NPOI.Util.Collections;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
namespace NamecheapUITests.PageObject.HelperPages
{
    public class PurchaseFlow
    {



        public List<SortedDictionary<string, string>> PurchasingNcProducts(List<SortedDictionary<string, string>> mergedScAndCartWidgetList)
        {

            if (PageInitHelper<PurchaseFlow>.PageInit.CheckoutNav.Text.Contains("Setup"))
            {
                PageInitHelper<PurchaseFlow>.PageInit.Accountpage(); 
            }

                if (PageInitHelper<PurchaseFlow>.PageInit.CheckoutNav.Text.Contains("Setup"))
            {
                PageInitHelper<PurchaseFlow>.PageInit.PaymentContinueBtn.Click();
            }
            if (PageInitHelper<PurchaseFlow>.PageInit.CheckoutNav.Text.Contains("Billing"))
            {
                FlagCount = 1;
                PageInitHelper<PurchaseFlow>.PageInit.BillingCheckoutPage();
            }
            if (PageInitHelper<PurchaseFlow>.PageInit.CheckoutNav.Text.Contains("Order"))
            {
                PageInitHelper<PurchaseFlow>.PageInit.OrderReviewPage();
            }
            PageInitHelper<PurchaseFlow>.PageInit.DoPayment();
            if (BrowserInit.Driver.FindElement(By.XPath("//html[contains(@class,'no-js')]")).GetAttribute(UiConstantHelper.AttributeClass).Contains("modal-open"))
            {
                BrowserInit.Driver.SwitchTo().Window(BrowserInit.Driver.WindowHandles.Last());
                BrowserInit.Driver.FindElement(By.CssSelector("#shopper-approved-modal > header > a")).Click();
                BrowserInit.Driver.SwitchTo().Window(BrowserInit.Driver.WindowHandles.FirstOrDefault());
            }
            var purcasedOrderNumber = PageInitHelper<PurchaseFlow>.PageInit.PurchaseSummaryPageValidation();
            AVerifyAndMergeList mergeTWoListOfDic = new VerifyAndMergingTwoList();
            var mergedPurchaseSummaryItemsAndScItemsList = mergeTWoListOfDic.MergingTwoListOfDic(purcasedOrderNumber, mergedScAndCartWidgetList);
            return mergedPurchaseSummaryItemsAndScItemsList;
        }
         void Accountpage()
        {
            PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
            PageInitHelper<PurchaseFlow>.PageInit.AccountInfoOrganizationNametxt.SendKeys(UiConstantHelper.OrganizationName);
            PageInitHelper<PurchaseFlow>.PageInit.AccountInfoJobTitletxt.SendKeys(UiConstantHelper.JobTitle);
            PageInitHelper<PurchaseFlow>.PageInit.AccountInfoAddress1Txt.SendKeys(UiConstantHelper.Address1);
            PageInitHelper<PurchaseFlow>.PageInit.AccountInfoAddress2Txt.SendKeys(UiConstantHelper.Address2);
            PageInitHelper<PurchaseFlow>.PageInit.AccountInfoCityTxt.SendKeys(UiConstantHelper.Cityname);
            PageInitHelper<PurchaseFlow>.PageInit.AccountInfoStateTxt.SendKeys(UiConstantHelper.StateName);
            PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(PageInitHelper<PurchaseFlow>.PageInit.AccountInfoScrollText);
            PageInitHelper<PurchaseFlow>.PageInit.AccountInfoZipCodeTxt.SendKeys(UiConstantHelper.Zipcode);
            var element = PageInitHelper<PurchaseFlow>.PageInit.ContactPageCountLbl;
            var executor = (IJavaScriptExecutor)BrowserInit.Driver;
            executor.ExecuteScript("arguments[0].click();", element);
            PageInitHelper<PurchaseFlow>.PageInit.ContactPageCountIndiaDdl.Click();
            PageInitHelper<PurchaseFlow>.PageInit.AccountInfoPhonetxt.Clear();
            PageInitHelper<PageValidationHelper>.PageInit.RandomGenratorstringBuilder(3);
            PageInitHelper<PurchaseFlow>.PageInit.AccountInfoPhonetxt.SendKeys(PageInitHelper<PageValidationHelper>.PageInit.RandomGenratorstringBuilder(5).ToString());
            PageInitHelper<PurchaseFlow>.PageInit.AccountInfoFaxtxt.SendKeys(PageInitHelper<PageValidationHelper>.PageInit.RandomGenratorstringBuilder(4).ToString());
            PageInitHelper<PurchaseFlow>.PageInit.ContinueBtn.Click();
        }
        private void BillingCheckoutPage()
        {
           

            Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt("Payment Option"),
                "Page should be navigated to billing page is the expected one, but it navigates to " + BrowserInit.Driver.Title);
            if (AppConfigHelper.MainUrl.Equals("live", StringComparison.CurrentCultureIgnoreCase))
                Assert.Inconclusive("For namecheap production site auto test verifies until payment confirmation page until this page all flows works fine, because for production we don't have access for purchasing");
            var preSelectedPayment =
               BrowserInit.Driver.FindElement(
                   By.XPath(".//label[contains(@for,'po-r-')][contains(.,'" + AppConfigHelper.PaymentMethod +
                            "')]//input")).Selected;
            if (preSelectedPayment == false)
                BrowserInit.Driver.FindElement(By.XPath(".//label[contains(@for,'po-r-')][contains(.,'" + AppConfigHelper.PaymentMethod + "')]//input")).Click();
            var paraCount = PageInitHelper<PurchaseFlow>.PageInit.InfoText;
            foreach (var enumerable in paraCount.Select(message => message.Text.Contains("don't have")).Where(contains => contains))
                if (enumerable)
                    PageInitHelper<PurchaseFlow>.PageInit.PaymentChange();
            if (AppConfigHelper.PaymentMethod == "Card")
                PageInitHelper<PurchaseFlow>.PageInit.PaymentChange();
            if (Option < 3)
                PageInitHelper<PurchaseFlow>.PageInit.PaymentContinueBtn.Click();
            if (Option == 3)
                throw new TestFailedException("All PaymentMethods thows an insuffient fund exception");
            /*  if ((BrowserInit.Driver.Url.Contains("sandbox") || BrowserInit.Driver.Url.Contains("bsbx")))
              {
                  var preSelected =
                      BrowserInit.Driver.FindElement(
                          By.XPath(".//label[contains(@for,'po-r-')][contains(.,'" + AppConfigHelper.PaymentMethod +
                                   "')]//input")).Selected;
                  if (preSelected == false)
                  {
                      BrowserInit.Driver.FindElement(
                          By.XPath(".//label[contains(@for,'po-r-')][contains(.,'" + AppConfigHelper.PaymentMethod +
                                   "')]//input")).Click();
                  }
                  //var paratextCount = PageInitHelper<PurchaseFlow>.PageInit.InfoText.Count;
                 /* if (paratextCount == 2)
                  {#1#
                      foreach (
                          var contains in
                              PageInitHelper<PurchaseFlow>.PageInit.InfoText.Select(
                                  message => message.Text.Contains("don't have") || PageInitHelper<PurchaseFlow>.PageInit.CardDetailsDropdownTxt.Text.Equals("Add new card",
                          StringComparison.CurrentCultureIgnoreCase)).Where(contains => contains))
                      if (contains)
                      PageInitHelper<PurchaseFlow>.PageInit.PaymentChange();
                //  }
                  if (Option < 3)
                  {
                      PageInitHelper<PurchaseFlow>.PageInit.PaymentContinueBtn.Click();
                      Thread.Sleep(2000);
                  }
                  else
                  {
                      throw new TestFailedException("All PaymentMethods thows an insuffient fund exception");
                  }
              }*/
        }
        internal void OrderReviewPage()
        {
            if (FlagCount == 0)
            {
                if (AppConfigHelper.MainUrl.Equals("live"))
                {
                    Assert.Pass("For namecheap production site auto test verifies until payment confirmation page until this page all flows works fine, because for production we don't have access for purchasing");
                    return;
                }
                var paymentMethod = PageInitHelper<PurchaseFlow>.PageInit.PaymentMethod.Text.IndexOf(AppConfigHelper.PaymentMethod,
                    StringComparison.OrdinalIgnoreCase) != -1;
                if (paymentMethod == false)
                {
                    PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(ChangeLnk);
                    PageInitHelper<PurchaseFlow>.PageInit.ChangeLnk.Click();
                    PageInitHelper<PurchaseFlow>.PageInit.PaymentChange();
                    if (Option < 3)
                    {
                        PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(PaymentContinueBtn);
                        PageInitHelper<PurchaseFlow>.PageInit.PaymentContinueBtn.Click();
                    }
                    else
                    {
                        throw new TestFailedException("All PaymentMethods thows an insuffient fund exception");
                    }
                }
            }
            var tandAelement = BrowserInit.Driver.PageSource.Contains("your-cart-terms");
            if (!tandAelement) return;
            var tosCheckbox = PageInitHelper<PurchaseFlow>.PageInit.TermsAgreementsChk.Selected;
            if (tosCheckbox == false)
            {
                PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(TermsAgreementsChk);
                PageInitHelper<PurchaseFlow>.PageInit.TermsAgreementsChk.Click();
            }
        }
        internal void PaymentChange()
        {
            Option = 0;
            foreach (var pmethod in PageInitHelper<PurchaseFlow>.PageInit.PaymentOptions)
            {
                pmethod.FindElement(By.TagName("input")).Click();
                if (pmethod.Text.Contains("Card"))
                {
                    if (PageInitHelper<PurchaseFlow>.PageInit.CardDetailsDropdownTxt.Text.Equals("Add new card",
                        StringComparison.CurrentCultureIgnoreCase))
                    {
                        PageInitHelper<PurchaseFlow>.PageInit.AddNewCardToPayment();
                    }
                    if (BrowserInit.Driver.FindElement(By.XPath(".//*[@id='CardBillingAddressWithLabel']/div[1]/span[1]")).Text.Contains("Add new contact"))
                    {
                        var userDetails = PageInitHelper<UserPersonalDataGenetrator>.PageInit.CardBillingAddress();
                        if (!BrowserInit.Driver.FindElement(By.XPath("//*[@id='billing_register_company_checkbox']")).Selected)
                            BrowserInit.Driver.FindElement(By.XPath("//*[@id='billing_register_company_checkbox']")).Click();
                        BrowserInit.Driver.FindElement(By.XPath("//*[@id='billing_first_name']")).SendKeys(userDetails.Item1);
                        BrowserInit.Driver.FindElement(By.XPath("//*[@id='billing_last_name']")).SendKeys(userDetails.Item2);
                        BrowserInit.Driver.FindElement(By.XPath("//*[@id='billing_company_name']")).SendKeys(userDetails.Item3);
                        BrowserInit.Driver.FindElement(By.XPath("//*[@id='billing_job_title']")).SendKeys(userDetails.Item4);
                        BrowserInit.Driver.FindElement(By.XPath("//*[@id='billing_address1']")).SendKeys(userDetails.Item5.Item1);
                        BrowserInit.Driver.FindElement(By.XPath("//*[@id='billing_address2']")).SendKeys(userDetails.Item5.Item2);
                        BrowserInit.Driver.FindElement(By.XPath("//*[@id='billing_city']")).SendKeys(userDetails.Item5.Item4);
                        BrowserInit.Driver.FindElement(By.XPath("//*[@id='billing_state']")).SendKeys(userDetails.Item5.Item3);
                        BrowserInit.Driver.FindElement(By.XPath("//*[@id='billing_zip']")).SendKeys(userDetails.Item5.Item5);
                    }
                    var reCaptcha = BrowserInit.Driver.FindElement(By.XPath("//*[@id='AddOrEditCardDiv']/div[contains(@class,'big-spacing remember-settings')]/div")).GetCssValue("display").Contains("block");
                    if (reCaptcha)
                    {
                        BrowserInit.Driver.SwitchTo()
                        .Frame(BrowserInit.Driver.FindElement(By.XPath("//*[@id='reCaptcha']/div/div/iframe")));
                        var divCount = BrowserInit.Driver.FindElements(By.XPath(".//*/div[contains(@class,'rc-anchor-alert')]/../div"));
                        foreach (
                            var enumerable in
                                divCount.Select(message => message.GetAttribute(UiConstantHelper.AttributeClass).Contains("rc-anchor rc-anchor-normal rc-anchor-light"))
                                    .Where(contains => contains))
                            if (enumerable)
                                BrowserInit.Driver.FindElement(By.XPath("/html/body/div[2]/div[3]/div[1]/div")).Click();
                        BrowserInit.Driver.FindElement(By.XPath("/html/body/div[2]/div[3]/div[1]/div")).Click();
                        MessageBox.Show(@"Recaptcha" + Environment.NewLine + @"Select the captcha image wich mataches: ",
                            @"Important Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        Thread.Sleep(10 * 1000);
                        MessageBox.Show(@"Once Completed Captcha click Ok button To further Automation process",
                            @"Important Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        BrowserInit.Driver.SwitchTo().ParentFrame();
                    }
                }
                var paratextCount = PageInitHelper<PurchaseFlow>.PageInit.InfoText.Count;
                if (paratextCount == 2)
                {
                    Option =
                     PageInitHelper<PurchaseFlow>.PageInit.InfoText.Select(
                            message => message.Text.Contains("don't have"))
                            .Where(contains => contains)
                            .Aggregate(Option, (current, contains) => 1 + current);
                }
                else
                {
                    break;
                }
            }
        }
        internal void DoPayment()
        {
            var paymentMethod = PageInitHelper<PurchaseFlow>.PageInit.PaymentMethod.Text;
            if (paymentMethod.IndexOf("Paypal", StringComparison.OrdinalIgnoreCase) != -1)
            {
                PageInitHelper<PurchaseFlow>.PageInit.PayNowBtn.Click();
                PageInitHelper<APaypalPurchasePage>.PageInit.PaypalPurchase();
            }
            else
            {
                PageInitHelper<PurchaseFlow>.PageInit.PayNowBtn.Click();
                Thread.Sleep(2000);
                if (BrowserInit.Driver.Title.Equals("Place Your Order"))
                {
                    bool errormsg =
                        BrowserInit.Driver.FindElement(By.XPath(".//*[contains(@id,'errorDisplayParentDiv')]"))
                            .Displayed;
                    if (errormsg)
                    {
                        if (BrowserInit.Driver.FindElement(By.XPath(".//*[contains(@id,'errorDisplayParentDiv')]//p")).Text.Contains("Terms and Agreements"))
                        {
                            var tosCheckbox = PageInitHelper<PurchaseFlow>.PageInit.TermsAgreementsChk.Selected;
                            if (tosCheckbox == false)
                                PageInitHelper<PurchaseFlow>.PageInit.TermsAgreementsChk.Click();
                            PageInitHelper<PurchaseFlow>.PageInit.PayNowBtn.Click();
                        }
                    }
                }
            }
            PageInitHelper<PurchaseFlow>.PageInit.OrderProcessing();
        }
        internal void AddNewCardToPayment()
        {
            PageInitHelper<UpdateUserInfo>.PageInit.NewCardDetails(string.Empty);
        }
        internal void OrderProcessing()
        {
            PageInitHelper<PageNavigationHelper>.PageInit.WaitForPageLoad();
            var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(30));
            Func<IWebDriver, bool> searchtestCondition = x => BrowserInit.Driver.Url.IndexOf("confirmation", StringComparison.InvariantCultureIgnoreCase) >= 0;
            wait.Until(searchtestCondition);
        }
        internal List<SortedDictionary<string, string>> PurchaseSummaryPageValidation()
        {
            List<SortedDictionary<string, string>> purchaseOrderNumberList = new List<SortedDictionary<string, string>>();
            SortedDictionary<string, string> purchaseOrderNumberDic = new SortedDictionary<string, string>();
            purchaseOrderNumberDic.Add(EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString(), PageInitHelper<PurchaseFlow>.PageInit.OrderNumber.Text.Trim());
            var alertMsgs = PageInitHelper<PurchaseFlow>.PageInit.PurchaseSummaryAlertMessages;

            foreach (var alertMsg in alertMsgs)
            {
                var alertMsgTxt = alertMsg.Text;
                if (alertMsgTxt.Contains("Some items on the list weren't processed correctly"))
                {
                    Assert.Fail("Purchase Alert message Exception : " + alertMsgTxt);
                    var alertAnchor = alertMsg.FindElements(By.TagName(UiConstantHelper.TagAnchor));
                    foreach(var anchor in alertAnchor)
                        if (anchor.Text.Contains("reprocess them"))
                            anchor.Click();
                }
            }

            
            Assert.IsTrue(
                PageInitHelper<PurchaseFlow>.PageInit.PurchaseSummaryHeader.Text.Contains(
                    UiConstantHelper.PurchaseSummary), "At order summary page page heading should be " + UiConstantHelper.PurchaseSummary + ", but it shown as" + PageInitHelper<PurchaseFlow>.PageInit.PurchaseSummaryHeader.Text);
            var productGroupsXpath = ".//*[contains(@class,'product-group')][not(contains(@class,'subtotal'))]";
            var productGroups = BrowserInit.Driver.FindElements(By.XPath(productGroupsXpath)).Count;
            
            for (int productGroupCount=1; productGroupCount <= productGroups; productGroupCount++)
            {
                var productGroup = BrowserInit.Driver.FindElement(By.XPath(productGroupsXpath + "[" + productGroupCount + "]"));
                PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(productGroup);

                var productDiv = BrowserInit.Driver.FindElements(By.XPath(productGroupsXpath + "[" + productGroupCount + "]/*[contains(@class,'cart-item')]"));
                foreach (var error in productDiv)
                {
                    var harErrorOrNot = error.GetAttribute(UiConstantHelper.AttributeClass);
                    if (harErrorOrNot.Contains("has-error"))
                        throw new TestFailedException("In order summary page product or domain is getting failed");
                }
            }

            for (var productGroup = 1; productGroup <= productGroups; productGroup++)
            {
                var harErrorOrNot = BrowserInit.Driver.FindElements(By.XPath(productGroupsXpath + "[" + productGroup + "]/*[contains(@class,'cart-item')]")).Count;
                for (var harError = 1; harError <= harErrorOrNot; harError++)
                {
                    var hasErrorOrNot =
                        BrowserInit.Driver.FindElement(By.XPath(".//*[contains(@class,'cart-item')][" + harError + "]"))
                            .GetAttribute("class");
                    if (hasErrorOrNot.Contains("has-error"))
                        throw new TestFailedException("In order summary page product or domain is getting failed");
                }
            }
            //purchaseOrderNumberDic.Add(EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString(), PageInitHelper<PurchaseFlow>.PageInit.OrderNumber.Text.Trim());
            var para =
                BrowserInit.Driver.FindElement(
                    By.XPath(".//*[contains(@class,'your-cart summary')]/div[contains(@class,'thank-you')]/p[1]")).Text;
            var dandT = para.Substring(para.Remove(para.LastIndexOf("completed.", StringComparison.Ordinal)).IndexOf("on", StringComparison.Ordinal));
            var convertedDandT = DateTime.Parse(dandT.Remove(dandT.LastIndexOf("is", StringComparison.Ordinal)).Replace("on", string.Empty).Trim()).ToString("MMM d, yyyy,  hh:mm tt");
            purchaseOrderNumberDic.Add(EnumHelper.OrderSummaryKeys.PurchaseOrderdateAndtime.ToString(), convertedDandT);
            purchaseOrderNumberDic.Add(EnumHelper.OrderSummaryKeys.PaymentTransactionId.ToString(), PageInitHelper<PurchaseFlow>.PageInit.ProductTransactionId.Text.Trim());
            var paymentMethod = PageInitHelper<PurchaseFlow>.PageInit.PaymentMethodTxt.Text.Trim();
            if (paymentMethod.Replace("Payment Method", string.Empty).Trim().IndexOf("Funds", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                paymentMethod = "Account Balance".Trim();
            }
            if (paymentMethod.Replace("Payment Method", string.Empty).Trim().IndexOf("Credit Card", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                paymentMethod = "Secure Card Payment".Trim();
                var cardNumber = BrowserInit.Driver.FindElement(By.ClassName("cc-number")).Text.Trim();
                 // var last4Digits = cardNumber.Substring(cardNumber.Length - 4);
                purchaseOrderNumberDic.Add(EnumHelper.OrderSummaryKeys.CardEndNumber.ToString(), cardNumber.Substring(Math.Max(0, cardNumber.Length - 4)));
            }
            if (paymentMethod.Replace("Payment Method", string.Empty).Trim().IndexOf("Paypal", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                var paypalUserName = PageInitHelper<PurchaseFlow>.PageInit.CardNumber.Text.Trim();
                purchaseOrderNumberDic.Add(EnumHelper.OrderSummaryKeys.PayPalUserName.ToString(), paypalUserName);
            }
            purchaseOrderNumberDic.Add(EnumHelper.OrderSummaryKeys.PaymentMethod.ToString(), paymentMethod);
            purchaseOrderNumberList.Add(purchaseOrderNumberDic);
            return purchaseOrderNumberList;
        }
        #region PageFactory
        [FindsBy(How = How.XPath, Using = ".//*[@class='checkout-nav']//li[@class='selected']/a")]
        [CacheLookup]
        internal IWebElement CheckoutNav { get; set; }
        [FindsBy(How = How.XPath, Using = ".//label[contains(@for,'po-r-')]")]
        [CacheLookup]
        internal IList<IWebElement> PaymentOptions { get; set; }
        //Card
        [FindsBy(How = How.XPath, Using = "(.//*/h2)[1]/following::div[1]/span[1]")]
        [CacheLookup]
        internal IWebElement CardDetailsDropdownTxt { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(@class,'content-panel')][contains(@style,'block')]//p")]
        [CacheLookup]
        internal IList<IWebElement> InfoText { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[contains(@class,'cart spacer-bottom side-cart')]/p/a")]
        [CacheLookup]
        internal IWebElement PaymentContinueBtn { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@class='group checkout-terms']/label/span/input")]
        [CacheLookup]
        internal IWebElement TermsAgreementsChk { get; set; }
        [FindsBy(How = How.XPath,
            Using = ".//*[contains(@class,'product-group')]//*[text()[contains(.,'Payment Method')]]/following::div[1]")
        ]
        [CacheLookup]
        internal IWebElement PaymentMethod { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@class='grid-col one-quarter']/div/div/p[1]/input")]
        [CacheLookup]
        internal IWebElement PayNowBtn { get; set; }
        [FindsBy(How = How.XPath,
            Using = ".//*[contains(@class,'your-cart summary')]/div[contains(@class,'thank-you')]/p/strong")]
        [CacheLookup]
        internal IWebElement OrderNumber { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[@class='your-cart summary group']/div[2]/h3")]
        [CacheLookup]
        internal IWebElement PurchaseSummaryHeader { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[contains(@class,'alert')]")]
        [CacheLookup]
        internal IList<IWebElement> PurchaseSummaryAlertMessages { get; set; }

        [FindsBy(How = How.Id, Using = "po-r-2")]
        [CacheLookup]
        internal IWebElement PayPalRdo { get; set; }
        [FindsBy(How = How.LinkText, Using = "CHANGE")]
        [CacheLookup]
        internal IWebElement ChangeLnk { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@class='order-details']//strong[normalize-space()='Transaction']/following-sibling::span")]
        [CacheLookup]
        internal IWebElement ProductTransactionId { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@class='order-detail-item']//strong[normalize-space()='Payment Method']/..")]
        [CacheLookup]
        internal IWebElement PaymentMethodTxt { get; set; }
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
        //*[@id="content"]/div/div[1]/div[2]/ul/li/div[7]/div[2]/div/ul/li[contains(.,'India')]
        [FindsBy(How = How.CssSelector, Using = "#content > div > div.grid-col.three-quarters > div.module.receipt-details-module.nc_address > ul > li > div:nth-child(8) > div.b-core-ui-select__dropdown.show > div > ul > li:nth-child(103)")]
        [CacheLookup]
        internal IWebElement ContactPageCountIndiaDdl { get; set; }
        [FindsBy(How = How.Id, Using = "btn_Submit")]
        [CacheLookup]
        internal IWebElement ContinueBtn { get; set; }
        internal IWebElement CardNumber { get; set; }
        internal int FlagCount { get; set; }
        internal int Option { get; set; }
        #endregion
    }
}