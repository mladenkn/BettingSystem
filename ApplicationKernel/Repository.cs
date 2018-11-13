using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ApplicationKernel
{
    public interface IRepository<TModel>
        where TModel : class
    {
        void Insert(TModel model);
        void Update(TModel model);
        void Delete(TModel model);
        IQueryable<TModel> GenericQuery();
    }

    public class Repository<TModel> : IRepository<TModel>
        where TModel : class
    {
        private readonly DbSet<TModel> _dbSet;

        public Repository(DbSet<TModel> dbSet)
        {
            _dbSet = dbSet;
        }

        public void Insert(TModel model) => _dbSet.Add(model);

        public void Update(TModel model) => _dbSet.Update(model);

        public void Delete(TModel model) => _dbSet.Remove(model);

        public IQueryable<TModel> GenericQuery() => _dbSet;
    }
}
