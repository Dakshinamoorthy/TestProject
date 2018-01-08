using System;
using System.Reflection;

namespace TestRailRunner
{
    public class TestInfo
    {
        public global::TestRail.Types.Test TestRailInfo;

        public Type TestClass;
        public MethodInfo TestMethod;
        public MethodInfo SetupMethod;
        public MethodInfo TeardownMethod;
        public string FuncParam;
    }
}
