using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NamecheapUITests.PagefactoryObject.CMSPageFactory.WebPageValidationPageFactory
{
    public class WebPageValidationPageFactory
    {
        //Home page links
        [FindsBy(How = How.XPath, Using = ".//*[contains(text(), 'Sign Up') and contains(@href, '/myaccount/signup.aspx')]")]
        [CacheLookup]
        internal IWebElement SignUpGlobalHatHlk { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(text(), 'Sign In')]")]
        [CacheLookup]
        internal IWebElement SignIpGlobalHatHlk { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@class='notification-control notification']/following-sibling:: span")]
        [CacheLookup]
        internal IWebElement NotificationIcon { get; set; }

        //Main  Nav Items
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//li[1]//a[contains(text(),'Domains')]")]
        internal IWebElement DomainNavLbl { get; set; }
        
        [FindsBy(How = How.XPath, Using = ".//div[@class='gb-header']//div[contains(@class,'gb-dropdown')]//*[contains(text(),'Domains')]")]
        internal IWebElement DomainNavLblReact { get; set; }

        [FindsBy(How = How.XPath, Using = "//header[@id='header']//li[2]/a[text()='Hosting' or @href='/hosting.aspx']")]
        [CacheLookup]
        internal IWebElement HostingNavLbl { get; set; }

        [FindsBy(How = How.XPath, Using = "//header[@id='header']//li[3]//a[contains(text(),'Apps')]")]
        [CacheLookup]
        internal IWebElement AppsNavLbl { get; set; }

        [FindsBy(How = How.XPath, Using = "//header[@id='header']//li[4]//a[contains(text(),'Security')]")]
        [CacheLookup]
        internal IWebElement SecurityNavLbl { get; set; }

        [FindsBy(How = How.XPath, Using = "//header[@id='header']//li[5]//a[contains(text(),'Account')] | .//div[contains(@class,'gb-dropdown')]//a/span[contains(text(),'Account')]")]
        [CacheLookup]
        internal IWebElement AccountNavLbl { get; set; }

        //Common UI For Domains Tab
        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'hero')]//h1")]
        [CacheLookup]
        internal IWebElement HeroBannerTxt { get; set; }

        //Domain Sub Menu options
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(@href,'domain-name-search.aspx')]")]
        internal IWebElement DomainNameSearchTopNav { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[contains(@id,'inputSingleDomain')]")]
        [CacheLookup]
        internal IWebElement RegistrationDomainSearchInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[contains(@id,'inputSingleDomain')]/../a")]
        [CacheLookup]
        internal IWebElement RegistrationDomainSearchBulkLink { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[contains(@id,'inputSingleDomain')]/../button")]
        [CacheLookup]
        internal IWebElement RegistrationDomainSearchBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'featured-grid')]/div[1]")]
        [CacheLookup]
        internal IWebElement RegistrationDomainGenaricTldsGrid { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'featured-grid')]/div[1]//a")]
        [CacheLookup]
        internal IWebElement RegistrationDomainViewAllGenaricTldsLinksandBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'featured-grid')]/div[2]")]
        [CacheLookup]
        internal IWebElement RegistrationDomainCountryTldsGrid { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'featured-grid')]/div[2]//a")]
        [CacheLookup]
        internal IWebElement RegistrationDomainViewAllCountryTldsLinksandBtn { get; set; }

        //Transfer Sub Menu options
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(@href,'transfer.aspx')]")]
        [CacheLookup]
        internal IWebElement TransferTopNav { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[contains(@id,'inputTransferDomain')]")]
        [CacheLookup]
        internal IWebElement TransferDomainSearchInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[contains(@id,'inputTransferDomain')]/../a")]
        [CacheLookup]
        internal IWebElement TransferDomainSearchBulkLink { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[contains(@id,'inputTransferDomain')]/../button")]
        [CacheLookup]
        internal IWebElement TransferDomainSearchBtn { get; set; }

        //NewTLDs Sub Menu Options
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(text(),'New TLDs')]")]
        [CacheLookup]
        internal IWebElement NewTldsTopNav { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@id,'tabList')]//li[1]")]
        [CacheLookup]
        internal IWebElement NewTldsExploreTab { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@id,'tabList')]//li[2]")]
        [CacheLookup]
        internal IWebElement NewTldsWatchedTab { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@id,'tabList')]//li[3]")]
        [CacheLookup]
        internal IWebElement NewTldsPreOrderTab { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@id,'tabList')]//li[3]")]
        [CacheLookup]
        internal IWebElement NewTldsFaqTab { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[contains(@id,'searchnewtld')]")]
        [CacheLookup]
        internal IWebElement NewTldsSearchInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[contains(@id,'searchnewtld')]/../button")]
        [CacheLookup]
        internal IWebElement NewTldsSearchBtn { get; set; }

        //Personal Domain Sub Menu Options
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(@href,'personal.aspx')]")]
        [CacheLookup]
        internal IWebElement PersonalDomainTopNav { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[contains(@id,'inputFirstNameWithToggle')]")]
        [CacheLookup]
        internal IWebElement PersonalDomainFirstNameInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[contains(@id,'inputLastNameWithToggle')]")]
        [CacheLookup]
        internal IWebElement PersonalDomainLasrNameInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[contains(@id,'inputLastNameWithToggle')]/../button")]
        [CacheLookup]
        internal IWebElement PersonalDomainSearchBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'personal-domain')]/a")]
        [CacheLookup]
        internal IWebElement PersonalDomainLink { get; set; }

        //Marketplace Domains 
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(@href,'buy-domains.aspx')]")]
        [CacheLookup]
        internal IWebElement DomainMarketplaceTopNav { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[contains(@name,'search')]")]
        [CacheLookup]
        internal IWebElement DomainMarketplaceSearchInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='input-group']//button")]
        [CacheLookup]
        internal IWebElement DomainMarketplaceSearchBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[contains(@class,'nav-tabs')]//li[1]")]
        [CacheLookup]
        internal IWebElement DomainMarketplaceAllTab { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[contains(@class,'nav-tabs')]//li[2]")]
        [CacheLookup]
        internal IWebElement DomainMarketplaceFeaturedTab { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[contains(@class,'nav-tabs')]//li[1]")]
        [CacheLookup]
        internal IWebElement DomainMarketplacePremiumTab { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[contains(@class,'nav-tabs')]//li[1]")]
        [CacheLookup]
        internal IWebElement DomainMarketplaceClosingTab { get; set; }

        [FindsBy(How = How.XPath, Using = "//ul[contains(@class,'filter')]")]
        [CacheLookup]
        internal IWebElement DomainMarketplaceFilters { get; set; }

        //WhoisLookUp
        [FindsBy(How = How.XPath, Using = "//a[contains(@href,'whois.aspx')][contains(@class,'gb-dropdown')]")]  // //header[@id='header']//a[contains(@href,'whois.aspx')]
        [CacheLookup]
        internal IWebElement WhoisTopNav { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[contains(@id,'inputWhoisDomain')]")]
        [CacheLookup]
        internal IWebElement WhoisSearchInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[contains(@id,'inputWhoisDomain')]/../button")]
        [CacheLookup]
        internal IWebElement WhoisSearchBtn { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[contains(@id,'featuredItemContainer')]/p/a")]
        [CacheLookup]
        internal IWebElement WhoisKnowledgeBaseLink { get; set; }

        //PremiumDns
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(@href,'premiumdns.aspx')]")]
        [CacheLookup]
        internal IWebElement PremiumDNSTopNav { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[@class='pdns-logo']")]
        [CacheLookup]
        internal IWebElement PremiumDnsLogo { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='product-grid']")]
        [CacheLookup]
        internal IWebElement PremiumDnsProductGrid { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='product-grid']//p")]
        [CacheLookup]
        internal IWebElement PremiumDnsPriceTxt { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='product-grid']//a")]
        [CacheLookup]
        internal IWebElement PremiumDnsProductAddToCartBtn { get; set; }

        //FreeDns
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(@href,'freedns.aspx')]")]
        [CacheLookup]
        internal IWebElement FreeDNSTopNav { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[contains(@class,'ng-pristine')]")]
        [CacheLookup]
        internal IWebElement FreeDnsSearchInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[contains(@class,'btn-bulk-search')]")]
        [CacheLookup]
        internal IWebElement FreeDnsBulkLink { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[contains(@class,'btn-hero-search')]")]
        [CacheLookup]
        internal IWebElement FreeDnsSearchBtn { get; set; }

        // Breadcrumbs
        [FindsBy(How = How.XPath, Using = ".//*[@class='breadcrumbs']/p")]
        [CacheLookup]
        internal IWebElement BreadcrumbarrowLblTxt { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@class='breadcrumb']/li[3]")]
        [CacheLookup]
        internal IWebElement BreadcrumbarrowTxtReact { get; set; }

        //Hosting Sub Menu options
        //Common Hosting UI 
        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'product-grid')]/div[1][contains(@id,'productFeatureColumn')]")]
        [CacheLookup]
        internal IWebElement ProductGrid1 { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'product-grid')]/div[2][contains(@id,'productFeatureColumn')]")]
        [CacheLookup]
        internal IWebElement ProductGrid2 { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'product-grid')]/div[3][contains(@id,'productFeatureColumn')]")]
        [CacheLookup]
        internal IWebElement ProductGrid3 { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'product-grid')]/div[4][contains(@id,'productFeatureColumn')]")]
        [CacheLookup]
        internal IWebElement ProductGrid4 { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[@class='amount']")]
        [CacheLookup]
        internal IWebElement ProductPrice { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[@class='renewal-price-display']")]
        [CacheLookup]
        internal IWebElement ProductRenewalPrice { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'coreuiselect')]")]
        [CacheLookup]
        internal IWebElement ProductYearAndDataCenterSelection { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[@type='submit']")]
        [CacheLookup]
        internal IWebElement ProductAddToCartBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'product-grid')]/div[1][contains(@id,'productFeatureColumn')]/header/h2")]
        [CacheLookup]
        internal IWebElement ProductPlan1 { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'product-grid')]/div[2][contains(@id,'productFeatureColumn')]/header/h2")]
        [CacheLookup]
        internal IWebElement ProductPlan2 { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'product-grid')]/div[3][contains(@id,'productFeatureColumn')]/header/h2")]
        [CacheLookup]
        internal IWebElement ProductPlan3 { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'product-grid')]/div[4][contains(@id,'productFeatureColumn')]/header/h2")]
        [CacheLookup]
        internal IWebElement ProductPlan4 { get; set; }

        //Shared Hosting
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(@href,'shared.aspx')]")]
        [CacheLookup]
        internal IWebElement SharedHostingTopNav { get; set; }

        //WordPress Hosting
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(@href,'wordpress-hosting')]")]
        [CacheLookup]
        internal IWebElement WordPressTopNav { get; set; }

        //Reseller Hosting
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(@href,'reseller.aspx')]")]
        [CacheLookup]
        internal IWebElement ResellerHostingTopNav { get; set; }

        //VPS Hosting
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(@href,'vps.aspx')]")]
        [CacheLookup]
        internal IWebElement VpsHostingTopNav { get; set; }

        //Dedicated Server
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(@href,'dedicated-servers.aspx')]")]
        [CacheLookup]
        internal IWebElement DedicatedServersTopNav { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'dedicated-servers')]//h2")]
        [CacheLookup]
        internal IWebElement DedicatedServersHeader { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'server-list')]")]
        [CacheLookup]
        internal IWebElement DedicatedServersList { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'server-list')]/div")]
        [CacheLookup]
        internal IWebElement DedicatedServersListItems { get; set; }

        //Private Email
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(@href,'email.aspx')]")]
        [CacheLookup]
        internal IWebElement PrivateEmailHostingTopNav { get; set; }

        //Migrate To Namecheap
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(@href,'hosting-migrate-to-namecheap.aspx')]")]
        [CacheLookup]
        internal IWebElement MigratetoNcHostingTopNav { get; set; }

        [FindsBy(How = How.XPath, Using = "//p[contains(@id,'subHeadingLiteral')]")]
        [CacheLookup]
        internal IWebElement MigratetoNcSubHeader { get; set; }

        [FindsBy(How = How.XPath, Using = "//p[contains(@id,'subHeadingLiteral')]/a[1]")]
        [CacheLookup]
        internal IWebElement MigratetoNcUpTimeGuaranteeLink { get; set; }

        //Apps Sub Menu options
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//li[1]//a[contains(@href,'apps')]")]
        [CacheLookup]
        internal IWebElement AppsMarketplaceTopNav { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='hero group']//h1")]
        [CacheLookup]
        internal IWebElement AppsMarketplaceHeroImg { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@class='why-apps']")]
        [CacheLookup]
        internal IWebElement AppsMarketplaceLearnMoreLink { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='bundle-finder-form']/div/div")]
        [CacheLookup]
        internal IWebElement AppsMarketplaceDropDown { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='bundle-finder-results']/div[1]")]
        [CacheLookup]
        internal IWebElement AppsMarketplaceSetOfAppsGrid { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='bundle-finder-results']/div[2]//a")]
        [CacheLookup]
        internal IWebElement AppsMarketplaceAppsGridAddToCartBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@href='#EMAIL']")]
        [CacheLookup]
        internal IWebElement AppsMarketplaceProductivityTab { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@href='#WEBSITE']")]
        [CacheLookup]
        internal IWebElement AppsMarketplaceWebsiteTab { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@href='#TOOLS']")]
        [CacheLookup]
        internal IWebElement AppsMarketplaceToolsTab { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@href='#BRANDIDENTITY']")]
        [CacheLookup]
        internal IWebElement AppsMarketplaceBrandIdentityTab { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[@href='#All']")]
        [CacheLookup]
        internal IWebElement AppsMarketplaceAllTab { get; set; }

        [FindsBy(How = How.XPath, Using = "//li[@class='view-options']")]
        [CacheLookup]
        internal IWebElement AppsMarketplaceViewOptions { get; set; }

        [FindsBy(How = How.XPath, Using = "//li[@class='sort-options']")]
        [CacheLookup]
        internal IWebElement AppsMarketplaceSortOptions { get; set; }

        //Apps - Subscription
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(@href,'subscriptions')]")]
        [CacheLookup]
        internal IWebElement AppsSubscriptionsTopNav { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='site-filter']/div")]
        [CacheLookup]
        internal IWebElement AppsSubscriptionsFilter { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='sortby-container']/div")]
        [CacheLookup]
        internal IWebElement AppsSubscriptionsSort { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='subscription-container']/div[1]")]
        [CacheLookup]
        internal IWebElement AppsSubscriptionsUI_ContainerDiv { get; set; }

        // Security - SSL Menu options
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(@href,'ssl-certificates.aspx')]")]
        [CacheLookup]
        internal IWebElement SecuritySslCertificatesTopNav { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='hero-btn']/a")]
        [CacheLookup]
        internal IWebElement SecuritySslCertificatesLearnMoreLink { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='ssl-shop-filters']//li[@class='expandable'][1]//a[1]")]
        [CacheLookup]
        internal IWebElement SslValidationTab { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='ssl-shop-filters']//li[@class='expandable'][1]//a[2]")]
        [CacheLookup]
        internal IWebElement SslLevelOfValidationDropdown { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='ssl-shop-filters']//li[@class='expandable'][2]//a[1]")]
        [CacheLookup]
        internal IWebElement SslDomainsTab { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='ssl-shop-filters']//li[@class='expandable'][2]//a[2]")]
        [CacheLookup]
        internal IWebElement SslDomainsTabDropdown { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='view-options']/a[1]")]
        [CacheLookup]
        internal IWebElement SslProductGridView { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='view-options']/a[2]")]
        [CacheLookup]
        internal IWebElement SslProductListView { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='sort']")]
        [CacheLookup]
        internal IWebElement SslProductPricingSort { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='sort']")]
        [CacheLookup]
        internal IWebElement SslProductList { get; set; }

        //Security - WhoisGaurd
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(@href,'whoisguard.aspx')]")]
        [CacheLookup]
        internal IWebElement SecurityWhoisGuardTopNav { get; set; }

        [FindsBy(How = How.XPath, Using = "//img[contains(@src,'whoisguard.png')]")]
        [CacheLookup]
        internal IWebElement SecurityWhoisGuardImg { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='product-grid']")]
        [CacheLookup]
        internal IWebElement SecurityWhoisGuardProductGrid { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='product-grid']//p")]
        [CacheLookup]
        internal IWebElement SecurityWhoisGuardPrice { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[@class='product-grid']//a")]
        [CacheLookup]
        internal IWebElement SecurityWhoisGuardAddToCartBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//table[contains(@class,'default-table')]")]
        [CacheLookup]
        internal IWebElement SecurityWhoisGuardPriceTableGrid { get; set; }

        [FindsBy(How = How.XPath, Using = "//header[@id='header']//li[4]//a[contains(@href,'premiumdns.aspx')]")]
        [CacheLookup]
        internal IWebElement SecurityPremiumDnsTopNav { get; set; }

        //Accounts-DashBoard
        [FindsBy(How = How.XPath, Using = ".//*[@id='header']//li[5]/div/ul/li[1]/a")]
        [CacheLookup]
        internal IWebElement AccountsDashboardTopNav { get; set; }

        [FindsBy(How = How.XPath, Using = "(//*[contains(@ng-if,'UserInformation.AccountBalance')])[1]")]
        [CacheLookup]
        internal IWebElement AccountsBalanceDiv { get; set; }

        [FindsBy(How = How.XPath, Using = "(//*[contains(@ng-if,'UserInformation.TFAEnabled ')])[1]")]
        [CacheLookup]
        internal IWebElement Accounts2FaDiv { get; set; }

        [FindsBy(How = How.XPath, Using = "(//*[contains(@ng-if,'UserInformation.FirstName')])[1]")]
        [CacheLookup]
        internal IWebElement AccountsFirstNameDiv { get; set; }

        [FindsBy(How = How.XPath, Using = "(//*[contains(@ng-if,'UserInformation.LastLogin')])[1]")]
        [CacheLookup]
        internal IWebElement AccountsLastLoginDiv { get; set; }

        //Accounts-Expiring Soon
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//a[contains(.,'Expiring Soon')]")]
        [CacheLookup]
        internal IWebElement AccountsExpiringSoonTopNav { get; set; }
        
        [FindsBy(How = How.XPath, Using = ".//div[contains(@class,'filter-bar')]//a[contains(@routerlink,'domains')]")]    // Previous - .//*[contains(@class,'tab domain')]
        [CacheLookup]
        internal IWebElement ExpringSoonDomainsTab { get; set; }
        
        [FindsBy(How = How.XPath, Using = ".//div[contains(@class,'filter-bar')]//a[contains(@routerlink,'products')]")]    // Previous - .//*[contains(@class,'tab apps')]
        [CacheLookup]
        internal IWebElement ExpringSoonProductTab { get; set; }

        //Accounts-Domain List
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//li[5]//a[contains(text(),'Domain List')] | .//div[contains(@class,'gb-dropdown')]//a/span[contains(text(),'Domain List')]")]   // Previous - //header[@id='header']//a[contains(.,'Domain List')]
        [CacheLookup]
        internal IWebElement AccountsDomainListTopNav { get; set; }

        //Accounts-Product List
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//li[5]//a[contains(text(),'Product List')] | .//div[contains(@class,'gb-dropdown')]//a/span[contains(text(),'Product List')]")]   // Previous - //header[@id='header']//a[contains(.,'Product List')]
        [CacheLookup]
        internal IWebElement AccountsProductListTopNav { get; set; }

        //Accounts-Profile
        [FindsBy(How = How.XPath, Using = "//header[@id='header']//li[5]//a[contains(text(),'Profile')] | .//div[contains(@class,'gb-dropdown')]//a/span[contains(text(),'Profile')]")]   // Previous - //header[@id='header']//a[contains(@href,'profile')]
        [CacheLookup]
        internal IWebElement AccountsProfileTopNav { get; set; }


        // New UI Breadcrumbs
        [FindsBy(How = How.XPath, Using = "//ol[@class='breadcrumb']//li[1]")]
        [CacheLookup]
        internal IWebElement NewUiBreadcrumbMainMenu { get; set; }

        [FindsBy(How = How.XPath, Using = "//ol[@class='breadcrumb']//li[2]")]
        [CacheLookup]
        internal IWebElement NewUiBreadcrumbSubMenu { get; set; }

        //New UI For Header
        [FindsBy(How = How.XPath, Using = "//div[@class='headline']/h1")]
        [CacheLookup]
        internal IWebElement NewUiHeroBannerTxt { get; set; }

        //Security SSL List Items
        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'ssl-product')]")]
        [CacheLookup]
        internal IList<IWebElement> SslProductCount { get; set; }

        [FindsBy(How = How.XPath, Using = "//li[@class='certificate']//a")]
        [CacheLookup]
        internal IList<IWebElement> SslProductCertificate { get; set; }

        [FindsBy(How = How.XPath, Using = "//span[@class='amount']")]
        [CacheLookup]
        internal IList<IWebElement> SslProductPrice { get; set; }

        [FindsBy(How = How.XPath, Using = "//li[@class='fieldset']/p/div[1]")]
        [CacheLookup]
        internal IList<IWebElement> SslProductFieldSet { get; set; }

        [FindsBy(How = How.XPath, Using = "//li[@class='action']//button")]
        [CacheLookup]
        internal IList<IWebElement> SslProductActionBtn { get; set; }

        [FindsBy(How = How.XPath, Using = "//li[@class='learn-more']//a")]
        [CacheLookup]
        internal IList<IWebElement> SslProductLearnMoreLnk{ get; set; }

        //Validate Hero Banner Image Status
        //[FindsBy(How = How.XPath, Using = "((.//*/form//picture)/ancestor::div[contains(@style,'background-image')])| (.//*/div[contains(concat(' ',normalize-space(@class),' '),'hero')])/picture/child::img")]
        [FindsBy(How = How.XPath, Using = "//form/div[contains(@class,'hero group')]")]
        [CacheLookup]
        internal IWebElement HeroBannerOldUiPage { get; set; }

        [FindsBy(How = How.XPath, Using = "//picture/img")]
        [CacheLookup]
        internal IWebElement HeroBannerNewUiPage { get; set; }

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'hero')]/picture/img")]
        [CacheLookup]
        internal IWebElement HeroBannerReactUiPage { get; set; }
        
        // NC Logo on the top left corner of the landing page after login 
        [FindsBy(How = How.CssSelector, Using = "header > nav > .logo>a")]
        [CacheLookup]
        internal IWebElement NcLogoSigninLandingPage { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='header']//div[contains(@class,'top')]//ul/li[contains(@class,'user-menu')] | .//a[contains(@class,'gb-dropdown')][contains(@href,'dashboard')]")]
        [CacheLookup]
        internal IWebElement LblUserNameHeader { get; set; }
    }
}
