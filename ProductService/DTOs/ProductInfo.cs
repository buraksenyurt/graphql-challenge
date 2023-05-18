namespace ProductService.DTOs
{
    public class ProductInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockLevel { get; set; }
        public List<string> Comments { get; set; }
        public List<string> Photos { get; set; }
        public bool InStock { get; set; }
    }
}