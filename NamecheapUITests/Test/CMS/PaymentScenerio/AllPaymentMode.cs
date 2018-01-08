using System;
using System.Text.RegularExpressions;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.SecurityPageFactory;
using NamecheapUITests.PageObject.CMSPages.SecurityPage;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NamecheapUITests.PageObject.ValidationPages;
using NUnit.Framework;
using PurchaseFlow = NamecheapUITests.PageObject.HelperPages.PaymentProcess.PurchaseFlow;

namespace NamecheapUITests.Test.CMS.PaymentModes
{
    [TestFixture]
    public class AllPaymentMode
    {
        [Test, Category("Functional Test"),
         Description("Purchasing SSL Product Using SecureCard, PayPal and AccountFunds")]
        [Category("SSL")]
        [TestCase("Card")]
        [TestCase("Paypal")]
        [TestCase("Funds")]
        public void SslDomainValidationCeritificatePurchase(string paymentMethod)
        {
            try
            {
                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Security, UiConstantHelper.SslCertificates);
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(UiConstantHelper.SslPagetitle.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + UiConstantHelper.SslPagetitle);
                PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<SslCertificatePageFactory>.PageInit.ValidationFilter, PageInitHelper<SslCertificatePageFactory>.PageInit.DvOption);
                var searchResultDomainsList = PageInitHelper<SslCertificatePage>.PageInit.AddingSslProductToCart(Regex.Replace("DVWithSingleDomain", "With", " ").Trim());
                var mergedOrderNumbertoScWidgetList =
                    PageInitHelper<PurchaseFlow>.PageInit.PurchasingNcProducts(searchResultDomainsList, paymentMethod,
                        false);
                var namespaceName = GetType().Namespace;
                PageInitHelper<TestFinalizerHelper>.PageInit.Testclosure(namespaceName);
                IDomainListValidation domainListValidation = new SslProductListValidation();
                domainListValidation.DomainListValidation(mergedOrderNumbertoScWidgetList);
                IOrdersValidation orderHistory = new ValidateSslOrderInBillingPage();
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