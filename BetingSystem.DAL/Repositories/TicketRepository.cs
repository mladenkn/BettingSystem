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
    public class TicketRepository : Repository<Ticket>
    {
        public TicketRepository(DbSet<Ticket> dbSet) : base(dbSet)
        {
        }
    }
}
