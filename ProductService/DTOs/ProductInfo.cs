using ProductService.Model;

namespace ProductService.DTOs
{
    public class ProductInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int StockLevel { get; set; }
        public List<UserComment> Comments { get; set; } = new List<UserComment>();
        public List<string> Photos { get; set; } = new List<string>();
        public bool InStock { get; set; }
    }
}