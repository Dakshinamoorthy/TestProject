using System.Collections.Generic;

namespace NamecheapUITests.PageObject.Interface
{
    public abstract class AVerify
    {
        public abstract void VerifyTwoListOfDic(List<SortedDictionary<string, string>> newList,
          List<SortedDictionary<string, string>> listToBeVerified);
    }
}