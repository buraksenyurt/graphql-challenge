using ProductService.Context;
using ProductService.DTOs;
using ProductService.Model;

namespace ProductService.Contracts;

public class Query
{
    public ProductInfo GetProductInfo(SouthWindDbContext dbContext, int productId)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Category> GetCategories(SouthWindDbContext dbContext) => dbContext.Categories.OrderBy(c => c.Title);
}