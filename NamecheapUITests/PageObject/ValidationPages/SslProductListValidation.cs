using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PageObject.ValidationPages
{
    public class SslProductListValidation : IDomainListValidation
    {
        public void DomainListValidation(List<SortedDictionary<string, string>> mergedScAndCartWidgetListWithOrderNum, string viewTab)
        {
            BrowserInit.Driver.Navigate().GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "ProductList/SslCertificates"));
            Thread.Sleep(2000);
            foreach (var dic in mergedScAndCartWidgetListWithOrderNum)
            {
                var certificateName = dic[EnumHelper.Ssl.CertificateName.ToString()];
                Thread.Sleep(1000);
                PageInitHelper<SslProductListValidation>.PageInit.SearchBox.Clear();
                PageInitHelper<SslProductListValidation>.PageInit.SearchBox.SendKeys(certificateName);
                PageInitHelper<SslProductListValidation>.PageInit.SearchBox.SendKeys(Keys.Enter);
                Thread.Sleep(5000);
                for (var i = 1; i <= BrowserInit.Driver.FindElements(By.XPath("((.//td/p[contains(@class,'text ssl-logo')]| //span[@class='highlighted'])[normalize-space()='" + certificateName + "'])")).Count; i++)
                {
                    BrowserInit.Driver.FindElement(By.XPath("((.//td/p[contains(@class,'text ssl-logo')]| //span[@class='highlighted'])[normalize-space()='" + certificateName + "'])[" + i +
                                "]/../..//button[contains(@class,'dropdown-toggle')]")).Click();
                    Thread.Sleep(700);
                    BrowserInit.Driver.FindElement(By.XPath("((.//td/p[contains(@class,'text ssl-logo')]| //span[@class='highlighted'])[normalize-space()='" + certificateName + "'])[" + i +
                               "]/../..//ul/li[contains(normalize-space(.),'View SSL details')]/a")).Click();
                    Thread.Sleep(700);
                    if (
                        !PageInitHelper<SslProductListValidation>.PageInit.OrderId.Text.Trim().Equals(dic[EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString()])) continue;
                    var certificateNameIndetailpage = Regex.Replace(Regex.Replace(
                       PageInitHelper<SslProductListValidation>.PageInit.CertificateName.Text.Replace("Certificate Details:", string.Empty).Substring(0, BrowserInit.Driver.FindElement(By.XPath(".//h1[@class='section-title']")).Text.Replace("Certificate Details:", string.Empty).LastIndexOf("ALERT", StringComparison.Ordinal)), "ALERT", string.Empty), "ComodoSSL", string.Empty).Trim();
                    var certificateStatusIndetailpage =
                       PageInitHelper<SslProductListValidation>.PageInit.CertificateStatus.Text.Trim();
                    var certificateValidityIndetailpage =
                       PageInitHelper<SslProductListValidation>.PageInit.CertificateValidity.Text.Trim();
                    var certificateValidationLevelIndetailpage =
                        PageInitHelper<SslProductListValidation>.PageInit.CertificateValadationLevel.Text.Trim();
                    var certificateId = PageInitHelper<SslProductListValidation>.PageInit.CertificateId.Text.Trim();
                    var certificateBadgeStatusIndetailpage =
                        PageInitHelper<SslProductListValidation>.PageInit.CertificateBadgeStatus.Text.Trim();
                    Assert.IsTrue(dic[EnumHelper.Ssl.CertificateName.ToString()].Equals(certificateNameIndetailpage), "In Product list detail page ssl certificate name is mismatching expected certificate name should be " + dic[EnumHelper.Ssl.CertificateName.ToString()] + ", but actual certificate id shown in product detail page as " + certificateName);
                    Assert.IsTrue("Alert".Equals(certificateStatusIndetailpage, StringComparison.OrdinalIgnoreCase) || "Active".Equals(certificateStatusIndetailpage, StringComparison.OrdinalIgnoreCase), "In Product list detail page for ssl certificate name " + dic[EnumHelper.Ssl.CertificateName.ToString()] + " product current status should be Alert, but status shown in product detail page as " + certificateStatusIndetailpage);
                    Assert.AreEqual(dic[EnumHelper.Ssl.CertificateDuration.ToString()].ToLowerInvariant(), certificateValidityIndetailpage.Trim().ToLowerInvariant(), "In Product list detail page for ssl certificate name " + dic[EnumHelper.Ssl.CertificateName.ToString()] + " 'product validity' is mismatching expected validity should be " + dic[EnumHelper.Ssl.CertificateDuration.ToString()] + ", but actual validity shown in product detail page as " + certificateValidityIndetailpage);
                    StringAssert.Contains(dic[EnumHelper.Ssl.ValidationType.ToString()], Regex.Replace(certificateValidationLevelIndetailpage, "Validation ", "").Trim(), "In Product list detail page for ssl certificate name " + dic[EnumHelper.Ssl.CertificateName.ToString()] + " 'validation level' is mismatching expected validation level should be " + dic[EnumHelper.Ssl.ValidationType.ToString()] + ", but actual validity shown in product detail page as " + certificateBadgeStatusIndetailpage);
                    Assert.AreEqual("NEW", certificateBadgeStatusIndetailpage, "In Product list detail page for ssl certificate name " + dic[EnumHelper.Ssl.CertificateName.ToString()] + " 'certificate versions grid status'should be New, but actual status shown for certificate id" + certificateId + " is " + certificateValidationLevelIndetailpage);
                    break;
                }
                if (PageInitHelper<SslProductListValidation>.PageInit.OrderId.Text.Trim().Equals(dic[EnumHelper.OrderSummaryKeys.PurchaseOrderNumber.ToString()]))
                {
                    BrowserInit.Driver.Navigate().Back();
                    Thread.Sleep(3000);
                }
                PageInitHelper<SslProductListValidation>.PageInit.SearchBox.Clear();
                PageInitHelper<SslProductListValidation>.PageInit.SearchBox.SendKeys(certificateName);
                PageInitHelper<SslProductListValidation>.PageInit.SearchBox.SendKeys(Keys.Enter);
                Thread.Sleep(2000);
                Assert.AreEqual(BrowserInit.Driver.FindElement(By.XPath(".//td/p[contains(@class,'text ssl-logo')]")).Text.Trim(), dic[EnumHelper.Ssl.CertificateName.ToString()], "In Product list landing page ssl certificate name is mismatching expected certificate name should be " + dic[EnumHelper.Ssl.CertificateName.ToString()] + ", but actual certificate id shown in product list landing page as " + BrowserInit.Driver.FindElement(By.XPath(".//td/p[contains(@class,'text ssl-logo')]")).Text.Trim());
                var productduration =
                    dic[EnumHelper.Ssl.CertificateDuration.ToString()].Substring(0,
                        dic[EnumHelper.Ssl.CertificateDuration.ToString()].LastIndexOf("ea",
                            StringComparison.Ordinal)).ToLowerInvariant() + "r";
                Assert.AreEqual(PageInitHelper<SslProductListValidation>.PageInit.ProductDuration.Text.Trim().ToLowerInvariant(), productduration, "In Product list landing page for ssl certificate name " + dic[EnumHelper.Ssl.CertificateName.ToString()] + " 'product duration' is mismatching expected validity should be " + productduration + ", but actual validity shown in product list landing page as " + BrowserInit.Driver.FindElement(By.XPath(".//td[3]/ng-pluralize")).Text.Trim().ToLowerInvariant());
            }
        }
        #region SslProductListValidation
        [FindsBy(How = How.XPath, Using = ".//*[contains(@ng-model,'certificatesListCtrl')]|.//*[@name='search']")]
        [CacheLookup]
        internal IWebElement SearchBox { get; set; }
        [FindsBy(How = How.XPath, Using = ".//h1[@class='section-title']")]
        [CacheLookup]
        internal IWebElement CertificateName { get; set; }
        [FindsBy(How = How.XPath, Using = ".//h1[@class='section-title']/span")]
        [CacheLookup]
        internal IWebElement CertificateStatus { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='maincontent']/div/div[6]/div[1]/div[2]/p")]
        [CacheLookup]
        internal IWebElement CertificateValidity { get; set; }
        [FindsBy(How = How.XPath, Using = ".//*[@id='maincontent']/div/div[6]/div[2]/div[2]/p")]
        [CacheLookup]
        internal IWebElement CertificateValadationLevel { get; set; }
        [FindsBy(How = How.XPath, Using = "//tbody/tr/td[1]/strong")]
        [CacheLookup]
        internal IWebElement CertificateId { get; set; }
        [FindsBy(How = How.XPath, Using = "//tbody/tr/td[2]/p")]
        [CacheLookup]
        internal IWebElement CertificateBadgeStatus { get; set; }
        [FindsBy(How = How.XPath, Using = ".//label[contains(.,'Order ID')]/following::p[1]")]
        [CacheLookup]
        internal IWebElement OrderId { get; set; }
        [FindsBy(How = How.XPath, Using = ".//td[3]/ng-pluralize")]
        [CacheLookup]
        internal IWebElement ProductDuration { get; set; }
        #endregion
    }
}