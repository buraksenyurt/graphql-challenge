using ProductService.Cache;
using ProductService.Context;
using ProductService.DTOs;
using ProductService.Model;
using StackExchange.Redis;

namespace ProductService.Contracts;

public class Query
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger _logger;
    private readonly ICacheService _cacheService;
    public Query(IHttpClientFactory clientFactory, ILogger<Query> logger, ICacheService cacheService)
    {
        _clientFactory = clientFactory;
        _logger = logger;
        _cacheService = cacheService;
    }
    public string Ping() => "Pong";
    public async Task<ProductInfo> GetProductInfo(SouthWindDbContext dbContext, int productId)
    {
        _logger.LogInformation($"{productId} numaralı ürün için bilgiler alınacak");
        var productInfo = (from p in dbContext.Products
                           join c in dbContext.Categories
                           on p.CategoryId equals c.Id
                           where p.Id == productId
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

        _logger.LogInformation($"'{productInfo.Name}' isimli ürün bulundu");
        productInfo.Comments = await LoadComments(productId);
        productInfo.Photos = await LoadPhotos("pencil_");

        return productInfo;
    }

    private async Task<List<UserComment>> LoadComments(int productId)
    {
        List<UserComment> comments = new List<UserComment>();
        try
        {
            var client = _clientFactory.CreateClient(name: "UserCommentService");
            var request = new HttpRequestMessage(method: HttpMethod.Get, requestUri: $"api/user/comments/{productId}");
            var response = await client.SendAsync(request);
            var userComments = await response.Content.ReadFromJsonAsync<IEnumerable<UserComment>>();
            comments = userComments.ToList();
        }
        catch (Exception excp)
        {
            _logger.LogError(excp.Message);
        }
        return comments;
    }

    private async Task<List<Photo>> LoadPhotos(string keyName)
    {
        List<Photo> photos = new List<Photo>();
        try
        {
            for (int i = 1; i <= 4; i++)
            {
                string key = $"{keyName}0{i}.png";
                _logger.LogInformation($"{key} fotoğrafı cache'den çekilecek.");
                var photoContent = _cacheService.GetData(key);
                _logger.LogInformation($"{photoContent.Length} boyutunda veri bulundu.");
                if (photoContent != null)
                {
                    var photo = new Photo
                    {
                        Name = key,
                        Base64Content = photoContent
                    };
                    photos.Add(photo);
                }

            }
        }
        catch (Exception excp)
        {
            _logger.LogError(excp.Message);
        }
        return photos;
    }

    public IQueryable<Category> GetCategories(SouthWindDbContext dbContext) => dbContext.Categories.OrderBy(c => c.Title);
}