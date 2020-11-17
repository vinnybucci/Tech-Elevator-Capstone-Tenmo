using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDAO : ITransfersDAO
    {
        private readonly string connectionString;

        public TransferSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Transfer InsertTransfer(Transfer transfer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount)" +
                        " OUTPUT Inserted.transfer_id VALUES (@transfer_type_id, @transfer_status_id, @account_from, @account_to, @amount)", conn);

                    cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferTypeID);
                    cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatusID);
                    cmd.Parameters.AddWithValue("@account_from", transfer.UserFromID);
                    cmd.Parameters.AddWithValue("@account_to", transfer.UserToID);
                    cmd.Parameters.AddWithValue("@amount", transfer.TransferAmount);

                    int transferID = (int)cmd.ExecuteScalar();
                    transfer.TransferID = transferID;
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return transfer;
        }
        public List<Transfer> ListTransfers()
        {
            List<Transfer> transfers = new List<Transfer>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT transfer_id, a.username as userfrom, b.username as userto, transfers.transfer_type_id," +
                                           " transfers.transfer_status_id, transfer_types.transfer_type_desc, transfer_statuses.transfer_status_desc," +
                                           " account_from, account_to, amount" + " FROM transfers" +
                                           " JOIN transfer_statuses ON transfer_statuses.transfer_status_id = transfers.transfer_status_id" +
                                           " JOIN transfer_types ON transfer_types.transfer_type_id = transfers.transfer_type_id" +
                                           " JOIN accounts AS f ON f.account_id = transfers.account_from" +
                                           " JOIN accounts AS t ON t.account_id = transfers.account_to" +
                                           " JOIN users AS a ON a.user_id = f.user_id" +
                                           " JOIN users AS b ON b.user_id = t.user_id", conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Transfer t = GetTransferFromReader(reader);
                            transfers.Add(t);
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return transfers;
        }
        public Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer transfer = new Transfer()
            {
                TransferID = Convert.ToInt32(reader["transfer_id"]),
                TransferTypeID = Convert.ToInt32(reader["transfer_type_id"]),
                TransferTypeDescription = Convert.ToString(reader["transfer_type_desc"]),
                TransferStatusID = Convert.ToInt32(reader["transfer_status_id"]),
                TransferStatusDescription = Convert.ToString(reader["transfer_status_desc"]),
                UserFromID = Convert.ToInt32(reader["account_from"]),
                UsernameFrom = Convert.ToString(reader["userfrom"]),
                UserToID = Convert.ToInt32(reader["account_to"]),
                UsernameTo = Convert.ToString(reader["userto"]),
                TransferAmount = Convert.ToDecimal(reader["amount"])

            };
            return transfer;
        }
        public Transfer GetTransfer(int id)
        {
            Transfer transfer = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT transfer_id, a.username as userfrom, b.username as userto, transfers.transfer_type_id," +
                                          " transfers.transfer_status_id, transfer_types.transfer_type_desc, transfer_statuses.transfer_status_desc," +
                                          " account_from, account_to, amount" + " FROM transfers" +
                                          " JOIN transfer_statuses ON transfer_statuses.transfer_status_id = transfers.transfer_status_id" +
                                          " JOIN transfer_types ON transfer_types.transfer_type_id = transfers.transfer_type_id" +
                                          " JOIN accounts AS f ON f.account_id = transfers.account_from" +
                                          " JOIN accounts AS t ON t.account_id = transfers.account_to" +
                                          " JOIN users AS a ON a.user_id = f.user_id" +
                                          " JOIN users AS b ON b.user_id = t.user_id" +
                                          " WHERE transfer_id = @id", conn);

                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        transfer = GetTransferFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return transfer;
        }
        public Transfer UpdateBalance(Transfer transfer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = conn.CreateCommand();
                    SqlTransaction transaction = conn.BeginTransaction();

                    cmd.Connection = conn;
                    cmd.Transaction = transaction;

                    cmd.Parameters.AddWithValue("@transferAmount", transfer.TransferAmount);

                    cmd.CommandText = "UPDATE accounts SET balance = (balance - @transferAmount) WHERE account_id = @accountFrom";
                    cmd.Parameters.AddWithValue("@accountFrom", transfer.UserFromID);
                    bool isSubtracted = cmd.ExecuteNonQuery() > 0;

                    cmd.CommandText = "UPDATE accounts SET balance = (balance + @transferAmount) WHERE account_id = @accountTo";
                    cmd.Parameters.AddWithValue("@accountTo", transfer.UserToID);
                    bool isAdded = cmd.ExecuteNonQuery() > 0;
                    transaction.Commit();

                    if (isSubtracted && isAdded)
                    {
                        return transfer;
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return null;
        }
       
    }
}