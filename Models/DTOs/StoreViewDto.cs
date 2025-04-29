namespace Eshop.Models.DTOs
{
    public class StoreViewDto
    {
        public StoreDto Store { get; set; }
        public List<ProductDto> Products { get; set; }
        public List<string> ProductCategories { get; set; }
    }
}
