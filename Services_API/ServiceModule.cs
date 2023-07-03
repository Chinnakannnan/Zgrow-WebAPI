﻿using Autofac;
using Services_API.Banking;
using Services_API.PaymentGateway;
using System.Reflection;
using Module = Autofac.Module;

namespace Services_API
{
    public class ServiceModule : Module
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

                builder.RegisterType<BankService>().AsImplementedInterfaces();
                builder.RegisterType<PaymentGatewayService>().AsImplementedInterfaces();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
 