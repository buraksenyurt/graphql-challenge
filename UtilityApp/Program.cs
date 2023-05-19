using StackExchange.Redis;

Console.WriteLine("Redis tarafına görsel atmak için yardımcı programdır.");

var redis = ConnectionMultiplexer.Connect("127.0.0.1:6378");
IDatabase db = redis.GetDatabase();

var files = Directory.GetFiles("assets", "*.png");
foreach (var file in files)
{
    var fileName = new FileInfo(file).Name;
    if (!db.KeyExists(fileName))
    {
        Console.WriteLine($"Redis tarafında '{fileName}' isimli key yok.\nYükleniyor...");
        byte[] imgArray = File.ReadAllBytes(file);
        string base64Str = Convert.ToBase64String(imgArray);
        db.StringSet(fileName, base64Str);
        Console.WriteLine($"{base64Str.Length}, Yüklendi");
    }
}