using ApplicationKernel;
using IUnitOfWork = BetingSystem.DAL.IUnitOfWork;

namespace BetingSystem
{
    public class AbstractService : AbstractService<IUnitOfWork>
    {
        public AbstractService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
