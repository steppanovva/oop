using Banks.Services;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            // var bankConsole = new BankConsole();
            // var centralBank = new CentralBank();
            // var bank = new Bank(5, 0.01, 50000, 1000, "bank1");
            // centralBank.RegisterBank(bank);
            // var director = new Director();
            // var builder = new ClientBuilder();
            // director.Builder = builder;
            // string name = bankConsole.EnterName();
            // int id = bankConsole.EnterId();
            // string address = bankConsole.EnterAddress();
            // director.BuildFullClient(name, address, id);
            // Client client = builder.GetClient();
            // director.BuildFullClient("Maxim Maximov", "Rostov", 124);
            // Client client2 = builder.GetClient();
            // bank.AddClient(client2, "yes");
            // var account2 = new CreditAccount(client2, bank.BaseCreditLimit, bank.Commission) { Id = "credit1" };
            // bank.AddAccount(account2);
            // client2.AccountId = account2.Id;
            // client2.Account = account2;
            // while (true)
            // {
            //     switch (bankConsole.StartMenu())
            //     {
            //         case "1":
            //             bankConsole.CreateAccount(bank, client);
            //             break;
            //         case "2":
            //             bankConsole.Withdraw(bank, client.Account);
            //             break;
            //         case "3":
            //             bankConsole.Transfer(bank, client.Account);
            //             break;
            //         case "4":
            //             bankConsole.TopUp(bank, client.Account);
            //             break;
            //         case "5":
            //             bankConsole.Balance(client.Account);
            //             break;
            //         case "6":
            //             bankConsole.ReadNotifications(client);
            //             break;
            //         case "7":
            //             return;
            //     }
            // }
        }
    }
}
