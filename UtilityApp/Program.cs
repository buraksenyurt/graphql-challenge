using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

Console.WriteLine("Redis tarafına görsel atmak için yardımcı programdır.");

var redis = ConnectionMultiplexer.Connect("127.0.0.1:6378");
IDatabase db = redis.GetDatabase();

if (db.StringGet("pencil1").IsNull)
{
    Console.WriteLine("Redis'te yok. Yükleniyor");
    byte[] imageArray = File.ReadAllBytes(@"assets/pencil_01.png");
    string base64Str = Convert.ToBase64String(imageArray);
    db.StringSet("pencil1", base64Str);
}
