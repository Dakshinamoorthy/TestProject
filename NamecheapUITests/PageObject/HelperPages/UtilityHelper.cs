using System;
using System.Collections.Generic;
using System.Text;
namespace NamecheapUITests.PageObject.HelperPages
{
    public class UtilityHelper
    {
        public static Tuple<string, string, string, string, StringBuilder, string, string, Tuple<string>> CardDetails;
        public static Tuple<string, string, string, string, string> UserBasicInfoDetails;
        public static Tuple<Tuple<string, string, string, string, string, string>> StreetInformation;
        public static Tuple<StringBuilder, StringBuilder, string> ContactInformation;
        public static Tuple<string, string, string, string, string> NewtoNamecheap;
        public static Tuple<string, string, string, string, Tuple<string, string, string, string, string, string>> BillingAddress;
        public static SortedDictionary<Enum, string> LivecardDetails;
        public static SortedDictionary<Enum, string> LivePaypalDetails;

    }
}