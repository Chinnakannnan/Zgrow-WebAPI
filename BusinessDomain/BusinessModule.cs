
namespace BusinessDomain
{
    using Autofac;
    using BusinessDomain.Auth;
    using BusinessDomain.Payout;
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
               
            

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
