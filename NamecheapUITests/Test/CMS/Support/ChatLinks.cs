using System;
using NamecheapUITests.PageObject.CMSPages.ChatLinksPage;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace NamecheapUITests.Test.CMS.Support
{
    [TestFixture]
    public class ChatLinks
    {
        [Test, Category("Functional Test"),
         NUnit.Framework.Description("Verify and validate the chat link page for all the products")]
        [TestCase("General")]
        [TestCase("Domains")]
        [TestCase("Domain Transfer")]
        [TestCase("Hosting")]
        [TestCase("Email")]
        [TestCase("SSL")]
        [TestCase("Apps")]
        public void ChatLinksValidation(string page)
        {
            try
            {
                PageInitHelper<ChatLinksPage>.PageInit.CheckHomePageLiveChat(page);
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
