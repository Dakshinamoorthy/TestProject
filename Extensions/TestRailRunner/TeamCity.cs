namespace TestRailRunner
{
    public static class TeamCity
    {
        public static string BuildTypeId = "";
        public static string BuildId = "";

        public static string GetScreenUrl()
        {
            return $"{ConfigHelper.TeamCityConfig.Url}/repository/download/{BuildTypeId}/{BuildId}:id";
        }
    }
}
