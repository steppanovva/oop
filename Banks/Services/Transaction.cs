namespace Banks.Services
{
    public class Transaction
    {
        public Transaction(string id, string accountFrom, string accountTo, double money)
        {
            Id = id;
            AccountFrom = accountFrom;
            AccountTo = accountTo;
            Money = money;
        }

        public string Id { get; set; }
        public string AccountFrom { get; set; }
        public string AccountTo { get; set; }
        public double Money { get; set; }
    }
}