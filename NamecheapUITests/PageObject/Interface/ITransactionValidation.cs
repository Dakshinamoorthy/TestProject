using System.Collections.Generic;
namespace NamecheapUITests.PageObject.Interface
{
    public interface ITransactionValidation
    {
        void VerifyPurchasedTransactionDetailsInBillingTransactionPage(
            List<SortedDictionary<string, string>> listOfDicNameToBeVerified);
    }
}