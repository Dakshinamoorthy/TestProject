using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace TestRailResultProcessor.Test
{
    public static class TestResultsParser
    {
        public static List<TestCase> Parse(string pathToTestFile)
        {
            var res = new List<TestCase>();

            var doc = Load(pathToTestFile);

            var tests = doc.SelectNodes("//test-case");

            if (tests == null)
                return res;

            foreach (XmlNode t in tests)
            {
                var tc = new TestCase();

                if (t.Attributes != null)
                {
                    Debug.WriteLine($"{t.Attributes["name"].Value} - {t.Attributes["result"].Value}");

                    tc.Name = t.Attributes["name"].Value;
                    tc.Fulname = t.Attributes["fullname"].Value;
                    tc.Result = t.Attributes["result"].Value;
                    //tc.MethodName = t.Attributes["methodname"].Value;

                    if (t.Attributes["result"].Value != "Failed" && t.Attributes["result"].Value != "Failure")
                    {
                        res.Add(tc);
                        continue;
                    }
                    var selectSingleNode = t.SelectSingleNode("failure/stack-trace");

                    if (selectSingleNode != null)
                        tc.StackTrace = selectSingleNode.InnerText;

                    var singleNode = t.SelectSingleNode("failure/message");

                    if (singleNode != null)
                        tc.ErrorMessage = singleNode.InnerText;
                }

                res.Add(tc);
            }

            return res;
        }

        private static XmlElement Load(string pathToTestFile)
        {
            var doc = new XmlDocument();
            doc.Load(pathToTestFile);
            return doc.DocumentElement;
        }
    }
}
