using System;
using System.Linq;

namespace Banks.Services
{
    public class BankConsole : IConsole
    {
        public string StartMenu()
        {
            Console.WriteLine("Choose an action:\n");
            Console.WriteLine("1. Create account\n" +
                              "2. Withdraw cash\n" +
                              "3. Transfer money\n" +
                              "4. Top up account\n" +
                              "5. Balance\n" +
                              "6. Read notifications\n" +
                              "7. Quit\n");
            return Console.ReadLine();
        }

        public void Balance(Account account)
        {
            Console.WriteLine("BALANCE: " + account.Money + "\n");
        }

        public void CreateAccount(Bank bank, Client client)
        {
            Console.WriteLine("Choose a type of account:\n" +
                              "1. Debit\n" +
                              "2. Credit\n" +
                              "3. Deposit\n");
            switch (Console.ReadLine())
            {
                case "1":
                    CreateDebitAccount(bank, client);
                    break;
                case "2":
                    CreateCreditAccount(bank, client);
                    break;
                case "3":
                    CreateDepositAccount(bank, client);
                    break;
            }

            Console.WriteLine("Account created successfully\n");
        }

        public void CreateDebitAccount(Bank bank, Client client)
        {
            bank.AddClient(client, AcceptAgreement());
            var account = new DebitAccount(client, bank.PercentageOnDailyBalance) { Id = "debit" + Bank.Id };
            Bank.Id++;
            bank.Accounts.Add(account);
            bank.DebitAccounts.Add(account);
        }

        public void CreateCreditAccount(Bank bank, Client client)
        {
            bank.AddClient(client, AcceptAgreement());
            var account = new CreditAccount(client, bank.BaseCreditLimit, bank.Commission) { Id = "credit" + Bank.Id };
            Bank.Id++;
            bank.Accounts.Add(account);
            bank.CreditAccounts.Add(account);
        }

        public void CreateDepositAccount(Bank bank, Client client)
        {
            bank.AddClient(client, AcceptAgreement());
            var account = new DepositAccount(client, bank.PercentageOnDailyBalance, DateTime.Today.AddYears(1)) { Id = "deposit" + Bank.Id };
            Bank.Id++;
            bank.Accounts.Add(account);
            bank.DepositAccounts.Add(account);
        }

        public double EnterSumToOperate()
        {
            Console.WriteLine("Enter sum to operate with:");
            return Convert.ToDouble(Console.ReadLine());
        }

        public string EnterAccountId()
        {
            Console.WriteLine("Enter Id of an account you want to transfer money to: ");
            return Console.ReadLine();
        }

        public void Transfer(Bank bank, Account accFrom)
        {
            Account accTo = bank.Accounts.Find(x => x.Id == EnterAccountId());
            bank.Transfer(EnterSumToOperate(), accFrom, accTo);
            Console.WriteLine("SUCCESS\n");
        }

        public void TopUp(Bank bank, Account account)
        {
            bank.TopUp(EnterSumToOperate(), account);
            Console.WriteLine("SUCCESS\n");
        }

        public void Withdraw(Bank bank, Account account)
        {
            bank.Withdraw((int)EnterSumToOperate(), account);
            Console.WriteLine("SUCCESS\n");
        }

        public string EnterName()
        {
            Console.WriteLine("Enter your name: ");
            return Console.ReadLine();
        }

        public int EnterId()
        {
            Console.WriteLine("Enter your Id: ");
            return Convert.ToInt32(Console.ReadLine());
        }

        public string AcceptAgreement()
        {
            Console.WriteLine("Do you want to get notifications?");
            return Console.ReadLine();
        }

        public string EnterAddress()
        {
            Console.WriteLine("Do you want to enter address? An account without it will work with restrictions.");
            string answer = Console.ReadLine();
            if (answer is not("yes" or "Yes")) return null;
            Console.WriteLine("Enter address:");
            return Console.ReadLine();
        }

        public void ReadNotifications(Client client)
        {
            if (!client.MessagesFromBank.Any())
                Console.WriteLine("No messages");
            foreach (string message in client.MessagesFromBank)
            {
                Console.WriteLine(message + "\n");
            }
        }
    }
}