using System;
using System.Linq;
using NUnit.Framework;
using Banks.Services;
using Banks.Tools;

namespace Banks.Tests
{
    [TestFixture]
    public class BanksTest
    {
        [Test]
        public void SubscriptionToNotifications_NotificationsSent()
        {
            var centralBank = new CentralBank();
            var bank = new Bank(5, 0.01, 50000, 1000, "bank1");
            centralBank.RegisterBank(bank);
            Assert.IsTrue(centralBank.Banks.Exists(x => x.Name == "bank1"));
            var director = new Director();
            var builder = new ClientBuilder();
            director.Builder = builder;
            director.BuildMinimalClient("Petr Petrov", 123);
            Client client1 = builder.GetClient();
            director.BuildFullClient("Maxim Maximov", "Rostov", 124);
            Client client2 = builder.GetClient();
            bank.AddClient(client1, "no");
            bank.AddClient(client2, "yes");
            var account1 = new DebitAccount(client1, bank.PercentageOnDailyBalance) { Id = "debit1" };
            var account2 = new CreditAccount(client2, bank.BaseCreditLimit, bank.Commission) { Id = "credit1" };
            bank.AddAccount(account1);
            bank.AddAccount(account2);
            client1.AccountId = account1.Id;
            client2.AccountId = account2.Id;
            bank.ChangeBaseCreditLimit(55000);
            Assert.IsTrue(bank.Clients.Exists(x => x.Name == "Petr Petrov"));
            Assert.IsTrue(bank.Clients.Exists(x => x.Name == "Maxim Maximov"));
            Assert.IsTrue(bank.CreditAccounts.Exists(x => x.Owner.Name == "Maxim Maximov"));
            Assert.IsTrue(bank.DebitAccounts.Exists(x => x.Owner.Name == "Petr Petrov"));
            Assert.IsTrue(client1.MessagesFromBank.Count == 0);
            Assert.IsTrue(client2.MessagesFromBank.Count == 1);
        }

        [Test]
        public void TopUpMoreThanAllowed_ThrowException()
        {
            var centralBank = new CentralBank();
            var bank = new Bank(5, 0.01, 50000, 1000, "bank");
            centralBank.RegisterBank(bank);
            var director = new Director();
            var builder = new ClientBuilder();
            director.Builder = builder;
            director.BuildMinimalClient("Petr Petrov", 123);
            Client client1 = builder.GetClient();
            director.BuildFullClient("Maxim Maximov", "Rostov", 124);
            Client client2 = builder.GetClient();
            bank.AddClient(client1, "no");
            bank.AddClient(client2, "yes");
            var account1 = new DebitAccount(client1, bank.PercentageOnDailyBalance) { Id = "debit1" };
            var account2 = new CreditAccount(client2, bank.BaseCreditLimit, bank.Commission) { Id = "credit1" };
            bank.AddAccount(account1);
            bank.AddAccount(account2);
            client1.AccountId = account1.Id;
            client2.AccountId = account2.Id;
            Assert.Catch<BanksException>(() =>
            {
                bank.TopUp(1100, account1);
            });
            Assert.Catch<BanksException>(() =>
            {
                bank.Transfer(3000, account1, account2);
            });
        }

        [Test]
        public void CancelTransaction_TransactionCanceled()
        {
            var centralBank = new CentralBank();
            var bank = new Bank(5, 0.01, 50000, 1000, "bank");
            centralBank.RegisterBank(bank);
            var director = new Director();
            var builder = new ClientBuilder();
            director.Builder = builder;
            director.BuildMinimalClient("Petr Petrov", 123);
            Client client1 = builder.GetClient();
            director.BuildFullClient("Maxim Maximov", "Rostov", 124);
            Client client2 = builder.GetClient();
            bank.AddClient(client1, "no");
            bank.AddClient(client2, "yes");
            var account1 = new DebitAccount(client1, bank.PercentageOnDailyBalance) { Id = "debit1" };
            var account2 = new CreditAccount(client2, bank.BaseCreditLimit, bank.Commission) { Id = "credit1" };
            bank.AddAccount(account1);
            bank.AddAccount(account2);
            client1.AccountId = account1.Id;
            client2.AccountId = account2.Id;
            bank.Transfer(500, account2, account1);
            bank.CancelTransaction(bank.Transactions.Last().Id);
            Assert.IsTrue(account2.Money == 50000 && account1.Money == 0);
            Assert.IsTrue(bank.Transactions.Count == 0);
        }

        [Test]
        public void WithdrawMoneyBeforeExpirationDate_ThrowException()
        {
            var centralBank = new CentralBank();
            var bank = new Bank(5, 0.01, 50000, 1000, "bank");
            centralBank.RegisterBank(bank);
            var director = new Director();
            var builder = new ClientBuilder();
            director.Builder = builder;
            director.BuildFullClient("Maxim Maximov", "Rostov", 124);
            Client client = builder.GetClient();
            bank.AddClient(client, "yes");
            var dateTime = new DateTime(2022, 10, 01);
            var account = new DepositAccount(client, bank.PercentageOnDailyBalance, dateTime);
            bank.AddAccount(account);
            bank.TopUp(200, account);
            Assert.IsTrue(bank.DepositAccounts.Exists(x => x.Owner.Name == "Maxim Maximov"));
            Assert.Catch<BanksException>(() =>
            {
                bank.Withdraw(200, account);
            });
        }

        [Test]
        public void EventPayOut_PaidOut()
        {
            var centralBank = new CentralBank();
            var bank = new Bank(5, 0.01, 50000, 1000, "bank");
            centralBank.RegisterBank(bank);
            var director = new Director();
            var builder = new ClientBuilder();
            director.Builder = builder;
            director.BuildMinimalClient("Petr Petrov", 123);
            Client client1 = builder.GetClient();
            director.BuildFullClient("Maxim Maximov", "Rostov", 124);
            Client client2 = builder.GetClient();
            bank.AddClient(client1, "no");
            bank.AddClient(client2, "yes");
            var account1 = new DebitAccount(client1, bank.PercentageOnDailyBalance) { Id = "debit1" };
            var account2 = new CreditAccount(client2, bank.BaseCreditLimit, bank.Commission) { Id = "credit1" };
            bank.AddAccount(account1);
            bank.AddAccount(account2);
            client1.AccountId = account1.Id;
            client2.AccountId = account2.Id;
            account1.Money = 300;
            var date = new DateTime(2021, 11, 11);
            for (int i = 0; i < 3; i++)
            {
                date = date.AddDays(1);
                foreach (var account in bank.Accounts)
                    account.Accrual();
                centralBank.ToPayOut(date);
            }
            Assert.IsTrue(account1.Money == 306 && account2.Money == bank.BaseCreditLimit);
        }
    }
}