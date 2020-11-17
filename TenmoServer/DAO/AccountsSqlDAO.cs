using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountsDAO
    {
        private readonly string connectionString;

        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Account GetAccount(int userId)
        {
            Account account = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT account_id, user_id, balance FROM accounts " +
                        "WHERE user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", userId); 
                    SqlDataReader reader = cmd.ExecuteReader(); 

                    if (reader.HasRows && reader.Read())
                    {
                        account = GetAccountFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            { 
                Console.WriteLine("There was a problem with the database connection.");
            }

            return account;
        }

        public decimal GetBalance(int userID)
        {
            Account account = new Account();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT balance FROM accounts JOIN users ON users.user_id = accounts.account_id WHERE users.user_id = @user_id", conn);
                    cmd.Parameters.AddWithValue("@user_id", userID);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            account.balance = Convert.ToDecimal(reader["balance"]);
                        }
                    }

                }
            }
            catch (SqlException)
            {
                throw;
            }
            return account.balance;
        }
       

        private Account GetAccountFromReader(SqlDataReader reader) 
        {
            Account account = new Account()
            {
                accountId = Convert.ToInt32(reader["account_id"]),
                userId = Convert.ToInt32(reader["user_id"]),
                balance = Convert.ToDecimal(reader["balance"]),
            };

            return account;
        }
    }
}

