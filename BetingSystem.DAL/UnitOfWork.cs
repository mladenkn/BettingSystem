using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetingSystem.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BetingSystemDbContext _dbContext;

        public UnitOfWork(BetingSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(object o) => _dbContext.Add(o);

        public void Update(object o) => _dbContext.Update(o);

        public void Delete(object o) => _dbContext.Remove(o);

        public async Task SaveChanges() => await _dbContext.SaveChangesAsync();
    }
}
