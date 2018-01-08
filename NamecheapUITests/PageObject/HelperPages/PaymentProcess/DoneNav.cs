using System.Collections.Generic;
using NamecheapUITests.PageObject.Interface;
using NamecheapUITests.PageObject.ValidationPages;

namespace NamecheapUITests.PageObject.HelperPages.PaymentProcess
{
    public class DoneNav
    {
        public List<SortedDictionary<string, string>> ValidateInPurchaseSumarypage(List<SortedDictionary<string, string>> mergedScAndCartWidgetList)
        {
            APurchaseSummaryPageValidation ordersummary = new ValidatePurchaseSummary();
            var purcasedOrderNumber = ordersummary.PurchaseSummaryPage();
            AMerge mergeTWoListOfDic = new MergeData();
            var mergedPurchaseSummaryItemsAndScItemsList = mergeTWoListOfDic.MergingTwoListOfDic(purcasedOrderNumber, mergedScAndCartWidgetList);
            return mergedPurchaseSummaryItemsAndScItemsList;
        }
    }
}