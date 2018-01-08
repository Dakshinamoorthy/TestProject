using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NUnit.Framework;

namespace NamecheapUITests.PageObject.HelperPages
{
    public class VerifyData : AVerify
    {
        public override void VerifyTwoListOfDic(List<SortedDictionary<string, string>> newList,
           List<SortedDictionary<string, string>> listToBeVerified)
        {
            for (var i = 0; i < newList.Count; i++)
            {
                var searchKeys = listToBeVerified[i].Keys.ToList();
                var widgetKeys = newList[i].Keys.ToList();
                char[] whitespace = { ' ', '\t' };
                if (newList[i].Keys.ToList().Contains("ProductName"))
                {
                    IList<string> ssizes = newList[i].Keys.ToList().Contains("ProductName").ToString().Split(whitespace);
                    var truecount = ssizes.Select(
                        pdetails => listToBeVerified[i].Keys.ToList().Contains("ProductName").ToString()
                            .Contains(pdetails)).Count(istrue => istrue);
                    Assert.IsTrue(truecount.Equals(ssizes.Count),
                        "In Shopping Cart Page Shopping cart Product Name in shopping cart dic & product name in Cart widget Dic are not equal In shopping cart dic product name shown as " + listToBeVerified[i].Keys.ToList().Contains("ProductName") + ", but in cart widget product name shown as " + newList[i].Keys.ToString().Contains("Product Name"));
                    Thread.Sleep(2000);
                    /*
                    Assert.IsTrue(
                        widgetKeys.Where(x => newList[i].Keys.ToList().Contains("ProductName").ToString() !=
                                              "ProductName")
                            .SequenceEqual(searchKeys.Where(
                                x =>
                                    listToBeVerified[i].Keys.ToList().Contains("ProductName").ToString() !=
                                    "ProductName")),
                        "In Shopping Cart Page Shopping cart Product Dic Values & Cart Widget Dic Values are not equal expected as " + widgetKeys + ", But actucal Result in shopping cart page as " + searchKeys);
                        */
                }
                if (newList[i].Keys.ToList().Contains(EnumHelper.Ssl.CertificateName.ToString()))
                {
                    foreach (var skey in listToBeVerified)
                    {
                        foreach (var wkey in newList)
                        {
                            var shoppingcartCertificateName =
                               skey[EnumHelper.Ssl.CertificateName.ToString()];
                            var cartwidgetcertificateName =
                               wkey[EnumHelper.Ssl.CertificateName.ToString()];
                            foreach (var wKfey in newList[i].Keys.ToList())
                            {
                                foreach (var sKfey in listToBeVerified[i].Keys.ToList())
                                {
                                    if (!wKfey.Equals(sKfey)) continue;
                                    var wKeyValue = listToBeVerified[i][wKfey];
                                    var sKeyValue = newList[i][sKfey];
                                    var j = 0;
                                    string fortext;
                                    Label:
                                    if (Regex.Replace(
                                        Regex.Replace(shoppingcartCertificateName, " ",
                                            string.Empty), @"[^\w\d]", string.Empty).Contains(Regex.Replace(Regex.Replace(Regex.Replace(cartwidgetcertificateName, " ",
                                                string.Empty), @"[^\w\d]", string.Empty), "Comodo", string.Empty)))
                                    {
                                        if (wKfey.Equals(EnumHelper.Ssl.CertificateName.ToString()) || sKfey.Equals(EnumHelper.Ssl.CertificateName.ToString()))
                                        {
                                            continue;
                                        }
                                        if (wKeyValue.Equals(sKeyValue, StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            fortext = newList[i].Values.ToList()[j];
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        j = j + 1;
                                        goto Label;
                                    }
                                    Assert.IsTrue(wKeyValue.Equals(sKeyValue, StringComparison.InvariantCultureIgnoreCase),
                                        " In " + BrowserInit.Driver.Title + " page for item " + fortext +
                                        wKfey + " : value - " + wKeyValue + " and " + sKfey + " :  value - " + sKeyValue +
                                        " are mismatching");
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    searchKeys = listToBeVerified[i].Keys.ToList();
                    widgetKeys = newList[i].Keys.ToList();
                    foreach (var wKey in widgetKeys.ToList())
                    {
                        foreach (var sKey in searchKeys.ToList())
                        {
                            if (!wKey.Equals(sKey)) continue;
                            var wKeyValue = listToBeVerified[i][wKey];
                            var sKeyValue = newList[i][sKey];
                            var j = 0;
                            string fortext;
                            Label:
                            if (newList[i].Keys.ToList()[j].Equals(EnumHelper.DomainKeys.DomainName.ToString()) || newList[i].Keys.ToList()[j].Equals(EnumHelper.HostingKeys.ProductDomainName.ToString()))
                            {
                                if (wKeyValue.Equals(sKeyValue, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    fortext = newList[i].Values.ToList()[j];
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                j = j + 1;
                                goto Label;
                            }
                            Assert.IsTrue(wKeyValue.Equals(sKeyValue, StringComparison.InvariantCultureIgnoreCase),
                                " In " + BrowserInit.Driver.Title + " page for item " + fortext +
                                wKey + " : value - " + wKeyValue + " and " + sKey + " :  value - " + sKeyValue +
                                " are mismatching");
                            break;
                        }
                    }
                }
            }
        }
    
}
}
