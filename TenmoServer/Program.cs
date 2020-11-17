using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TenmoServer.DAO;

namespace TenmoServer
{
    public class Program
    {
       
        public static void Main(string[] args)
        {
            IUserDAO userDAO = new UserSqlDAO(@"Data Source =.\SQLEXPRESS;Initial Catalog = tenmo; Integrated Security = True");
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
