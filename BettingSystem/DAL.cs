using System.Collections.Generic;
using System.Threading.Tasks;
using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BetingSystem
{
    public interface IDatabase : ApplicationKernel.IDatabase
    {
        IDataProvider DataProvider { get; }
    }

    public interface IDataProvider
    {
        Task<IReadOnlyCollection<TicketDto>> GetUsersTickets(string userId);
        Task<IEnumerable<ITicketBonus>> GetActiveBonuses();
    }

    public class Database : ApplicationKernel.Database, IDatabase
    {
        public Database(DbContext db, IDataProvider dataProvider) : base(db)
        {
            DataProvider = dataProvider;
        }

        public IDataProvider DataProvider { get; }
    }
}
