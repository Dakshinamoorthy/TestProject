using System.Collections.Generic;
namespace NamecheapUITests.PageObject.Interface
{
    public interface IOrdersValidation
    {
        void VerifyPurchasedOrderInBillingHistoryPage(
            List<SortedDictionary<string, string>> listOfDicNameToBeVerified);
    }
}