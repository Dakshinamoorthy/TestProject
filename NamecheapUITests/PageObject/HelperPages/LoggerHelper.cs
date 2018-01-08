using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NLog;
using NLog.Config;
using NLog.Targets;


namespace NamecheapUITests.PageObject.HelperPages
{
    public class LoggerHelper
    {
        private string _fileName;
        public void CaptureException(Exception ex)
        {
            if (!ex.GetType().ToString().Contains(UiConstantHelper.CustomException))
            {
             _fileName = GenerateLoggerDirectoryAndFile();      
             CustomLoggerConfiguration();
             LoggerInformation(ex);
            }
        }

        private string GenerateLoggerDirectoryAndFile()
        {
            string testName = Regex.Replace(NUnit.Framework.TestContext.CurrentContext.Test.Name, @"[^0-9a-zA-Z]+"," - ");
            List<string> folderPath = new List<string>();
            string loggerFolder = AppConfigHelper.LoggerFolder;
            folderPath.Add(loggerFolder);
            var mergeFolderWithTimestamp = loggerFolder + @"\Log\" + DateTime.Now.ToString("dd-MMM-yy");         
            folderPath.Add(mergeFolderWithTimestamp);
            foreach (var createPath in folderPath.Where(createPath => !Directory.Exists(createPath)))
            {
                Directory.CreateDirectory(createPath);
            }
            return (string.Join("", folderPath.Last()) + @"\" + testName +
                              DateTime.Now.ToString("dd-MMM-yy HH-mm-ss-ffff-tt") + ".txt");
        }

        private void CustomLoggerConfiguration()
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            // Step 3. Set target properties 
            fileTarget.FileName = _fileName;
            fileTarget.Layout = "${message}";

            // Step 4. Define rules
            var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            // Step 5. Activate the configuration
            LogManager.Configuration = config;
        }

        private void LoggerInformation(Exception ex)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Logged Exception :");
            logger.Info("Exception Type : " + ex.GetType() + Environment.NewLine + Environment.NewLine);
            logger.Error("Logged Message : " + Environment.NewLine + ex.Message + Environment.NewLine + Environment.NewLine);
            logger.Info("Exception Info : " + Environment.NewLine + ex.StackTrace + Environment.NewLine + Environment.NewLine);
            logger.Info(ex.InnerException != null ? ex.InnerException.ToString() : "No Inner Exception");
            logger.Info("========================================================================");
        }
    }
}
