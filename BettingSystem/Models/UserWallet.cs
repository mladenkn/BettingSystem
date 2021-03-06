﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetingSystem.Models
{
    public class UserWallet
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }

        public string Currency { get; set; }

        public decimal MoneyAmmount { get; set; }

        public IReadOnlyCollection<WalletTransaction> Transactions { get; set; }
    }

    public class WalletTransaction
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public decimal MoneyInvolved { get; set; } // negative if money is subtracted
        public WalletTransactionType Type { get; set; }

        public enum WalletTransactionType
        {
            TicketCommit
        }
    }
}
