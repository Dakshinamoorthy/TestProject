using System;
using NamecheapUITests.PageObject.CMSPages.DomainsPage;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NamecheapUITests.PageObject.ValidationPages;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
namespace NamecheapUITests.Test.CMS.Domains
{
    [TestFixture]
    public class FreeDns
    {
        [Test, Category("Functional Test"), Description(
             "Adding free dns services for following types of domain option for")]
        [Category("Domains")]
        [TestCase("SingleDomain")]
        [TestCase("BulkDomains")]
        public void FreeDnsServiceForDomainName(string purchasingDomainFor)
        {
            try
            {
                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Domains, UiConstantHelper.FreeDns);
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(UiConstantHelper.Freednspagetitle.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + UiConstantHelper.Freednspagetitle);
                var freeDnsDomainNames = PageInitHelper<RandomDomainNameGenerator>.PageInit.DomainName(purchasingDomainFor.Trim());
                var searchResultDomainsList = PageInitHelper<FreeDnsPage>.PageInit.AddingfreeDnsDomainNamesToCartAndPurchase(purchasingDomainFor, freeDnsDomainNames);
                ICartValidation cartWidgetValidation = new DomainListCartValidation();
                var mergedSearchdDomainAndCartWidgetList = cartWidgetValidation.CartWidgetValidation(searchResultDomainsList);
                var namespaceName = GetType().Namespace;
                PageInitHelper<TestFinalizerHelper>.PageInit.Testclosure(namespaceName);
                if (purchasingDomainFor == UiConstantHelper.SingleDomain)
                {
                    PageInitHelper<FreeDnsPage>.PageInit.ManageDomain(mergedSearchdDomainAndCartWidgetList);
                }
                else
                {
                    IDomainListValidation domainListValidation = new ValidateProductsInDomainList();
                    domainListValidation.DomainListValidation(mergedSearchdDomainAndCartWidgetList);
                }
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