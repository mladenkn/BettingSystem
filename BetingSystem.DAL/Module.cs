using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.EntityFrameworkCore;

namespace BetingSystem.DAL
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BetingSystemDbContext>().As<DbContext>();
            builder.RegisterType<TicketBonusRepository>().As<ITicketBonusRepository>();
        }
    }
}
