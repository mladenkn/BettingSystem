using System.Threading.Tasks;
using BetingSystem.Models;

namespace BetingSystem.Services
{
    public interface IWalletService
    {
        Task SubtractMoney(decimal moneyAmmount, WalletTransaction.WalletTransactionType type);
    }

    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserAccessor _userAccessor;
        private readonly IDataProvider _dataProvider;

        public WalletService(IUnitOfWork unitOfWork, ICurrentUserAccessor userAccessor, IDataProvider dataProvider)
        {
            _unitOfWork = unitOfWork;
            _userAccessor = userAccessor;
            _dataProvider = dataProvider;
        }

        public async Task SubtractMoney(decimal moneyAmmount, WalletTransaction.WalletTransactionType type)
        {
            var userId = _userAccessor.Id();
            var wallet = await _dataProvider.UsersWallet(userId);

            if (wallet == null)
                throw new ModelNotFound(typeof(UserWallet));

            var transaction = new WalletTransaction
            {
                WalletId = wallet.Id,
                MoneyInvolved = -moneyAmmount,
                Type = type
            };

            await CommitTransaction(wallet, transaction);
        }

        private Task CommitTransaction(UserWallet wallet, WalletTransaction transaction)
        {
            wallet.MoneyAmmount += transaction.MoneyInvolved;
            _unitOfWork.Add(transaction);
            _unitOfWork.Update(wallet);
            return _unitOfWork.SaveChanges();
        }
    }
}
