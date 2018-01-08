using System;
using System.Collections.Generic;
using System.Reflection;

namespace TestRailRunner
{
    public class AssemblyInfo
    {
        public AssemblyInfo(Assembly assembly)
        {
            Assembly = assembly;
        }

        public Assembly Assembly { get; }

        public Type SetupClass;
        public MethodInfo SetupMethod;
        public MethodInfo TeardownMethod;

        public List<TestInfo> TestsList = new List<TestInfo>();
    }
}
