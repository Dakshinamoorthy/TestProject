using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestRail.Types;
using Ui.Tools.Helpers;

namespace TestRailRunner
{
    class TestExecutor
    {
        private static readonly Assembly Asm = Assembly.LoadFrom(ConfigHelper.AssemblyPath);

        public static void RunTests(List<global::TestRail.Types.Test> testsList)
        {
            var setupClass = Asm.GetTypes().FirstOrDefault(type => type.GetCustomAttributes().Any(atr => atr.GetType().Name == "SetUpFixtureAttribute"));

            if (setupClass == null)
                throw new Exception("[SetupClass is not defined] is not defined.");
            
            var classInstance = Activator.CreateInstance(setupClass);
            var setupMethod = setupClass.GetMethods().FirstOrDefault(x=>x.GetCustomAttributes().Any(atr=>atr.GetType().Name == "SetUpAttribute"));
            var teardownMethod = setupClass.GetMethods().FirstOrDefault(x => x.GetCustomAttributes().Any(atr => atr.GetType().Name == "TearDownAttribute"));

            ExecMethod(classInstance, setupMethod);
            testsList.ForEach(RunTest);
            ExecMethod(classInstance, teardownMethod);
        }

        private static void RunTest(global::TestRail.Types.Test test)
        {
            MethodInfo method = null;

            var tag = TestRail.GetTag(test);
            var tagDelimiter = '@';
            var tagVals = tag.Split(tagDelimiter);

            var funcName = tag.Contains(tagDelimiter) ? tagVals[0] : tag;
            var funcParam = tag.Contains(tagDelimiter) ? tagVals[1] : null;

            var caseId = $"C{test.CaseID}";
            var screenUrl = TeamCity.GetScreenUrl();

            Program.Logger.Info($" +++++ Run:  {funcName}-{funcParam??""}, {caseId}, {screenUrl}");

            try
            {
                var type = Asm.GetTypes().First(x => (method = GetMethod(x, funcName, caseId)) != null);

                if (type == null)
                    throw new Exception("[Type] is not defined.");

                if (method == null)
                    throw new Exception("[Method] is not defined.");

                var classInstance = Activator.CreateInstance(type);

                var setupMethod =
                    type.GetMethods()
                        .FirstOrDefault(x => x.GetCustomAttributes().Any(atr => atr.GetType().Name == "SetUpAttribute"));
                var teardownMethod =
                    type.GetMethods()
                        .FirstOrDefault(
                            x => x.GetCustomAttributes().Any(atr => atr.GetType().Name == "TearDownAttribute"));

                if (setupMethod != null)
                    ExecMethod(classInstance, setupMethod);

                ExecMethod(classInstance, method, funcParam);

                //if (teardownMethod != null)
                //    ExecMethod(type, teardownMethod);

                TestRail.SetResult(test, ResultStatus.Passed);
                Program.Logger.Info("Success:");
            }
            catch (Exception e)
            {
                if (method == null)
                {
                    TestRail.SetResult(test, ResultStatus.Failed, e.Message);
                    Program.Logger.Info($"Error: {e.Message}");
                    return;
                }

                var screeenFile = $"{screenUrl}/{method.Name}.png";
                var exc = e.InnerException ?? e;

                var message = exc.Message + Environment.NewLine +
                              screeenFile + Environment.NewLine +
                              exc.StackTrace + Environment.NewLine;

                Program.Logger.Info($"Error: {message}");
                TestRail.SetResult(test, ResultStatus.Failed, message);

                WebDriverHelper.CreateScreenshot(method.Name, false);
            }
        }

        private static MethodInfo GetMethod(Type type, string tag, string caseId)
        {
            return type.GetMethod(string.IsNullOrEmpty(tag) ? caseId : tag);
        }

        private static void ExecMethod(object classInstance, MethodInfo method, string param = null)
        {
            //var classInstance = Activator.CreateInstance(type);
            method.Invoke(classInstance, param == null ? null : new object[] {param});
        }
/*
        private static void ExecMethod(string className, string methodName)
        {
            var classType = Asm.GetTypes().First(x => x.FullName == className);
            var Class = Activator.CreateInstance(classType);
            var method = classType.GetMethod(methodName);
            method.Invoke(Class, null);
        }
*/
    }
}
