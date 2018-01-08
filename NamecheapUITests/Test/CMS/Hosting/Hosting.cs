using System;
using NamecheapUITests.PageObject.CMSPages.DomainsPage;
using NamecheapUITests.PageObject.CMSPages.HostingPage;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.PaymentProcess;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;
using NamecheapUITests.PageObject.ValidationPages;
using NUnit.Framework;

namespace NamecheapUITests.Test.CMS.Hosting
{
    [TestFixture]
    class Hosting
    {
        [Test, Category("Hosting")]
        [TestCase("Shared Hosting")]
        [TestCase("WordPress Hosting")]
        [TestCase("Reseller Hosting")]
        [TestCase("VPS Hosting")]
        [TestCase("Dedicated Servers")]
        [TestCase("Private Email Hosting")]
        [TestCase("Migrate to Namecheap")]
        public void PurchaseHosting(string hostingType)
        {
            try
            {
                //Adding Domain to cart 
                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Domains, UiConstantHelper.DomainNameSearch);
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(UiConstantHelper.DomainNameSearchPageTitle.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + UiConstantHelper.DomainNameSearchPageTitle);
                //Add domain
                var newDomainNames = PageInitHelper<RandomDomainNameGenerator>.PageInit.DomainName();
                var searchResultDomainsList = PageInitHelper<DomainsPage>.PageInit.AddingDomainNamesToCart("ReDomains", newDomainNames);

                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Hosting, hostingType); //UiConstantHelper.DomainNameSearch
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(hostingType.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + hostingType);
                var dicHostingProduct = PageInitHelper<HostingPage>.PageInit.SelectHostingProduct(hostingType);
                //Select domain
                var listDicHostingProduct = PageInitHelper<DomainSelectionPage>.PageInit.DomainNamesForHosting(hostingType, dicHostingProduct[0], searchResultDomainsList[0]);
                //Validate Domain & Product info in Shopping Cart
                IShoppingCartValidation cartValidation = new AddProductShoppingCartItems();
                var listDicFromCart = PageInitHelper<AddProductShoppingCartItems>.PageInit.AddShoppingCartItemsToDic(listDicHostingProduct, "", "");

                var mergedOrderNumbertoScWidgetList = PageInitHelper<PurchaseFlow>.PageInit.PurchasingNcProducts(listDicFromCart);
                var namespaceName = GetType().Namespace;
                PageInitHelper<TestFinalizerHelper>.PageInit.Testclosure(namespaceName);
                IDomainListValidation domainListValidation = new ValidateProductsInDomainList();
                domainListValidation.DomainListValidation(mergedOrderNumbertoScWidgetList);
                IOrdersValidation orderHistory = new ValidateDomainOrderInBillingPage();
                orderHistory.VerifyPurchasedOrderInBillingHistoryPage(mergedOrderNumbertoScWidgetList);
                ITransactionValidation transactionHistory = new ValidatingDomainTransationDetails();
                transactionHistory.VerifyPurchasedTransactionDetailsInBillingTransactionPage(mergedOrderNumbertoScWidgetList);
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

        [Test, Category("Hosting")]
        [TestCase("Shared Hosting")]
        [TestCase("Reseller Hosting")]
        [TestCase("VPS Hosting")]
        [TestCase("Dedicated Servers")]
        [TestCase("Migrate to Namecheap")]
        public void IsFreeDomainIsCharged(string hostingType)
        {
            try
            {
                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Hosting, hostingType); //UiConstantHelper.DomainNameSearch
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(hostingType.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + hostingType);
                var dicHostingProduct = PageInitHelper<HostingPage>.PageInit.SelectHostingProduct(hostingType);
                //Select domain
                 var listDicHostingProduct = PageInitHelper<DomainSelectionPage>.PageInit.DomainNamesForHosting(hostingType, dicHostingProduct[0], null, UiConstantHelper.FreeDomain);

                AMerge mergeTWoListOfDic = new MergeData();
                var mergedListDic = mergeTWoListOfDic.MergingTwoListOfDic(dicHostingProduct,listDicHostingProduct);

                //ICartValidation cartValidation = new ProductListCartValidation();
                //var dicWidgetValidation = cartValidation.CartWidgetValidation(mergedListDic);
                
                //Remove Hosting from Cart Items
                PageInitHelper<HostingPage>.PageInit.ValidateFreeDomainIsCharged(mergedListDic);

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

        [Test, Category("Hosting")]
        [TestCase("Shared Hosting")]
        [TestCase("Reseller Hosting")]
        [TestCase("VPS Hosting")]
        public void ChangingDataCenterAndYears(string hostingType)
        {
            try
            {
                PageInitHelper<PageNavigationHelper>.PageInit.NavigationTo(UiConstantHelper.Hosting, hostingType);
                Assert.IsTrue(PageInitHelper<PageValidationHelper>.PageInit.TitleIsAt(hostingType.Trim()), "The Page Redirect to some other page - " + BrowserInit.Driver.Title + " instead of " + hostingType);

                var dicHostingProduct = PageInitHelper<HostingPage>.PageInit.ChangeYears(hostingType);

                //var dicHostingProduct = PageInitHelper<HostingPage>.PageInit.SelectHostingProduct(hostingType, "YDC");

                var listDicHostingProduct = PageInitHelper<DomainSelectionPage>.PageInit.DomainNamesForHosting(hostingType, dicHostingProduct[0], null, UiConstantHelper.ShuffleYn);

                AMerge mergeTWoListOfDic = new MergeData();
                var mergedListDic = mergeTWoListOfDic.MergingTwoListOfDic(dicHostingProduct, listDicHostingProduct);

                //ICartValidation cartValidation = new ProductListCartValidation();
                //var dicWidgetValidation = cartValidation.CartWidgetValidation(mergedListDic);

                //var dicHostingChangeYers = 
                    PageInitHelper<HostingPage>.PageInit.VarifyChangingDataCenterAndYears(mergedListDic);
                //var listDicHostingProduct = PageInitHelper<HostingPage>.PageInit.ChangeDataCenterAndYears(hostingType, dicHostingProduct, null, UiConstantHelper.FreeDomain);
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

        [TearDown]
        public void TearDown()
        {
            PageInitHelper<ClearCart>.PageInit.ClearCartItems();
        }
    }
}
