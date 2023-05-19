namespace ProductService.Cache;


public interface ICacheService
{
    string GetData(string key);

    bool SetData(string key, string value);
}