using ProductService.Context;
using ProductService.DTOs;
using ProductService.Model;
using System.Linq;

namespace ProductService.Contracts;

public class Query
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly SouthWindDbContext _dbContext;
    public Query(SouthWindDbContext dbContext, IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _dbContext = dbContext;
    }
    public string Ping() => "Pong";
    public async Task<ProductInfo> GetProductInfo(int productId)
    {
        var productInfo = (from p in _dbContext.Products
                           join c in _dbContext.Categories
                           on p.CategoryId equals c.Id
                           where p.Id==productId
                           select new ProductInfo
                           {
                               Id = productId,
                               Name = p.Name,
                               CategoryName = c.Title,
                               CategoryId = c.Id,
                               StockLevel = p.StockLevel,
                               UnitPrice = p.UnitPrice
                               //InStock= false // Başka bir servise soracağız
                               //Photos = new List<string>() // Fiziki diskten okuma yapan başka bir servisten alacağız
                           }).Single();

        try
        {
            var client = _clientFactory.CreateClient(name: "UserCommentService");
            var request = new HttpRequestMessage(method: HttpMethod.Get, requestUri: $"api/user/comments/{productId}");
            var response = await client.SendAsync(request);
            var comments = await response.Content.ReadFromJsonAsync<IEnumerable<UserComment>>();
            productInfo.Comments = comments.ToList();
        }
        catch (Exception excp)
        {
            Console.WriteLine(excp.ToString());
        }

        return productInfo;
    }

    public IQueryable<Category> GetCategories(SouthWindDbContext dbContext) => dbContext.Categories.OrderBy(c => c.Title);
}