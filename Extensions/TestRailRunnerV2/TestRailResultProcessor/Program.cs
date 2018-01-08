using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestRail.Types;
using TestRailResultProcessor.Test;

namespace TestRailResultProcessor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string pathToTestDir;
            string testRunNumberStr;

            switch (args.Length)
            {
                case 4:
                    pathToTestDir = args[0];
                    testRunNumberStr = args[1];
                    TeamCity.BuildTypeId = args[2];
                    TeamCity.BuildId = args[3];
                    break;

                case 2:
                    pathToTestDir = args[0];
                    testRunNumberStr = args[1];
                    break;

                default:
                    Console.WriteLine("TestRailResultProcessor Tool");
                    Console.WriteLine();
                    Console.WriteLine("Usage: TestRailResultProcessor.exe [Path] [TestRunId]");
                    Console.WriteLine("Usage: TestRailResultProcessor.exe [Path] [TestRunId] [system.teamcity.buildType.id] [teamcity.build.id]");
                    Console.WriteLine();

                    Console.WriteLine("Path:");
                    Console.WriteLine("   path to directory with test result files (parse all *.xml files in the directory)");
                    Console.WriteLine();

                    Console.WriteLine("TestRunId:");
                    Console.WriteLine("   Test Run Identifier(digits only)");
                    Console.WriteLine();

                    return;
            }

            var testRunNumber = ulong.Parse(testRunNumberStr);
            var testsFileDir = new DirectoryInfo(pathToTestDir);
            var testResults = new List<TestCase>();

            foreach (var fileInfo in testsFileDir.GetFiles("*.xml"))
            {
                testResults.AddRange(TestResultsParser.Parse(fileInfo.FullName));
            }
            
            var testRailTestsList = TestRail.TestRail.GetTestList(testRunNumber);

            SetResults(testRailTestsList, testResults);
        }

        private static void SetResults(List<global::TestRail.Types.Test> testRailTestsList, List<TestCase> testResults)
        {
            foreach (var testRailTest in testRailTestsList)
            {
                var tag = TestRail.TestRail.GetTag(testRailTest);
                var funcName = TestRail.TestRail.GetFullFuncName(testRailTest);
                var isAutomated = TestRail.TestRail.IsAutomated(testRailTest);

                if (!isAutomated)
                    continue;

                var tcRes = testResults.Find(x => x.Name == $"C{testRailTest.CaseID.ToString()}" || x.Name == tag || x.Fulname == funcName);
                //var tcRes = testResults.Find(x => x.Fulname == funcName);

                if (tcRes == null)
                    continue;

                if (tcRes.Result.ToLower() == "passed")
                    TestRail.TestRail.SetResult(testRailTest, ResultStatus.Passed);
                else
                    TestRail.TestRail.SetResult(testRailTest, ResultStatus.Failed,
                        tcRes.ErrorMessage + Environment.NewLine + 
                        tcRes.StackTrace + Environment.NewLine + Environment.NewLine + 
                        $"{TeamCity.GetScreenUrl()}/{GetFuncName(funcName)}.png");
            }
        }

        private static string GetFuncName(string funcName)
        {
            return funcName.Split('.').Last();
        }
    }
}
