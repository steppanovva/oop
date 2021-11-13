namespace Banks.Services
{
    public class CreditAccount : Account
    {
        public CreditAccount(Client client, double creditLimit, double commission)
            : base(client)
        {
            CreditLimit = creditLimit;
            Commission = commission;
            Money = creditLimit;
            Type = "credit";
        }

        private double CreditLimit { get; }
        private double Commission { get; }

        public override void Accrual()
        {
            if (Money < CreditLimit)
                PayOutSum -= Commission;
        }
    }
}