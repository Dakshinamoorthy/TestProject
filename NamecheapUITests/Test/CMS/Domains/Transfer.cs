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
    public class Transfer
    {
        [Test, Category("Functional Test"), Description("Transfer my Existing Domain to Namecheap, Types of Transfer Domain are:")]
        [Category("Domains")]
        [TestCase("SingleDomainTransfer")]
        [TestCase("BulkDomainsTransfer")]
        public void TransferExistingDomainToNamecheap(string purchasingDomainFor)
        {
            try
            {
                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Domains, UiConstantHelper.Transfer);
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(UiConstantHelper.DomainTransferPageTitle.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + UiConstantHelper.DomainTransferPageTitle);
                var transferDomainNames = PageInitHelper<RandomDomainNameGenerator>.PageInit.DomainName(purchasingDomainFor.Trim());
                var searchResultDomainsList = PageInitHelper<TransferDomainPage>.PageInit.AddingDomainNamesToCart(purchasingDomainFor, transferDomainNames);
                ICartValidation cartWidgetValidation = new DomainListCartValidation();
                var mergedSearchdDomainAndCartWidgetList = cartWidgetValidation.CartWidgetValidation(searchResultDomainsList);
                IShoppingCartValidation addShoppingCartItems = new AddProductShoppingCartItems();
                var mergedScAndCartWidgetList = addShoppingCartItems.AddShoppingCartItemsToDic(mergedSearchdDomainAndCartWidgetList);
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