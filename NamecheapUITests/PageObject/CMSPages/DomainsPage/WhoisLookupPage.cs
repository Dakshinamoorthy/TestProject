using System;
using System.Collections.Generic;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.DomainsPageFactory;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NUnit.Framework;
namespace NamecheapUITests.PageObject.CMSPages.DomainsPage
{
    public class WhoisLookupPage
    {
        internal void VerifyDomainOwnerInfo(List<string> whoisDomainNames)
        {
            foreach (var whoisDomainName in whoisDomainNames)
            {
                PageInitHelper<WhoisLookupPagefactory>.PageInit.DomainSearchForWhoisTxt.Clear();
                PageInitHelper<WhoisLookupPagefactory>.PageInit.DomainSearchForWhoisTxt.SendKeys(whoisDomainName);
                PageInitHelper<WhoisLookupPagefactory>.PageInit.WhoisLookuphBtn.Click();
                Assert.IsTrue(PageInitHelper<WhoisLookupPagefactory>.PageInit.WhoisDomainLabel.Text.Trim().Equals(whoisDomainName.Trim()),
                    "On WhoisLookup detail page searched Domain name Result is not matched with actual domain name detail listed expected domain Result should be " + whoisDomainName + ", he actual Result shown for domain name " + PageInitHelper<WhoisLookupPagefactory>.PageInit.WhoisDomainLabel.Text);
                var result = PageInitHelper<WhoisLookupPagefactory>.PageInit.WhoisResult.Text;
                if (result.Contains("No match for domain") || result.Contains("Domain not found")) Assert.Fail("On WhoisLookup Result page for the domain name " + whoisDomainNames + " owner info details is not listed");
                Assert.IsTrue(PageInitHelper<WhoisLookupPagefactory>.PageInit.WhoisResult.Text.Replace(" ", string.Empty).Trim().IndexOf("DomainName:" + whoisDomainName, StringComparison.InvariantCultureIgnoreCase) >= 0,
                    "Given Domain name is differ from the resulted Domain name expected Domain Name as " + whoisDomainName + ", but actual Result shown as " + PageInitHelper<WhoisLookupPagefactory>.PageInit.WhoisResult.Text.Replace(" ", string.Empty).Trim());
            }
        }
    }
}