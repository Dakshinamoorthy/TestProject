using System;
using NamecheapUITests.PageObject.CMSPages.SupportPage;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NUnit.Framework;

namespace NamecheapUITests.Test.CMS.Support
{
    public class GlobalNcSearch
    {
        [TestFixture]
        public class NamecheapSearch
        {
            [Test, Category("Functional Test"),
             NUnit.Framework.Description("Verify and validate the Namecheap Articles and Websites")]
            [TestCase("Knowledgebase")]
            [TestCase("Website")]
            public void NcSearchValidation(string articleType)
            {
                try
                {
                    BrowserInit.Driver.Navigate().GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP"));
                    string searchingContent=PageInitHelper<GlobalNCSearchPage>.PageInit.performGlobalNCSearch();
                    PageInitHelper<GlobalNCSearchPage>.PageInit.validateSearchResultSetAndProcess(searchingContent);
                    PageInitHelper<GlobalNCSearchPage>.PageInit.checkArticleTypeAndValidateArticle(searchingContent, articleType);
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
}
