namespace NamecheapUITests.PageObject.Interface
{
    public abstract class AUserInfoUpdation
    {
        public abstract void NewCardDetails(string cardUseAsOption);
        public abstract string BasicInformation();
        public abstract void BillingAddressDetails();
        public abstract void ContactInformation();
        public abstract void ManagingCardInfo(string empty);
        public abstract void LiveCard();
        public abstract void DummyCardDetails();
    }
}