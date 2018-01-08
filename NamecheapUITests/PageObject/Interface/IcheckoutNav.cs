using System.Security.Cryptography.X509Certificates;

namespace NamecheapUITests.PageObject.Interface
{
    public interface ICheckoutNav
    {
        bool PlaceOrderCheckoutFlow(string paymentOption = "", bool changePaymentMode = true);
    }
}