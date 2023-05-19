using StackExchange.Redis;

namespace ProductService.Cache;

public class PhotoCacheService
    : ICacheService
{
    private IDatabase _db;
    public PhotoCacheService()
    {
        var redis = ConnectionMultiplexer.Connect("127.0.0.1:6378");
        _db = redis.GetDatabase();
    }
    public string GetData(string key)
    {
        var value = _db.StringGet(key);
        if (!string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }
        return value;
    }
    public bool SetData(string key, string value)
    {
        var isSet = _db.StringSet(key, value);
        return isSet;
    }
}