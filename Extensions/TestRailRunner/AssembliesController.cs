using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Registry.Stubs.Abstractions.Console;
using TestRail.Types;
using Ui.Tools.Helpers;

namespace TestRailRunner
{
    public static class AssembliesController
    {
        public static readonly List<AssemblyInfo> Assemblies = new List<AssemblyInfo>();
        static readonly string ScreenUrl = TeamCity.GetScreenUrl();

        public static void LoadAssemblies(string assembliesList, ulong testRun)
        {
            var currDir = Directory.GetCurrentDirectory() + "\\";

            var asmList = assembliesList.Split(',').ToList();

            foreach (var assemblyPath in asmList.Select(t => currDir + t.Trim()))
            {
                if (! File.Exists(assemblyPath))
                    throw new Exception($"Assembly [{assemblyPath}] not found.");

                Assemblies.Add(new AssemblyInfo(Assembly.LoadFrom(assemblyPath)));
            }

            AddSetupTeardown();
            AddTests(testRun);
        }

        public static void RunTests()
        {
            foreach (var asm in Assemblies)
            {
                Program.Logger.Info($"ScreenUrl:  {ScreenUrl}");
                Program.Logger.Info($"===== Asm: {asm.Assembly.GetName()}; TestCount={asm.TestsList.Count}");

                if (asm.TestsList.Count < 1)
                    continue;

                var classInstance = Activator.CreateInstance(asm.SetupClass);

                //Program.Logger.Info("----- Asm.Setup");
                ExecMethod(classInstance, asm.SetupMethod);
                try
                {
                    asm.TestsList.ForEach(RunTest);
                }
                catch (Exception e)
                {
                    Program.Logger.Info($"Error: {e.Message}");
                }
                finally
                {
                    //Program.Logger.Info("---- Asm.TearDown");
                    ExecMethod(classInstance, asm.TeardownMethod);
                }
            }
            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        private static void RunTest(TestInfo testInfo)
        {
            //Program.Logger.Info($" +++++ Run:  {testInfo.TestMethod}({testInfo.FuncParam ?? ""}), {testInfo.TestRailInfo.CaseID}, {ScreenUrl}");
            Program.Logger.Info($" +++++ Run:  {testInfo.TestMethod}({testInfo.FuncParam ?? ""}), {testInfo.TestRailInfo.CaseID}");

            var classInstance = Activator.CreateInstance(testInfo.TestClass);

            //Program.Logger.Info("--- Test.Setup");
            ExecMethod(classInstance, testInfo.SetupMethod);

            try
            {
                //Program.Logger.Info("--- Test.Run");
                ExecMethod(classInstance, testInfo.TestMethod, testInfo.FuncParam);

                TestRail.SetResult(testInfo.TestRailInfo, ResultStatus.Passed);
                Program.Logger.Info("Success");
            }
            catch (Exception e)
            {
                if (testInfo.TestMethod == null)
                {
                    TestRail.SetResult(testInfo.TestRailInfo, ResultStatus.Failed, e.Message);
                    Program.Logger.Info($"Error: {e.Message}");
                    return;
                }

                var screeenFile = $"{ScreenUrl}/{testInfo.TestMethod.Name}.png";
                var exc = e.InnerException ?? e;

                var message = exc.Message + Environment.NewLine +
                              screeenFile + Environment.NewLine +
                              exc.StackTrace + Environment.NewLine;

                Program.Logger.Info($"Error: {message}");
                TestRail.SetResult(testInfo.TestRailInfo, ResultStatus.Failed, message);

                WebDriverHelper.CreateScreenshot(testInfo.TestMethod.Name, false);

            }
            finally
            {
                //Program.Logger.Info("--- Test.Teardown");
                ExecMethod(classInstance, testInfo.TeardownMethod);
            }
        }

        private static void ExecMethod(object classInstance, MethodInfo method, string param = null)
        {
            if (classInstance == null || method == null)
                return;

            method.Invoke(classInstance, param == null ? null : new object[] { param });
        }

        private static void AddSetupTeardown()
        {
            foreach (var asm in Assemblies)
            {
                asm.SetupClass = asm.Assembly.GetTypes().FirstOrDefault(type => type.GetCustomAttributes().Any(atr => atr.GetType().Name == "SetUpFixtureAttribute"));

                if (asm.SetupClass == null)
                    throw new Exception("[SetupClass is not defined] is not defined.");

                asm.SetupMethod = asm.SetupClass.GetMethods().FirstOrDefault(x => x.GetCustomAttributes().Any(atr => atr.GetType().Name == "SetUpAttribute"));
                asm.TeardownMethod = asm.SetupClass.GetMethods().FirstOrDefault(x => x.GetCustomAttributes().Any(atr => atr.GetType().Name == "TearDownAttribute"));
            }
        }

        private static void AddTests(ulong testRun)
        {
            var testsList = TestRail.GetTestList(testRun);

            if (testsList.Count == 0)
                throw new Exception($"There are no tests in Run [{testRun}]");

            foreach (var test in testsList)
            {
                foreach (var asm in Assemblies)
                {
                    MethodInfo method = null;

                    var tag = TestRail.GetTag(test);
                    var tagVals = tag.Split('@');
                    var funcName = tagVals[0];
                    var funcParam = tagVals.Length > 1 ? tagVals[1] : null;
                    var caseId = $"C{test.CaseID}";
                    
                    var type = asm.Assembly.GetTypes().FirstOrDefault(x => (method = GetMethod(x, funcName, caseId)) != null);

                    if (type == null || method == null)
                        continue;

                    var methods = type.GetMethods();
                    var setupMethod = methods.FirstOrDefault(x => x.GetCustomAttributes().Any(atr => atr.GetType().Name == "SetUpAttribute"));
                    var teardownMethod = methods.FirstOrDefault(x => x.GetCustomAttributes().Any(atr => atr.GetType().Name == "TearDownAttribute"));

                    asm.TestsList.Add(new TestInfo()
                    {
                        TestRailInfo = test,
                        TestClass = type,
                        TestMethod = method,
                        SetupMethod = setupMethod,
                        TeardownMethod = teardownMethod,
                        FuncParam = funcParam
                    });
                }
            }
        }
        private static MethodInfo GetMethod(Type type, string tag, string caseId)
        {
            MethodInfo res = null;

            if(!string.IsNullOrEmpty(tag))
                res = type.GetMethod(tag);

            return res ?? type.GetMethod(caseId);
        }

    }
}
