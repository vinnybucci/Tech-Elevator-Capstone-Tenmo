using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {
        private static ITransfersDAO TransfersSqlDAO;
        private static IAccountsDAO AccountsSqlDAO;
        private static IUserDAO UserSqlDAO;

        
        public TransferController(ITransfersDAO _transferDAO, IAccountsDAO _accountDAO, IUserDAO _userDAO)
        {
            TransfersSqlDAO = _transferDAO;
            AccountsSqlDAO = _accountDAO;
            UserSqlDAO = _userDAO;
        }

        [HttpPost]
        public ActionResult<Transfer> InsertTransfer(Transfer transferToInsert)
        {
            Transfer transfer = TransfersSqlDAO.InsertTransfer(transferToInsert);
            if (transfer != null)
            {
                return Ok(transfer);
            }
            else return NotFound();

        }

        [HttpGet]
        public List<Transfer> ListTransfers()
        {
            return TransfersSqlDAO.ListTransfers();
        }

        [HttpGet("{id}")]
        public ActionResult<Transfer> GetTransfer(int id)
        {
            Transfer transfer = TransfersSqlDAO.GetTransfer(id);
            if (transfer != null)
            {
                return Ok(transfer);
            }
            else return NotFound();
        }

        [HttpPut("{id}")]
        public ActionResult<Transfer> UpdateBalance(Transfer transfer)
        {
            if (transfer == null)
            {
                return NotFound();
            }
            Transfer verifiedTransfer = TransfersSqlDAO.GetTransfer(transfer.TransferID);
            if (verifiedTransfer.TransferStatusID == 2) 
            {
                TransfersSqlDAO.UpdateBalance(verifiedTransfer);
                return Ok(transfer);
            }
            else return BadRequest();

        }

       
    }
}
