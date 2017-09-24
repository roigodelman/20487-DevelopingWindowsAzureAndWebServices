using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BankDatabaseInitializer : System.Data.Entity.DropCreateDatabaseAlways<BankContext>
    {
        protected override void Seed(BankContext context)
        {
            Account account1 = new Account { AccountId = 1111, Balance = 1000 };
            Account account2 = new Account { AccountId = 2222, Balance = 2000 };
            Account account3 = new Account { AccountId = 3333, Balance = 3000 };
            Account account4 = new Account { AccountId = 4444, Balance = 4000 };

            context.Accounts.Add(account1);
            context.Accounts.Add(account2);
            context.Accounts.Add(account3);
            context.Accounts.Add(account4);

            context.SaveChanges();
        }
    }
}
