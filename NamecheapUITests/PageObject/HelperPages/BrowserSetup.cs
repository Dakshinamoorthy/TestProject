using System;
using Gallio.Framework;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
namespace NamecheapUITests.PageObject.HelperPages
{
    public class BrowserSetup
    {
        public static IWebDriver Initialize(EnumHelper.DriverOptions browserName)
        {
            BrowserInit.Driver = GetDriver(browserName);
            BrowserInit.Driver.Manage().Window.Maximize();
            return BrowserInit.Driver;
        }
        private static IWebDriver GetDriver(EnumHelper.DriverOptions browserName)
        {
            switch (browserName)
            {
                case EnumHelper.DriverOptions.Chrome:
                    var relativePath = AppConfigHelper.ChromeDriverFolder;
                    return new ChromeDriver(relativePath);
                case EnumHelper.DriverOptions.Firefox:
                    Environment.SetEnvironmentVariable("webdriver.gecko.driver", "D:\\ProjectRelated\\GeckoDriver\\geckodriver.exe");
                    var service = FirefoxDriverService.CreateDefaultService(@"D:\ProjectRelated\GeckoDriver");
                    service.FirefoxBinaryPath = @"C:\\Program Files\\Mozilla Firefox\\firefox.exe";
                    BrowserInit.Driver = new FirefoxDriver(service);
                    return new FirefoxDriver(service);
                case EnumHelper.DriverOptions.Ie:
                    relativePath = AppConfigHelper.ChromeDriverFolder;
                    return new InternetExplorerDriver(relativePath);
                default:
                    throw new TestFailedException(browserName + " is not supported in NC Web Autotest");
            }
        }
        public void Quit()
        {
            BrowserInit.Driver.Quit();
        }
    }
}