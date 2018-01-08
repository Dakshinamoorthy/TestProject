using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.HostingPageFactory;
using NamecheapUITests.PagefactoryObject.ValidationPagefactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace NamecheapUITests.PageObject.CMSPages.HostingPage
{
    class HostingPage
    {
        internal List<SortedDictionary<string, string>> SelectHostingProduct(string productType, string dataCenterAndYears="", int isTrial = 0)
        {
            var _actions = new Actions(BrowserInit.Driver);
            var productDetailDic = new SortedDictionary<string, string>();
            var productDetailDicList = new List<SortedDictionary<string, string>>();

            var productTypeGridXpath =
                "(.//*[contains(@class,'grid-row group')]/div[contains(@class,'product-item')] | .//*[@id='aspnetForm']/div/div/div/div/ul | .//*[contains(@class,'grid-row group')]/div/div[contains(@class,'product-grid')])";
            //var productTypeGrid = PageInitHelper<HostingPageFactory>.PageInit.DedicatedServersCpuGrid;
            var productPlans = BrowserInit.Driver.FindElements(By.XPath(productTypeGridXpath));
            var domainSelectionOption = PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(productPlans.Count, 0);
            _actions.MoveToElement(productPlans[domainSelectionOption]).Build().Perform();

            PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(productPlans[domainSelectionOption]);

            if (productType.Contains(UiConstantHelper.WordPressHosting))
            {
                productDetailDic.Add(EnumHelper.HostingKeys.ProductName.ToString(), "EasyWP");
                var productName = productPlans[domainSelectionOption].FindElements(By.TagName(UiConstantHelper.TagParagraph));
                foreach (var product in productName)
                {
                    var price = product.GetAttribute(UiConstantHelper.AttributeClass).Contains("price")
                        ? EnumHelper.HostingKeys.ProductPrice.ToString()
                        : EnumHelper.HostingKeys.ProductRenewalPrice.ToString();

                    var priceDuration = product.Text.Split('/');

                    productDetailDic.Add(price, Regex.Replace(priceDuration[0], @"[^\d..][^\w\s]*", "").Trim());
                    if (price.Equals(EnumHelper.HostingKeys.ProductPrice.ToString()))
                        productDetailDic.Add(EnumHelper.HostingKeys.ProductDuration.ToString(), priceDuration[1].Equals("yr") ? "1 Year" : "1 Month");

                    //productDetailDic.Add(EnumHelper.DicionaryKeys.ProductPrice.ToString(), Regex.Replace(product.FindElement(By.ClassName("amount")).Text, @"[^\d..][^\w\s]*", "").Trim());
                    //productDetailDic.Add(price, product.Text);
                }
            }
            else if (productType.Contains(UiConstantHelper.DedicatedServers))
            {
                var productHeaders = PageInitHelper<HostingPageFactory>.PageInit.DedicatedServersHeaders;
                var headers = productHeaders.FindElements(By.TagName(UiConstantHelper.TagList));
                var productHeaderList = new List<string>();
                //Adding productHeaders.Text to productNamePrice list
                headers.ToList().ForEach(productHeader => { productHeaderList.Add(productHeader.Text.Equals(string.Empty) ? "Order" : productHeader.Text); });
                //Product Details
                var product = productPlans[domainSelectionOption].FindElements(By.TagName(UiConstantHelper.TagList));
                var listPointer = 0;
                product.ToList().ForEach(productDetail =>
                {
                    if (!productDetail.Text.Equals("Order"))
                    {
                        if (productHeaderList[listPointer].IndexOf("PRICE", StringComparison.InvariantCultureIgnoreCase) != -1)
                        {
                            productDetailDic.Add(EnumHelper.HostingKeys.ProductPrice.ToString(), Regex.Replace(productDetail.FindElement(By.ClassName("amount")).Text, @"[^\d..][^\w\s]*", "").Trim());
                            productDetailDic.Add(EnumHelper.HostingKeys.ProductDuration.ToString(), "1 " + productDetail.FindElement(By.TagName(UiConstantHelper.TagAbbreviation)).GetAttribute("title").Trim());
                        }
                        else
                            productDetailDic.Add(productHeaderList[listPointer].Equals("CPU") ? EnumHelper.HostingKeys.ProductName.ToString() : productHeaderList[listPointer], productDetail.Text);
                    }
                    listPointer = listPointer + 1;
                    if (productDetail.Text.IndexOf("Order", StringComparison.InvariantCultureIgnoreCase) != -1)
                        productDetail.Click();
                });
            }
            else
            {
                //Product Name & Price
                var productName = productPlans[domainSelectionOption].FindElement(By.TagName(UiConstantHelper.TagHeading2)).Text.Trim();
                productDetailDic.Add(EnumHelper.HostingKeys.ProductName.ToString(), productName);

                if (!dataCenterAndYears.Equals(string.Empty))
                {
                    var productDic = ChangeYears(productType);
                    //var listDicHostingProduct = PageInitHelper<DomainSelectionPage>.PageInit.DomainNamesForHosting(productType, productDic, null, UiConstantHelper.FreeDomain);
                    productDetailDicList.Add(productDetailDic);
                    return productDetailDicList;
                }

                var productPrice = Regex.Replace(productPlans[domainSelectionOption].FindElement(By.ClassName("amount")).Text.Trim(), @"[^\d..][^\w\s]*", "");
                productDetailDic.Add(EnumHelper.HostingKeys.ProductPrice.ToString(), productPrice);
                
                var productDuration = productPlans[domainSelectionOption].FindElement(By.TagName(UiConstantHelper.TagAbbreviation)).Text.Trim();
                productDetailDic.Add(EnumHelper.HostingKeys.ProductDuration.ToString(), productDuration.Equals("yr") ? "1 Year" : "1 Month");
                //End

                if (productType.Equals(UiConstantHelper.SharedHosting) || productType.Equals(UiConstantHelper.ResellerHosting) || productType.Equals(UiConstantHelper.MigrateToNamecheap) || productType.Equals(UiConstantHelper.PrivateEmailHosting))
                {
                    var productDurationDD = productPlans[domainSelectionOption].FindElement(By.XPath("//div[contains(@class,'duration')]")).Text.Trim();
                    //var productDurationDC = productTypeGrid[domainSelectionOption].FindElement(By.XPath("//div[contains(@class,'coreuiselect')][not(contains(@class,'duration'))]")).Text.Trim();
                    //productDetailDic.Add(EnumHelper.DicionaryKeys.ProductDatacenter.ToString(), productDurationDC);
                }

                //if Trial
                if (isTrial.Equals(1))
                {
                    var xpathDuration = ".//*[contains(@class,'product-grid')]/div" + "[" + (domainSelectionOption + 1) + "]//fieldset/div[contains(@class,'duration')]";
                    var duration = BrowserInit.Driver.FindElement(By.XPath(xpathDuration));
                    //string duration = _driver.FindElement(By.XPath(productGridXpath + "[" + domainSelectionOption + "]" + "//fieldset/div[contains(@class,'duration')]")).Text;
                    if (!duration.Text.Contains("months"))
                    {
                        duration.Click();
                        var isOpen = duration.GetAttribute(UiConstantHelper.AttributeClass);
                        if (isOpen.Contains(UiConstantHelper.Open))
                        {
                            var trialDurationXpath = xpathDuration + "/../div[contains(@class,'show')]//ul/li[contains(.,'month')]";
                            var purchaseDuration = BrowserInit.Driver.FindElement(By.XPath(trialDurationXpath)).Text;
                            BrowserInit.Driver.FindElement(By.XPath(trialDurationXpath)).Click();
                        }
                    }
                }
                
                /* var dshfkjgdsk = BrowserInit.Driver.FindElement(By.XPath(productTypeGridXpath + "[" + domainSelectionOption + "]//button"));
                _actions.MoveToElement(dshfkjgdsk).Build().Perform();
                dshfkjgdsk.Click();
                */
                _actions.MoveToElement(productPlans[domainSelectionOption].FindElement(By.TagName(UiConstantHelper.TagButton))).Build().Perform();
                var dshfkjgk = productPlans[domainSelectionOption].FindElement(By.TagName(UiConstantHelper.TagButton)).GetAttribute("class");
                productPlans[domainSelectionOption].FindElement(By.TagName(UiConstantHelper.TagButton)).Click();
            }
            productDetailDicList.Add(productDetailDic);
            return productDetailDicList;
        }

        internal void ChangeDataCenterAndYears(string hostingType)
        {
            string domainName;
            var productDic = new Dictionary<string, string>();
            //Change Duration
            //productDic = ChangeYears(hostingType);
            //domainName = CommonSelectDomain();
            //Error Checking
            /*var currentUrl = _driver.Url;
            if (currentUrl.Contains("cart"))
            {
                var checkForError = _driver.FindElement(By.XPath(".//*[@id='content']/div/div[1]")).GetAttribute(UiConstants.AttributeClass);
                if (checkForError.Contains("error"))
                    Assert.Fail(_driver.FindElement(By.XPath(".//*[@id='content']/div/div[1]")).Text);
            }*/
            //if (!(hostingType.Contains(UiConstantHelper.MigrateToNamecheap) || hostingType.Contains(UiConstantHelper.SharedHosting)))
            //    _cart.CartUI_ContinueBtn.Click();
            //Error Checking
            /*var currentUrl = _driver.Url;
            if (currentUrl.Contains("cart"))
            {
                var checkForError = _driver.FindElement(By.XPath(".//*[@id='content']/div/div[1]")).GetAttribute(UiConstants.AttributeClass);
                if (checkForError.Contains("error"))
                    Assert.Fail(_driver.FindElement(By.XPath(".//*[@id='content']/div/div[1]")).Text);
            }*/
            //--------------ChangeYearsInCartForHosting(domainName, productDic);
            /*//Remove Hosting From Cart
            CartPage _cartPage = new CartPage(_driver);
            _cartPage.RemoveHostingFromCart(domainName, Hosting.Plan.Ultimate);*/
        }

        internal List<SortedDictionary<string, string>> ChangeYears(string hostingType)
        {
            var productDic = new SortedDictionary<string, string>();
            var productDicList = new List<SortedDictionary<string, string>>();
            Random random = new Random(DateTime.Now.Millisecond);
            //int productNum = random.Next(1, count + 1);
            if (hostingType.Contains("DedicatedServers"))
            {
            }
            else
            {
                //Products
                string productGridXpath = ".//*[contains(@class,'product-grid')]/div";
                var products = BrowserInit.Driver.FindElements(By.XPath(productGridXpath)).Count;
                int rndProduct = random.Next(1, products + 1);
                var productXpath = productGridXpath + "[" + rndProduct + "]";
                var product = BrowserInit.Driver.FindElement(By.XPath(productXpath));


                var productFieldsetXpath = productXpath + "//fieldset/div";
                var fieldsetDivCount = BrowserInit.Driver.FindElements(By.XPath(productFieldsetXpath)).Count;

                for (int verifyDiv = fieldsetDivCount; verifyDiv >= 1; verifyDiv--)
                {
                    var productFeildsetDivXpath = productFieldsetXpath + "[" + verifyDiv + "]";
                    var productFieldset = BrowserInit.Driver.FindElement(By.XPath(productFeildsetDivXpath));
                    var verifyDivClass = productFieldset.GetAttribute(UiConstantHelper.AttributeClass);
                    if (verifyDivClass.Contains("renewalprice"))
                    {
                        var renewalText = productFieldset.Text;
                        if (renewalText.Equals(string.Empty)) continue;
                        var productRenewal = productFieldset.FindElement(By.XPath("//span[contains(@class,'-display')]")).Text.Trim();
                        var productRenewalPerYear = Regex.Replace(productRenewal, "Renewal price", "").Trim();
                        productDic.Add("RenewalPrice", productRenewalPerYear);
                        continue;
                    }
                    else if (verifyDivClass.Contains("coreuiselect"))
                    {
                        productFieldset.Click();
                        var isOpen = productFieldset.GetAttribute(UiConstantHelper.AttributeClass);
                        if (isOpen.Contains("open"))
                        {
                            var verifyDrpDwnListClass = BrowserInit.Driver.FindElement(By.XPath(productFeildsetDivXpath + "/following-sibling::div[1]")).GetAttribute(UiConstantHelper.AttributeClass);
                            if (verifyDrpDwnListClass.Contains("show"))
                            {
                                var drpDwnListCount = BrowserInit.Driver.FindElements(By.XPath(productFeildsetDivXpath + "/following-sibling::div[1]//li")).Count;
                                var rndSelect = random.Next(1, drpDwnListCount + 1);
                                var productDataTxt = BrowserInit.Driver.FindElement(By.XPath(productFeildsetDivXpath + "/following-sibling::div[1]//li[" + rndSelect + "]")).Text;
                                if (!verifyDivClass.Contains("duration") && verifyDivClass.Contains("coreuiselect"))
                                    productDic.Add("Datacenter", productDataTxt);
                                else
                                    productDic.Add("ProductDuration", productDataTxt);
                                BrowserInit.Driver.FindElement(By.XPath(productFeildsetDivXpath + "/following-sibling::div[1]//li[" + rndSelect + "]")).Click();
                                continue;
                            }
                        }
                    }
                }
                
                //Product header
                var productNamePrice = new List<string>(product.FindElement(By.TagName("header")).Text.Split());
                productNamePrice = productNamePrice.Where(namePrice => !string.IsNullOrWhiteSpace(namePrice)).Distinct().ToList();
                //Product Name & Price
                string productName = product.FindElement(By.TagName("h2")).Text.Trim();
                if (productName.Contains("VPS"))
                {
                }
                productDic.Add("ProductName", productName);
                string productPrice = product.FindElement(By.ClassName("amount")).Text.Trim();
                productDic.Add("ProductPrice", productPrice);
                string productPeriod = product.FindElement(By.TagName("abbr")).Text.Trim();
                productDic.Add("ProductPeriod", "1" + productPeriod);
                //End
                //Product Renewal Price, Years, Datacenter
                
                
                BrowserInit.Driver.FindElement(By.XPath(productXpath + "//button")).Click();
            }
            productDicList.Add(productDic);
            return productDicList;
        }

        internal void ValidateFreeDomainIsCharged(List<SortedDictionary<string, string>> listDicShoppingCartItems)
        {
            //Check the page - Shopping cart

            //Remove Hosting from cart Item
            RemoveHostingFromCart(listDicShoppingCartItems);
        }
        internal void RemoveHostingFromCart(List<SortedDictionary<string, string>> listDicShoppingCartItems)
        {
            var productGroups = PageInitHelper<ShoppingCartPageFactory>.PageInit.ProductGroup;

            foreach (var pGroup in productGroups)
            {
                //var cartItems = pGroup.FindElements(By.TagName(UiConstantHelper.TagDivision));
                var cartItemsXpath = "//*[contains(@class,'product-group')]/div[contains(@class,'cart-item')]";
                var cartItems = BrowserInit.Driver.FindElements(By.XPath(cartItemsXpath));
                string domainName = string.Empty;
                foreach (var cartItem in cartItems)
                {
                    var divClass = cartItem.GetAttribute(UiConstantHelper.AttributeClass);
                    string dName =  string.Empty;
                    if (divClass.Contains("add-on")) continue;
                    if (divClass.Contains("domain-name"))
                    {
                        var domainNameFromCart = cartItem.FindElement(By.TagName(UiConstantHelper.TagDivision)).Text;

                        foreach (var dicShoppingCartItems in listDicShoppingCartItems)
                        {
                            var domainNamefromDic = dicShoppingCartItems[EnumHelper.DomainKeys.DomainName.ToString()].ToLower();
                            
                            if (domainNameFromCart.IndexOf(domainNamefromDic,
                                    StringComparison.InvariantCultureIgnoreCase) >= 0)
                            {
                                domainName = domainNamefromDic;
                                break;
                            }
                        }
                    }
                    if (divClass.Contains("cart-item") && (!divClass.Contains("domain-name") && !divClass.Contains("add-on")))
                    {
                        var productNameFromCart = cartItem.FindElement(By.TagName(UiConstantHelper.TagStrong)).Text;
                        var domainNameFromCart = BrowserInit.Driver.FindElement(By.XPath(cartItemsXpath + "/div")).Text;

                        if (domainNameFromCart.IndexOf(domainName, StringComparison.InvariantCultureIgnoreCase) >= 0)
                        {
                            foreach (var dicShoppingCartItems in listDicShoppingCartItems)
                            {
                                //if (domainNameFromCart.Contains(dicShoppingCartItems[EnumHelper.DicionaryKeys.DomainName.ToString()]))
                                //if (domainNameFromCart.IndexOf(domainName, StringComparison.InvariantCultureIgnoreCase) == 1)
                                //{
                                    var productNamefromDic = dicShoppingCartItems[EnumHelper.HostingKeys.ProductName.ToString()];
                                    if (productNameFromCart.Contains(productNamefromDic))
                                    {
                                        cartItem.FindElement(By.ClassName("remove")).Click();

                                        productGroups = PageInitHelper<ShoppingCartPageFactory>.PageInit.ProductGroup;
                                        foreach (var pGroupAfterHostingRemoved in productGroups)
                                        {
                                        //Change to 3 divs
                                            var cartItemsAfterHostingRemoved = pGroupAfterHostingRemoved.FindElements(By.TagName(UiConstantHelper.TagDivision));

                                            foreach (var cartItemAfterHostingRemoved in cartItemsAfterHostingRemoved)
                                            {
                                                var divClassAfterHostingRemoved = cartItemAfterHostingRemoved.GetAttribute(UiConstantHelper.AttributeClass);
                                                //string domainName;
                                                if (divClassAfterHostingRemoved.Contains("add-on")) continue;
                                                if (divClassAfterHostingRemoved.Contains("domain-name"))
                                                {
                                                    var domainPriceAfterHostingRemoved = decimal.Parse(Regex.Replace(cartItemAfterHostingRemoved.FindElement(By.ClassName("amount")).Text, @"[^\d..][^\w\s]*", "").Trim());
                                                    var domainPriceFromDic = decimal.Parse(dicShoppingCartItems[EnumHelper.DomainKeys.DomainPrice.ToString()]);

                                                    Assert.Greater(domainPriceAfterHostingRemoved, domainPriceFromDic,
                                                        "Domain price is not changed, still the Domain price seems tobe '0.00'. ");
                                                    break;
                                                }
                                            }
                                        }

                                    }
                                //}
                            }
                        }
                    }
                }
            }
        }

        internal void VarifyChangingDataCenterAndYears(List<SortedDictionary<string, string>> dicHostingProduct)
        {
            var cartProductGroupXpath = "//*[contains(@class,'your-cart group')]/*[contains(@class,'product-group')]";
            var cartProductGroupCount1 = BrowserInit.Driver.FindElements(By.XPath(cartProductGroupXpath)).Count;
            for (int cartProductGroup = 1; cartProductGroup <= cartProductGroupCount1; cartProductGroup++)
            {
                var cartItemsXpath = "/*[contains(@class,'cart-item')]";
                var cartItems = BrowserInit.Driver.FindElements(By.XPath(cartProductGroupXpath + "[" + cartProductGroup + "]" + cartItemsXpath)).Count;
                for (int item = 1; item <= cartItems; item++)
                {
                    var itemXpath = cartProductGroupXpath + "[" + cartProductGroup + "]" + cartItemsXpath + "[" + item + "]";
                    var itemClass = BrowserInit.Driver.FindElement(By.XPath(itemXpath)).GetAttribute(UiConstantHelper.AttributeClass);
                    if (itemClass.Contains("domain-name") || itemClass.Contains("add-on"))
                    { continue; }
                    if (itemClass.Contains("cart-item") && !(itemClass.Contains("domain-name") || itemClass.Contains("add-on")))
                    {
                        var productName = BrowserInit.Driver.FindElement(By.XPath(itemXpath + "//strong")).Text.Replace("UPDATE", "").Trim();
                        if (productName.Contains("VPS"))
                        {
                            var vpsProductNo = Regex.Replace(productName, "[^0-9.]", "");
                            var vpsProductName = Regex.Replace(productName, "[0-9.]", "").Trim();
                            productName = vpsProductName.Replace(" ", " " + vpsProductNo + " - ");
                        }
                        //productName;
                        if (productName.Contains(dicHostingProduct[0]["ProductName"]))
                        {
                            var updateCartDurationDrpDwn = BrowserInit.Driver.FindElement(By.XPath(itemXpath + "//div[contains(@class,'updateCartDuration')]"));
                            var durationInCart = updateCartDurationDrpDwn.Text;
                            Assert.True(durationInCart.IndexOf(dicHostingProduct[0]["ProductDuration"], StringComparison.InvariantCultureIgnoreCase) != -1);
                            if (productName.Contains("VPS"))
                            { break; }
                            var updateCartDataCenter = BrowserInit.Driver.FindElements(By.XPath(itemXpath + "/ul/li"));
                            foreach (var cartDataCenter in updateCartDataCenter)
                            {
                                if (cartDataCenter.Text.IndexOf("DataCenter", StringComparison.InvariantCultureIgnoreCase) != -1)
                                {
                                    //var dataCenterInCart = cartDataCenter.Text;
                                    //Assert.AreEqual(dataCenterInCart, productDic["Datacenter"]);
                                    Assert.True(cartDataCenter.Text.IndexOf(dicHostingProduct[0]["Datacenter"], StringComparison.InvariantCultureIgnoreCase) != -1);
                                }
                            }
                        }
                        else
                            continue;
                    }
                }
            }
        }
    }
}
