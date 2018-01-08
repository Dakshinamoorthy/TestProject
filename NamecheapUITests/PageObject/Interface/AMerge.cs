using System.Collections.Generic;

namespace NamecheapUITests.PageObject.Interface
{
    public abstract class AMerge
    {
        public abstract List<SortedDictionary<TKey, TValue>> MergingTwoListOfDic<TKey, TValue>(
         List<SortedDictionary<TKey, TValue>> list1ToBeMerged,
         List<SortedDictionary<TKey, TValue>> list2ToBeMergedWith);
    }
}