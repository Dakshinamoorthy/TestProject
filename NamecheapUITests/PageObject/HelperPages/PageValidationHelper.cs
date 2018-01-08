using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using OpenQA.Selenium;
namespace NamecheapUITests.PageObject.HelperPages
{
    public class PageValidationHelper
    {
        #region Constants
        private static readonly int PAGE_LOAD_TIMEOUT = 60;
        #endregion
        public static int Number;
        public static StringBuilder Number1;
        internal bool ElementIsAt(IWebElement xPath)
        {
            var element = xPath;
            return PageInitHelper<TimeSpanHelper>.PageInit.WaitUntilElementIsDisplayed(element, PAGE_LOAD_TIMEOUT);
        }
        internal bool TitleIsAt(string title)
        {
            var pageTitle = title;
            return PageInitHelper<TimeSpanHelper>.PageInit.WaitUntilpageTiltleIsDisplayed(pageTitle, PAGE_LOAD_TIMEOUT);
        }
        public int RandomGenrator(int count, int start = 1)
        {
            return Number = new Random(DateTime.Now.Millisecond).Next(start, count);
        }
        public StringBuilder RandomGenratorstringBuilder(int count, int start = 1)
        {
            var numbers = new HashSet<int>();
            var uniqueNumber = new StringBuilder();

            while (numbers.Count < count)
            {
                numbers.Add(new Random().Next(9, 99));
            }
            var s = long.Parse(string.Join(",", numbers).Replace(",", ""));
            uniqueNumber.Append(s);
            return uniqueNumber;
        }
    }
}