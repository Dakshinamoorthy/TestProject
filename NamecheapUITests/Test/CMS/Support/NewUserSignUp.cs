using System;
using System.Text.RegularExpressions;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.SecurityPageFactory;
using NamecheapUITests.PageObject.CMSPages.SecurityPage;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.PaymentProcess;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NamecheapUITests.PageObject.ValidationPages;
using NamecheapUITests.Test.CMS.Domains;
using NUnit.Framework;

namespace NamecheapUITests.Test.CMS.Support
{
    [TestFixture]
    public class NewUserSignUp
    {
        [Test, Category("Functional Test"),
         Description("Create New User Account and Purchasing domains by Re Tld")]
        [Category("Domains")]
        public void A_NewUserSignUpAndPurchaseDomain()
        {
            try
            {
                if (AppConfigHelper.MainUrl.Equals("live") && AppConfigHelper.NewUserSignupinProduction.Equals("N"))
                    throw new IgnoreException("We Cant able to do this test in Production site Because New User Sign up - email validation will be charged by 3rd party vendor");
                PageInitHelper<LoginPageHelper>.PageInit.NcLogout();
                PageInitHelper<LoginPageHelper>.PageInit.UserSignUpPage();
                var namespaceName = GetType().Namespace;
                PageInitHelper<TestFinalizerHelper>.PageInit.Testclosure(namespaceName);
                PageInitHelper<DomainNameSearch>.PageInit.PurchaseNewDomain("ReDomains");
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
        [Test, Category("Functional Test"),
       Description("Add Product to cart and than signup and validate cart item")]
        [Category("Security")]
        public void B_AddProductToCartAndSignUp()
        {
            try
            {
                PageInitHelper<LoginPageHelper>.PageInit.NcLogout();
                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Security, UiConstantHelper.SslCertificates);
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(UiConstantHelper.SslPagetitle.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + UiConstantHelper.SslPagetitle);
                PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<SslCertificatePageFactory>.PageInit.ValidationFilter, PageInitHelper<SslCertificatePageFactory>.PageInit.DvOption);
                var searchResultDomainsList = PageInitHelper<SslCertificatePage>.PageInit.AddingSslProductToCart(Regex.Replace("DVWithSingleDomain", "With", " ").Trim());
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(UiConstantHelper.LoginSignUpPageTitle.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + UiConstantHelper.SslPagetitle);
                PageInitHelper<LoginPageHelper>.PageInit.LoginPage();
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
    }
}