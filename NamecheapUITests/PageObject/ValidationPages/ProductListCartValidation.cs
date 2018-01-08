using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using NamecheapUITests.PagefactoryObject.ValidationPagefactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NUnit.Framework;
using OpenQA.Selenium;
namespace NamecheapUITests.PageObject.ValidationPages
{
    public class ProductListCartValidation : ICartValidation
    {
        public List<SortedDictionary<string, string>> CartWidgetValidation(List<SortedDictionary<string, string>> domainListValidation)
        {
            var cartBelongsTo =
                PageInitHelper<CartWidgetPageFactory>.PageInit.CartContent.GetAttribute(UiConstantHelper
                    .AttributeId);
            Assert.IsTrue(cartBelongsTo.Contains("productUl"));
            var cartWidgetList = new List<SortedDictionary<string, string>>();
            var cartWidgetDic = new SortedDictionary<string, string>();
            foreach (var cartItem in PageInitHelper<CartWidgetPageFactory>.PageInit.CartWidgetWindowItems.Select((value, i) => new { i, value }))
            {
                var subitemvalue = cartItem.value;
                var subitemindex = cartItem.i + 1;
                var cItemClass = subitemvalue.GetAttribute("class");
                if (!cItemClass.Equals(string.Empty)) continue;
                if (BrowserInit.Driver.FindElement(By.XPath("((.//*[contains(@class,'cart spacer-bottom')]//ul/li[@class='register'] | //ul/li[@class='transfer'] | .//*[@class='cart-widget']/ul/li|(//ul/li[not(@class)]/strong[contains(@class,'product')]/..)| .//*[@class='cart-widget']/ul/li)[not(contains(@class,'subtotal'))][not(contains(@class,'more'))])[" + subitemindex + "]/.."))
                   .GetAttribute(UiConstantHelper.AttributeClass)
                        .Contains("WhoisGuard"))
                {
                    var pCount = subitemvalue.FindElements(By.TagName("p"));
                    foreach (var spanClass in from itemTxt in pCount let pClassText = itemTxt.GetAttribute(UiConstantHelper.AttributeClass) where pClassText.Equals(string.Empty) select itemTxt.FindElements(By.TagName("span")) into spanCount from spanClass in spanCount select spanClass)
                    {
                        if (spanClass.GetAttribute(UiConstantHelper.AttributeClass).Contains("item"))
                        {
                            cartWidgetDic.Add(EnumHelper.ShoppingCartKeys.WhoisGuardForDomainDuration.ToString(),
                                spanClass.Text.Replace("subscription", "").Trim());
                        }
                        else if (spanClass.GetAttribute(UiConstantHelper.AttributeClass)
                            .Contains("price"))
                        {
                            var priceText = spanClass.Text.Equals("FREE")
                                ? "0.00"
                                : Regex.Replace(spanClass.Text, @"[^\d..][^\w\s]*", "");
                            cartWidgetDic.Add(EnumHelper.ShoppingCartKeys.WhoisGuardForDomainPrice.ToString(),
                                priceText.Trim());
                        }
                    }
                }
                else
                {
                    var strongClassName = subitemvalue.FindElement(By.TagName("strong"))
                        .GetAttribute(UiConstantHelper.AttributeClass);
                    var productName = subitemvalue.FindElement(By.TagName("strong")).Text;
                    if (strongClassName.Equals("productname".Trim()) && !productName.Contains("."))
                    {
                        if (subitemvalue.Text.Contains("Xeon"))
                        {
                            var dServers = Regex.Split(subitemvalue.Text, "(,)")[0];
                            var dedicatedServers = dServers.Split(' ')[0] + " " + dServers.Split(' ')[1] + " " +
                                                      dServers.Split(' ')[2];
                            cartWidgetDic.Add(EnumHelper.HostingKeys.ProductName.ToString(), dedicatedServers);
                        }
                        else
                        {
                            cartWidgetDic.Add(EnumHelper.HostingKeys.ProductName.ToString(),
                                subitemvalue.FindElement(By.TagName("strong")).Text);
                        }
                        var pCount = subitemvalue.FindElements(By.TagName("p"));
                        foreach (var itemTxt in pCount)
                        {
                            var pClassText = itemTxt.GetAttribute(UiConstantHelper.AttributeClass);
                            if (pClassText.Equals(string.Empty))
                            {
                                var spanCount = itemTxt.FindElements(By.TagName("span"));
                                foreach (var spanClass in spanCount)
                                {
                                    if (spanClass.GetAttribute(UiConstantHelper.AttributeClass).Contains("item"))
                                    {
                                        cartWidgetDic.Add(EnumHelper.HostingKeys.ProductDuration.ToString(),
                                            spanClass.Text.Replace("subscription", "").Trim());
                                    }
                                    else if (spanClass.GetAttribute(UiConstantHelper.AttributeClass)
                                        .Contains("price"))
                                    {
                                        cartWidgetDic.Add(EnumHelper.HostingKeys.ProductPrice.ToString(),
                                            Regex.Replace(spanClass.Text, @"[^\d..][^\w\s]*", "").Trim());
                                    }
                                }
                            }
                            if (!pClassText.EndsWith("addon")) continue;
                            {
                                var spanCount = itemTxt.FindElements(By.TagName("span"));
                                var associatedDicKey = "";
                                var associatedDicValue = "";
                                for (var i = 0; i < spanCount.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        string associatedDicKeyTxt = spanCount[i].Text;
                                        associatedDicKey = associatedDicKeyTxt.Contains("transfer")
                                            ? associatedDicKeyTxt.Replace("transfer", "").Trim()
                                            : associatedDicKeyTxt;
                                    }
                                    else
                                    {
                                        associatedDicValue = spanCount[i].Text;
                                    }
                                }
                                var priceText = associatedDicValue.Equals("FREE")
                                    ? "0.00"
                                    : Regex.Replace(associatedDicValue, @"[^\d..][^\w\s]*", "");
                                cartWidgetDic.Add(associatedDicKey.Trim(), priceText.Trim());
                            }
                        }
                    }
                    else if (strongClassName.Contains("productname domain".Trim()) || productName.Contains("."))
                    {
                        cartWidgetDic.Add(EnumHelper.HostingKeys.ProductDomainName.ToString(),
                            subitemvalue.FindElement(By.TagName("strong")).Text);
                        var pCount = subitemvalue.FindElements(By.TagName("p"));
                        foreach (var itemTxt in pCount)
                        {
                            var spanCount = itemTxt.FindElements(By.TagName("span"));
                            foreach (var spanClass in spanCount.Select((value, i) => new { i, value }))
                            {
                                if (spanClass.value.GetAttribute(UiConstantHelper.AttributeClass)
                                    .Contains("item") &&
                                    spanClass.value.Text != "ICANN fee")
                                {
                                    cartWidgetDic.Add(EnumHelper.HostingKeys.ProductDomainDuration.ToString(),
                                        spanClass.value.Text.Replace("registration", string.Empty).Replace("transfer", string.Empty).Trim());
                                }
                                else if (spanClass.value.GetAttribute(UiConstantHelper.AttributeClass)
                                    .Contains("price") && pCount.IndexOf(itemTxt) != 1)
                                {
                                    cartWidgetDic.Add(EnumHelper.HostingKeys.ProductDomainPrice.ToString(),
                                        decimal.Parse(Regex.Replace(spanClass.value.Text, @"[^\d..][^\w\s]*", "").Trim()).ToString(CultureInfo.InvariantCulture));
                                }
                                else if (spanClass.i.ToString() == "1")
                                {
                                    if (spanClass.value.GetAttribute(UiConstantHelper.AttributeClass)
                                        .Contains("price"))
                                    {
                                        cartWidgetDic.Add(EnumHelper.CartWidget.IcanPrice.ToString(),
                                            decimal.Parse(Regex.Replace(spanClass.value.Text, @"[^\d..][^\w\s]*", "").Trim()).ToString(CultureInfo.InvariantCulture));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            var subTotal = cartWidgetDic.Where(dicCartWidgetItem => dicCartWidgetItem.Value.Contains(".") && !dicCartWidgetItem.Key.Equals(EnumHelper.HostingKeys.ProductDomainName.ToString()) && !dicCartWidgetItem.Key.Equals(EnumHelper.HostingKeys.ProductName.ToString())).Aggregate(0.0m, (current, dicCartWidgetItem) => current + decimal.Parse(dicCartWidgetItem.Value));
            cartWidgetDic.Add(EnumHelper.CartWidget.SubTotal.ToString(), subTotal.ToString(CultureInfo.InvariantCulture));
            cartWidgetList.Add(cartWidgetDic);
            var widgetSubTotal =
            decimal.Parse(Regex.Replace(
                PageInitHelper<CartWidgetPageFactory>.PageInit.CartWidgetSubTotal.Text,
                @"[^\d..][^\w\s]*", ""));
            var dictWithKey =
                cartWidgetList.First(d => d.ContainsKey(EnumHelper.CartWidget.SubTotal.ToString()));
            var dicSubTotalValue =
                decimal.Parse(
                    Regex.Replace(dictWithKey[EnumHelper.CartWidget.SubTotal.ToString()], "\"[^\"]*\"", "")
                        .Trim());
            Assert.IsTrue(dicSubTotalValue.Equals(widgetSubTotal), "Sub total is miss matching with widget sub total");
            AVerify verifingTwoListOfDic = new VerifyData();
            verifingTwoListOfDic.VerifyTwoListOfDic(domainListValidation, cartWidgetList);
            AMerge mergeTWoListOfDic = new MergeData();
            cartWidgetList = mergeTWoListOfDic.MergingTwoListOfDic(domainListValidation, cartWidgetList);
            PageInitHelper<CartWidgetPageFactory>.PageInit.ViewCartButton.Click();
            if (!PageInitHelper<CartWidgetPageFactory>.PageInit.ShoppingCartHeadingTxt.Text.Trim().Equals("Shopping Cart"))
            {
                PageInitHelper<CartWidgetPageFactory>.PageInit.ViewCartButton.Click();
            }
            return cartWidgetList;
        }
        public SortedDictionary<string, string> CartWidgetValidation(SortedDictionary<string, string> domainListValidation)
        {
            var cartContent = PageInitHelper<CartWidgetPageFactory>.PageInit.CartContent;
            PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(cartContent);
            var cartBelongsTo = cartContent.GetAttribute(UiConstantHelper.AttributeId);
            Assert.IsTrue(cartBelongsTo.Contains("productUl"));
            var cartWidgetDic = CartWidgetWindowItems();

            var subTotal = cartWidgetDic.Where(dicCartWidgetItem => dicCartWidgetItem.Value.Contains(".") && !dicCartWidgetItem.Key.Equals(EnumHelper.HostingKeys.ProductDomainName.ToString()) && !dicCartWidgetItem.Key.Equals(EnumHelper.HostingKeys.ProductName.ToString())).Aggregate(0.0m, (current, dicCartWidgetItem) => current + decimal.Parse(dicCartWidgetItem.Value));
            cartWidgetDic.Add(EnumHelper.CartWidget.SubTotal.ToString(), subTotal.ToString(CultureInfo.InvariantCulture));
            //var widgetSubTotal = decimal.Parse(Regex.Replace(PageInitHelper<CartWidgetPageFactory>.PageInit.CartWidgetSubTotal.Text, @"[^\d..][^\w\s]*", ""));
            var widgetSubTotal = PageInitHelper<DataGenerator>.PageInit.GetPrice(PageInitHelper<CartWidgetPageFactory>.PageInit.CartWidgetSubTotal.Text);
            var dictWithKey = cartWidgetDic.ContainsKey(EnumHelper.CartWidget.SubTotal.ToString());
            //var dicSubTotalValue = decimal.Parse(Regex.Replace(cartWidgetDic[DicKeyEnumHelper.CartWidget.SubTotal.ToString()], "\"[^\"]*\"", "").Trim());
            var dicSubTotalValue = PageInitHelper<DataGenerator>.PageInit.GetPrice(cartWidgetDic[EnumHelper.CartWidget.SubTotal.ToString()]);
            Assert.IsTrue(dicSubTotalValue.Equals(widgetSubTotal), "Sub total is miss matching with widget sub total");
            //AVerifyAndMergeList verifingTwoListOfDic = new VerifyAndMergingTwoList();
            //verifingTwoListOfDic.VerifyTwoListOfDic(domainListValidation, cartWidgetDic);
            //AVerifyAndMergeList mergeTwoListOfDic = new VerifyAndMergingTwoList();
            //cartWidgetList = mergeTwoListOfDic.MergingTwoListOfDic(domainListValidation, cartWidgetList);
            //PageInitHelper<CartWidgetPageFactory>.PageInit.ViewCartButton.Click();
            var div = new SortedDictionary<string, string>();
            //if (!PageInitHelper<CartWidgetPageFactory>.PageInit.ShoppingCartHeadingTxt.Text.Trim().Equals("Shopping Cart"))
            //{
            //    PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(PageInitHelper<CartWidgetPageFactory>.PageInit.ViewCartButton);
            //    //Get all datas
            //    var cartWidgetItemsXpath = "//*[contains(@class,'side-cart')]/ul/li[not(contains(@class,'more'))][not(contains(@class,'subtotal'))]";
            //    var cartWidgetSubtotalXpath = "//*[contains(@class,'side-cart')]/ul/li[not(contains(@class,'more'))][contains(@class,'subtotal')]";

            //    var cartWidgetItems = BrowserInit.Driver.FindElements(By.XPath(cartWidgetItemsXpath));
            //    var listCartWidgetItems = new List<SortedDictionary<string, string>>();
            //    var dicCartWidgetItems = new SortedDictionary<string, string>();
            //    foreach (var cartWidgetItem in cartWidgetItems)
            //    {
            //        var productName = cartWidgetItem.FindElement(By.TagName(UiConstantHelper.TagStrong)).Text;
            //        var domainName = cartWidgetItem.FindElement(By.TagName("em")).Text;

            //        var addons = cartWidgetItem.FindElements(By.TagName(UiConstantHelper.TagParagraph));

            //        foreach (var addon in addons)
            //        {
            //            var addonText = addon.Text;
            //            var id = addon.GetAttribute("id");

            //            var spans = addon.FindElements(By.TagName(UiConstantHelper.TagSpan));

            //            foreach (var span in spans)
            //            {
            //                var spanClass = span.GetAttribute(UiConstantHelper.AttributeClass);
            //                if (spanClass.Contains("item"))
            //                {
            //                    if (span.Text.Contains("subscription"))
            //                    {
            //                        var spanText = span.Text.Replace("subscription", "");
            //                        var letters = Regex.Replace(spanText, "[0-9.]", "").Trim();
            //                        spanText = Regex.Replace(spanText, @"[^\d..][^\w\s]*", "").Trim() + " " + letters.First().ToString().ToUpper() + letters.Substring(1);
            //                        cartWidgetDic.Add(DicKeyEnumHelper.HostingKeys.ProductDuration.ToString(), spanText);
            //                    }

            //                }
            //            }

            //            if (addonText.Contains("subscription"))
            //            {

            //            }

            //            if (id.Contains("ram"))
            //            {

            //            }
            //            if (id.Contains("hdd"))
            //            {

            //            }
            //            if (id.Contains("port"))
            //            {

            //            }
            //            if (id.Contains("operatingsystem") || id.Contains("ds"))
            //            {

            //            }
            //            if (id.Contains("management"))
            //            {

            //            }
            //            if (id.Contains("bandwith"))
            //            {

            //            }
            //            if (id.Contains("ip"))
            //            {

            //            }
            //        }    
            //    }
            //    PageInitHelper<CartWidgetPageFactory>.PageInit.ViewCartButton.Click();
            //}
            return cartWidgetDic;
        }

        private SortedDictionary<string, string> CartWidgetWindowItems()
        {
            var cartWidgetDic = new SortedDictionary<string, string>();
            foreach (var cartItem in PageInitHelper<CartWidgetPageFactory>.PageInit.CartWidgetWindowItems.Select((value, i) => new { i, value }))
            {
                var subitemvalue = cartItem.value;
                var subitemindex = cartItem.i + 1;
                var cItemClass = subitemvalue.GetAttribute("class");
                if (!cItemClass.Equals(string.Empty)) continue;

                var xpath = "((.//*[contains(@class,'cart spacer-bottom')]//ul/li[@class='register'] | //ul/li[@class='transfer'] | .//*[@class='cart-widget']/ul/li|(//ul/li[not(@class)]/strong[contains(@class,'product')]/..)| .//*[@class='cart-widget']/ul/li)[not(contains(@class,'subtotal'))][not(contains(@class,'more'))])[" + subitemindex + "]/..";

                if (BrowserInit.Driver.FindElement(By.XPath(xpath)).GetAttribute(UiConstantHelper.AttributeClass).Contains("WhoisGuard"))
                {
                    var pCount = subitemvalue.FindElements(By.TagName(UiConstantHelper.TagParagraph));
                    foreach (var spanClass in from itemTxt in pCount let pClassText = itemTxt.GetAttribute(UiConstantHelper.AttributeClass) where pClassText.Equals(string.Empty) select itemTxt.FindElements(By.TagName(UiConstantHelper.TagSpan)) into spanCount from spanClass in spanCount select spanClass)
                    {
                        if (spanClass.GetAttribute(UiConstantHelper.AttributeClass).Contains("item"))
                            cartWidgetDic.Add(EnumHelper.ShoppingCartKeys.WhoisGuardForDomainDuration.ToString(), spanClass.Text.Replace("subscription", "").Trim());
                        else if (spanClass.GetAttribute(UiConstantHelper.AttributeClass).Contains("price"))
                        {
                            var priceText = spanClass.Text.Equals("FREE")
                                ? "0.00"
                                : PageInitHelper<DataGenerator>.PageInit.GetPrice(spanClass.Text);
                            //var priceText = spanClass.Text.Equals("FREE") ? "0.00" : Regex.Replace(spanClass.Text, @"[^\d..][^\w\s]*", "");
                            cartWidgetDic.Add(EnumHelper.ShoppingCartKeys.WhoisGuardForDomainPrice.ToString(), priceText.Trim());
                        }
                    }
                }
                else
                {
                    var strongClassName = subitemvalue.FindElement(By.TagName(UiConstantHelper.TagStrong)).GetAttribute(UiConstantHelper.AttributeClass);
                    var productName = subitemvalue.FindElement(By.TagName(UiConstantHelper.TagStrong)).Text;
                    if (strongClassName.Equals("productname".Trim()) && !productName.Contains("."))
                    {
                        if (productName.Contains("Xeon"))
                        {
                            char[] delimeter = { '(', ')' };
                            string[] splitedName = productName.Split(delimeter).Where(x => !string.IsNullOrEmpty(x)).ToArray();

                            cartWidgetDic.Add(EnumHelper.HostingKeys.ProductName.ToString(), splitedName[0].Trim());

                            if (splitedName[1].Contains("RAM") || splitedName[1].Contains("SATA") || splitedName[1].Contains("SSD"))
                            {
                                var configuration = splitedName[1].Split(',');

                                foreach (var config in configuration)
                                {
                                    if (config.Contains("RAM"))
                                    {
                                        cartWidgetDic.Add("RAM", config.Trim());
                                        continue;
                                    }
                                    if (config.Contains("SATA"))
                                        cartWidgetDic.Add("SATA", config.Trim());
                                    if (config.Contains("SSD"))
                                        cartWidgetDic.Add("SSD", config.Trim());
                                }
                            }
                            /*
                            var domainName = subitemvalue.FindElement(By.ClassName("fordomain")).Text;
                            var dServersd1 = Regex.Split(productName, "(,)")[0];
                            var dServersds = Regex.Split(productName, "\r\n")[0];
                            var dServers1 = Regex.Split(subitemvalue.Text, "(,)")[0];
                            var dServerss = Regex.Split(subitemvalue.Text, "\r\n")[0];
                            var dedicatedServers = dServers1.Split(' ')[0] + " " + dServers1.Split(' ')[1] + " " + dServers1.Split(' ')[2];
                            var dedicatedServers1 = dServerss.Replace(dedicatedServers, "").Trim();
                            var serverConfig = dedicatedServers1.Replace(dedicatedServers1.First().ToString(), "").Replace(dedicatedServers1.Last().ToString(), "");
                            var dServers = Regex.Split(subitemvalue.Text, "(,)")[0];
                            cartWidgetDic.Add(DicKeyEnumHelper.HostingKeys.ProductName.ToString(), dedicatedServers);
                            */
                        }
                        else
                            cartWidgetDic.Add(EnumHelper.HostingKeys.ProductName.ToString(), subitemvalue.FindElement(By.TagName(UiConstantHelper.TagStrong)).Text);
                        var pCount = subitemvalue.FindElements(By.TagName(UiConstantHelper.TagParagraph));
                        foreach (var itemTxt in pCount)
                        {
                            var pClassText = itemTxt.GetAttribute(UiConstantHelper.AttributeClass);
                            if (pClassText.Equals(string.Empty))
                            {
                                var spanCount = itemTxt.FindElements(By.TagName(UiConstantHelper.TagSpan));
                                foreach (var spanClass in spanCount)
                                {
                                    if (spanClass.GetAttribute(UiConstantHelper.AttributeClass).Contains("item"))
                                    {
                                        var pDuration = spanClass.Text.Replace("subscription", "").Trim();
                                        cartWidgetDic.Add(EnumHelper.HostingKeys.ProductDuration.ToString(), pDuration.Contains("month") ? "1 Month" : "1 Year");
                                    }
                                    else if (spanClass.GetAttribute(UiConstantHelper.AttributeClass)
                                        .Contains("price"))
                                    {
                                        cartWidgetDic.Add(EnumHelper.HostingKeys.ProductPrice.ToString(), PageInitHelper<DataGenerator>.PageInit.GetPrice(spanClass.Text));
                                    }
                                }
                            }
                            if (!pClassText.EndsWith("addon")) continue;
                            {
                                var spanCount = itemTxt.FindElements(By.TagName(UiConstantHelper.TagSpan));
                                var associatedDicKey = "";
                                var associatedDicValue = "";
                                for (var i = 0; i < spanCount.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        string associatedDicKeyTxt = spanCount[i].Text;
                                        associatedDicKey = associatedDicKeyTxt.Contains("transfer")
                                            ? associatedDicKeyTxt.Replace("transfer", "").Trim()
                                            : associatedDicKeyTxt;
                                    }
                                    else
                                        associatedDicValue = spanCount[i].Text;
                                }
                                var priceText = associatedDicValue.Equals("FREE")
                                    ? "0.00"
                                    : Regex.Replace(associatedDicValue, @"[^\d..][^\w\s]*", "");
                                cartWidgetDic.Add(associatedDicKey.Trim(), priceText.Trim());
                            }
                        }
                    }
                    else if (strongClassName.Contains("productname domain".Trim()) || productName.Contains("."))
                    {
                        cartWidgetDic.Add(EnumHelper.HostingKeys.ProductDomainName.ToString(),
                            subitemvalue.FindElement(By.TagName(UiConstantHelper.TagStrong)).Text);
                        var pCount = subitemvalue.FindElements(By.TagName(UiConstantHelper.TagParagraph));
                        foreach (var itemTxt in pCount)
                        {
                            var spanCount = itemTxt.FindElements(By.TagName(UiConstantHelper.TagSpan));
                            foreach (var spanClass in spanCount.Select((value, i) => new { i, value }))
                            {
                                if (spanClass.value.GetAttribute(UiConstantHelper.AttributeClass)
                                    .Contains("item") &&
                                    spanClass.value.Text != "ICANN fee")
                                {
                                    cartWidgetDic.Add(EnumHelper.HostingKeys.ProductDomainDuration.ToString(),
                                        spanClass.value.Text.Replace("registration", string.Empty).Replace("transfer", string.Empty).Trim());
                                }
                                else if (spanClass.value.GetAttribute(UiConstantHelper.AttributeClass)
                                    .Contains("price") && pCount.IndexOf(itemTxt) != 1)
                                {
                                    cartWidgetDic.Add(EnumHelper.HostingKeys.ProductDomainPrice.ToString(),
                                        decimal.Parse(Regex.Replace(spanClass.value.Text, @"[^\d..][^\w\s]*", "").Trim()).ToString(CultureInfo.InvariantCulture));
                                }
                                else if (spanClass.i.ToString() == "1")
                                {
                                    if (spanClass.value.GetAttribute(UiConstantHelper.AttributeClass)
                                        .Contains("price"))
                                    {
                                        cartWidgetDic.Add(EnumHelper.CartWidget.IcanPrice.ToString(),
                                            decimal.Parse(Regex.Replace(spanClass.value.Text, @"[^\d..][^\w\s]*", "").Trim()).ToString(CultureInfo.InvariantCulture));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return cartWidgetDic;
        }

    }
}