using System.Configuration;
namespace NamecheapUITests.PageObject.HelperPages.WrapperFactory
{
    public class AppConfigHelper
    {
        static AppConfigHelper()
        {
            MainUrl = ConfigurationManager.AppSettings["MainUrl"];
            СlientIp = ConfigurationManager.AppSettings["ClientIP"];
            ReleaseManagentNumber = ConfigurationManager.AppSettings["RM"];
            NewUserSignupinProduction = ConfigurationManager.AppSettings["NewUserSignupinProduction"];
            UserName = ConfigurationManager.AppSettings["UserName"];
            Password = ConfigurationManager.AppSettings["Password"];
            ChromeDriverFolder = ConfigurationManager.AppSettings["ChromeDriverFolder"];
            PaymentMethod = ConfigurationManager.AppSettings["PaymentMethod"];
            PremiumReDomain = ConfigurationManager.AppSettings["PremiumReDomain"];
            PremiumEnomDomain = ConfigurationManager.AppSettings["PremiumEnomDomain"];
            LivePaypalPurchase = ConfigurationManager.AppSettings["LivePaypal"];
            LiveCardPurchase = ConfigurationManager.AppSettings["LiveCard"];
            APIKey = ConfigurationManager.AppSettings["APIKey"];
            CMSZone = ConfigurationManager.AppSettings["CMSZone"];
            APZone = ConfigurationManager.AppSettings["APZone"];
            ScreenShotFolder = ConfigurationManager.AppSettings["ScreenShotFolder"];
            LoggerFolder= ConfigurationManager.AppSettings["LoggerFolder"];
        }
        public static string MainUrl { get; private set; }
        public static string ScreenShotFolder { get; private set; }
        public static string СlientIp { get; private set; }
        public static string ReleaseManagentNumber { get; private set; }
        public static string NewUserSignupinProduction { get; private set; }
        public static string UserName { get; private set; }
        public static string Password { get; private set; }
        public static string ChromeDriverFolder { get; private set; }
        public static string PaymentMethod { get; private set; }
        public static string PremiumReDomain { get; private set; }
        public static string PremiumEnomDomain { get; private set; }
        public static string LivePaypalPurchase { get; private set; }
        public static string LiveCardPurchase { get; private set; }
        public static string APIKey { get; private set; }
        public static string CMSZone { get; private set; }
        public static string APZone { get; set; }
        public static string LoggerFolder { get; set; }
    }
}