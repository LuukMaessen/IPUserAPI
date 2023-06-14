using userAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace userAPI.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<User> _userCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _userCollection = database.GetCollection<User>(mongoDBSettings.Value.CollectioName);
        }

        public async Task CreateAsync(User user)
        {
            await _userCollection.InsertOneAsync(user);
            return;
        }

        public async Task<List<User>> GetAsync()
        {
            return await _userCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task AddToEmployeesAsync(string id, string employees)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Eq("id", id);
            UpdateDefinition<User> update = Builders<User>.Update.AddToSet<string>("employees", employees);
            await _userCollection.UpdateOneAsync(filter, update);
            return;
        }
    }
}
