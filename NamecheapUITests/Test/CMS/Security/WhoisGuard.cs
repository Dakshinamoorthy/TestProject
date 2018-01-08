using System;
using NamecheapUITests.PageObject.CMSPages.SecurityPage;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.PaymentProcess;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NamecheapUITests.PageObject.ValidationPages;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
namespace NamecheapUITests.Test.CMS.Security
{
    [TestFixture]
    public class WhoisGuard
    {
        [Test, Category("Functional Test"), Description(
             "Purchasing WhoisGuard for domain by associated different domain registar, The whoisguard are Associated with:")]
        [Category("Domains")]
        [Category("Security")]
        [TestCase("NewDomain")]
        [TestCase("NcDomain")]
        [TestCase("TransferDomain")]
        public void PurchaseWhoisGuardWithDomains(string purchasingDomainFor)
        {
            try
            {
                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Security, UiConstantHelper.WhoisGuard);
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(UiConstantHelper.WhoisGuardPageTitle.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + UiConstantHelper.WhoisGuardPageTitle);
                var whoisGuardDetailsList = PageInitHelper<WhoisGuardPage>.PageInit.AddingWhoisGuardWithDomainNamesToCart(purchasingDomainFor);
                IShoppingCartValidation addShoppingCartItems = new AddProductShoppingCartItems();
                var mergedScAndCartWidgetList = addShoppingCartItems.AddShoppingCartItemsToDic(whoisGuardDetailsList);
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