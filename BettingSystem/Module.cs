using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BetingSystem.Services;

namespace BetingSystem
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BonusService>().As<IBonusService>();
            builder.RegisterType<IBonusApplier>().As<IBonusApplier>();
        }
    }
}
