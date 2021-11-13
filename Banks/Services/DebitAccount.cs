namespace Banks.Services
{
    public class DebitAccount : Account
    {
        public DebitAccount(Client client, double percentage)
            : base(client)
        {
            Type = "debit";
            Percentage = percentage;
        }

        private double Percentage { get; }

        public override void Accrual()
        {
            PayOutSum += Percentage * Money;
        }
    }
}