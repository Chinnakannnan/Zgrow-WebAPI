using Autofac;
using DataAccess.Admin;
using DataAccess.Auth;
using DataAccess.PaymentGateway;
using DataAccess.Payout;
using DataAccess.Report;
using DataAccess.User;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess
{

    public class RepositoryModule : Module
    {
        private readonly IConfiguration _config;
        private readonly string _dbConnection;

        public RepositoryModule(IConfiguration configInstance)
        {
            _config = configInstance;
            _dbConnection = _config.GetConnectionString("DevDatabase");
        }
        public RepositoryModule()
        {
            var configurationBuilder = new ConfigurationBuilder();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            _dbConnection = configurationBuilder.Build().GetSection("ConnectionStrings:SqlServerDbCon").Value;
        }

        protected override void Load(ContainerBuilder builder)
        {
            try
            {
                if (builder == null)
                {
                    string builderNull = "Repository Builder is Null";
                    throw new ArgumentNullException(builderNull);
                }

                // Register here your Repository Service
                // 
                builder.RegisterType<AuthDA>().As<IAuthDA>().WithParameter("databaseConfig", new DatabaseConfig(_dbConnection));
                builder.RegisterType<PayoutDA>().As<IPayoutDA>().WithParameter("databaseConfig", new DatabaseConfig(_dbConnection));
                builder.RegisterType<UserDA>().As<IUserDA>().WithParameter("databaseConfig", new DatabaseConfig(_dbConnection));
                builder.RegisterType<PaymentGatewayDA>().As<IPaymentGatewayDA>().WithParameter("databaseConfig", new DatabaseConfig(_dbConnection));
                builder.RegisterType<ReportDA>().As<IReportDA>().WithParameter("databaseConfig", new DatabaseConfig(_dbConnection));
                builder.RegisterType<AdminDA>().As<IAdminDA>().WithParameter("databaseConfig", new DatabaseConfig(_dbConnection));


                


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
