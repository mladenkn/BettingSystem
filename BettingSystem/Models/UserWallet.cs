using System.Collections.Generic;

namespace BettingSystem.Models
{
    public class UserWallet
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string Currency { get; set; }

        public double MoneyAmmount { get; set; }

        public IReadOnlyCollection<WalletTransaction> Transactions { get; set; }
    }

    public class WalletTransaction
    {
        public int WalletId { get; set; }
        public double MoneyInvolved { get; set; }
        public WalletTransactionType Type { get; set; }

        public enum WalletTransactionType
        {
            TicketCommit
        }
    }
}
