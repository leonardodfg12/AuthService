using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AuthService.Infrastructure.Persistence;

public class MongoUserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _collection;

    public MongoUserRepository(IConfiguration config)
    {
        var connectionString = config["MONGO_CONNECTION_STRING"];
        var databaseName = config["MONGO_DATABASE_NAME"];

        var mongo = new MongoClient(connectionString);
        var database = mongo.GetDatabase(databaseName);
        _collection = database.GetCollection<User>("Users");
    }

    public async Task<User?> GetByEmailAsync(string email) =>
        await _collection.Find(x => x.Email == email).FirstOrDefaultAsync();

    public async Task AddAsync(User user) =>
        await _collection.InsertOneAsync(user);

    public async Task<IEnumerable<User>> GetAllAsync() =>
        await _collection.Find(_ => true).ToListAsync();
}