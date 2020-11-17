using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Account
    {
        public int accountId { get; set; }
        [Required(ErrorMessage = "You need to enter the account ID.")]
        public int userId { get; set; }
        public decimal balance { get; set; }


        
    }
}
