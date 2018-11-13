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
    public class BetedPairRepository : Repository<BetedPair>, IBetedPairRepository
    {
        public BetedPairRepository(DbSet<BetedPair> dbSet) : base(dbSet)
        {
        }
    }
}
