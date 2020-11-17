﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
     public interface IAccountsDAO
    {
        Account GetAccount(int id);

        decimal GetBalance(int id);

    }
}
