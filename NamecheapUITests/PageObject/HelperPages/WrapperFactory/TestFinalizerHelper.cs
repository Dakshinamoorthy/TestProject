using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NamecheapUITests.PagefactoryObject.CMSPageFactory.WebPageValidationPageFactory;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
namespace NamecheapUITests.PageObject.HelperPages.WrapperFactory
{
    public class TestFinalizerHelper
    {
        public void Testclosure(string namespaceName, Exception ex = null)
        {
            var testStatus = ex?.GetType().Name ?? "Pass";
            if (string.IsNullOrEmpty(AppConfigHelper.ReleaseManagentNumber)) return;
            var testName = Regex.Replace(TestContext.CurrentContext.Test.Name, @"[^0-9a-zA-Z]+", "-") + "-";
            //testName = Regex.Replace(testName, "[A-Z]", " $0").Trim();
            var folderPath = PageInitHelper<TestFinalizerHelper>.PageInit.GenerateWrapper(namespaceName);
            //  var testStatus = ex.Equals(null) ? "Pass" : ex.GetType().Name;
            string testResult = GetTestResult(testStatus);
            //var exMessage = ex.Equals(null) ? "" : Seg(ex.Message);
            if (testResult.Equals("Ignore")) return;
            PageInitHelper<TestFinalizerHelper>.PageInit.GenerateSnapshot(folderPath, testResult, testName, testStatus);
            PageInitHelper<PageNavigationHelper>.PageInit.ScrollToElement(PageInitHelper<WebPageValidationPageFactory>.PageInit.LblUserNameHeader);
        }
        internal string GenerateWrapper(string namespaceName)
        {
            var folderPaths = new List<string>();
            string testResultFolder = AppConfigHelper.ScreenShotFolder + "/Auto-Test Result";
            folderPaths.Add(testResultFolder);
            var folderDate = testResultFolder + "/" + DateTime.Now.ToString("dd-MMM-yy");
            folderPaths.Add(folderDate);
            var folderRm = folderDate + "/" + AppConfigHelper.ReleaseManagentNumber;
            folderPaths.Add(folderRm);
            var envFolder = folderRm + "/" + AppConfigHelper.MainUrl;
            folderPaths.Add(envFolder);
            var namespaceList = namespaceName.Split('.');
            var module = "";
            var testingCategory = "";
            foreach (var checkNamespace in namespaceList)
            {
                if (checkNamespace.Contains(UiConstantHelper.Cms) || checkNamespace.Contains(UiConstantHelper.Ap)) module = checkNamespace;
                if (checkNamespace.Contains(UiConstantHelper.Functional) || checkNamespace.Contains(UiConstantHelper.Smoke))
                    testingCategory = checkNamespace;
            }
            var moduleFolder = envFolder + "/" + module;
            folderPaths.Add(moduleFolder);
            var testingCategoryFolder = moduleFolder + "/" + testingCategory;
            folderPaths.Add(testingCategoryFolder);
            foreach (var createPath in folderPaths.Where(createPath => !Directory.Exists(createPath)))
            {
                Directory.CreateDirectory(createPath);
            }
            return testingCategoryFolder;
        }
        internal void GenerateSnapshot(string testName, string folderPathName, string testStatus)
        {
            var fileName = testName + "-" + testStatus + ".png";
            const string pattern = " *[\\/:|\"-()]+ *";
            const string replacement = "-";
            var sanitized = Regex.Replace(@fileName, pattern, replacement);
            var fileList = Directory.GetFiles(folderPathName, testName + "*.*");
            foreach (var file in fileList.Where(file => file.ToUpper().Contains(testName.ToUpper())))
            {
                System.Diagnostics.Debug.WriteLine(file + "will be deleted");
                File.Delete(file);
            }
             ((ITakesScreenshot)BrowserInit.Driver).GetScreenshot().SaveAsFile(@folderPathName + "\\" + sanitized, ImageFormat.Bmp);
        }
        internal string GetTestResult(string testStatus)
        {
            var testResult = testStatus.Equals(EnumHelper.ExceptionTypes.InconclusiveException.ToString())
                ? "Inconclusive"
                : testStatus.Equals(EnumHelper.ExceptionTypes.IgnoreException.ToString())
                ? "Ignore"
                : testStatus.Equals(EnumHelper.ExceptionTypes.WebException.ToString()) ||
                testStatus.Equals(EnumHelper.ExceptionTypes.WebDriverException.ToString()) ||
                testStatus.Equals(EnumHelper.ExceptionTypes.WebDriverTimeoutException.ToString()) ||
                testStatus.Equals(EnumHelper.ExceptionTypes.NoSuchElementException.ToString()) ||
                testStatus.Equals(EnumHelper.ExceptionTypes.ElementNotVisibleException.ToString()) ||
                testStatus.Equals(EnumHelper.ExceptionTypes.StaleElementReferenceException.ToString()) ||
                testStatus.Equals(EnumHelper.ExceptionTypes.IndexOutOfRangeException.ToString()) ||
                testStatus.Equals(EnumHelper.ExceptionTypes.NullReferenceException.ToString()) ||
                testStatus.Equals(EnumHelper.ExceptionTypes.AssertionException.ToString()) ||
                testStatus.Equals(EnumHelper.ExceptionTypes.TestFailedException.ToString())
                ? "Failed"
                : "Passed";
            return testResult;
        }
        internal void GenerateSnapshot(string folderPathName, string testResult, string testName, string testStatus)
        {
            var fileName = testResult + "--" + testName + testStatus + ".png";
            const string pattern = " *[\\/:|\"-()]+ *";
            const string replacement = "-";
            var sanitized = Regex.Replace(@fileName, pattern, replacement);
            var fileList = Directory.GetFiles(folderPathName, testName + "*.*");
            foreach (var file in fileList.Where(file => file.ToUpper().Contains(testName.ToUpper())))
            {
                System.Diagnostics.Debug.WriteLine(file + "will be deleted");
                File.Delete(file);
            }
            var resultScreenshot = Screenshots.Snapshots.GetFullPageScreenshot(BrowserInit.Driver);
            resultScreenshot.Save(@folderPathName + "\\" + sanitized, ImageFormat.Bmp);
            //((ITakesScreenshot)BrowserInit.Driver).GetScreenshot().SaveAsFile(@folderPathName + "\\" + sanitized, ImageFormat.Bmp);
        }
        #region PageFactory
        [FindsBy(How = How.XPath, Using = ".//a[contains(text(),'Edit Cart')]")]
        [CacheLookup]
        internal IWebElement DropdownEditCart { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[text()='Clear all items']")]
        [CacheLookup]
        internal IWebElement DropDownItemClearAllItems { get; set; }
        [FindsBy(How = How.XPath, Using = "//div[contains(@class, 'alert')]//p")]
        [CacheLookup]
        internal IWebElement AlertMessage { get; set; }
        #endregion
    }
}