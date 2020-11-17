using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;
using TenmoServer.Security;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {

        private readonly IAccountsDAO AccountsSqlDAO;

        public AccountController(IAccountsDAO _accountsDAO)
        {
            AccountsSqlDAO = _accountsDAO;

        }

        [HttpGet("balance")]
        public ActionResult<decimal> GetAccountBalance()
        {
            int currentID = int.Parse(User.FindFirst("sub").Value);
            decimal balance = AccountsSqlDAO.GetBalance(currentID);
            if (balance >= 0)
            {
                return Ok(balance);
            }
            else return NotFound();
        }

        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(int id)
        {
            Account account = AccountsSqlDAO.GetAccount(id);
            if (account != null)
            {
                return Ok(account);
            }
            else return NotFound();
        }

      
    }
}
