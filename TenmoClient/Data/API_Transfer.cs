using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class API_Transfer
    {
        public int transferID { get; set; }
        public int transferTypeID { get; set; }
        public string transferTypeDescription { get; set; }
        public int transferStatusID { get; set; }
        public string transferStatusDescription { get; set; }
        public int userFromID { get; set; }
        public string usernameFrom { get; set; }
        public int userToID { get; set; }
        public string usernameTo { get; set; }
        public decimal transferAmount { get; set; }
    }
}

