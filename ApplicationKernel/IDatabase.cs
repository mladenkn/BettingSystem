using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationKernel
{
    public interface IDatabase
    {
        Task SaveChanges();
    }
}
