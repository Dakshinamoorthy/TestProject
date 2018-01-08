using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.FooterPageFactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.ValidationPages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Assert = MbUnit.Framework.Assert;

namespace NamecheapUITests.PageObject.CMSPages.FooterPage
{
    class FooterPage
    {
        public T ParseEnum<T>(string input)
        {
            return (T)Enum.Parse(typeof(T), input, true);
        }

        internal void ValidateFooter(string process)
        {
            if (!BrowserInit.Driver.Url.Contains(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator()))
                BrowserInit.Driver.Navigate().GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator());
            PageInitHelper<UrlNavigationHelper>.PageInit.ValidateUrl();
            PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(PageInitHelper<FooterPageFactory>.PageInit.Footer);
            if (process.Contains(UiConstantHelper.NewsletterAndSocialNetwork))
                FooterNewsLetterAndSocialNetwork();
            else if (process.Contains(UiConstantHelper.Legal))
                FooterLegal();
            else if (process.Contains(UiConstantHelper.Causes))
                FooterCauses();
            else if (process.Contains(UiConstantHelper.Certifications))
                FooterCertifications();
            else
                FooterPageLinks(process);
        }

        private void FooterNewsLetterAndSocialNetwork()
        {
            //Footer Namecheap Logo
            string altTxt = PageInitHelper<FooterPageFactory>.PageInit.FooterNamecheapLogo.GetAttribute(UiConstantHelper.Alt).Trim();
            Assert.Contains(altTxt, UiConstantHelper.Namecheap.Trim());
            //Footer About Namecheap
            Assert.IsTrue(PageInitHelper<FooterPageFactory>.PageInit.FooterNamecheapAboutText.Displayed);
            PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(PageInitHelper<FooterPageFactory>.PageInit.FooterNamecheapAboutLink);
            PageInitHelper<FooterPageFactory>.PageInit.FooterNamecheapAboutLink.Click();
            PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
            BrowserInit.Driver.Navigate().Back();
            //Footer Namecheap Blog
            PageInitHelper<FooterPageFactory>.PageInit.FooterNamecheapAboutBlogLink.Click();
            PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
            BrowserInit.Driver.Navigate().Back();
            //Footer Newsletter Form
            Assert.Contains(PageInitHelper<FooterPageFactory>.PageInit.FooterNamecheapNewsletter.Text, UiConstantHelper.Newsletter);
            Assert.IsTrue(PageInitHelper<FooterPageFactory>.PageInit.FooterNamecheapNewsletterPeriodic.Displayed);
            foreach (var footerNewsletter in PageInitHelper<FooterPageFactory>.PageInit.FooterNamecheapJoinNewsletter)
            {
                string inputAttribute = footerNewsletter.GetAttribute(UiConstantHelper.Name);
                Assert.IsTrue(inputAttribute.Contains(UiConstantHelper.Email.ToLower()) || inputAttribute.Contains(UiConstantHelper.Action));
            }
            //Footer Social
            string footerSocial = "//*[@id='footer']//*[contains(@class,'social')]/li";
            int footerSocialCount = BrowserInit.Driver.FindElements(By.XPath(footerSocial)).Count;
            for (int social = 1; social <= footerSocialCount; social++)
            {
                BrowserInit.Driver.FindElement(By.XPath(footerSocial + "[" + social + "]")).GetAttribute(UiConstantHelper.AttributeClass);
                BrowserInit.Driver.FindElement(By.XPath(footerSocial + "[" + social + "]/a")).Click();
                PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
                BrowserInit.Driver.Navigate().Back();
            }
        }

        private void FooterLegal()
        {
            //Footer Copyright
            string footerLegalCopyrightTxt = PageInitHelper<FooterPageFactory>.PageInit.FooterLegalCopyright.Text;
            Assert.Contains(footerLegalCopyrightTxt, UiConstantHelper.Copyright);
            Assert.Contains(footerLegalCopyrightTxt, DateTime.Now.Year.ToString());
            //Footer Legal Links
            string footerLegalLinks = ".//*[@id='footer']/*[contains(@class,'legal')]/ul/li";
            int footerLegalLinksCount = BrowserInit.Driver.FindElements(By.XPath(footerLegalLinks)).Count;
            for (int footerLegalLink = 1; footerLegalLink <= footerLegalLinksCount; footerLegalLink++)
            {
                BrowserInit.Driver.FindElement(By.XPath(footerLegalLinks + "[" + footerLegalLink + "]")).Click();
                PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
                BrowserInit.Driver.Navigate().Back();
            }
        }

        private void FooterCauses()
        {
            Assert.IsTrue(PageInitHelper<FooterPageFactory>.PageInit.FooterCausesSupport.Text.IndexOf(UiConstantHelper.WeSupport, StringComparison.InvariantCultureIgnoreCase) != -1);
            //We Support EFF & FFTF
            foreach (var footerOrganization in PageInitHelper<FooterPageFactory>.PageInit.FooterCausesOrganization)
                Assert.IsTrue(footerOrganization.FindElement(By.XPath("/*")).Displayed);
        }

        private void FooterCertifications()
        {
            //ICANN
            Assert.Contains(PageInitHelper<FooterPageFactory>.PageInit.FooterIcannCertification.Text, UiConstantHelper.Icann);
            Assert.Contains(PageInitHelper<FooterPageFactory>.PageInit.FooterIcannCertification.Text, UiConstantHelper.AccreditedRegistrar);
            //Payment Options
            string footerPaymentOptions = ".//*[@id='footer']/*[contains(@class,'certifications')]/*[2]/dd";
            for (int footerPaymentOption = 1; footerPaymentOption <= BrowserInit.Driver.FindElements(By.XPath(footerPaymentOptions)).Count; footerPaymentOption++)
            {
                string paymentOption = BrowserInit.Driver.FindElement(By.XPath(footerPaymentOptions + "[" + footerPaymentOption + "]")).Text;
                Assert.IsTrue(paymentOption.Contains(UiConstantHelper.AmericanExpress) || paymentOption.Contains(UiConstantHelper.Bitcoin) || paymentOption.Contains(UiConstantHelper.Dwolla) || paymentOption.Contains(UiConstantHelper.MasterCard) || paymentOption.Contains(UiConstantHelper.PayPal) || paymentOption.Contains(UiConstantHelper.Visa));
            }
            //Security and Privacy Certifications
            string footerSecurityandPrivacy = ".//*[@id='footer']/*[contains(@class,'certifications')]/*[3]/li";
            for (int footerSecurityandPrivacyLinks = 1; footerSecurityandPrivacyLinks <= BrowserInit.Driver.FindElements(By.XPath(footerSecurityandPrivacy)).Count; footerSecurityandPrivacyLinks++)
            {
                PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(BrowserInit.Driver.FindElement(By.XPath(footerSecurityandPrivacy + "[" + footerSecurityandPrivacyLinks + "]/a")));
                BrowserInit.Driver.FindElement(By.XPath(footerSecurityandPrivacy + "[" + footerSecurityandPrivacyLinks + "]/a")).Click();
                PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
                var popupWindow = BrowserInit.Driver.WindowHandles.Count;
                if (popupWindow > 1)
                {
                    BrowserInit.Driver.SwitchTo().Window(BrowserInit.Driver.WindowHandles[1]).Close();
                    BrowserInit.Driver.SwitchTo().Window(BrowserInit.Driver.WindowHandles.Last());
                }
                else
                    BrowserInit.Driver.Navigate().Back();
            }
        }

        private void FooterPageLinks(string process)
        {
            string navModuleLinksXpath = ".//*[@id='footer']//nav//b[contains(.,'" + process + "')]/a";
            //Header Links
            BrowserInit.Driver.FindElement(By.XPath(navModuleLinksXpath)).Click();
            IWait<IWebDriver> wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(200));
            wait.Until(driver1 => ((IJavaScriptExecutor)BrowserInit.Driver).ExecuteScript("return document.readyState").Equals("complete"));
            PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
            BrowserInit.Driver.Navigate().Back();
            if (process.Contains(EnumHelper.PageLinks.PremiumDNS.ToString()) || process.Contains(EnumHelper.PageLinks.FreeDNS.ToString()) || process.Contains(EnumHelper.PageLinks.WhoisGuard.ToString())) return;
            if (process.Contains(EnumHelper.PageLinks.Affiliates.ToString()) || process.Contains(EnumHelper.PageLinks.Careers.ToString()) || process.Contains(EnumHelper.PageLinks.SendUsFeedback.ToString())) return;
            //Sub links
            int navModuleLinksCount = BrowserInit.Driver.FindElements(By.XPath(navModuleLinksXpath + "/../following-sibling::ul[1]//li")).Count;
            if (navModuleLinksCount.Equals(0)) return;
            string[] pageLinks = new string[navModuleLinksCount];
            switch (ParseEnum<EnumHelper.PageLinks>(Regex.Replace(process, " ", "")))
            {
                case EnumHelper.PageLinks.Domains:
                    pageLinks = UiConstantHelper.DomainNavLinks;
                    break;
                case EnumHelper.PageLinks.Hosting:
                    pageLinks = UiConstantHelper.HostingNavLinks;
                    break;
                case EnumHelper.PageLinks.PremiumDNS:
                    pageLinks = UiConstantHelper.PremiumDNSNavLinks;
                    break;
                case EnumHelper.PageLinks.FreeDNS:
                    pageLinks = UiConstantHelper.FreeDNSNavLinks;
                    break;
                case EnumHelper.PageLinks.WhoisGuard:
                    pageLinks = UiConstantHelper.WhoisGuardNavLinks;
                    break;
                case EnumHelper.PageLinks.SslCertificates:
                    pageLinks = UiConstantHelper.SslCertificatesNavLinks;
                    break;
                case EnumHelper.PageLinks.Resellers:
                    pageLinks = UiConstantHelper.ResellersNavLinks;
                    break;
                case EnumHelper.PageLinks.Affiliates:
                    pageLinks = UiConstantHelper.AffiliatesNavLinks;
                    break;
                case EnumHelper.PageLinks.Support:
                    pageLinks = UiConstantHelper.SupportNavLinks;
                    break;
                case EnumHelper.PageLinks.Careers:
                    pageLinks = UiConstantHelper.CareersNavLinks;
                    break;
                case EnumHelper.PageLinks.SendUsFeedback:
                    pageLinks = UiConstantHelper.SendUsFeedbackNavLinks;
                    break;
            }
            string hasError = string.Empty;
            for (int pageLink = 1; pageLink <= navModuleLinksCount; pageLink++)
            {
                var pageLinksName = BrowserInit.Driver.FindElement(By.XPath(navModuleLinksXpath + "/../following-sibling::ul[1]//li[" + pageLink + "]/a"));
                var linkInterchange = pageLinks[pageLink - 1].Equals(pageLinksName.Text) ? "" : pageLinksName.Text + " link is interchanged, link should be " + pageLinks[pageLink - 1];
                hasError = linkInterchange.Equals(string.Empty) ? "" : hasError.Equals(string.Empty) ? linkInterchange : hasError + "  --&&--  " + linkInterchange;
                pageLinksName.Click();
                PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
                BrowserInit.Driver.Navigate().Back();
            }
            if (!hasError.Equals(string.Empty))
                throw new InconclusiveException(hasError);
        }
    }
}
