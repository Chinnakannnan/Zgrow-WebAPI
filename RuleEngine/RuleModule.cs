using Autofac;
using RuleEngine.Payout;

namespace RuleEngine
{
    public class RuleModule : Module
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

                builder.RegisterType<PayoutRule>().AsImplementedInterfaces(); 
 

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

}

 