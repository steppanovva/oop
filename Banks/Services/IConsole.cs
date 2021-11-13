namespace Banks.Services
{
    public interface IConsole
    {
        public string StartMenu();
        public void TopUp(Bank bank, Account account);
        public void Transfer(Bank bank, Account accFrom);
        public void Withdraw(Bank bank, Account account);
        public void Balance(Account account);
        public void CreateAccount(Bank bank, Client client);
        public void CreateDebitAccount(Bank bank, Client client);
        public void CreateCreditAccount(Bank bank, Client client);
        public void CreateDepositAccount(Bank bank, Client client);
        public double EnterSumToOperate();
        public string EnterAccountId();
        public string EnterName();
        public int EnterId();
        public string AcceptAgreement();
        public string EnterAddress();
        public void ReadNotifications(Client client);
    }
}