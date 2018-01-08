using System.Collections.Generic;
using System.Linq;
using NamecheapUITests.PageObject.HelperPages.WrapperFactory;
using NamecheapUITests.PageObject.Interface;

namespace NamecheapUITests.PageObject.HelperPages
{
    public class MergeData : AMerge
    {
        public override List<SortedDictionary<TKey, TValue>> MergingTwoListOfDic<TKey, TValue>(List<SortedDictionary<TKey, TValue>> list1ToBeMerged, List<SortedDictionary<TKey, TValue>> list2ToBeMergedWith)
        {
            var returnListDics = new List<SortedDictionary<TKey, TValue>>(list2ToBeMergedWith);
            var listDicCartItemsFromSearchCount = list1ToBeMerged.Count;
            for (var i = 0; i < listDicCartItemsFromSearchCount; i++)
            {
                foreach (var dicCartItemsFromSearch in list1ToBeMerged[i].Where(dicCartItemsFromSearch => !dicCartItemsFromSearch.Key.Equals(EnumHelper.Ssl.CertificateName.ToString())))
                {
                    returnListDics[i][dicCartItemsFromSearch.Key] = dicCartItemsFromSearch.Value;
                }
            }
            return returnListDics;
        }
    }

}