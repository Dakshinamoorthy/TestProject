using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Gallio.Framework;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.DomainsPageFactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace NamecheapUITests.PageObject.CMSPages.DomainsPage
{
    public class MarketPlacePage
    {
        public List<string> DomainNames(string purchasingDomainForm)
        {
            var linkHref = purchasingDomainForm.Contains("All") ? "buy-domains" : purchasingDomainForm.ToLower();
            BrowserInit.Driver.FindElement(By.XPath("//*[@class='tab-container']//li[contains(@class,'nav-item')]/a[contains(@href,'" + linkHref + "')]")).Click();
            if (PageInitHelper<MarketPlacePageFactory>.PageInit.DomainItemsTxt.Text.Trim().Contains("no items"))
                throw new InconclusiveException(PageInitHelper<MarketPlacePageFactory>.PageInit.DomainItemsTxt.Text.Trim() + " for " + purchasingDomainForm + " tab in the domains markrt place");
            var totalPageNumberCount = PageInitHelper<MarketPlacePageFactory>.PageInit.TotalPageNumberCountLst;
            if (totalPageNumberCount.Count <= 0)
                return
                    PageInitHelper<MarketPlacePageFactory>.PageInit.DomainsNamesLst.Select(
                        strongTxt => strongTxt.Text.Trim()).ToList();
            var randomPageNumber = PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(totalPageNumberCount.Count);
            PageInitHelper<PageNavigationHelper>.PageInit.MoveToElement(PageInitHelper<MarketPlacePageFactory>.PageInit.TotalPageNumberCountLst[randomPageNumber - 1]);
            PageInitHelper<MarketPlacePageFactory>.PageInit.TotalPageNumberCountLst[(randomPageNumber) - 1].Click();
            PageInitHelper<PageNavigationHelper>.PageInit.WaitForPageLoad();
            Thread.Sleep(3000);
            return PageInitHelper<MarketPlacePageFactory>.PageInit.DomainsNamesLst.Select(strongTxt => strongTxt.Text.Trim()).ToList();
        }
        internal List<SortedDictionary<string, string>> AddingDomainNamesToCart(List<string> newDomainNames, string purchasingDomainForm)
        {
            var dicMarketPlaceProductDetailsList = new List<SortedDictionary<string, string>>();
            var dicMarketPlaceProductDetailsDic = new SortedDictionary<string, string>();
            foreach (var domainname in newDomainNames)
            {
                PageInitHelper<MarketPlacePageFactory>.PageInit.DomainNameSearchInput.Clear();
                PageInitHelper<MarketPlacePageFactory>.PageInit.DomainNameSearchInput.SendKeys(domainname);
                PageInitHelper<MarketPlacePageFactory>.PageInit.DomainNameSearchBtn.Click();
                PageInitHelper<TimeSpanHelper>.PageInit.VerifyElementPresentBasedonEleText(PageInitHelper<MarketPlacePageFactory>.PageInit.TableLoading, UiConstantHelper.AttributeClass, "loading");
                if (PageInitHelper<MarketPlacePageFactory>.PageInit.DomainItemsTxt.Text.Contains("no items"))
                    throw new TestFailedException("Search Functionality is not working on domain market place for '" +
                                                  purchasingDomainForm + "',Tab while searching  domain name " +
                                                  domainname);
                var domainDiv =
                    "(//mp-list//nc-grid[contains(@class,'nc-grid')]/div/nc-grid-table//table/tbody/tr/td/a[contains(@class,'link')][normalize-space(.)='" +
                    domainname + "']/../../td)";
                var domainDivEle = BrowserInit.Driver.FindElements(By.XPath(domainDiv));
                for (int i = 1; i <= domainDivEle.Count; i++)
                {
                    var tagName = BrowserInit.Driver.FindElement(By.XPath(domainDiv + "[" + i + "]/child::*"));
                    if (tagName.TagName.Equals("a"))
                    {
                        var dDetails = tagName.Text.Contains("\r\n")
                      ? tagName.Text.Split(new[] { "\r\n" }, StringSplitOptions.None)[0]
                      : tagName.Text;
                        if (dDetails.Contains(domainname))
                            dicMarketPlaceProductDetailsDic.Add(EnumHelper.DomainKeys.DomainName.ToString(), dDetails);
                    }
                    else if (tagName.TagName.Equals("span"))
                    {
                        var dDetails = BrowserInit.Driver.FindElement(By.XPath(domainDiv + "[" + i + "]")).Text;
                        if (tagName.GetAttribute(UiConstantHelper.AttributeClass).Contains("closing-on"))
                            dicMarketPlaceProductDetailsDic.Add(EnumHelper.DomainKeys.DomainDuration.ToString(),
                                dDetails.Trim().Contains("minutes")
                                    ? dDetails.Trim()
                                    : DateTime.Parse(dDetails.Trim()).ToString("MMM d, yyyy"));
                        else
                            dicMarketPlaceProductDetailsDic.Add(EnumHelper.DomainKeys.DomainPrice.ToString(),
                                  Convert.ToDecimal(Regex.Replace(dDetails, @"[^\d..][^\w\s]*", "").Trim()).ToString(CultureInfo.InvariantCulture));
                    }
                }
                dicMarketPlaceProductDetailsList.Add(dicMarketPlaceProductDetailsDic);
                BrowserInit.Driver.FindElement(By.XPath(domainDiv + "/a[contains(@class,'btn btn-grey btn-lg')]")).Click();
                var alertMsg = PageInitHelper<MarketPlacePageFactory>.PageInit.SuccessMessageTxt.Text.Trim();
                if (alertMsg.Contains("Seller Name and BuyerName can not be same"))
                {
                    BrowserInit.Driver.Navigate()
                       .GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("CMS",
                           "/domains/marketplace/buy-domains.aspx"));
                    continue;
                }
                if (alertMsg.IndexOf("No Item added to the cart", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    throw new TestFailedException(
                    "The market place domain isn't added in the shopping cart page it shows error message for the domain name " +
                    domainname + " while adding product from " + purchasingDomainForm + " tab. Error message is : " + alertMsg);
                break;
            }
            return dicMarketPlaceProductDetailsList;
        }
        public void PerformTestOnDomainFilters(string filterName)
        {
            Assert.IsTrue(PageInitHelper<MarketPlacePageFactory>.PageInit.RefineSearchGrid.Displayed, "In Market place page none of the filter search option is available");
            switch (EnumHelper.ParseEnum<EnumHelper.MarketPlaceFilters>(filterName))
            {
                case EnumHelper.MarketPlaceFilters.Categories:
                    var domainItemsTxt = PageInitHelper<MarketPlacePageFactory>.PageInit.DomainItemsTxt.Text.Trim();
                    if (domainItemsTxt.Contains("no items"))
                        throw new InconclusiveException(domainItemsTxt + " in the domains market place");
                    var categoriesMoreLink = PageInitHelper<MarketPlacePageFactory>.PageInit.CategoriesMoreLink;
                    PageInitHelper<TimeSpanHelper>.PageInit.WaitUntilElementIsDisplayed(categoriesMoreLink);
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElement(categoriesMoreLink);
                    categoriesMoreLink.Click();
                    PageInitHelper<TimeSpanHelper>.PageInit.VerifyElementPresentBasedonattribute(PageInitHelper<MarketPlacePageFactory>.PageInit.SelectCategoriesModalpopup, "aria-hidden", "false");
                    var catName = "";
                    PageInitHelper<MarketPlacePageFactory>.PageInit.UncheckAllLnk.Click();
                    var checkedItems = new StringBuilder();
                    foreach (var categoriesName in PageInitHelper<MarketPlacePageFactory>.PageInit.ModalCategoryList.Where(categoriesName => categoriesName.Selected))
                        checkedItems.Append(categoriesName.FindElement(By.TagName("label")).Text);
                    var checkedItemsCount = checkedItems.Length > 0;
                    if (checkedItemsCount)
                        throw new TestInconclusiveException("In Select your categories Modal Window after click on uncheck links these categories are remain unchecked " + "{0} chars: {1}" + checkedItems.Length + checkedItems);
                    if (catName == string.Empty)
                    {
                        var catNumber = PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(
                            PageInitHelper<MarketPlacePageFactory>.PageInit.ModalCategoryList.Count);
                        var cat = BrowserInit.Driver.FindElement(By.XPath("(.//div[contains(@class,'form-group')]/div/div/label)[" + catNumber + "]"));
                        catName = cat.Text;
                        cat.Click();
                    }
                    PageInitHelper<MarketPlacePageFactory>.PageInit.DoneBtn.Click();
                    PageInitHelper<TimeSpanHelper>.PageInit.VerifyElementPresentBasedonEleText(PageInitHelper<MarketPlacePageFactory>.PageInit.TableLoading, UiConstantHelper.AttributeClass, "loading");
                    domainItemsTxt = PageInitHelper<MarketPlacePageFactory>.PageInit.DomainItemsTxt.Text.Trim();
                    if (domainItemsTxt.Contains("no items"))
                        throw new InconclusiveException("No data are available in the grid for the category " + catName.Trim() + " in the domain marketplace");
                    var domainIndex = PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(
                           PageInitHelper<MarketPlacePageFactory>.PageInit.DomainsNamesLst.Count);
                    var dname = PageInitHelper<MarketPlacePageFactory>.PageInit.DomainsNamesLst[domainIndex];
                    var domainName = dname.Text;
                    dname.Click();

                    Assert.IsTrue(domainName.Trim().Equals(PageInitHelper<MarketPlacePageFactory>.PageInit.DNameinDomainListing.Text.Trim(), StringComparison.InvariantCultureIgnoreCase), "Market place landing page domain name is missmatching in market palce detail page domain name expected domain name in detail page is " + domainName + ", but actual domain name shown as " + PageInitHelper<MarketPlacePageFactory>.PageInit.DNameinDomainListing.Text.Trim());
                    foreach (var category in PageInitHelper<MarketPlacePageFactory>.PageInit.CategoriesNamesWebElement.Where(category => category.Text.Contains(catName)))
                    {
                        Assert.IsTrue(category.Text.Contains(catName), "Category Name in Listed Domain page is mismatches expected category name " + catName + " , but the actual values are shown as " + category.Text);
                        break;
                    }

                    break;
                case EnumHelper.MarketPlaceFilters.Price:
                    var priceCat = new StringBuilder();
                    var vartionDname = new StringBuilder();
                    const string inputXpath = "(.//*[@id='filter1']/li[2]/ul/li/div/input)";
                    for (var i = 1; i <= BrowserInit.Driver.FindElements(By.XPath(inputXpath)).Count; i++)
                    {
                        var selected = BrowserInit.Driver.FindElement(By.XPath(inputXpath + "[" + i + "]")).Selected;
                        if (selected)
                            BrowserInit.Driver.FindElement(By.XPath(inputXpath + "[" + i + "]/following-sibling::label")).Click();
                        Thread.Sleep(1000);
                        PageInitHelper<TimeSpanHelper>.PageInit.VerifyElementPresentBasedonEleText(PageInitHelper<MarketPlacePageFactory>.PageInit.TableLoading, UiConstantHelper.AttributeClass, "loading");

                    }
                    for (var i = 1; i <= BrowserInit.Driver.FindElements(By.XPath(inputXpath)).Count; i++)
                    {

                        var selected = BrowserInit.Driver.FindElement(By.XPath(inputXpath + "[" + i + "]")).Selected;
                        if (selected) continue;
                        var followingLabelXpath = inputXpath + "[" + i + "]/following-sibling::label";
                        // var followingLabelXpath = inputXpath + "[5]/following-sibling::label";
                        BrowserInit.Driver.FindElement(By.XPath(followingLabelXpath)).Click();
                        Thread.Sleep(3000);
                        var wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(10));
                        wait.Until(driver => BrowserInit.Driver.FindElement(By.XPath("(//mp-list//nc-grid[contains(@class,'nc-grid')]/div/nc-grid-table//table/tbody/tr/td[1]/a)[1]")).Displayed);
                        PageInitHelper<TimeSpanHelper>.PageInit.VerifyElementPresentBasedonEleText(PageInitHelper<MarketPlacePageFactory>.PageInit.TableLoading, UiConstantHelper.AttributeClass, "loading");
                        var priceFilterText = BrowserInit.Driver.FindElement(By.XPath(followingLabelXpath)).Text;
                        var priceFilter = priceFilterText.Contains("Under")
                            ? priceFilterText.Replace("Under", "$0 to ")
                            : priceFilterText.Contains("Over")
                                ? priceFilterText.Replace("Over", "").Trim() + " to " + int.MaxValue
                                : priceFilterText;
                        if (PageInitHelper<MarketPlacePageFactory>.PageInit.DomainItemsTxt.Text.Contains("no items"))
                        {
                            priceCat.Append(priceFilterText + ", ");
                            BrowserInit.Driver.FindElement(By.XPath(followingLabelXpath)).Click();
                            continue;
                        }
                        char[] whitespace = { ' ', '\t' };
                        var ssizes = Regex.Replace(priceFilter, "[A-Za-z ]", " ").Split(whitespace);
                        ssizes = ssizes.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        string dnamestrong = string.Empty;
                        foreach (var dlistPriceTag in BrowserInit.Driver.FindElements(By.XPath("(//mp-list//nc-grid[contains(@class,'nc-grid')]/div/nc-grid-table//table/tbody/tr/td/*)")))
                        {
                            var tagName = dlistPriceTag.GetAttribute(UiConstantHelper.AttributeClass);
                            if (tagName.Contains("link-gray"))
                            {
                                dnamestrong = dlistPriceTag.Text.Trim();
                            }

                            if (!tagName.Contains("padding-right")) continue;
                            var dnamespan = dlistPriceTag.Text.Trim();
                            var pricerange = Convert.ToDecimal(Regex.Replace(dnamespan, @"[^\d..][^\w\s]*", "").Trim());
                            var fPrice = Convert.ToDecimal(Regex.Replace(ssizes[0], @"[^\d..][^\w\s]*", "").Trim().ToString(CultureInfo.InvariantCulture));
                            var tPrice = Convert.ToDecimal(Regex.Replace(ssizes[1], "[^a-zA-Z0-9-]", "").Trim().ToString(CultureInfo.InvariantCulture));
                            var fromPrice = $"{fPrice:0.00}";
                            var toPrice = $"{tPrice:0.00}";
                            var pPrice = $"{pricerange:0.00}";
                            if (!(Convert.ToDecimal(pPrice) >= Convert.ToDecimal(fromPrice) && pricerange <= Convert.ToDecimal(toPrice)))
                                vartionDname.Append("For Domain " + dnamestrong + "- Actual price: " + pricerange + ", but expected range bwt:- " + priceFilter + ", ");
                        }

                        BrowserInit.Driver.FindElement(By.XPath(followingLabelXpath)).Click();
                        Thread.Sleep(1000);
                        var wait1 = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromMinutes(10));
                        wait1.Until(driver => BrowserInit.Driver.FindElement(By.XPath("(//mp-list//nc-grid[contains(@class,'nc-grid')]/div/nc-grid-table//table/tbody/tr/td[1]/a)[1]")).Displayed);
                        PageInitHelper<TimeSpanHelper>.PageInit.VerifyElementPresentBasedonEleText(PageInitHelper<MarketPlacePageFactory>.PageInit.TableLoading, UiConstantHelper.AttributeClass, "loading");
                    }
                    if (vartionDname.Length > 0 && priceCat.Length == 0)
                        throw new TestFailedException("Price varation domains are: " + string.Format(vartionDname.ToString()));
                    if (vartionDname.Length == 0 && priceCat.Length > 0)
                        throw new InconclusiveException("No domains are availabe in the domains market place grid for the price filters- " + string.Format(priceCat.ToString()));
                    if (vartionDname.Length > 0 && priceCat.Length > 0)
                        throw new TestFailedException("There are no items to display for Domain Names: " + string.Format(priceCat.ToString()) + "Price varation domains are: " + string.Format(vartionDname.ToString()));
                    break;
                case EnumHelper.MarketPlaceFilters.Content:
                    throw new NotImplementedException("Not yet Implemented");
                    break;
                case EnumHelper.MarketPlaceFilters.MaxLength:
                    var dnameMissingmatching = new StringBuilder();
                    var maxLength = PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(10);
                    BrowserInit.Driver.FindElement(By.XPath(".//*[@id='filter1']/li[4]/input")).Clear();
                    BrowserInit.Driver.FindElement(By.XPath(".//*[@id='filter1']/li[4]/input")).SendKeys(maxLength + Keys.Enter);
                    Thread.Sleep(2000);
                    PageInitHelper<PageNavigationHelper>.PageInit.WaitForPageLoad();
                    if (PageInitHelper<MarketPlacePageFactory>.PageInit.DomainItemsTxt.Text.Contains("no items"))
                        throw new InconclusiveException("No domains are availabe in the domains market place grid for the price filters- " + string.Format(maxLength.ToString()));
                    foreach (var dnameTxt in PageInitHelper<MarketPlacePageFactory>.PageInit.DomainsNamesLst)
                    {
                        var dnameindex = dnameTxt.Text.Remove(dnameTxt.Text.IndexOf(".", StringComparison.Ordinal) + 1);
                        var numberOfLetters = dnameindex.Replace(".", string.Empty).ToCharArray().Length;
                        if (numberOfLetters > maxLength)
                            dnameMissingmatching.Append(dnameTxt.Text + "- " + numberOfLetters + "chars ,");
                    }
                    if (dnameMissingmatching.Length > 0)
                        throw new TestFailedException(string.Format(dnameMissingmatching.ToString()) + " For these domain names in market place Max-Length as " + maxLength + " is missmatching");
                    break;
                case EnumHelper.MarketPlaceFilters.Seller:
                    throw new NotImplementedException("Not yet Implemented");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(filterName + ", filter name is not listed in Market Place Filters enum type in EnumHelper.class");
            }
        }
    }
}