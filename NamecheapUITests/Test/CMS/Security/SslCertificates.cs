using System;
using System.Text.RegularExpressions;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.SecurityPageFactory;
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
    public class SslCertificates
    {
        [Test, Category("Functional Test"),
         Description("SSL Certification Purchase With Following Domain (DV) Validation and Domains as:")]
        [Category("Security")]
        [TestCase("DVWithSingleDomain")]
        [TestCase("DVWithMultipleDomain")]
        [TestCase("DVWithWildcard")]
        public void PurchaseNewDomainValidationCeritificate(string purchasingSslFor)
        {
            try
            {
                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Security, UiConstantHelper.SslCertificates);
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(UiConstantHelper.SslPagetitle.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + UiConstantHelper.SslPagetitle);
                PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<SslCertificatePageFactory>.PageInit.ValidationFilter, PageInitHelper<SslCertificatePageFactory>.PageInit.DvOption);
                var searchResultDomainsList = PageInitHelper<SslCertificatePage>.PageInit.AddingSslProductToCart(Regex.Replace(purchasingSslFor, "With", " ").Trim());
                var mergedOrderNumbertoScWidgetList = PageInitHelper<PurchaseFlow>.PageInit.PurchasingNcProducts(searchResultDomainsList);
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
        [Test, Category("Functional Test"), Description(
           "SSL Certification Purchase With Following Organization (OV) Validation and Domains as:")]
        [Category("Security")]
        [TestCase("OVWithSingleDomain")]
        [TestCase("OVWithMultipleDomain")]
        [TestCase("OVWithWildcard")]
        public void PurchaseNewOrganizationValidationCeritificate(string purchasingSslFor)
        {
            try
            {
                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Security, UiConstantHelper.SslCertificates);
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(UiConstantHelper.SslPagetitle.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + UiConstantHelper.SslPagetitle);
                PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<SslCertificatePageFactory>.PageInit.ValidationFilter, PageInitHelper<SslCertificatePageFactory>.PageInit.OvOption);
                var searchResultDomainsList = PageInitHelper<SslCertificatePage>.PageInit.AddingSslProductToCart(Regex.Replace(purchasingSslFor, "With", " ").Trim());
                var mergedOrderNumbertoScWidgetList = PageInitHelper<PurchaseFlow>.PageInit.PurchasingNcProducts(searchResultDomainsList);
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
        [Test, Category("Functional Test"), Description(
          "SSL Certification Purchase With Following Extended (EV) Validation and Domains as:")]
        [Category("Security")]
        [TestCase("EVWithSingleDomain")]
        [TestCase("EVWithMultipleDomain")]
        [TestCase("EVWithWildcard")]
        public void PurchaseNewExtendedValidationCeritificate(string purchasingSslFor)
        {
            try
            {
                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Security, UiConstantHelper.SslCertificates);
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(UiConstantHelper.SslPagetitle.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + UiConstantHelper.SslPagetitle);
                PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<SslCertificatePageFactory>.PageInit.ValidationFilter, PageInitHelper<SslCertificatePageFactory>.PageInit.EvOption);
                var searchResultDomainsList = PageInitHelper<SslCertificatePage>.PageInit.AddingSslProductToCart(Regex.Replace(purchasingSslFor, "With", " ").Trim());
                var mergedOrderNumbertoScWidgetList = PageInitHelper<PurchaseFlow>.PageInit.PurchasingNcProducts(searchResultDomainsList);
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