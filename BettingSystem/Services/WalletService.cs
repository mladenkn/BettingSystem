using System.Threading.Tasks;
using BetingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BetingSystem.Services
{
    public interface IWalletService
    {
        Task SubtractMoney(decimal moneyAmmount, WalletTransaction.WalletTransactionType type);
    }

    public class WalletService : IWalletService
    {
        private readonly DbContext _db;
        private readonly ICurrentUserAccessor _userAccessor;

        public WalletService(DbContext db, ICurrentUserAccessor userAccessor)
        {
            _db = db;
            _userAccessor = userAccessor;
        }

        public async Task SubtractMoney(decimal moneyAmmount, WalletTransaction.WalletTransactionType type)
        {
            var userId = _userAccessor.Id();
            var wallet = await _db.Set<UserWallet>().FirstOrDefaultAsync(w => w.UserId == userId);

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
            _db.Add(transaction);
            _db.Update(wallet);
            return _db.SaveChangesAsync();
        }
    }
}
