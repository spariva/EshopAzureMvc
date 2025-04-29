namespace Eshop.Models.DTOs
{
    public class ProfileDto
    {
        public User User { get; set; }
        public Store Store { get; set; }
        public List<Purchase> Purchases { get; set; }
    }
}
