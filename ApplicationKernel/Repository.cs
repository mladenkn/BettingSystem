using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void Insert(TModel model)
        {
            throw new NotImplementedException();
        }

        public void Update(TModel model)
        {
            throw new NotImplementedException();
        }

        public void Delete(TModel model)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TModel> GenericQuery()
        {
            throw new NotImplementedException();
        }
    }
}
