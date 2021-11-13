using System;
using System.Collections.Generic;

namespace Banks.Services
{
    public class CentralBank
    {
        public delegate void PayOutTime();

        public event PayOutTime NotifyToPayOut;
        public interface IBank
        {
            public void AddClient(Client client, string agreement);
            public void AddAccount(Account account);
            public void Withdraw(int amount, Account account);
            public void TopUp(double amount, Account account);
            public void Transfer(double amount, Account accFrom, Account accTo);
            public void CancelTransaction(string transactionId);
            public void ChangeMaxSumToOperateWithIncompleteAcc(int sum);
            public void ChangeBaseCreditLimit(int sum);
            public void PayOut();
        }

        public List<Bank> Banks { get; } = new ();

        public void ToPayOut(DateTime date)
        {
            if (date.Day != 13) return;
            NotifyToPayOut?.Invoke();
        }

        public void RegisterBank(Bank bank)
        {
            Banks.Add(bank);
            NotifyToPayOut += bank.PayOut;
        }
    }
}