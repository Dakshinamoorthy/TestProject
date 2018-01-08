using System;
using System.Net;
using Gallio.Framework;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.ValidationPages;
using NUnit.Framework;
using OpenQA.Selenium;
namespace NamecheapUITests.PageObject.HelperPages
{
    public class ExceptionType
    {
        private static T ParseEnum<T>(string input)
        {
            return (T)Enum.Parse(typeof(T), input, true);
        }
        private enum ExceptionTypes
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
            IgnoreException,
            FormatException
        }
        public void ThrowException(string excp, Exception exception)
        {
            if (excp.Contains("CustomException"))
            {
                Assert.Pass(exception.Message);
            }
            var ex = excp.Split('.');
            var exc = ex[ex.Length - 1];
            if (exc.Equals("WebDriverException"))
            {
                exc = "ElementNotVisibleException";
            }
            else if (exc.Equals("TargetInvocationException"))
            {
                exc = "WebDriverTimeoutException";
            }
            switch (ParseEnum<ExceptionTypes>(exc))
            {
                case ExceptionTypes.WebDriverTimeoutException:
                    throw new InconclusiveException(exception.Message);
                case ExceptionTypes.NoSuchElementException:
                    throw new NoSuchElementException(exception.Source + " - " + UiConstantHelper.ElementNotFound +
                                                     " in " + BrowserInit.Driver.Title + exception.StackTrace);
                case ExceptionTypes.StaleElementReferenceException:
                    throw new StaleElementReferenceException(exception.Source + " - " + UiConstantHelper.ElementRefExc +
                                                             " - " +
                                                             "Element No longer availabe in DOM page, Check UI Changes" +
                                                             " ( " + BrowserInit.Driver.Title + " ) " +
                                                             exception.StackTrace);
                case ExceptionTypes.ElementNotVisibleException:
                    throw new ElementNotVisibleException(exception.Source + " - " + UiConstantHelper.ElementNotVisible +
                                                         " - " + " Element is Present in DOM but not visible - " +
                                                         BrowserInit.Driver.Title + exception.StackTrace);
                case ExceptionTypes.WebDriverException:
                    throw new WebDriverException(BrowserInit.Driver.Title +
                                                 " Page/Elements takes too long time to load " + exception.Message +
                                                 UiConstantHelper.WebDriverException);
                case ExceptionTypes.IndexOutOfRangeException:
                    throw new IndexOutOfRangeException(exception.Message + exception.StackTrace);
                case ExceptionTypes.AssertionException:
                    throw new AssertionException(exception.Message + exception.StackTrace);
                case ExceptionTypes.InconclusiveException:
                    throw new InconclusiveException(exception.Message);
                case ExceptionTypes.NullReferenceException:
                    throw new WebException(exception.Source + " - " + UiConstantHelper.NullReference +
                                           " - Element having null value in code for the page - " +
                                           BrowserInit.Driver.Title + exception.StackTrace);
                case ExceptionTypes.TestFailedException:
                    throw new TestFailedException(exception.Message);
                case ExceptionTypes.WebException:
                    PageInitHelper<WebPageResponse>.PageInit.VerifyPageWebResponseStatusCode();
                    break;
                case ExceptionTypes.IgnoreException:
                    throw new IgnoreException(exception.Message + exception.StackTrace);
                case ExceptionTypes.FormatException:
                    throw new FormatException(exception.Message + exception.StackTrace);
                default:
                    throw new SystemException("New Type Of exception Occurs - Exception Type " +
                                              exception.GetType().Name + " in " + BrowserInit.Driver.Title +
                                              exception.StackTrace);
            }
        }
    }
}