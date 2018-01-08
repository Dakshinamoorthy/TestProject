using System;
using System.Net;
using System.Text.RegularExpressions;
using Gallio.Framework;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.WebPageValidationPageFactory;
using NamecheapUITests.PageObject.HelperPages;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NUnit.Framework;
namespace NamecheapUITests.PageObject.CMSPages.SupportPage
{
    public class WebPageValidationPage
    {
        // IWait<IWebDriver> wait = new WebDriverWait(BrowserInit.Driver, TimeSpan.FromSeconds(200.00));
        private T ParseEnum<T>(string domainMenuCategory)
        {
            return (T)Enum.Parse(typeof(T), domainMenuCategory, true);
        }
        public void VerifyHomepageWebResponse()
        {
            if (BrowserInit.Driver.Url.Equals(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", string.Empty), StringComparison.InvariantCultureIgnoreCase))
                Assert.IsTrue(BrowserInit.Driver.Title.Contains(UiConstantHelper.Dashboard.Trim()), "Page Title Mis-Match" + BrowserInit.Driver.Title);
            else
                throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
        }
        protected void VerifyimageActive(string uiType)
        {
            WebRequest request = null;
            if (BrowserInit.Driver.Url.Contains("sandbox"))
            {
                request = uiType.Contains("ReactUi") ? WebResponseStatus(request, uiType) : WebResponseStatus(request, uiType = "OldUi");
            }
            else
                request = WebResponseStatus(request, uiType);
            using (var webResponseStatus = (HttpWebResponse)WebResponseStatus(request, uiType).GetResponse())
            {
                if (webResponseStatus.StatusCode != (HttpStatusCode)200)
                    InvalidImageCount++;
            }
        }

        public int InvalidImageCount { get; private set; }

        internal WebRequest WebResponseStatus(WebRequest request, string uiType)
        {
            switch (ParseEnum<EnumHelper.UiType>(uiType))
            {
                case EnumHelper.UiType.NewUi:
                    request = (HttpWebRequest)WebRequest.Create(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerNewUiPage.GetAttribute("src"));
                    break;
                case EnumHelper.UiType.OldUi:
                    string str = PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerOldUiPage.GetCssValue("background-image");
                    if (str.Contains(".jpg") || str.Contains(".png"))
                    {
                        request = (HttpWebRequest)WebRequest.Create(str.Substring(str.IndexOf("http", StringComparison.Ordinal) + 0, str.IndexOf(".jpg", StringComparison.Ordinal) - str.IndexOf('(') + 2));
                    }
                    else
                        throw new InconclusiveException(" The Backgroud Hero Image Is Not In Image Format");
                    break;
                case EnumHelper.UiType.ReactUi:
                    request = (HttpWebRequest)WebRequest.Create(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerReactUiPage.GetAttribute("src"));
                    break;
            }
            return request;
        }
        internal void VerifyDomainsPageWebResponseandElements(string domainMenuCategory)
        {
            switch (ParseEnum<EnumHelper.Domains>(domainMenuCategory))
            {
                case EnumHelper.Domains.DomainNameSearch:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainNameSearchTopNav);
                    VerifyimageActive(EnumHelper.UiType.OldUi.ToString());
                    if (BrowserInit.Driver.Url.Contains("domain-name-search.aspx"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.DomainRegistrationPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Domains.DomainNameSearch);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.BreadcrumbarrowLblTxt.Text.Equals(UiConstantHelper.DomainRegistrationBreadCrumbs), ExceptionConstantHelper.IncorrectBreadcrum + EnumHelper.Domains.DomainNameSearch);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Text.Contains(UiConstantHelper.DomainNameSearch), ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Domains.DomainNameSearch);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.RegistrationDomainSearchInput.Displayed, ExceptionConstantHelper.DomainSearchIsNotEnable + EnumHelper.Domains.DomainNameSearch);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.RegistrationDomainSearchBulkLink.Enabled, ExceptionConstantHelper.BulkDomainLinkIsNotEnable + EnumHelper.Domains.DomainNameSearch);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.RegistrationDomainSearchBtn.Enabled, ExceptionConstantHelper.DomainSearchButtonIsNotEnable + EnumHelper.Domains.DomainNameSearch);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Domains.Transfer:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.TransferTopNav);
                    VerifyimageActive(EnumHelper.UiType.NewUi.ToString());
                    if (BrowserInit.Driver.Url.Contains("transfer.aspx"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.DomainTransferPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Domains.Transfer);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.BreadcrumbarrowLblTxt.Text.Equals(UiConstantHelper.DomainTransferBreadCrumbs), ExceptionConstantHelper.IncorrectBreadcrum + EnumHelper.Domains.Transfer);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Text.Contains(UiConstantHelper.Transfer), ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Domains.Transfer);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.TransferDomainSearchInput.Enabled, ExceptionConstantHelper.DomainSearchIsNotEnable + EnumHelper.Domains.Transfer);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.TransferDomainSearchBulkLink.Enabled, ExceptionConstantHelper.BulkDomainLinkIsNotEnable + EnumHelper.Domains.Transfer);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.TransferDomainSearchBtn.Enabled, ExceptionConstantHelper.DomainSearchButtonIsNotEnable + EnumHelper.Domains.Transfer);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Domains.NewTlds:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.NewTldsTopNav);
                    VerifyimageActive(EnumHelper.UiType.NewUi.ToString());
                    if (BrowserInit.Driver.Url.Contains("new-tlds/explore.aspx"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.IsTrue(BrowserInit.Driver.Title.Contains(UiConstantHelper.DomainNewTldsPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Domains.NewTlds);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.BreadcrumbarrowLblTxt.Text.Equals(UiConstantHelper.DomainNewTldsBreadCrumbs), ExceptionConstantHelper.IncorrectBreadcrum + EnumHelper.Domains.NewTlds);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Text.Contains(UiConstantHelper.Tlds), ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Domains.NewTlds);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.NewTldsExploreTab.Displayed, ExceptionConstantHelper.ExploreTabIsNotDisplayed + EnumHelper.Domains.NewTlds);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.NewTldsWatchedTab.Displayed, ExceptionConstantHelper.NewTldsWatchedTabIsNotDisplayed + EnumHelper.Domains.NewTlds);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.NewTldsFaqTab.Displayed, ExceptionConstantHelper.FaqTabIsNotDisplayed + EnumHelper.Domains.NewTlds);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.NewTldsSearchInput.Enabled, ExceptionConstantHelper.DomainSearchIsNotEnable + EnumHelper.Domains.NewTlds);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.NewTldsSearchBtn.Enabled, ExceptionConstantHelper.DomainSearchButtonIsNotEnable + EnumHelper.Domains.NewTlds);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Domains.PersonalDomain:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.PersonalDomainTopNav);
                    VerifyimageActive(EnumHelper.UiType.NewUi.ToString());
                    if (BrowserInit.Driver.Url.Contains("personal.aspx"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.HomepageTitle) || BrowserInit.Driver.Title.Contains(UiConstantHelper.PersonalDomain), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Domains.PersonalDomain);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.BreadcrumbarrowLblTxt.Text.Equals(UiConstantHelper.DomainPersonalDomainBreadCrumbs), ExceptionConstantHelper.IncorrectBreadcrum + EnumHelper.Domains.PersonalDomain);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Text.Equals(UiConstantHelper.PersonalDomain), ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Domains.PersonalDomain);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.PersonalDomainFirstNameInput.Enabled, ExceptionConstantHelper.DomainSearchIsNotEnable + EnumHelper.Domains.PersonalDomain);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.PersonalDomainLasrNameInput.Enabled, ExceptionConstantHelper.DomainSearchIsNotEnable + EnumHelper.Domains.PersonalDomain);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.PersonalDomainSearchBtn.Enabled, ExceptionConstantHelper.DomainSearchButtonIsNotEnable + EnumHelper.Domains.PersonalDomain);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.PersonalDomainLink.Enabled, ExceptionConstantHelper.PersonalDomainLinkIsNotEnable + EnumHelper.Domains.PersonalDomain);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Domains.Marketplace:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainMarketplaceTopNav);
                    VerifyimageActive(EnumHelper.UiType.ReactUi.ToString());
                    if (BrowserInit.Driver.Url.Contains("marketplace/buy-domains"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.DomainMarketPlacePageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Domains.Marketplace);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.BreadcrumbarrowTxtReact.Text.Equals(UiConstantHelper.DomainMarketPlaceBreadCrumbs), ExceptionConstantHelper.IncorrectBreadcrum + EnumHelper.Domains.Marketplace);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Text.Contains(UiConstantHelper.Marketplace), ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Domains.Marketplace);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainMarketplaceSearchInput.Enabled, ExceptionConstantHelper.DomainSearchIsNotEnable + EnumHelper.Domains.Marketplace);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainMarketplaceSearchBtn.Enabled, ExceptionConstantHelper.DomainSearchButtonIsNotEnable + EnumHelper.Domains.Marketplace);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainMarketplaceAllTab.Displayed, ExceptionConstantHelper.AllTabIsNotDisplayed + EnumHelper.Domains.Marketplace);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainMarketplaceFeaturedTab.Displayed, ExceptionConstantHelper.FeaturedTabIsNotDisplayed + EnumHelper.Domains.Marketplace);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainMarketplacePremiumTab.Displayed, ExceptionConstantHelper.PremiumTabIsNotDisplayed + EnumHelper.Domains.Marketplace);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainMarketplaceClosingTab.Displayed, ExceptionConstantHelper.ClosingTabIsNotDisplayed + EnumHelper.Domains.Marketplace);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainMarketplaceFilters.Displayed, ExceptionConstantHelper.DomainFiltersTabIsNotDisplayed + EnumHelper.Domains.Marketplace);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Domains.Whois:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainNavLblReact, PageInitHelper<WebPageValidationPageFactory>.PageInit.WhoisTopNav);
                    VerifyimageActive(EnumHelper.UiType.NewUi.ToString());
                    if (BrowserInit.Driver.Url.Contains("whois.aspx"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.DomainWhoIsPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Domains.Whois);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.BreadcrumbarrowLblTxt.Text.Equals(UiConstantHelper.DomainWhoIsBreadCrumbs), ExceptionConstantHelper.IncorrectBreadcrum + EnumHelper.Domains.Whois);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Text.Contains(UiConstantHelper.Whois), ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Domains.Whois);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.WhoisSearchInput.Enabled, ExceptionConstantHelper.DomainSearchIsNotEnable + EnumHelper.Domains.Whois);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.WhoisSearchBtn.Enabled, ExceptionConstantHelper.DomainSearchButtonIsNotEnable + EnumHelper.Domains.Whois);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.WhoisKnowledgeBaseLink.Enabled, ExceptionConstantHelper.KnowledgeBaseLinkIsNotEnable + EnumHelper.Domains.Whois);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Domains.PremiumDns:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.PremiumDNSTopNav);
                    VerifyimageActive(EnumHelper.UiType.OldUi.ToString());
                    if (BrowserInit.Driver.Url.Contains("premiumdns.aspx"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.DomainPremiumPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Domains.PremiumDns);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.BreadcrumbarrowLblTxt.Text.Equals(UiConstantHelper.DomainPremiumBreadCrumbs), ExceptionConstantHelper.IncorrectBreadcrum + EnumHelper.Domains.PremiumDns);
                        //Commended due to Environment Changes
                        //Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Text.Contains(UiConstants.Uptime),ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Domains.PremiumDns);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Displayed, ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Domains.PremiumDns);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.PremiumDnsLogo.Displayed, EnumHelper.Domains.PremiumDns + ExceptionConstantHelper.LogoIsNotDisplayed);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.PremiumDnsProductGrid.Enabled, EnumHelper.Domains.PremiumDns + ExceptionConstantHelper.ProductGridIsNotEnable);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.PremiumDnsPriceTxt.Displayed, EnumHelper.Domains.PremiumDns + ExceptionConstantHelper.ProductPricingIsNotDisplayed);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.PremiumDnsProductAddToCartBtn.Enabled, EnumHelper.Domains.PremiumDns + ExceptionConstantHelper.ProductAddToCartButtonIsNotEnable);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Domains.FreeDns:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.FreeDNSTopNav);
                    VerifyimageActive(EnumHelper.UiType.ReactUi.ToString());
                    if (BrowserInit.Driver.Url.Contains("/store/domains/freedns"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.IsTrue((PageInitHelper<WebPageValidationPageFactory>.PageInit.NewUiBreadcrumbMainMenu.Text + PageInitHelper<WebPageValidationPageFactory>.PageInit.NewUiBreadcrumbSubMenu.Text).Equals(UiConstantHelper.DomainFreeDnsBreadCrumbs), ExceptionConstantHelper.IncorrectBreadcrum + EnumHelper.Domains.FreeDns);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.NewUiHeroBannerTxt.Text.Contains(UiConstantHelper.FreeDns), ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Domains.FreeDns);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.FreeDnsSearchInput.Enabled, ExceptionConstantHelper.DomainSearchIsNotEnable + EnumHelper.Domains.FreeDns);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.FreeDnsBulkLink.Enabled, ExceptionConstantHelper.BulkDomainLinkIsNotEnable + EnumHelper.Domains.FreeDns);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.FreeDnsSearchBtn.Enabled, ExceptionConstantHelper.DomainSearchButtonIsNotEnable + EnumHelper.Domains.FreeDns);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    PageInitHelper<UrlNavigationHelper>.PageInit.GoToUrl();
                    break;
            }
        }
        internal void VerifyHostingPageWebResponseandElements(string domainMenuCategory)
        {
            switch (ParseEnum<EnumHelper.Hosting>(domainMenuCategory))
            {
                case EnumHelper.Hosting.Shared:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.HostingNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.SharedHostingTopNav);
                    VerifyimageActive("NewUi");
                    if (BrowserInit.Driver.Url.Contains("/shared.aspx"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.IsTrue(BrowserInit.Driver.Title.Contains(UiConstantHelper.SharedHostingPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Hosting.Shared);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.BreadcrumbarrowLblTxt.Text.Equals(UiConstantHelper.SharedHostingBreadCrumbs), ExceptionConstantHelper.IncorrectBreadcrum + EnumHelper.Hosting.Shared);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Text.Contains(UiConstantHelper.SharedHosting), ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Hosting.Shared);
                        AsserValidation(EnumHelper.Hosting.Shared.ToString(), 4);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.ProductRenewalPrice.Displayed, ExceptionConstantHelper.ProductRenewalPricingIsNotDisplayed + EnumHelper.Hosting.Shared);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Hosting.WordPressHosting:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.HostingNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.WordPressTopNav);
                    VerifyimageActive("OldUi");
                    if (BrowserInit.Driver.Url.Contains("/wordpress-hosting"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.IsTrue(BrowserInit.Driver.Title.Contains(UiConstantHelper.WordPressHostingPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Hosting.WordPressHosting);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Hosting.Reseller:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.HostingNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.ResellerHostingTopNav);
                    VerifyimageActive("NewUi");
                    if (BrowserInit.Driver.Url.Contains("/reseller.aspx"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.IsTrue(BrowserInit.Driver.Title.Contains(UiConstantHelper.ResellerHostingPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Hosting.Reseller);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.BreadcrumbarrowLblTxt.Text.Equals(UiConstantHelper.ResellerHostingBreadCrumbs), ExceptionConstantHelper.IncorrectBreadcrum + EnumHelper.Hosting.Reseller);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Text.Contains(UiConstantHelper.ResellerHosting), ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Hosting.Reseller);
                        AsserValidation(EnumHelper.Hosting.Reseller.ToString(), 4);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Hosting.Vps:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.HostingNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.VpsHostingTopNav);
                    VerifyimageActive("NewUi");
                    if (BrowserInit.Driver.Url.Contains("/vps.aspx"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.IsTrue(BrowserInit.Driver.Title.Contains(UiConstantHelper.VPSHostingPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Hosting.Vps);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.BreadcrumbarrowLblTxt.Text.Equals(UiConstantHelper.VPSHostingBreadCrumbs), ExceptionConstantHelper.IncorrectBreadcrum + EnumHelper.Hosting.Vps);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Text.Contains(UiConstantHelper.VpsHostingTxt), ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Hosting.Vps);
                        AsserValidation(EnumHelper.Hosting.Vps.ToString(), 4);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Hosting.Dedicated:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.HostingNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.DedicatedServersTopNav);
                    VerifyimageActive("NewUi");
                    if (BrowserInit.Driver.Url.Contains("/dedicated-servers.aspx"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.IsTrue(BrowserInit.Driver.Title.Contains(UiConstantHelper.DedicatedServersPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Hosting.Dedicated);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.BreadcrumbarrowLblTxt.Text.Equals(UiConstantHelper.DedicatedServersBreadCrumbs), ExceptionConstantHelper.IncorrectBreadcrum + EnumHelper.Hosting.Dedicated);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Text.Contains(UiConstantHelper.DedicatedServers), ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Hosting.Dedicated);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.DedicatedServersHeader.Displayed, EnumHelper.Hosting.Dedicated + ExceptionConstantHelper.DedicatedServerHeaderIsNotDisplayed);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.DedicatedServersList.Displayed, EnumHelper.Hosting.Dedicated + ExceptionConstantHelper.DedicatedServerListIsNotDisplayed);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.DedicatedServersListItems.Displayed, EnumHelper.Hosting.Dedicated + ExceptionConstantHelper.DedicatedServerListItemsIsNotDisplayed);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Hosting.PrivateEmail:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.HostingNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.PrivateEmailHostingTopNav);
                    VerifyimageActive("NewUi");
                    if (BrowserInit.Driver.Url.Contains("/email.aspx"))
                    {
                        Assert.IsTrue(BrowserInit.Driver.Title.Contains(UiConstantHelper.PrivateEmailHostingPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Hosting.PrivateEmail);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.BreadcrumbarrowLblTxt.Text.Equals(UiConstantHelper.PrivateEmailHostingBreadCrumbs), ExceptionConstantHelper.IncorrectBreadcrum + EnumHelper.Hosting.PrivateEmail);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Text.Contains(UiConstantHelper.PrivateEmailHosting), ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Hosting.PrivateEmail);
                        AsserValidation(EnumHelper.Hosting.PrivateEmail.ToString(), 3);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Hosting.MigrateToNamecheap:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.HostingNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.MigratetoNcHostingTopNav);
                    VerifyimageActive("OldUi");
                    if (BrowserInit.Driver.Url.Contains("hosting-migrate-to-namecheap.aspx"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.IsTrue(BrowserInit.Driver.Title.Contains(UiConstantHelper.MigratetoNamecheapPageTitle) || BrowserInit.Driver.Title.Contains(UiConstantHelper.MigratetoNamecheapBranchesPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Hosting.MigrateToNamecheap);
                        //Commended due to hero banner changed accross environment  
                        //Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Text.Contains(UiConstants.MigrateToNamecheapTxt));
                        AsserValidation(EnumHelper.Hosting.MigrateToNamecheap.ToString(), 3);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.ProductRenewalPrice.Displayed, ExceptionConstantHelper.ProductRenewalPricingIsNotDisplayed + EnumHelper.Hosting.MigrateToNamecheap);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
            }
        }
        protected void AsserValidation(string productPage, int productGridCount)
        {
            Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.ProductGrid1.Displayed, ExceptionConstantHelper.ProductGridIsNotDisplayed + productPage);
            Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.ProductGrid2.Displayed, ExceptionConstantHelper.ProductGridIsNotDisplayed + productPage);
            Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.ProductGrid3.Displayed, ExceptionConstantHelper.ProductGridIsNotDisplayed + productPage);
            Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.ProductPrice.Enabled, ExceptionConstantHelper.ProductPricingIsNotDisplayed + productPage);
            Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.ProductYearAndDataCenterSelection.Enabled, ExceptionConstantHelper.ProductYearAndDataCenterIsNotEnable + productPage);
            Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.ProductAddToCartBtn.Enabled, ExceptionConstantHelper.ProductAddToCartButtonIsNotEnable + productPage);
            Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.ProductPlan1.Displayed, ExceptionConstantHelper.ProductPlanIsNotDisplayed + productPage);
            Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.ProductPlan2.Displayed, ExceptionConstantHelper.ProductPlanIsNotDisplayed + productPage);
            Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.ProductPlan3.Displayed, ExceptionConstantHelper.ProductPlanIsNotDisplayed + productPage);
            if (productGridCount == 4)
            {
                Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.ProductGrid4.Displayed, ExceptionConstantHelper.ProductGridIsNotDisplayed + productPage);
                Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.ProductPlan4.Displayed, ExceptionConstantHelper.ProductPlanIsNotDisplayed + productPage);
            }
        }
        internal void VerifySecurityPageWebResponseandElements(string domainMenuCategory)
        {
            switch (ParseEnum<EnumHelper.Security>(domainMenuCategory))
            {
                case EnumHelper.Security.SslCertificate:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.SecurityNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.SecuritySslCertificatesTopNav);
                    //Commended Due To Environment Changed
                    //VerifyimageActive("NewUi");
                    if (BrowserInit.Driver.Url.Contains("/ssl-certificates.aspx"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.SslCertificatesPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Security.SslCertificate);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.BreadcrumbarrowLblTxt.Text.Equals(UiConstantHelper.SslCertificatesBreadCrumbs), ExceptionConstantHelper.IncorrectBreadcrum + EnumHelper.Security.SslCertificate);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Text.Contains(UiConstantHelper.SslCertificates), ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Security.SslCertificate);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SecuritySslCertificatesLearnMoreLink.Enabled, ExceptionConstantHelper.LearnMoreLinkIsNotEnable + EnumHelper.Security.SslCertificate);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslValidationTab.Displayed, ExceptionConstantHelper.ValidationTabIsNotDisplayed + EnumHelper.Security.SslCertificate);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslLevelOfValidationDropdown.Displayed, ExceptionConstantHelper.LevelOfValidationTabIsNotDisplayed + EnumHelper.Security.SslCertificate);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslDomainsTab.Displayed, EnumHelper.Security.SslCertificate + ExceptionConstantHelper.DomainsTabIsNotDisplayed);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslDomainsTabDropdown.Displayed, EnumHelper.Security.SslCertificate + ExceptionConstantHelper.DomainsTabDropDownIsNotDisplayed);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslProductGridView.Displayed, EnumHelper.Security.SslCertificate + ExceptionConstantHelper.ProductGridIsNotDisplayed);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslProductListView.Displayed, EnumHelper.Security.SslCertificate + ExceptionConstantHelper.ProductListViewIsNotDisplayed);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslProductPricingSort.Displayed, EnumHelper.Security.SslCertificate + ExceptionConstantHelper.ProductPricingSortIsNotDisplayed);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslProductCount.Count.Equals(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslProductCertificate.Count),
                            EnumHelper.Security.SslCertificate + ExceptionConstantHelper.CertificateCountMisMatch);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslProductCount.Count.Equals(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslProductPrice.Count),
                            EnumHelper.Security.SslCertificate + ExceptionConstantHelper.CertificatePriceCountMisMatch);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslProductCount.Count.Equals(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslProductFieldSet.Count),
                            EnumHelper.Security.SslCertificate + ExceptionConstantHelper.CertificateFieldSetCountMisMatch);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslProductCount.Count.Equals(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslProductActionBtn.Count),
                            EnumHelper.Security.SslCertificate + ExceptionConstantHelper.CertificateActionsCountMisMatch);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslProductCount.Count.Equals(PageInitHelper<WebPageValidationPageFactory>.PageInit.SslProductLearnMoreLnk.Count),
                            EnumHelper.Security.SslCertificate + ExceptionConstantHelper.CertificateLinksCountMisMatch);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Security.WhoisGuard:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.SecurityNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.SecurityWhoisGuardTopNav);
                    VerifyimageActive("OldUi");
                    if (BrowserInit.Driver.Url.Contains("/whoisguard.aspx"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.WhoisguardPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Security.WhoisGuard);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.BreadcrumbarrowLblTxt.Text.Equals(UiConstantHelper.WhoisguardBreadCrumbs), ExceptionConstantHelper.IncorrectBreadcrum + EnumHelper.Security.WhoisGuard);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Text.Contains(UiConstantHelper.WhoisGuard), ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Security.WhoisGuard);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SecurityWhoisGuardImg.Enabled, EnumHelper.Security.WhoisGuard + ExceptionConstantHelper.LogoIsNotDisplayed);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SecurityWhoisGuardProductGrid.Displayed, ExceptionConstantHelper.ProductGridIsNotDisplayed + EnumHelper.Security.WhoisGuard);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SecurityWhoisGuardPrice.Displayed, ExceptionConstantHelper.ProductPricingIsNotDisplayed + EnumHelper.Security.WhoisGuard);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SecurityWhoisGuardAddToCartBtn.Enabled, ExceptionConstantHelper.ProductAddToCartButtonIsNotEnable + EnumHelper.Security.WhoisGuard);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.SecurityWhoisGuardPriceTableGrid.Displayed, ExceptionConstantHelper.PricingTableGridIsNotDisplayed + EnumHelper.Security.WhoisGuard);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Security.PremiumDns:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.SecurityNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.SecurityPremiumDnsTopNav);
                    VerifyimageActive("OldUi");
                    if (BrowserInit.Driver.Url.Contains("/premiumdns.aspx"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.DomainPremiumPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Security.PremiumDns);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.BreadcrumbarrowLblTxt.Text.Equals(UiConstantHelper.DomainPremiumBreadCrumbs), ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Security.PremiumDns);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Displayed, ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Security.PremiumDns);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.PremiumDnsLogo.Displayed, EnumHelper.Security.PremiumDns + ExceptionConstantHelper.LogoIsNotDisplayed);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.PremiumDnsProductGrid.Enabled, EnumHelper.Security.PremiumDns + ExceptionConstantHelper.ProductGridIsNotEnable);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.PremiumDnsPriceTxt.Displayed, EnumHelper.Security.PremiumDns + ExceptionConstantHelper.ProductPricingIsNotDisplayed);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.PremiumDnsProductAddToCartBtn.Enabled, EnumHelper.Security.PremiumDns + ExceptionConstantHelper.ProductAddToCartButtonIsNotEnable);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
            }
        }
        internal void VerifyMainMenuPageWebResponseandElements(string mainMenuCategory)
        {
            BrowserInit.Driver.Navigate().GoToUrl(PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("AP", "/dashboard"));
            PageInitHelper<UrlNavigationHelper>.PageInit.ValidateUrl();
            switch (ParseEnum<EnumHelper.Dashboard>(mainMenuCategory))
            {
                case EnumHelper.Dashboard.Domains:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(null, PageInitHelper<WebPageValidationPageFactory>.PageInit.DomainNavLbl);
                    if (BrowserInit.Driver.Url.Contains("domains.aspx"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.DomainPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Dashboard.Domains);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Displayed, ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Dashboard.Domains);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.RegistrationDomainSearchInput.Displayed, ExceptionConstantHelper.DomainSearchIsNotEnable + EnumHelper.Dashboard.Domains);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.RegistrationDomainSearchBulkLink.Enabled, ExceptionConstantHelper.BulkDomainLinkIsNotEnable + EnumHelper.Dashboard.Domains);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.RegistrationDomainSearchBtn.Displayed, ExceptionConstantHelper.DomainSearchButtonIsNotEnable + EnumHelper.Dashboard.Domains);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Dashboard.Hosting:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(null, PageInitHelper<WebPageValidationPageFactory>.PageInit.HostingNavLbl);
                    if (BrowserInit.Driver.Url.Contains("hosting.aspx"))
                    {
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.SharedHostingPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Dashboard.Hosting);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Displayed, ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Dashboard.Hosting);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Dashboard.Apps:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(null, PageInitHelper<WebPageValidationPageFactory>.PageInit.AppsNavLbl);
                    if (BrowserInit.Driver.Url.Contains("apps/"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.AppsPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Dashboard.Apps);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Displayed, ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Dashboard.Apps);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Dashboard.Security:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(null, PageInitHelper<WebPageValidationPageFactory>.PageInit.SecurityNavLbl);
                    if (BrowserInit.Driver.Url.Contains("security.aspx"))
                    {
                        if (!string.IsNullOrEmpty(AppConfigHelper.CMSZone))
                            ZonesValidationinUrl();
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.SecurityPageTiltle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Dashboard.Security);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.HeroBannerTxt.Displayed, ExceptionConstantHelper.IncorrectHeroBanner + EnumHelper.Dashboard.Security);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Dashboard.Accounts:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(null, PageInitHelper<WebPageValidationPageFactory>.PageInit.AccountNavLbl);
                    if (BrowserInit.Driver.Url.Contains("ap."))
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.Dashboard), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Dashboard.Accounts);
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
            }
        }
        internal void VerifyAccountsPageWebResponseandElements(string accountMenuCategory)
        {
            switch (ParseEnum<EnumHelper.Accounts>(accountMenuCategory))
            {
                case EnumHelper.Accounts.DashBoard:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.AccountNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.AccountsDashboardTopNav);
                    if (BrowserInit.Driver.Url.Contains("ap.") || BrowserInit.Driver.Url.Contains("/dashboard"))
                    {
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.Dashboard), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Accounts.DashBoard);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.AccountsBalanceDiv.Displayed, ExceptionConstantHelper.AccountBalanceDivIsNotDisplayed + EnumHelper.Accounts.DashBoard);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.Accounts2FaDiv.Displayed, ExceptionConstantHelper.Accounts2FactoryAuthIsNotEnable + EnumHelper.Accounts.DashBoard);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.AccountsFirstNameDiv.Displayed, ExceptionConstantHelper.AccountNameDivIsNotDisplayed + EnumHelper.Accounts.DashBoard);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.AccountsLastLoginDiv.Displayed, ExceptionConstantHelper.AccountLastLogginDivIsNotDisplayed + EnumHelper.Accounts.DashBoard);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Accounts.ExpiringSoon:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.AccountNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.AccountsExpiringSoonTopNav);
                    if (BrowserInit.Driver.Url.Contains("/expiringsoon/domains") || BrowserInit.Driver.Url.Contains("/domains/expiringlist"))
                    {
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.Expiring), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Accounts.ExpiringSoon);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.ExpringSoonDomainsTab.Displayed, ExceptionConstantHelper.DomainsTabIsNotDisplayed + EnumHelper.Accounts.ExpiringSoon);
                        Assert.IsTrue(PageInitHelper<WebPageValidationPageFactory>.PageInit.ExpringSoonProductTab.Displayed, ExceptionConstantHelper.ProductsTabIsNotDisplayed + EnumHelper.Accounts.ExpiringSoon);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Accounts.DomainList:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.AccountNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.AccountsDomainListTopNav);
                    if (BrowserInit.Driver.Url.Contains(Regex.Replace(UiConstantHelper.DomainOnly, @"\s+", "")) || BrowserInit.Driver.Url.Contains(Regex.Replace(UiConstantHelper.DomainList, @"\s+", "")))
                    {
                        if (!BrowserInit.Driver.Title.Contains(UiConstantHelper.Domain_List))
                            Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.Domains), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Accounts.DomainList);
                        else
                            Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.Domain_List), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Accounts.DomainList);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
                case EnumHelper.Accounts.ProductList:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.AccountNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.AccountsProductListTopNav);
                    if (BrowserInit.Driver.Url.Contains("/SslCertificates"))
                    {
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.SslCertificatesPageTitle), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Accounts.ProductList);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    PageInitHelper<UrlNavigationHelper>.PageInit.GoToUrl();
                    break;
                case EnumHelper.Accounts.Profile:
                    PageInitHelper<PageNavigationHelper>.PageInit.MoveToElementClickHoldAndVerifyPageResponse(PageInitHelper<WebPageValidationPageFactory>.PageInit.AccountNavLbl, PageInitHelper<WebPageValidationPageFactory>.PageInit.AccountsProfileTopNav);
                    if (BrowserInit.Driver.Url.Contains("/profile/Info") || BrowserInit.Driver.Url.Contains("/personal-info"))
                    {
                        Assert.That(BrowserInit.Driver.Title.Contains(UiConstantHelper.PersonalInfo), ExceptionConstantHelper.PageTitleMisMatch + EnumHelper.Accounts.Profile);
                    }
                    else
                        throw new TestFailedException(BrowserInit.Driver.Url + UiConstantHelper.UrlFormatExcep);
                    break;
            }
        }

        private void ZonesValidationinUrl()
        {
            var currenturl = BrowserInit.Driver.Url.Substring(0, BrowserInit.Driver.Url.IndexOf("com", StringComparison.Ordinal) + 4);
            var returnUrl = PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("CMS", string.Empty, AppConfigHelper.CMSZone).Replace("https", "http");
            Assert.IsTrue(currenturl.Equals(returnUrl), "Zone: " + AppConfigHelper.CMSZone + " is not present in the url or navigation to expected url is wrong");
        }
    }
}