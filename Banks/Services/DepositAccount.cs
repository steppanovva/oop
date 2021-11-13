using System;

namespace Banks.Services
{
    public class DepositAccount : Account
    {
        public DepositAccount(Client client, double percentage, DateTime dateTime)
            : base(client)
        {
            Type = "deposit";
            Percentage = percentage;
            ExpirationDate = dateTime;
        }

        public DateTime ExpirationDate { get; }
        private double Percentage { get; }
        public override void Accrual()
        {
            PayOutSum += Money * Percentage;
        }
    }
}