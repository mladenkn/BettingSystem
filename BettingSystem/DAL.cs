using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationKernel;
using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;
using Utilities;

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

    public interface ITicketBonusesRepository
    {
        Task<IEnumerable<ITicketBonus>> GetActiveBonuses();
        Task Insert(ITicketBonus bonus);
        Task Update(ITicketBonus bonus);
    }

    public class Database : ApplicationKernel.Database, IDatabase
    {
        private readonly ITicketBonusesRepository _bonusesRepository;

        public Database(DbContext db, IDataProvider dataProvider, ITicketBonusesRepository bonusesRepository) : base(db)
        {
            _bonusesRepository = bonusesRepository;
            DataProvider = dataProvider;
        }

        public IDataProvider DataProvider { get; }

        public override IDatabaseTransaction NewTransaction() => new DatabaseTransaction(Db, _bonusesRepository);
    }

    public class DatabaseTransaction : ApplicationKernel.DatabaseTransaction
    {
        private readonly ITicketBonusesRepository _bonusesRepository;
        private readonly ICollection<Func<Task>> _asyncModifiers = new List<Func<Task>>();

        public DatabaseTransaction(DbContext db, ITicketBonusesRepository bonusesRepository) : base(db)
        {
            _bonusesRepository = bonusesRepository;
        }

        public override IDatabaseTransaction Insert(object o)
        {
            if (o is ITicketBonus b)
            {
                _asyncModifiers.Add(() => _bonusesRepository.Insert(b));
                return this;
            }
            else
                return base.Insert(o);
        }

        public override IDatabaseTransaction Update(object o)
        {
            if (o is ITicketBonus b)
            {
                _asyncModifiers.Add(() => _bonusesRepository.Update(b));
                return this;
            }
            else
                return base.Insert(o);
        }

        public override Task Commit()
        {
            var tasks = _asyncModifiers.Select(item => item()).WhenAll();
            return Task.WhenAll(tasks, base.Commit());
        }
    }
}
