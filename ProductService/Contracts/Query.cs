using ProductService.Cache;
using ProductService.Context;
using ProductService.DTOs;
using ProductService.Model;

namespace ProductService.Contracts;

public class Query(IHttpClientFactory clientFactory, ILogger<Query> logger, ICacheService cacheService)
{
    private readonly IHttpClientFactory _clientFactory = clientFactory;
    private readonly ILogger _logger = logger;
    private readonly ICacheService _cacheService = cacheService;

    public string Ping() => "Pong";
    public async Task<ProductInfo> GetProductInfo(SouthWindDbContext dbContext, int productId)
    {
        _logger.LogInformation("{} numaralı ürün için bilgiler alınacak", productId);
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
        List<UserComment> comments = [];
        try
        {
            var client = _clientFactory.CreateClient(name: "UserCommentService");
            var request = new HttpRequestMessage(method: HttpMethod.Get, requestUri: $"api/user/comments/{productId}");
            var response = await client.SendAsync(request);
            var userComments = await response.Content.ReadFromJsonAsync<IEnumerable<UserComment>>();
            comments = userComments == null ? [] : userComments.ToList();
        }
        catch (Exception excp)
        {
            _logger.LogError(excp.Message);
        }
        return comments;
    }

    private async Task<List<Photo>> LoadPhotos(string keyName)
    {
        List<Photo> photos = [];
        try
        {
            for (int i = 1; i <= 4; i++)
            {
                string key = $"{keyName}0{i}.png";
                _logger.LogInformation("{} fotoğrafı cache'den çekilecek.",key);
                var photoContent = _cacheService.GetData(key);
                _logger.LogInformation("{} boyutunda veri bulundu.", photoContent.Length);
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
            _logger.LogError("Error: {}", excp.Message);
        }
        return photos;
    }

    public IQueryable<Category> GetCategories(SouthWindDbContext dbContext) => dbContext.Categories.OrderBy(c => c.Title);
}