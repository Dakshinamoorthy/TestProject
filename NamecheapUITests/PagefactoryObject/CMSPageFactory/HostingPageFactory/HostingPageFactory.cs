using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace NamecheapUITests.PagefactoryObject.CMSPageFactory.HostingPageFactory
{
    class HostingPageFactory
    {
        [FindsBy(How = How.XPath, Using = "(.//*[contains(@class,'grid-row group')]/div[contains(@class,'product-item')] | .//*[@id='aspnetForm']/div/div/div/div/ul | .//*[contains(@class,'grid-row group')]/div/div[contains(@class,'product-grid')])")]
        [CacheLookup]
        internal IList<IWebElement> DedicatedServersCpuGrid { get; set; }

        [FindsBy(How = How.XPath, Using = "(.//div[contains(@class,'dedicated-servers')]//following-sibling::header)[1]")]
        [CacheLookup]
        internal IWebElement DedicatedServersHeaders { get; set; }
    }
}
