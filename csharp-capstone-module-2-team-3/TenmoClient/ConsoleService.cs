using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TenmoClient.Data;
using TenmoServer.Models;

namespace TenmoClient
{
    public class ConsoleService
    {
        /// <summary>
        /// Prompts for transfer ID to view, approve, or reject
        /// </summary>
        /// <param name="action">String to print in prompt. Expected values are "Approve" or "Reject" or "View"</param>
        /// <returns>ID of transfers to view, approve, or reject</returns>
        public int PromptForTransferID(string action)
        {
            Console.WriteLine("");
            Console.Write("Please enter transfer ID to " + action + " (0 to cancel): ");
            if (!int.TryParse(Console.ReadLine(), out int auctionId))
            {
                Console.WriteLine("Invalid input. Only input a number.");
                return 0;
            }
            else
            {
                return auctionId;
            }
        }

        public Data.LoginUser PromptForLogin()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            string password = GetPasswordFromConsole("Password: ");

            Data.LoginUser loginUser = new Data.LoginUser
            {
                Username = username,
                Password = password
            };
            return loginUser;
        }

        private string GetPasswordFromConsole(string displayMessage)
        {
            string pass = "";
            Console.Write(displayMessage);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar))
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Remove(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine("");
            return pass;
        }



        public static int GetNumberInRange(int min, int max)
        {
            string userInput = ""; 
            int value = 0;
            bool gettingNumberInRange = true;

            do
            {
                userInput = Console.ReadLine();
                if (!int.TryParse(userInput, out value))
                {
                    Console.WriteLine("Invalid input format. Please try again");
                }
                else
                {
                    if (min <= value && value <= max)
                    {
                        gettingNumberInRange = false;
                    }
                    else
                    {
                        Console.WriteLine($"Number you entered is not between {min} and {max}. Please try again.");
                    }
                }
            }
            while (gettingNumberInRange);

            return value;
        }

        public API_Transfer StartTransfer(List<API_User> users)
        {
            int selection = -1;
            while (selection != 0)
            {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Users");
                Console.WriteLine("ID         Name");
                Console.WriteLine("-------------------------------------");
                foreach (API_User user in users)
                {
                    Console.WriteLine($"{user.UserId}: {user.Username}");
                }
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("");
                API_Transfer transfer = new API_Transfer()
                {
                    userFromID = UserService.GetUserId(),
                    userToID = UserToReceiveTransfer(users),
                    transferStatusID = 2,
                    transferTypeID = 2,
                    transferAmount = AmountToTransfer()
                };
                return transfer;
            }
            return null;
        }

        public void WriteTransferList(List<API_Transfer> transfers)
        {
            int selection = -1;
            while (selection != 0)
            {
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Transfers");
                Console.WriteLine("ID        From/To      Amount");
                Console.WriteLine("-------------------------------------");
                foreach (API_Transfer transfer in transfers)
                {
                    if (transfer.userFromID == UserService.GetUserId())
                    {
                        Console.WriteLine($"{transfer.transferID} To: {transfer.usernameTo} ${transfer.transferAmount}");
                    }
                    else if (transfer.userFromID != UserService.GetUserId())
                    {
                        Console.WriteLine($"{transfer.transferID} From: {transfer.usernameFrom} ${transfer.transferAmount}");
                    }
                }
                break;
            }
        }
        public void WriteTransferDetail(List<API_Transfer> transfers, int id)
        {

            Console.WriteLine("Transfer Details");
            
            foreach (API_Transfer transfer in transfers)
            {
                if (transfer.transferID == id)
                {
                    Console.WriteLine($"Id: {transfer.transferID} ");
                    Console.WriteLine($"From: {transfer.usernameFrom}");
                    Console.WriteLine($"To: {transfer.usernameTo} ");
                    Console.WriteLine($"Type: {transfer.transferTypeDescription}");
                    Console.WriteLine($"Status: {transfer.transferStatusDescription}");
                    Console.WriteLine($"Amount: {transfer.transferAmount}");
                    break;
                }
            }
        }

        public int TransferToDetail(List<API_Transfer> transfers)
        {
            int inputID = -1;
            bool choosingID = false;
            while (!choosingID)
               
            {              
                Console.WriteLine("Please enter transfer ID to view details (0 to cancel): ");

                {
                    if (!int.TryParse(Console.ReadLine(), out inputID))
                    {
                        Console.WriteLine("Invalid input. Only input a number.");
                    }
                    choosingID = true;
                }
            }
            return inputID;
        }
        public void GetTransferDetails(API_Transfer transfer)
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Transfer Details");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("");
            Console.WriteLine($"ID: {transfer.transferID}");
            Console.WriteLine($"From: {transfer.usernameFrom}");
            Console.WriteLine($"To: {transfer.usernameTo}");
            Console.WriteLine($"Type: {transfer.transferTypeDescription}");
            Console.WriteLine($"Status: {transfer.transferStatusDescription}");
            Console.WriteLine($"Amount: ${transfer.transferAmount}");
        }

        public int UserToReceiveTransfer(List<API_User> users)
        {
            int inputID = -1;
            bool choosingID = false;
            while (!choosingID)
            {
                Console.WriteLine("Enter ID of user you are sending to (0 to cancel):");
                
                if (!int.TryParse(Console.ReadLine(), out inputID))
                {
                    Console.WriteLine("Invalid input. Only input a number.");

                }
                if (inputID == 0)
                {
                    choosingID = true;
                    break;
                }
                choosingID = true;
            }
            return inputID;
        }
        public decimal AmountToTransfer()
        {
            decimal inputAmount = -1;
            bool choosingAmount = false;
            while (!choosingAmount)
            {
                Console.WriteLine("Enter Amount:");
                if (!decimal.TryParse(Console.ReadLine(), out inputAmount))
                {
                    Console.WriteLine("Invalid input. Only input a valid amount.");
                }
                if (inputAmount <= 0)
                {
                    Console.WriteLine("Invalid input. Only input a positive amount.");
                }
                choosingAmount = true;
            }
            return inputAmount;
        }
    }
}
