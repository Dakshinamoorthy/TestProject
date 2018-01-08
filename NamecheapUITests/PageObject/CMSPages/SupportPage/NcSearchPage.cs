using System;
using System.Text;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.SupportPageFactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.ValidationPages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Assert = MbUnit.Framework.Assert;

namespace NamecheapUITests.PageObject.CMSPages.SupportPage
{
    public class GlobalNCSearchPage
    {
        public string performGlobalNCSearch()
        {
            string searchContent;
            if (AppConfigHelper.MainUrl.Contains("sandbox")|| AppConfigHelper.MainUrl.Contains("live"))
            {
            Assert.IsTrue(PageInitHelper<GlobalNCSearchPageFactory>.PageInit.HeaderSearchIcon.Enabled," Header Search Icon Is Not Enable");
            PageInitHelper<GlobalNCSearchPageFactory>.PageInit.HeaderSearchIcon.Click();
            Assert.IsTrue(PageInitHelper<GlobalNCSearchPageFactory>.PageInit.HeaderSearchInput.Displayed, " Header Search Input Is Not Displayed");            
            searchContent =UiConstantHelper.KnowledgebaseArticlesTitle[PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(UiConstantHelper.KnowledgebaseArticlesTitle.Length)];
            PageInitHelper<GlobalNCSearchPageFactory>.PageInit.HeaderSearchInput.SendKeys(searchContent);                
            Assert.IsTrue(PageInitHelper<GlobalNCSearchPageFactory>.PageInit.HeaderSearchBtn.Enabled, " Header Search Button Is Not Enable");
            PageInitHelper<GlobalNCSearchPageFactory>.PageInit.HeaderSearchBtn.Click();
            Assert.IsTrue(PageInitHelper<GlobalNCSearchPageFactory>.PageInit.KnowledgebaseBreadcrumbs.Text.Equals(UiConstantHelper.KnowledgebaseSearchResultBreadcrumbs)," Incorrect Knowledgebase Breadcrumbs");
            }
            else
            {
                throw new InconclusiveException("User Exception : We are not able to perform the tests due to insufficient knowledgebase articles  in " + AppConfigHelper.MainUrl );
            }
            return searchContent;
        }

        public void validateSearchResultSetAndProcess(string searchContent)
        {
            /*Validate the empty articles present in the current page*/
            if (PageInitHelper<GlobalNCSearchPageFactory>.PageInit.ArticleDivCount.Count == 2)
                selectResultItemAndPerformWebPageValidation();
            else
                throw new Exception("User Exception : Insufficent data for search on "+ searchContent);
        }

        private void selectResultItemAndPerformWebPageValidation()
        {
            new Actions(BrowserInit.Driver).MoveToElement(BrowserInit.Driver.FindElement(By.XPath("//span[contains(@id,'PageNumberDisplayDiv')]/li"))).Build().Perform();
            BrowserInit.Driver.FindElement(By.XPath("//span[contains(@id,'PageNumberDisplayDiv')]/li["+ PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(PageInitHelper<GlobalNCSearchPageFactory>.PageInit.ArticlePageNumber.Count-10)+"]")).Click();
            new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(200)).Until(driver1 => ((IJavaScriptExecutor)BrowserInit.Driver).ExecuteScript("return document.readyState").Equals("complete"));
            PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
        }

        internal void checkArticleTypeAndValidateArticle(string searchContent, string caseType)
        {
            int selectedArticle;
            int knowledgebaseArticleCount=PageInitHelper<GlobalNCSearchPageFactory>.PageInit.ArticleListItems.Count;
            if (PageInitHelper<GlobalNCSearchPageFactory>.PageInit.ArticleListItems.Count == 0)
                throw new Exception("User Exception : Insufficient knowledgebase articles available in the selected page");

            for (selectedArticle=1; selectedArticle <= knowledgebaseArticleCount; selectedArticle++)
            {
                StringBuilder wholePageTxt = new StringBuilder();
                /*articleName not required for validation and it may use of future validation*/
                //string articleName = BrowserInit.Driver.FindElement(By.XPath("//ul[contains(@class,'article-list')]/li[" + selectedArticle + "]/p/a[1]")).Text;
                string articleType = BrowserInit.Driver.FindElement(By.XPath("//ul[contains(@class,'article-list')]/li[" + selectedArticle + "]/p/a[2]")).Text;
                /*  Need To Satisfy both the Calling Case and articleType should be the same
                    Condition 1.Verify both Selected articleType and Calling Method
                    Condition 2.Verify both Selected articleType and Selected Type link  */
                if (articleType.Contains(caseType.ToLower())&& articleType.Contains(UiConstantHelper.Website))
                {
                    BrowserInit.Driver.FindElement(By.XPath("//ul[contains(@class,'article-list')]/li[" + selectedArticle + "]/p/a[1]")).Click();
                    new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(200)).Until(driver1 => ((IJavaScriptExecutor)BrowserInit.Driver).ExecuteScript("return document.readyState").Equals("complete"));
                    PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
                    /*Verify the Page title with searchcontent else get entire text from the page except "Header" & "Footer" and verfiy he text presence*/
                    if (BrowserInit.Driver.Title.IndexOf(searchContent, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Assert.IsTrue(BrowserInit.Driver.Title.IndexOf(searchContent, StringComparison.OrdinalIgnoreCase) >= 0,"Expected page is " + searchContent + " but getting wrong page " +BrowserInit.Driver.Title);
                        break;
                    }
                    /*else is not required due to the above condtion satisfied means, need not to comes down the below code*/
                    foreach (var pageBodyContent in PageInitHelper<GlobalNCSearchPageFactory>.PageInit.PageBodyContent)
                    {
                        string pageBodyContentText = pageBodyContent.Text.Replace("\r\n"," ");
                        if(string.IsNullOrEmpty(pageBodyContentText)|| string.IsNullOrWhiteSpace(pageBodyContentText)) continue;
                        wholePageTxt.Append(pageBodyContentText + " ");
                    }
                    Assert.IsTrue(wholePageTxt.ToString().ToLower().Contains(searchContent.ToLower())," Expected page is " + searchContent + " but getting wrong page " + BrowserInit.Driver.Title);
                    break;            
                }
                else if (articleType.Contains(caseType.ToLower()) && articleType.Contains(UiConstantHelper.Knowledgebase))
                {
                    BrowserInit.Driver.FindElement(By.XPath("//ul[contains(@class,'article-list')]/li[" + selectedArticle + "]/p/a[1]")).Click();
                    new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(200)).Until(driver1 => ((IJavaScriptExecutor)BrowserInit.Driver).ExecuteScript("return document.readyState").Equals("complete"));
                    PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
                    //Validate the sub article knowledgebase page
                    if (PageInitHelper<GlobalNCSearchPageFactory>.PageInit.ArticleHeader.GetAttribute("id").Contains("CategoryInfoDisplayControl1"))
                    {
                        BrowserInit.Driver.Navigate().Back();
                        continue;
                    }
                    wholePageTxt.Append(PageInitHelper<GlobalNCSearchPageFactory>.PageInit.ArticleBodyContent.Text);
                    Assert.IsTrue(wholePageTxt.ToString().Replace("\r\n", " ").ToLower().Contains(searchContent.ToLower()), " Expected knowledgebase article page is " + searchContent + " but getting wrong page " + BrowserInit.Driver.Title);              
                    break;    
                }
            }
            /* Searching article is not present in the current page. Throw, Inconslusive exception*/
            if (selectedArticle > knowledgebaseArticleCount)
                throw new InconclusiveException("User Exception : Insufficient "+ searchContent+" article avaliable in the selected page");
        }
    }
}
