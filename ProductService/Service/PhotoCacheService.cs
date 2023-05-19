using StackExchange.Redis;

namespace ProductService.Cache;

public class PhotoCacheService
    : ICacheService
{
    private IDatabase _db;
    private readonly ILogger _logger;
    public PhotoCacheService(ILogger<PhotoCacheService> logger)
    {
        var redis = ConnectionMultiplexer.Connect("127.0.0.1:6378");
        _db = redis.GetDatabase();
        _logger = logger;
    }
    public string GetData(string key)
    {
        var value = _db.StringGet(key);
        if (!string.IsNullOrEmpty(value))
        {
            _logger.LogInformation($"{key} için {value.Length} uzunluğunda içerik cache'den geliyor");
            return value;
        }
        return string.Empty;
    }
    public bool SetData(string key, string value)
    {
        var isSet = _db.StringSet(key, value);
        return isSet;
    }
}