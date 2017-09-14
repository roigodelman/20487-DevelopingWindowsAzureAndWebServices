using Contract;
using System;
using System.ServiceModel;
using System.Transactions;

namespace Client
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Client";

            ChannelFactory<IBankContract> factory = new ChannelFactory<IBankContract>(
                new NetTcpBinding { TransactionFlow = true },
                new EndpointAddress("net.tcp://localhost:8080/"));

            IBankContract proxy = factory.CreateChannel();

            Console.WriteLine("Press Enter after the service starts");
            Console.ReadLine();
            // Print the accounts balance, before starting the distributed transactions
            proxy.PrintAccounts("Before");

            // Start first distributed transaction
            using (TransactionScope scope = new TransactionScope())
            {
                // Perform two transfers and then set the transaction to complete
                Console.WriteLine("Starting a transaction");
                Console.WriteLine("Tranfering 100 from account 1111 to 2222");
                proxy.Transfer(1111, 2222, 100);
                Console.WriteLine("Tranfering 200 from account 3333 to 4444");
                proxy.Transfer(3333, 4444, 200);
                proxy.PrintAccounts("After transfers");
                scope.Complete();
            }
            Console.WriteLine("Transaction complete");
            proxy.PrintAccounts("After transaction ended (Complete() called)");

            using (TransactionScope scope = new TransactionScope())
            {
                // Perform two transfers without setting transaction to copmlete
                Console.WriteLine("Starting a transaction");
                Console.WriteLine("Tranfering 100 from account 1111 to 2222");
                proxy.Transfer(1111, 2222, 100);
                Console.WriteLine("Tranfering 200 from account 3333 to 4444");
                proxy.Transfer(3333, 4444, 200);
                proxy.PrintAccounts("After transfers");
                // Not setting the transaction to complete will rollback the changes
                //scope.Complete();
            }
            Console.WriteLine("Transaction complete");
            proxy.PrintAccounts("After transaction ended (Complete() not called)");
        }
    }
}
