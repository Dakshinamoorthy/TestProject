using System;

namespace NamecheapUITests.PageObject.HelperPages.WrapperFactory
{
    public class EnumHelper
    {
        public enum DriverOptions
        {
            Chrome,
            Firefox,
            Ie
        }
     
        public enum DomainKeys
        {
            DomainName,
            DomainPrice,
            DomainDuration,
            DomainRetailPrice,
            DomainNamePromotionCode
        }
        public enum CartWidget
        {
            IcanPrice,
            SubTotal,
        }
        public enum HostingKeys
        {
            ProductName,
            ProductDomainName,
            ProductPrice,
            ProductDomainPrice,
            ProductDomainDuration,
            ProductDuration,
            Datacenter,
            DatacenterPrice,
            ProductRenewalPrice
        }
        public enum OrderSummaryKeys
        {
            PurchaseOrderNumber,
            PurchaseOrderdateAndtime,
            PaymentTransactionId,
            PaymentMethod,
            CardEndNumber,
            PayPalUserName
        }
        public enum ShoppingCartKeys
        {
            DomainAutoRenewStatus,
            WhoisGuardForDomainStatus,
            WhoisGuardForDomainPrice,
            WhoisGuardForDomainAutoRenewStatus,
            WhoisGuardForDomainRentalPrice,
            WhoisGuardForDomainDuration,
            PremiumDnsForDomainStatus,
            PremiumDnsForDomainDuration,
            PremiumDnsForDomainRentalprice,
            PremiumDnsForDomainPrice,
            PremiumDnsForDomainAutoRenewStatus
        }
        public enum Ssl
        {
            WhoisGuardPrice,
            WhoisGuardDuration,
            PremiumDnsPrice,
            PremiumDnsDuration,
            CertificateName,
            ValidationType,
            DomainCategory,
            CertificatePrice,
            CertificateDuration
        }
        public enum DomainModuleSelection
        {
            NewDomain,
            NcDomain,
            AnotherRegistrarDomain,
            TransferDomain
        }
     
        internal enum ExcelDataEnum
        {
            ProductionEnomTlDs,
            ProductionReTlDs,
            SandboxEnomTlDs,
            SandboxReTlDs,
            BsbEnomTlDs,
            BsbReTlDs,
            BannedDomains,
            WhoisLookupDomains,
            Creditcardnumbers,
            AddressNameType,
            UserFirstName,
            UserLastName,
            CountryNames,
            EmailServer,
            ExtendedTlds,
            AddNewAddressFrom
        }

        public enum UserAccountKeys
        {
            CustomerName,
            UserName,
            SupportPin,
            Dashboard,
            Profile,
            Signout
        }
        internal enum MarketPlaceFilters
        {
            Categories,
            Price,
            Content,
            MaxLength,
            Seller
        }
        internal enum PaymentMethod
        {
            Card,
            Paypal,
            Funds
        }
        //WebPage Validation
        public enum Domains
        {
            DomainNameSearch,
            Transfer,
            NewTlds,
            PersonalDomain,
            Marketplace,
            Whois,
            PremiumDns,
            FreeDns
        }
        public enum Hosting
        {
            Shared,
            WordPressHosting,
            Reseller,
            Vps,
            Dedicated,
            PrivateEmail,
            MigrateToNamecheap
        }
    
        public enum Security
        {
            SslCertificate,
            WhoisGuard,
            PremiumDns
        }
        public enum Dashboard
        {
            Domains,
            Hosting,
            Apps,
            Security,
            Accounts
        }
        public enum Accounts
        {
            DashBoard,
            ExpiringSoon,
            DomainList,
            ProductList,
            Profile
        }

        public enum UiType
        {
            NewUi,
            OldUi,
            ReactUi
        }
     
        //Footer
        public enum PageLinks
        {
            Domains = 1,
            Hosting = 2,
            PremiumDNS = 3,
            FreeDNS = 4,
            WhoisGuard = 5,
            SslCertificates = 6,
            Resellers = 7,
            Affiliates = 8,
            Support = 9,
            Careers = 10,
            SendUsFeedback = 11
        }
        public enum ExceptionTypes
        {
            WebDriverException,
            WebDriverTimeoutException,
            NoSuchElementException,
            StaleElementReferenceException,
            ElementNotVisibleException,
            IndexOutOfRangeException,
            AssertionException,
            WebException,
            NullReferenceException,
            InconclusiveException,
            TestFailedException,
            IgnoreException
        }
        internal enum CardDetails
        {
            CardNumber,
            NameonCard,
            ExpMonth,
            ExpYear,
            CvvNumber
        }
        internal enum PayPalDetails
        {
            EmailAddress,
            Password
        }
        public static T ParseEnum<T>(string input)
        {
            var val = (int)Enum.Parse(typeof(T), input, true);
            Result =
                Enum.Parse(typeof(DomainModuleSelection), val.ToString()).ToString();
            return (T)Enum.Parse(typeof(T), input, true);
        }
        public static string Result { get; set; }
    }
}