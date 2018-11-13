namespace BetingSystem.Models
{
    public class User
    {
        public string UserId { get; set; }

        public int WalletId { get; set; }
        public UserWallet Wallet { get; set; }
    }
}
