using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
namespace NamecheapUITests.PageObject.HelperPages
{
    public class DataGenerator
    {
        static string alphaCaps = "QWERTYUIOPASDFGHJKLZXCVBNM";
        static string alphaLow = "qwertyuiopasdfghjklzxcvbnm";
        static string numerics = "1234567890";
        static string special = "@#$~%^&*()_+";
        private readonly string _allChars = alphaCaps + alphaLow + numerics + special;
        private readonly Random _r = new Random();
        public string UrlGenerator(string testCoverage = "", string associatedurlPath = "")
        {
            Associateurl = PageInitHelper<UrlNavigationHelper>.PageInit.UrlGenerator();
            return Associateurl;
        }

        public string EnomTlds()
        {
            if (AppConfigHelper.MainUrl.Contains("live"))
            {
                EnomTld =
                    PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                        EnumHelper.ExcelDataEnum.ProductionEnomTlDs.ToString());
            }
            else if (AppConfigHelper.MainUrl.Contains("sandbox"))
            {
                EnomTld =
                    PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                        EnumHelper.ExcelDataEnum.SandboxEnomTlDs.ToString());
            }
            else if (AppConfigHelper.MainUrl.IndexOf("bsb", StringComparison.OrdinalIgnoreCase) >= 0 |
                     AppConfigHelper.MainUrl.IndexOf("bsbx", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                EnomTld =
                    PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                        EnumHelper.ExcelDataEnum.BsbEnomTlDs.ToString());
            }
            return EnomTld;
        }

        public string ReTlds()
        {
            if (AppConfigHelper.MainUrl.Contains("live"))
            {
                ReTld =
                    PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                        EnumHelper.ExcelDataEnum.ProductionReTlDs.ToString());
            }
            else if (AppConfigHelper.MainUrl.Contains("sandbox"))
            {
                ReTld =
                    PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                        EnumHelper.ExcelDataEnum.SandboxReTlDs.ToString());
            }
            else if (AppConfigHelper.MainUrl.IndexOf("bsb", StringComparison.OrdinalIgnoreCase) >= 0 |
                     AppConfigHelper.MainUrl.IndexOf("bsbx", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                ReTld =
                    PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                        EnumHelper.ExcelDataEnum.BsbReTlDs.ToString());
            }
            return ReTld;
        }

        public string BannedDomainNamePicker()
        {
            BannedDomain =
                PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                    EnumHelper.ExcelDataEnum.BannedDomains.ToString());
            return BannedDomain;
        }

        public string WhoisLookUpdomainNamePicker()
        {
            WhoisDomain =
                PageInitHelper<ExcelDataHelper>.PageInit.DataFletcherFromExcel(
                    EnumHelper.ExcelDataEnum.WhoisLookupDomains.ToString());
            return WhoisDomain;
        }

        public List<string> BulkDomains()
        {
            var bulkDomainNames = new List<string>();
            var bulkTlds = new List<string>();
            var count = 5;
            string[] domainR = {"EnomTlds", "ReTlds"};
            var domainRIndex = PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(domainR.Length, 0);
            var domainRName = domainR[domainRIndex];
            label:
            var indexcount = count;
            for (var index = 1; index <= indexcount; index++)
            {
                string tldName = domainRName == "EnomTlds"
                    ? PageInitHelper<DataGenerator>.PageInit.EnomTlds()
                    : PageInitHelper<DataGenerator>.PageInit.ReTlds();
                var domainName = PageInitHelper<DataHelper>.PageInit.DomainName + "." + tldName;
                if (bulkTlds.Any(str => str.Contains(tldName))) continue;
                bulkTlds.Add(tldName);
                bulkDomainNames.Add(domainName);
            }
            if (bulkTlds.Count == 5)
                return bulkDomainNames;
            count = 10 - bulkTlds.Count;
            goto label;
        }

        public List<string> BulkTransferDomainList()
        {
            var bulkDomainNames = new List<string>();
            var count = 5;
            string[] domainR = {"com"};
            var domainRIndex = PageInitHelper<PageValidationHelper>.PageInit.RandomGenrator(domainR.Length, 0);
            var domainRName = domainR[domainRIndex];
            label:
            var indexcount = count;
            for (var index = 1; index <= indexcount; index++)
            {
                var tldName = domainRName;
                var domainName = PageInitHelper<DataHelper>.PageInit.TransferDomainName + "." + tldName;
                bulkDomainNames.Add(domainName);
            }
            if (bulkDomainNames.Count == 5)
                return bulkDomainNames;
            count = 10 - bulkDomainNames.Count;
            goto label;
        }
        public string GenerateNewPassword(int length)
        {
            var generatedPassword = "";
            if (length < 4)
                throw new Exception("Number of characters should be greater than 4.");
            var posArray = "0123456789";
            if (length < posArray.Length)
                posArray = posArray.Substring(0, length);
            var pLower = GetRandomPosition(ref posArray);
            var pUpper = GetRandomPosition(ref posArray);
            var pNumber = GetRandomPosition(ref posArray);
            var pSpecial = GetRandomPosition(ref posArray);
            for (int i = 0; i < length; i++)
            {
                if (i == pLower)
                    generatedPassword += GetRandomChar(alphaCaps);
                else if (i == pUpper)
                    generatedPassword += GetRandomChar(alphaLow);
                else if (i == pNumber)
                    generatedPassword += GetRandomChar(numerics);
                else if (i == pSpecial)
                    generatedPassword += GetRandomChar(special);
                else
                    generatedPassword += GetRandomChar(_allChars);
            }
            return generatedPassword;
        }
        private string GetRandomChar(string fullString)
        {
            return fullString.ToCharArray()[(int)Math.Floor(_r.NextDouble() * fullString.Length)].ToString();
        }
        private int GetRandomPosition(ref string posArray)
        {
            var randomChar = posArray.ToCharArray()[(int)Math.Floor(_r.NextDouble()
                                           * posArray.Length)].ToString();
            var pos = int.Parse(randomChar);
            posArray = posArray.Replace(randomChar, "");
            return pos;
        }

        internal string GetPrice(string priceText)
        {
            return Regex.Replace(priceText, @"[^\d..][^\w\s]*", "").Trim();
        }


        #region  Getter Setter Variables

        public string Associateurl { get; set; }
        public string EnomTld { get; set; }
        public string ReTld { get; set; }
        public string BannedDomain { get; set; }
        public string WhoisDomain { get; set; }
        public string GeneratedPassword { get; set; }
        #endregion
    }
}