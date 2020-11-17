using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferID { get; set; }
        public int TransferTypeID { get; set; }
        public string TransferTypeDescription { get; set; }
        public int TransferStatusID { get; set; }
        public string TransferStatusDescription { get; set; }
        public int UserFromID { get; set; }
        public string UsernameFrom { get; set; }
        public int UserToID { get; set; }
        public string UsernameTo { get; set; }
        public decimal TransferAmount { get; set; }
    }
   
}
