using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Products.Redis.Data;

public class Client
{
    private readonly IDatabase _db;
    public Client(IConfiguration configuration)
    {
        string redisConnectionString = configuration.GetConnectionString("Redis")!;

        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisConnectionString);

        var Db= redis.GetDatabase();
        _db = Db;
    }
    public IDatabase GetDatabase()=>_db;
}
