using System;
using NamecheapUITests.PageObject.CMSPages.Header;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NUnit.Framework;
namespace NamecheapUITests.Test.CMS.Header
{
    [TestFixture]
    public class SupportLinks
    {
        [Test, Category("Smoke Test"), Description("validating Support Links and redirections pages")]
        [Category("KnowledgeBase")]
        [TestCase("Support Center")]
        [TestCase("Status Updates")]
        [TestCase("Knowledgebase")]
        [TestCase("Submit Ticket")]
        [TestCase("Live Chat")]
        [TestCase("Report Abuse")]
        public void VerifySupportLinks(string page)
        {
            try
            {
                PageInitHelper<SupportLinksPage>.PageInit.GoToHeader(UiConstantHelper.Support);
                PageInitHelper<SupportLinksPage>.PageInit.PerformSupportLinkValidations(page);
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
        [Test, Category("Smoke Test"), Description("validating user menu links and redirections pages")]
        [Category("KnowledgeBase")]
        [TestCase("CustomerName")]
        [TestCase("UserName")]
        [TestCase("SupportPin")]
        [TestCase("Dashboard")]
        [TestCase("Profile")]
        [TestCase("Signout")]
        public void VerifyUserAccountLinks(string testAction)
        {
            try
            {
                PageInitHelper<SupportLinksPage>.PageInit.PerformUserAccountValidations(testAction);
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
        [TearDown]
        public void TearDown()
        {
            PageInitHelper<ClearCart>.PageInit.ClearCartItems();
        }
    }
}