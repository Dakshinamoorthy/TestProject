using System;
using NamecheapUITests.PageObject.CMSPages.DomainsPage;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.PaymentProcess;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NamecheapUITests.PageObject.ValidationPages;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
namespace NamecheapUITests.Test.CMS.Domains
{
    [TestFixture]
    public class DomainNameSearch
    {
        [Test, Category("Functional Test"), Description("Purchasing domains by various associated tlds, The Domain Name are Associated With:")]
        [Category("Domains")]
        [TestCase("ReDomains")]
        [TestCase("EnomDomains")]
        [TestCase("BannedNameDomains")]
        [TestCase("BulkDomains")]
        [TestCase("PremiumReDomains")]
        [TestCase("PremiumEnomDomains")]
        [TestCase("ReDomainsWithoutWhois")]
        [TestCase("EnomDomainsWithoutWhois")]
        [TestCase("ReDomainsWithPremiumDNS")]
        [TestCase("EnomDomainsWithPremiumDNS")]
        public void PurchaseNewDomain(string purchasingDomainFor)
        {
            try
            {
                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Domains, UiConstantHelper.DomainNameSearch);
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(UiConstantHelper.DomainNameSearchPageTitle.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + UiConstantHelper.DomainNameSearchPageTitle);
                var newDomainNames = PageInitHelper<RandomDomainNameGenerator>.PageInit.DomainName(purchasingDomainFor.Trim());
                var searchResultDomainsList = PageInitHelper<DomainsPage>.PageInit.AddingDomainNamesToCart(purchasingDomainFor, newDomainNames);
                if (searchResultDomainsList == null) return;
                ICartValidation cartWidgetValidation = new DomainListCartValidation();
                var mergedSearchdDomainAndCartWidgetList = cartWidgetValidation.CartWidgetValidation(searchResultDomainsList);
                var withoutWhois = string.Empty; string withPremiumDns = string.Empty;
                if (purchasingDomainFor.Contains("WithoutWhois")) withoutWhois = "yes";
                if (purchasingDomainFor.Contains("WithPremiumDNS")) withPremiumDns = "yes";
                IShoppingCartValidation addShoppingCartItems = new AddProductShoppingCartItems();
                var mergedScAndCartWidgetList = addShoppingCartItems.AddShoppingCartItemsToDic(mergedSearchdDomainAndCartWidgetList, withoutWhois, withPremiumDns);
                var mergedOrderNumbertoScWidgetList = PageInitHelper<PurchaseFlow>.PageInit.PurchasingNcProducts(mergedScAndCartWidgetList);
                var namespaceName = GetType().Namespace;
                PageInitHelper<TestFinalizerHelper>.PageInit.Testclosure(namespaceName);
                IDomainListValidation domainListValidation = new ValidateProductsInDomainList();
                domainListValidation.DomainListValidation(mergedOrderNumbertoScWidgetList);
                IOrdersValidation orderHistory = new ValidateDomainOrderInBillingPage();
                orderHistory.VerifyPurchasedOrderInBillingHistoryPage(mergedOrderNumbertoScWidgetList);
                ITransactionValidation transactionHistory = new ValidatingDomainTransationDetails();
                transactionHistory.VerifyPurchasedTransactionDetailsInBillingTransactionPage(mergedOrderNumbertoScWidgetList);
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