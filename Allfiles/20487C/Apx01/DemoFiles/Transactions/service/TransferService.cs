using System;
using System.ServiceModel;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using Contract;
using System.Transactions;

namespace Service
{
    public class TransferService : IBankContract
    {
        [OperationBehavior(TransactionScopeRequired=true, TransactionAutoComplete=true)]
        public bool Transfer(int from, int to, decimal amount)
        {
            bool result = false;

            using (BankContext context = new BankContext())
            {
                Account fromAccount = context.Accounts.Single(a => a.AccountId == from);
                Account toAccount = context.Accounts.Single(a => a.AccountId == to);

                if (fromAccount.Balance >= amount)
                {
                    fromAccount.Balance -= amount;
                    toAccount.Balance += amount;

                    context.SaveChanges();
                    result = true;
                }                
            }
            return result;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void PrintAccounts(string title)
        {
            using (BankContext context = new BankContext())
            {
                Console.WriteLine(title);
                foreach (Account account in context.Accounts)
                {
                    Console.WriteLine("Account {0}: {1}", account.AccountId, account.Balance);
                }
                Console.WriteLine();
            }
        }
    }

}
