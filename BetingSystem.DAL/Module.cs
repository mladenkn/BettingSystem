using Autofac;
using Microsoft.EntityFrameworkCore;

namespace BetingSystem.DAL
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DataProvider>().As<IDataProvider>();
        }
    }
}
