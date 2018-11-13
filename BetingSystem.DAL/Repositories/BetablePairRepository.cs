using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationKernel;
using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BetingSystem.DAL.Repositories
{
    public class BetablePairRepository : Repository<BetablePair>, IBetablePairsRepository
    {
        public BetablePairRepository(DbSet<BetablePair> dbSet) : base(dbSet)
        {
        }
    }
}
