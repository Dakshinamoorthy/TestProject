using System;
using NLog;

namespace TestRailRunner
{
    class Program
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            if (args.Length < 2)    
                throw new Exception("Expected 3 arguments: [testRunNumber], [system.teamcity.buildType.id] and [teamcity.build.id]");

            var testRunNumber = ulong.Parse(args[0]);
            TeamCity.BuildTypeId = args[1];
            TeamCity.BuildId = args[2];

            AssembliesController.LoadAssemblies(ConfigHelper.AssemblyPath, testRunNumber);
            AssembliesController.RunTests();

            //Console.WriteLine("Finished !!!");
            //Console.ReadLine();
            //RunTests(testRunNumber);
        }

        private static void RunTests(ulong testRun)
        {
            var testsList = TestRail.GetTestList(testRun);

            if(testsList.Count == 0)
                throw new Exception($"There are no tests in Run [{testRun}]");

            //WebDriverHelper.DeleteScreenshot();
            TestExecutor.RunTests(testsList);
        }
    }
}
