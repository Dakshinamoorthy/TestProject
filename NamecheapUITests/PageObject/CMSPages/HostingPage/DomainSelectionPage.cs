using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.HostingPageFactory;
using NamecheapUITests.PagefactoryObject.ValidationPagefactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NamecheapUITests.PageObject.ValidationPages;
using OpenQA.Selenium;

namespace NamecheapUITests.PageObject.CMSPages.HostingPage
{
    class DomainSelectionPage
    {
        public List<List<Tuple<string, string, string, decimal, decimal>>> DomainNames(string hostingType, SortedDictionary<string, string> dicFromHosting, SortedDictionary<string, string> dicWithDomainDetails, string domainselection = "")
        {
            string getUrl = BrowserInit.Driver.Url;
            //string domainName = PageInitHelper<HostingPage>.PageInit.DomainNameForHosting();
            //var listDicHostingProduct = new List<SortedDictionary<string, string>>();
            var listDicHostingProduct = new List<List<Tuple<string, string, string, decimal, decimal>>>();

            if (!(BrowserInit.Driver.Url.Contains("domainselection.aspx") || BrowserInit.Driver.Url.Contains("wordpress.aspx")))
                BrowserInit.Driver.Navigate().GoToUrl(getUrl);

            var domainOptionsLst = PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainOptionsLst.Count;

            for (int i = 1; i <= domainOptionsLst; i++)
            {
                var dicHostingProduct = new SortedDictionary<string, string>();
                var listDic = new List<Tuple<string, string, string, decimal, decimal>>();
                if (!BrowserInit.Driver.Url.Contains("domainselection.aspx"))
                    BrowserInit.Driver.Navigate().GoToUrl(getUrl);

                string domainOptionsLstXpath = "(.//*[contains(@class,'domain-select-options')]//a)[" + i + "]";
                var domainCategory = BrowserInit.Driver.FindElement(By.XPath(domainOptionsLstXpath));
                var domainSelectionOptionTxt = domainCategory.Text.Trim();

                if (!domainselection.Equals(string.Empty))
                    if (!domainSelectionOptionTxt.Contains(domainselection)) continue;

                domainCategory.Click();
                IDomainSelectOptions selectDomain;

                if (domainSelectionOptionTxt.Contains(UiConstantHelper.CartDomain))
                {
                    selectDomain = new DomainInCart();
                    listDic = selectDomain.HostingDomainSelection();
                }
                else if (domainSelectionOptionTxt.Contains(UiConstantHelper.FreeDomain))
                {
                    selectDomain = new FreeDomain();
                    listDic = selectDomain.HostingDomainSelection(dicFromHosting);
                }
                else if (domainSelectionOptionTxt.Contains(UiConstantHelper.NewDomain))
                {
                    selectDomain = new PurchaseNewDomain();
                    listDic = selectDomain.HostingDomainSelection();
                }
                else if (domainSelectionOptionTxt.Contains(UiConstantHelper.NcDomain))
                {
                    selectDomain = new UseDomainWithNc();
                    listDic = selectDomain.HostingDomainSelection();
                }
                else if (domainSelectionOptionTxt.Contains(UiConstantHelper.AnotherRegistrarDomain))
                {
                    selectDomain = new DomainFromanotherRegistrar();
                    listDic = selectDomain.HostingDomainSelection();
                }
                else if (domainSelectionOptionTxt.Contains(UiConstantHelper.TransferDomainToNc))
                {
                    selectDomain = new TransferDomainFromanotherRegistrar();
                    listDic = selectDomain.HostingDomainSelection();
                }

                PageInitHelper<CartWidgetPageFactory>.PageInit.ViewCartButton.Click();


                if (!PageInitHelper<CartWidgetPageFactory>.PageInit.ShoppingCartHeadingTxt.Text.Trim().Equals("Shopping Cart"))
                {
                    PageInitHelper<CartWidgetPageFactory>.PageInit.ViewCartButton.Click();
                }

                listDicHostingProduct.Add(listDic);

                if (hostingType.Contains(UiConstantHelper.PrivateEmailHosting) && !(domainSelectionOptionTxt.Contains("Use a domain I own with Namecheap")))
                {
                    //Move to ViewCartButton
                    PageInitHelper<CartWidgetPageFactory>.PageInit.ViewCartButton.Click();
                }
                if(!domainselection.Equals(string.Empty)) break;
            }
            return listDicHostingProduct;
        }

        public List<SortedDictionary<string, string>> DomainNamesForHosting(string hostingType, SortedDictionary<string, string> dicFromHosting, SortedDictionary<string, string> dicWithDomainDetails, string domainselection = "")
        {
            string getUrl = BrowserInit.Driver.Url;
            if (!(BrowserInit.Driver.Url.Contains("domainselection.aspx") || BrowserInit.Driver.Url.Contains("wordpress.aspx")))
                BrowserInit.Driver.Navigate().GoToUrl(getUrl);

            var domainOptionListCount = PageInitHelper<DomainSelectionPageFactory>.PageInit.DomainOptionsLst.Count;
            int domainOptionsLst = //domainselection.Equals("true")
                //? PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(domainOptionListCount) : 
                domainOptionListCount;

            var listDicHostingProduct = new List<SortedDictionary<string, string>>();

            for (int i = 1; i <= domainOptionsLst; i++)
            {
                if (!(BrowserInit.Driver.Url.Contains("domainselection.aspx") || BrowserInit.Driver.Url.Contains("wordpress.aspx")))
                    BrowserInit.Driver.Navigate().GoToUrl(getUrl);
                //if (domainselection.Equals("true")) i = domainOptionsLst;

                string domainOptionsLstXpath = "(.//*[contains(@class,'domain-select-options')]//a)[" + i + "]";
                var domainCategory = BrowserInit.Driver.FindElement(By.XPath(domainOptionsLstXpath));
                var domainSelectionOptionTxt = domainCategory.Text.Trim();
                
                if (!domainselection.Equals(string.Empty))
                    if (!domainSelectionOptionTxt.Contains(domainselection)) continue;

                var dicDomainDetails = new SortedDictionary<string, string>();
                var listDic = new List<Tuple<string, string, string, decimal, decimal>>();
                if (!BrowserInit.Driver.Url.Contains("domainselection.aspx"))
                    BrowserInit.Driver.Navigate().GoToUrl(getUrl);
                
                domainCategory.Click();
                IDomainSelectOptions selectDomain;

                if (domainSelectionOptionTxt.Contains(UiConstantHelper.CartDomain))
                {
                    selectDomain = new DomainInCart();
                    listDic = selectDomain.HostingDomainSelection(dicWithDomainDetails);
                }
                else if (domainSelectionOptionTxt.Contains(UiConstantHelper.FreeDomain))
                {
                    selectDomain = new FreeDomain();
                    listDic = selectDomain.HostingDomainSelection(dicFromHosting);
                }
                else if (domainSelectionOptionTxt.Contains(UiConstantHelper.NewDomain))
                {
                    selectDomain = new PurchaseNewDomain();
                    listDic = selectDomain.HostingDomainSelection();
                }
                else if (domainSelectionOptionTxt.Contains(UiConstantHelper.NcDomain))
                {
                    selectDomain = new UseDomainWithNc();
                    listDic = selectDomain.HostingDomainSelection();
                }
                else if (domainSelectionOptionTxt.Contains(UiConstantHelper.AnotherRegistrarDomain))
                {
                    selectDomain = new UseDomainFromOtherRegister();
                    listDic = selectDomain.HostingDomainSelection();
                }
                else if (domainSelectionOptionTxt.Contains(UiConstantHelper.TransferDomainToNc))
                {
                    selectDomain = new TransferDomainFromanotherRegistrar();
                    listDic = selectDomain.HostingDomainSelection();
                }

                foreach (var list in listDic)
                {
                    string dname = list.Item1;
                    string duration = list.Item2;
                    string promocode = list.Item3;
                    decimal price = list.Item4;
                    decimal regprice = list.Item5;

                    dicDomainDetails.Add(EnumHelper.DomainKeys.DomainName.ToString(), dname);
                    dicDomainDetails.Add(EnumHelper.DomainKeys.DomainDuration.ToString(), duration);
                    dicDomainDetails.Add(EnumHelper.DomainKeys.DomainNamePromotionCode.ToString(), promocode);
                    dicDomainDetails.Add(EnumHelper.DomainKeys.DomainPrice.ToString(), price.ToString(CultureInfo.InvariantCulture));
                    dicDomainDetails.Add(EnumHelper.DomainKeys.DomainRetailPrice.ToString(), regprice.ToString(CultureInfo.InvariantCulture));
                }
                MergeSortedDictionary(dicFromHosting, dicDomainDetails);

                if(!(hostingType.Equals("Migrate to Namecheap") || hostingType.Equals("Shared Hosting")))
                    PageInitHelper<CartWidgetPageFactory>.PageInit.ViewCartButton.Click();


                ICartValidation cartValidation = new ProductListCartValidation();
                var diccartWidget = cartValidation.CartWidgetValidation(dicDomainDetails);

                if (!PageInitHelper<CartWidgetPageFactory>.PageInit.ShoppingCartHeadingTxt.Text.Trim().Equals("Shopping Cart"))
                {
                    PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(PageInitHelper<CartWidgetPageFactory>.PageInit.ViewCartButton);
                    PageInitHelper<CartWidgetPageFactory>.PageInit.ViewCartButton.Click();
                }

                MergeSortedDictionary(dicDomainDetails, diccartWidget);

                listDicHostingProduct.Add(diccartWidget);
                if(!domainselection.Equals(string.Empty))
                    return listDicHostingProduct;
            }
            return listDicHostingProduct;
        }
        internal void MergeSortedDictionary(SortedDictionary<string, string> source, SortedDictionary<string, string> destination)
        {
            foreach (var o in source)
                destination[o.Key] = o.Value;
        }
    }
}
