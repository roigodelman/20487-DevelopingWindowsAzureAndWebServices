using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    [ServiceContract]
    public interface IBankContract
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        bool Transfer(int from, int to, decimal amount);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void PrintAccounts(string title);
    }

}
