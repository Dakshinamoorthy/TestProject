using System.Configuration;
using System.IO;

namespace TestRailRunner
{
    public static class ConfigHelper
    {
        static ConfigHelper()
        {
            AssemblyPath = /*Directory.GetCurrentDirectory() + "\\" + */ConfigurationManager.AppSettings["AssemblyPath"];

            TestRailConfig.Url = ConfigurationManager.AppSettings["testRail.Url"];
            TestRailConfig.UserName = ConfigurationManager.AppSettings["testRail.UserName"];
            TestRailConfig.Password = ConfigurationManager.AppSettings["testRail.Password"];
            TestRailConfig.AttrTag = ConfigurationManager.AppSettings["testRail.CustomAttribute.Tag"];
            TestRailConfig.AttrAutomation = ConfigurationManager.AppSettings["testRail.CustomAttribute.Automation"];

            TeamCityConfig.Url = ConfigurationManager.AppSettings["teamCity.URL"];
        }

        public static string AssemblyPath { get; private set; }
        public static TestRailConfig TestRailConfig = new TestRailConfig();
        public static TeamCityConfig TeamCityConfig = new TeamCityConfig();
    }

    public class TestRailConfig
    {
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AttrTag { get; set; }
        public string AttrAutomation { get; set; }
    }

    public class TeamCityConfig
    {
        public string Url { get; set; }
    }
}
