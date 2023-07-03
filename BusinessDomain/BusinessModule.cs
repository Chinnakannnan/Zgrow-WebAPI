
namespace BusinessDomain
{
    using Autofac;
    using BusinessDomain.Admin;
    using BusinessDomain.Auth;
    using BusinessDomain.PaymentGateway;
    using BusinessDomain.Payout;
    using BusinessDomain.Report;
    using BusinessDomain.User;
    using System;

    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            try
            {

                if (builder == null)
                {
                    throw new ArgumentNullException(nameof(builder));
                }

                // Register your service here
       
                builder.RegisterType<AuthBusiness>().AsImplementedInterfaces();
                builder.RegisterType<PayoutBusiness>().AsImplementedInterfaces();
                builder.RegisterType<UserBusiness>().AsImplementedInterfaces();
                builder.RegisterType<ReportBusiness>().AsImplementedInterfaces();
                builder.RegisterType<PaymentGatewayBussiness>().AsImplementedInterfaces();
                builder.RegisterType<AdminBusiness>().AsImplementedInterfaces();
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
