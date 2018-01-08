using System;
using NamecheapUITests.PageObject.CMSPages.FooterPage;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NUnit.Framework;

namespace NamecheapUITests.Test.CMS.Support
{
    [TestFixture]
    public class Footer
    {
        [Test, Category("Smoke Test"), NUnit.Framework.Description("Verify the Footer Main Attributes and the Web Page Response")]
        [TestCase("NewsletterAndSocialNetwork")]
        [TestCase("Legal")] //Terms and Conditions, Privacy Policy, UDRP
        [TestCase("Causes")]
        [TestCase("Certifications")]
        public void FooterValidation(string process)
        {
            try
            {
                PageInitHelper<FooterPage>.PageInit.ValidateFooter(process);
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

        [Test, Category("Smoke Test"), NUnit.Framework.Description("Verify the Footer Page Links, Attributes and the Web Page Response")]
        [TestCase("Domains")]
        [TestCase("Hosting")]
        [TestCase("PremiumDNS")]
        [TestCase("FreeDNS")]
        [TestCase("WhoisGuard")]
        [TestCase("SSL Certificates")]
        [TestCase("Resellers")]
        [TestCase("Affiliates")]
        [TestCase("Support")]
        [TestCase("Careers")]
        [TestCase("Send us Feedback")]
        public void FooterPageLinksValidation(string process)
        {
            try
            {
                PageInitHelper<FooterPage>.PageInit.ValidateFooter(process);
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
