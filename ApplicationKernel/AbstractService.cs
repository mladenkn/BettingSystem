namespace ApplicationKernel
{
    public abstract class AbstractService<TUnitOfWork>
        where TUnitOfWork : IUnitOfWork
    {
        protected AbstractService(TUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        protected TUnitOfWork UnitOfWork { get; }
    }
}
