using System.Collections.Generic;
using HtmlAgilityPack;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
namespace NamecheapUITests.PageObject.HelperPages
{
    public class RandomDomainNameGenerator
    {
        public List<string> DomainName(string provider = "", string sld = "")
        {
            var domainName = new List<string>();
            if (provider.Equals(UiConstantHelper.EnomDomains) || provider.Equals("EnomDomainsWithoutWhois") || provider.Equals("EnomDomainsWithPremiumDNS"))
            {
                domainName.Add(PageInitHelper<DataHelper>.PageInit.DomainName + "." + PageInitHelper<DataGenerator>.PageInit.EnomTlds());
                return domainName;
            }
            if (provider.Equals(UiConstantHelper.ReDomains) || provider.Equals("ReDomainsWithoutWhois") || provider.Equals("ReDomainsWithPremiumDNS"))
            {
                domainName.Add(PageInitHelper<DataHelper>.PageInit.DomainName + "." + PageInitHelper<DataGenerator>.PageInit.ReTlds());
                return domainName;
            }
            if (provider == UiConstantHelper.BannedDomainName)
            {
                domainName.Add(PageInitHelper<DataGenerator>.PageInit.BannedDomainNamePicker());
                return domainName;
            }
            if (provider == UiConstantHelper.WhoisLookup)
            {
                domainName.Add(PageInitHelper<DataGenerator>.PageInit.WhoisLookUpdomainNamePicker());
                return domainName;
            }
            if (provider.Contains("Premium") && !provider.Contains("PremiumDNS"))
            {
                var domainType = provider.Contains("Re")
                    ? AppConfigHelper.PremiumReDomain
                    : AppConfigHelper.PremiumEnomDomain;

                domainName.Add(domainType);
                return domainName;
            }
            if (provider.Contains("PremiumDNS"))
            {
                var domainType = provider.Contains("Re")
                    ? PageInitHelper<DataHelper>.PageInit.DomainName + "." + PageInitHelper<DataGenerator>.PageInit.ReTlds()
                    : PageInitHelper<DataHelper>.PageInit.DomainName + "." + PageInitHelper<DataGenerator>.PageInit.EnomTlds();

                domainName.Add(domainType);
                return domainName;
            }
            if (provider == UiConstantHelper.BulkDomains)
            {
                domainName = PageInitHelper<DataGenerator>.PageInit.BulkDomains();
                return domainName;
            }
            if (provider == UiConstantHelper.SingleDomainTransfer)
            {
                domainName.Add(PageInitHelper<DataHelper>.PageInit.TransferDomainName + "." + "com");
                return domainName;
            }
            if (provider == UiConstantHelper.SingleDomain)
            {
                domainName.Add(PageInitHelper<DataHelper>.PageInit.DomainName + "." + PageInitHelper<DataGenerator>.PageInit.ReTlds());
                return domainName;
            }
            if (provider == UiConstantHelper.BulkDomainsTransfer)
            {
                domainName = PageInitHelper<DataGenerator>.PageInit.BulkTransferDomainList();
                return domainName;
            }
            var locationUrl = PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("", "zones.aspx");
            var web = new HtmlWeb();
            var doc = web.Load(locationUrl);
            var rateNode = doc.DocumentNode.SelectNodes("//text()[preceding-sibling::br]").Count;
            var number = PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(rateNode);
            Tld = doc.DocumentNode.SelectSingleNode("//text()[preceding-sibling::br][" + number + "]").InnerText;
            if (sld == "")
            {
                domainName.Add(PageInitHelper<DataHelper>.PageInit.DomainName + "." + Tld);
                return domainName;
            }
            domainName.Add(sld + "." + Tld);
            return domainName;
        }
        public string Tlds()
        {
            var locationUrl = PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator("", "zones.aspx");
            var web = new HtmlWeb();
            var doc = web.Load(locationUrl);
            var rateNode = doc.DocumentNode.SelectNodes("//text()[preceding-sibling::br]").Count;
            var number = PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(rateNode);
            Tld = doc.DocumentNode.SelectSingleNode("//text()[preceding-sibling::br][" + number + "]").InnerText;
            return Tld;
        }
        public string Tld { get; set; }
    }
}