using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.SecurityPageFactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NamecheapUITests.PageObject.ValidationPages;
using NUnit.Framework;
using OpenQA.Selenium;
namespace NamecheapUITests.PageObject.CMSPages.SecurityPage
{
    public class SslCertificatePage
    {
        internal List<SortedDictionary<string, string>> AddingSslProductToCart(string purchasingSslFor)
        {
            var sslInfoList = new List<SortedDictionary<string, string>>();
            string[] splitString = purchasingSslFor.Split(new[] { " " }, StringSplitOptions.None);
            var dicSsl = new SortedDictionary<string, string>();
            var xpath =
                "//*[contains(@class,'ssl-filters')]//*[@class='ssl-shop-filters']/ul/li/a[.='Domains']/../div/ul/li//input[contains(@data-filter,'" +
                splitString[1].Substring(0, 5).ToLower() + "')]";
            PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<SslCertificatePageFactory>.PageInit.DomainsFilter, BrowserInit.Driver.FindElement(By.XPath(xpath)));
            var certificateCountXpath =
                "(//ul[contains(@class,'ssl-list')]/li[contains(@class,'" + Regex.Replace(splitString[1].Substring(0, 5), "Domain", string.Empty).ToLower() +
                "')][contains(@class,'" + splitString[0].ToLower() + "')])";
            Thread.Sleep(3000);
            if (BrowserInit.Driver.FindElement(By.XPath("(//ul[contains(@class,'ssl-list')])")).GetAttribute(UiConstantHelper.AttributeClass).Contains("fail"))
            {
                Assert.IsTrue(BrowserInit.Driver.FindElement(By.XPath(".//*[contains(@class,'no-results')]")).GetAttribute("style").Contains("block;"));
                throw new InconclusiveException("On SSL page currently there is no certificate available in the combination of '" + splitString[0] + "' validation and '" + splitString[1] + "' domains");
            }
            var certificateCount = BrowserInit.Driver.FindElements(By.XPath(certificateCountXpath));
            var randomCer = PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(certificateCount.Count, 0);
            var selectedSsl = certificateCount[randomCer];
            const int multiDomains = 0;
            foreach (var list in selectedSsl.FindElements(By.TagName("li")))
            {
                string listClass = list.GetAttribute(UiConstantHelper.AttributeClass);
                string dicKey;
                string listText;
                if (listClass.Equals("certificate"))
                {
                    dicKey = EnumHelper.Ssl.CertificateName.ToString();
                    listText = list.FindElement(By.TagName("a")).Text.Trim();
                }
                else if (listClass.Equals("validation"))
                {
                    dicKey = EnumHelper.Ssl.ValidationType.ToString();
                    listText = list.FindElement(By.TagName("p")).Text.Trim();
                    if (listText.Contains("\r\n"))
                    {
                        var replaceGreenBar = Regex.Replace(listText, "\r\n", ":").Trim();
                        var splitGreenBar = Regex.Split(replaceGreenBar, ":")[0];
                        listText = splitGreenBar;
                    }
                }
                else if (listClass.Equals("no-domains"))
                {
                    dicKey = EnumHelper.Ssl.DomainCategory.ToString();
                    listText = list.FindElement(By.TagName("p")).Text.Trim();
                }
                else if (listClass.Equals("great-for"))
                {
                    dicKey = "GreatFor";
                    listText = list.FindElement(By.TagName("p")).Text;
                }
                else if (listClass.Equals("fieldset"))
                {
                    dicKey = EnumHelper.Ssl.CertificateDuration.ToString();
                    listText = list.FindElements(By.TagName("span"))[0].Text.Trim();
                }
                else if (listClass.Equals("price-holder"))
                {
                    dicKey = EnumHelper.Ssl.CertificatePrice.ToString();
                    listText =
                        Regex.Replace(list.FindElement(By.ClassName("amount")).Text, @"[^\d..][^\w\s]*", "").Trim();
                }
                else continue;
                dicSsl.Add(dicKey, listText);
            }
            if (splitString[1].ToLower().Contains("multi") && multiDomains.Equals(0))
            {
                selectedSsl.FindElement(By.ClassName("action")).FindElement(By.TagName("button")).Click();
                var addtionalDomain =
                    Regex.Replace(PageInitHelper<SslCertificatePageFactory>.PageInit.AdditionalDomains.Text, "[a-zA-Z]",
                        "").Trim();
                var additionalDomainsCount = Int32.Parse(addtionalDomain);
                PageInitHelper<SslCertificatePageFactory>.PageInit.AdditionalDomainsTxt.Clear();
                var sendAdditionalDomainsCount =
                    PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(additionalDomainsCount);
                PageInitHelper<SslCertificatePageFactory>.PageInit.AdditionalDomainsTxt.SendKeys(
                    sendAdditionalDomainsCount.ToString());
                dicSsl.Add("ChargedAdditionalDomains", sendAdditionalDomainsCount.ToString());
                var aDomainPrice =
                    Regex.Replace(PageInitHelper<SslCertificatePageFactory>.PageInit.AdditionalDomainsPrice.Text,
                        @"[^\d..][^\w\s]*", "");
                var additionalDomainsPrice = aDomainPrice.Equals(string.Empty) ? "0.00" : aDomainPrice;
                dicSsl.Add("ChargedAdditionalDomainsPrice", additionalDomainsPrice.Trim());
                BrowserInit.Driver.FindElement(By.Id("btnViewCart")).Click();
            }
            else
            {
                selectedSsl.FindElement(By.ClassName("action")).FindElement(By.TagName("button")).Click();
            }
            sslInfoList.Add(dicSsl);
            IShoppingCartValidation domainPage = new AddSslShoppingCartItems();
            var listDicFromWidget = domainPage.AddShoppingCartItemsToDic(sslInfoList);
            return listDicFromWidget;
        }
    }
}