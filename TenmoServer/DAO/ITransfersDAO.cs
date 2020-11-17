using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransfersDAO
    {
        Transfer InsertTransfer(Transfer transfer);

        List<Transfer> ListTransfers();

        Transfer GetTransfer(int id);

        Transfer UpdateBalance(Transfer transfer);
       

    }
}
