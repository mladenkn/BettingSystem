namespace BettingSystem.Models
{
    public class UserWallet
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public Money MoneyAvailable { get; set; }
    }
}
