namespace Banks.Services
{
    public class Account
    {
        protected Account(Client client)
        {
            Owner = client;
        }

        public Client Owner { get; }
        public double Money { get; set; }
        public string Id { get; init; }
        public string Type { get; protected init; }
        protected double PayOutSum { get; set; }

        public virtual void Accrual()
        {
            PayOutSum = Money;
        }

        public void Pay()
        {
            Money += PayOutSum;
        }
    }
}