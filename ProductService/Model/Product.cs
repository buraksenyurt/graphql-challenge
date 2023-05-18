namespace ProductService.Model;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public decimal UnitPrice { get; set; }
    public int StockLevel { get; set; }
}