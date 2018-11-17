using Autofac;
using BetingSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace BetingSystem
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BonusService>().As<IBonusService>();
            builder.RegisterType<BonusApplier>().As<IBonusApplier>();
            builder.RegisterType<TicketService>().As<ITicketService>();
            builder.RegisterType<WalletService>().As<IWalletService>();
        }
    }
}
