using System;
using System.IO;

namespace TestRailTestsListCreator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("TestRailTestsListCreator Tool");
                Console.WriteLine();
                Console.WriteLine("Usage: TestRailTestsListCreator.exe [Path] [TestRunId] [Framework]");
                Console.WriteLine();

                Console.WriteLine("Path:");
                Console.WriteLine("   path to TestsList.txt file");
                Console.WriteLine();

                Console.WriteLine("TestRunId:");
                Console.WriteLine("   test run Identifier (digits only)");
                Console.WriteLine();

                Console.WriteLine("Framework:");
                Console.WriteLine("   framework of test execution = net/core");
                Console.WriteLine();

                return;
            }

            var pathToTestsListFile = args[0] + "\\TestsList.txt";
            var testRunNumber = ulong.Parse(args[1]);
            var framework = args[2];

            var res = string.Empty; 

            switch (framework)
            {
                case "net":
                        res = CreateTestsList(testRunNumber, Environment.NewLine);
                    break;
                case "core":
                        res = CreateTestsList(testRunNumber, ",");
                    break;
                default:
                    Console.WriteLine("Incorrect [framework]. See help");
                    return;
            }

            File.WriteAllText(pathToTestsListFile, res);
        }

        private static string CreateTestsList(ulong testRunNumber, string delimiter)
        {
            var res = string.Empty;

            var testRailTestsList = TestRail.TestRail.GetTestList(testRunNumber);
            testRailTestsList.ForEach(x=>res += $"{TestRail.TestRail.GetFullFuncName(x)}{delimiter}");

            return res;
        }
    }
}
