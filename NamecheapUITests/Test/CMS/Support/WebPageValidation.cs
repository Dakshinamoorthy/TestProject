using System;
using NamecheapUITests.PageObject.CMSPages.SupportPage;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NUnit.Framework;

namespace NamecheapUITests.Test.CMS.Support
{
    [TestFixture]
    public class WebPageValidation
    {
        [Test, Category("Smoke Test"), NUnit.Framework.Description("Verify the Home Page Attributes and the Web Page Response")]
        public void ADashBoardWebResponse()
        {
            try
            { 
                PageInitHelper<WebPageValidationPage>.PageInit.VerifyHomepageWebResponse();
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


        [Test, Category("Smoke Test"),NUnit.Framework.Description("Verify the Domains Menu and validate the Sub Menu and Attributes of the Web Elements and the Web Page Response")]
        [TestCase("DomainNameSearch")]
        [TestCase("Transfer")]
        [TestCase("NewTlds")]
        [TestCase("PersonalDomain")]
        [TestCase("Marketplace")]
        [TestCase("Whois")]
        [TestCase("PremiumDns")]
        [TestCase("FreeDns")]
        public void BDomainMenusWebResponse(string domainMenuCategory)
        {
            try
            {
                PageInitHelper<WebPageValidationPage>.PageInit.VerifyDomainsPageWebResponseandElements(domainMenuCategory);
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


        [Test, Category("Smoke Test"), NUnit.Framework.Description("Verify the Hosting Menu and validate the Sub Menu and Attributes of the Web Elements and the Web Page Response")]
        [TestCase("Shared")]
        [TestCase("WordPressHosting")]
        [TestCase("Reseller")]
        [TestCase("Vps")]
        [TestCase("Dedicated")]
        [TestCase("PrivateEmail")]
        [TestCase("MigrateToNamecheap")]
        public void CHostingMenusWebResponse(string hostingMenuSubCategory)
        {
            try
            {
                PageInitHelper<WebPageValidationPage>.PageInit.VerifyHostingPageWebResponseandElements(hostingMenuSubCategory);
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

        //Application Is Not Stable(Final Development Is In-Progress)
        //[Test, TestCategory("Smoke Test"), NUnit.Framework.Description("Verify the Apps Menu and validate the Sub Menu and Attributes of the Web Elements and the Web Page Response")]
        //[TestCase("Marketplace")]
        //[TestCase("Subscriptions")]
        public void AppsMenusWebResponse(string appsMenuSubCategory)
        {
            try
            {
                //Application Is Not Stable(Final Development Is In-Progress)
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

        [Test, Category("Smoke Test"), NUnit.Framework.Description("Verify the Security Menu and validate the Sub Menu and Attributes of the Web Elements and the Web Page Response")]
        [TestCase("SslCertificate")]
        [TestCase("WhoisGuard")]
        [TestCase("PremiumDns")]
        public void ESecurityMenusWebResponse(string securityMenuSubCategory)
        {
            try
            {
                PageInitHelper<WebPageValidationPage>.PageInit.VerifySecurityPageWebResponseandElements(securityMenuSubCategory);
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


        [Test, Category("Smoke Test"), NUnit.Framework.Description("Verify the Profile Menu and Attributes of the Web Elements and the Web Page Response")]
        [TestCase("Domains")]
        [TestCase("Hosting")]
  //    [TestCase("Apps")] //Application Is Not Stable(Final Development Is In-Progress)
        [TestCase("Security")]
        [TestCase("Accounts")]
        public void FMainMenuTabFromDashboard(string mainMenuCategory)
        {
            try
            {
                PageInitHelper<WebPageValidationPage>.PageInit.VerifyMainMenuPageWebResponseandElements(mainMenuCategory);
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


        [Test, Category("Smoke Test"), NUnit.Framework.Description("Verify the Accounts Menu and Attributes of the Web Elements and the Web Page Response")]
        [TestCase("DashBoard")]
        [TestCase("ExpiringSoon")]
        [TestCase("DomainList")]
        [TestCase("ProductList")]
        [TestCase("Profile")]
        public void GAccountMenusWebResponse(string accountMenuCategory)
        {
            try
            {
                PageInitHelper<WebPageValidationPage>.PageInit.VerifyAccountsPageWebResponseandElements(accountMenuCategory);
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
    }
}
