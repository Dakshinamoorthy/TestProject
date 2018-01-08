using System;
using NamecheapUITests.PageObject.CMSPages.DomainsPage;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
namespace NamecheapUITests.Test.CMS.Domains
{
    [TestFixture]
    public class Whois
    {
        [Test, Category("Functional Test"), Description("Verify the owner of the domainname")]
        [Category("Domains")]
        [TestCase("WhoisLookup")]
        public void WhoisLookUpForDomainName(string purchasingDomainFor)
        {
            try
            {
                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Domains, UiConstantHelper.Whois);
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(UiConstantHelper.Whois.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + UiConstantHelper.Whois);
                var whoisDomainNames = PageInitHelper<RandomDomainNameGenerator>.PageInit.DomainName(purchasingDomainFor.Trim());
                PageInitHelper<WhoisLookupPage>.PageInit.VerifyDomainOwnerInfo(whoisDomainNames);
                var namespaceName = GetType().Namespace;
                PageInitHelper<TestFinalizerHelper>.PageInit.Testclosure(namespaceName);
            }
            catch (Exception ex)
            {
                PageInitHelper<LoggerHelper>.PageInit.CaptureException(ex);
                var exceptionType = ex.GetType().ToString();
                var namespaceName = GetType().Namespace;
                PageInitHelper<TestFinalizerHelper>.PageInit.Testclosure(namespaceName, ex);
                PageInitHelper<ExceptionType>.PageInit.ThrowException(exceptionType, ex);
            }
        }
        [TearDown]
        public void TearDown()
        {
            PageInitHelper<ClearCart>.PageInit.ClearCartItems();
        }
    }
}