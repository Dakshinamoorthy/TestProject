using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using NamecheapUITests.PageObject.CMSPages.DomainsPage;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.PaymentProcess;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NamecheapUITests.PageObject.ValidationPages;
using NUnit.Framework;
namespace NamecheapUITests.Test.CMS.Domains
{
    [TestFixture]
    public class MarketPlace
    {
        [Test, Category("Functional Test"),
         Description("Purchasing domains from marketPlace, The Domain Name are Associated With following tab slection:")
        ]
        [Category("Domains")]
        [TestCase("All")]
        [TestCase("Featured")]
        [TestCase("Premium")]
        [TestCase("Closing")]
        public void PurchaseNewMarketPlaceDomain(string purchasingDomainForm)
        {
            try
            {
                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Domains, UiConstantHelper.Marketplace);
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(UiConstantHelper.MarketplacePageTitle.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + UiConstantHelper.MarketplacePageTitle);
                var newDomainNames = PageInitHelper<MarketPlacePage>.PageInit.DomainNames(purchasingDomainForm.Trim());
                var searchResultDomainsList = PageInitHelper<MarketPlacePage>.PageInit.AddingDomainNamesToCart(newDomainNames, purchasingDomainForm);
                IShoppingCartValidation addShoppingCartItems = new AddProductShoppingCartItems();
                var mergedScAndCartWidgetList = addShoppingCartItems.AddShoppingCartItemsToDic(searchResultDomainsList);
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
        [Test, Category("Smoke Test"), Description("Verify the following smoke in market place based on below listed filter names ")]
        [Category("Domains")]
        [TestCase("Categories")]
        [TestCase("Price")]
        [TestCase("Content")]
        [TestCase("MaxLength")]
        [TestCase("Seller")]
        public void MarketPlaceDomainValidation(string filterName)
        {
            try
            {
                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Domains, UiConstantHelper.Marketplace);
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(UiConstantHelper.MarketplacePageTitle.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + UiConstantHelper.MarketplacePageTitle);
                PageInitHelper<MarketPlacePage>.PageInit.PerformTestOnDomainFilters(filterName.Trim());
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