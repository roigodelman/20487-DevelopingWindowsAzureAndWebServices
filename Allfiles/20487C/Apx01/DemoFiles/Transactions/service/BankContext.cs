using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BankContext : DbContext
    {        
        public DbSet<Account> Accounts { get; set; }
    }
}
