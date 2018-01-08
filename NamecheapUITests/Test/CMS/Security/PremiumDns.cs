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
    public class PremiumDns
    {
        [Test, Category("Functional Test"),
         Description(
             "Purchasing PremiumDNS for domain by associated different domain registar, The PremiumDNS are Associated with:"
             )]
        [Category("Domains")]
        [Category("Security")]
        [TestCase("NewDomain")]
        [TestCase("NcDomain")]
        [TestCase("AnotherRegistrarDomain")]
        public void PurchasePremiumDnsWithDomains(string purchasingDomainFor)
        {
            try
            {
                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Security, UiConstantHelper.PremiumDns);
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(UiConstantHelper.PremiumDnsPageTitle.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + UiConstantHelper.PremiumDnsPageTitle);
                var listPremiumDnsDetails = PageInitHelper<PremiunDnsPage>.PageInit.AddingPremiumDnsWithDomainNamesToCart(purchasingDomainFor);
                IShoppingCartValidation addShoppingCartItems = new AddProductShoppingCartItems();
                var mergedScAndCartWidgetList = addShoppingCartItems.AddShoppingCartItemsToDic(listPremiumDnsDetails);
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