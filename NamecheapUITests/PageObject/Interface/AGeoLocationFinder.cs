using System;
namespace NamecheapUITests.PageObject.Interface
{
    public abstract class AGeoLocationFinder
    {
        public abstract Tuple<string, string, string, string, string, string> GenerateStreetInfo(string countryName);
    }
}