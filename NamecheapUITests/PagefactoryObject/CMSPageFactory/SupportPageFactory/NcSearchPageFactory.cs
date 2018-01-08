using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NamecheapUITests.PagefactoryObject.CMSPageFactory.SupportPageFactory
{
    public class GlobalNCSearchPageFactory
    {
        [FindsBy(How = How.CssSelector, Using = ".toggle-search")]
        [CacheLookup]
        internal IWebElement HeaderSearchIcon { get; set; }

        [FindsBy(How = How.XPath, Using = "//form[@novalidate='novalidate']/input[2]")]
        [CacheLookup]
        internal IWebElement HeaderSearchInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//form[@novalidate='novalidate']/button")]
        [CacheLookup]
        internal IWebElement HeaderSearchBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//nav[@class='breadcrumbs']")]
        [CacheLookup]
        internal IWebElement KnowledgebaseBreadcrumbs { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'kb-pages')]/div/span/span")]
        [CacheLookup]
        internal IList<IWebElement> ArticleDivCount { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[contains(@id,'PageNumberDisplayDiv')]/li")]
        [CacheLookup]
        internal IList<IWebElement> ArticlePageNumber { get; set; }

        [FindsBy(How = How.XPath, Using = "//form//div/p")]
        [CacheLookup]
        internal IList<IWebElement> PageBodyContent { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='grid-row group group']/div[1][contains(@class,'grid-col three-quarters')]")]
        [CacheLookup]
        internal IWebElement ArticleBodyContent { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'grid-row')]/div/span[1]")]
        [CacheLookup]
        internal IWebElement ArticleHeader { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@class='category'][text()='knowledgebase']")]
        [CacheLookup]
        internal IList<IWebElement> ArticleKnowledgebaseListItems { get; set; }

        [FindsBy(How = How.XPath, Using = "//ul[contains(@class,'article-list')]/li")]
        [CacheLookup]
        internal IList<IWebElement> ArticleListItems { get; set; }

        [FindsBy(How = How.XPath, Using = "//ul[contains(@class,'article-list')]/li")]
        [CacheLookup]
        internal IWebElement ArticleListItem { get; set; }

        [FindsBy(How = How.XPath, Using = "//ul[contains(@class,'article-list')]/li//a[1]")]
        [CacheLookup]
        internal IList<IWebElement> ArticleListItemsHyperLinks { get; set; }
    }
}
