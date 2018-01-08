using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NamecheapUITests.PagefactoryObject.CMSPageFactory.FooterPageFactory
{
   public class FooterPageFactory
    {
        [FindsBy(How = How.Id, Using = "footer")]
        [CacheLookup]
        internal IWebElement Footer { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']//nav")]
        [CacheLookup]
        internal IList<IWebElement> FooterNavLinkTags { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']//nav//a")]
        [CacheLookup]
        internal IList<IWebElement> FooterNavLinks { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']//*[contains(@class,'logo')]/a/img")]
        [CacheLookup]
        internal IWebElement FooterNamecheapLogo { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']//*[contains(@class,'about')]/p[1]")]
        [CacheLookup]
        internal IWebElement FooterNamecheapAboutText { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']//*[contains(@class,'about')]/p[2]/a")]
        [CacheLookup]
        internal IWebElement FooterNamecheapAboutLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']//*[contains(@class,'about')]/p[3]/a")]
        [CacheLookup]
        internal IWebElement FooterNamecheapAboutBlogLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']//*[contains(@class,'newsletter')]/b")] //*[@id='footer']//*[contains(@class,'newsletter')]/h2
        [CacheLookup]
        internal IWebElement FooterNamecheapNewsletter { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']//*[contains(@class,'newsletter')]/p")]
        [CacheLookup]
        internal IWebElement FooterNamecheapNewsletterPeriodic { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']//*[contains(@class,'newsletter')]//input")]
        [CacheLookup]
        internal IList<IWebElement> FooterNamecheapJoinNewsletter { get; set; }

        /*[FindsBy(How = How.XPath, Using = "//*[@id='footer']//*[contains(@class,'social')]/li")]
        internal IList<IWebElement> FooterSocial { get; set; }*/

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']/*[contains(@class,'legal')]/*[contains(@class,'copyright')]")]
        [CacheLookup]
        internal IWebElement FooterLegalCopyright { get; set; }

        /*[FindsBy(How = How.XPath, Using = ".//*[@id='footer']/*[contains(@class,'legal')]/ul/li")]
        internal IList<IWebElement> FooterLegalLinks { get; set; }*/

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']/*[contains(@class,'causes')]/div/b")] //*[@id='footer']/*[contains(@class,'causes')]/div/h2
        [CacheLookup]
        internal IWebElement FooterCausesSupport { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']/*[contains(@class,'causes')]/div/div")]
        [CacheLookup]
        internal IList<IWebElement> FooterCausesOrganization { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']/*[contains(@class,'certifications')]/*[contains(@class,'icann')]")]
        [CacheLookup]
        internal IWebElement FooterIcannCertification { get; set; }

    }
}
