using System.Collections.Generic;
using TestRail;
using TestRail.Types;
using TestRailResultProcessor;

namespace TestRailTestsListCreator.TestRail
{
    internal class TestRail
    {
        private static readonly TestRailClient Client;

        static TestRail()
        {
            var trc = ConfigHelper.TestRailConfig;
            Client = new TestRailClient(trc.Url, trc.UserName, trc.Password);
        }

        public static List<global::TestRail.Types.Test> GetTestList(ulong testRun)
        {
            return  Client.GetTests(testRun).FindAll(IsAutomated);
        }

        public static void SetResult(global::TestRail.Types.Test test, ResultStatus res, string message = "")
        {
            if (test.ID != null)
                Client.AddResult((ulong)test.ID, res, message);
        }

        public static string GetTag(global::TestRail.Types.Test test)
        {
            var tagToken = test.JsonFromResponse.GetValue(ConfigHelper.TestRailConfig.AttrTag).ToString();

            if (tagToken.Contains(","))
                tagToken = tagToken.Split(',')[0];

            return tagToken;
        }

        public static string GetFullFuncName(global::TestRail.Types.Test test)
        {
            return test.JsonFromResponse.GetValue(ConfigHelper.TestRailConfig.AttrGit).ToString();
        }

        public static bool IsAutomated(global::TestRail.Types.Test test)
        {
            var automatedToken = test.JsonFromResponse.GetValue(ConfigHelper.TestRailConfig.AttrAutomation);

            if (automatedToken == null)
                return false;

            return automatedToken.ToString() == "3";
        }
    }
}
