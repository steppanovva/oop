using System;
using System.Collections.Generic;
using Banks.Tools;

namespace Banks.Services
{
    public class Bank : CentralBank.IBank
    {
        private static int _transactionNumber;
        public Bank(double commission, double percentageOnDailyBalance, double baseCreditLimit, int maxSumToOperateWithIncompleteAcc, string name)
        {
            Commission = commission;
            PercentageOnDailyBalance = percentageOnDailyBalance;
            BaseCreditLimit = baseCreditLimit;
            MaxSumToOperateWithIncompleteAcc = maxSumToOperateWithIncompleteAcc;
            Name = name;
        }

        public delegate void MaxSumChanged(string message);
        public delegate void BaseCreditLimitChanged(string message);
        public event MaxSumChanged NotifyMaxSumChanged;
        public event BaseCreditLimitChanged NotifyBaseCreditLimitChanged;
        public string Name { get; set; }
        public double Commission { get; }
        public double BaseCreditLimit { get; private set; }
        public List<Transaction> Transactions { get;  } = new ();
        public double PercentageOnDailyBalance { get; }
        public List<Client> Clients { get; } = new ();
        public List<CreditAccount> CreditAccounts { get; } = new ();
        public List<DepositAccount> DepositAccounts { get; } = new ();
        public List<DebitAccount> DebitAccounts { get; } = new ();
        public List<Account> Accounts { get; } = new ();
        private int MaxSumToOperateWithIncompleteAcc { get; set; }

        public void AddClient(Client client, string agreement)
        {
            Clients.Add(client);
            if (agreement == "no") return;
            NotifyMaxSumChanged += client.GetMessage;
            NotifyBaseCreditLimitChanged += client.GetMessage;
        }

        public void AddAccount(Account account)
        {
            Accounts.Add(account);
            switch (account.Type)
            {
                case "credit":
                    CreditAccounts.Add(account as CreditAccount);
                    break;
                case "deposit":
                    DepositAccounts.Add(account as DepositAccount);
                    break;
                case "debit":
                    DebitAccounts.Add(account as DebitAccount);
                    break;
            }
        }

        public void Withdraw(int amount, Account account)
        {
            if (account.Owner.Address == null && amount > MaxSumToOperateWithIncompleteAcc)
            {
                throw new BanksException("You can not withdraw more than " + MaxSumToOperateWithIncompleteAcc +
                                         " with incomplete account. Fill in the missing information.");
            }

            if (account.Type == "deposit")
            {
                var acc = (DepositAccount)account;
                if (DateTime.Today < acc.ExpirationDate)
                    throw new BanksException("You can not withdraw money before expiration date");
            }

            if (amount > account.Money)
                throw new BanksException("There are not enough funds in the account");
            account.Money -= amount;
        }

        public void TopUp(double amount, Account account)
        {
            if (account.Owner.Address == null && amount > MaxSumToOperateWithIncompleteAcc)
            {
                throw new BanksException("You can not top up by more than " + MaxSumToOperateWithIncompleteAcc +
                                         " with incomplete account. Fill in the missing information.");
            }

            account.Money += amount;
        }

        public void Transfer(double amount, Account accFrom, Account accTo)
        {
            if (accFrom.Owner.Address == null && amount > MaxSumToOperateWithIncompleteAcc)
            {
                throw new BanksException("You can not transfer more than " + MaxSumToOperateWithIncompleteAcc +
                                         " with incomplete account. Fill in the missing information.");
            }

            if (accTo.Owner.Address == null && amount > MaxSumToOperateWithIncompleteAcc)
            {
                throw new BanksException("You can not withdraw more than " + MaxSumToOperateWithIncompleteAcc +
                                         " to a person with incomplete account.");
            }

            if (amount > accFrom.Money)
                throw new BanksException("There are not enough funds in the account " + accFrom.Id);
            accFrom.Money -= amount;
            accTo.Money += amount;
            var transaction = new Transaction("transfer" + _transactionNumber, accFrom.Id, accTo.Id, amount);
            _transactionNumber++;
            Transactions.Add(transaction);
        }

        public void CancelTransaction(string transactionId)
        {
            Transaction transaction = Transactions.Find(x => x.Id == transactionId);
            if (transaction == null)
                throw new BanksException("There is no transaction with id " + transactionId);
            Account accFrom = Accounts.Find(x => x.Id == transaction.AccountFrom);
            Account accTo = Accounts.Find(x => x.Id == transaction.AccountTo);
            if (accFrom == null || accTo == null)
                throw new BanksException("Incorrect data entered");
            accFrom.Money += transaction.Money;
            accTo.Money -= transaction.Money;
            Transactions.Remove(transaction);
        }

        public void ChangeMaxSumToOperateWithIncompleteAcc(int sum)
        {
            MaxSumToOperateWithIncompleteAcc = sum;
            NotifyMaxSumChanged?.Invoke("MaxSum changed to " + sum);
        }

        public void ChangeBaseCreditLimit(int sum)
        {
            BaseCreditLimit = sum;
            NotifyBaseCreditLimitChanged?.Invoke("BaseCreditLimit change to " + sum);
        }

        public void PayOut()
        {
            foreach (Account account in Accounts)
            {
                account.Pay();
            }
        }
    }
}