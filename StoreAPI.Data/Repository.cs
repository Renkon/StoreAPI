using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using StoreAPI.Core.Dto;
using StoreAPI.Core.Exceptions;
using StoreAPI.Core.Interfaces.Repositories;
using StoreAPI.Core.Model.Payloads;
using StoreAPI.Data.Model;
using System.Threading.Tasks;

namespace StoreAPI.Data
{
    public class Repository : IRepository
    {
        private readonly IMongoClient mongoClient;
        private readonly IConfiguration configuration;
        private readonly ILogger<Repository> logger;

        private IMongoDatabase mongoDatabase;

        public Repository(IMongoClient mongoClient, IConfiguration configuration, ILogger<Repository> logger)
        {
            this.mongoClient = mongoClient;
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserPayload userPayload)
        {
            this.logger.LogTrace("Creating user instance.");

            var user = new User
            {
                FirstName = userPayload.FirstName,
                LastName = userPayload.LastName,
                MoneySpent = 0,
                NationalId = userPayload.NationalId,
            };

            this.logger.LogTrace("Retrieving user db collection.");
            var collection = this.GetUsersCollection();

            this.logger.LogTrace("Inserting user entity.");
            await collection.InsertOneAsync(user);

            this.logger.LogTrace("Creating and returning new user dto.");
            return this.GetUserDto(user);
        }

        public async Task<UserDto> UpdateUserAsync(int nationalId, UpdateUserPayload userPayload)
        {
            this.logger.LogTrace("Updating user instance.");

            var updatedUser = await this.GetUsersCollection().FindOneAndUpdateAsync(
                Builders<User>.Filter.Where(user => user.NationalId == nationalId),
                Builders<User>.Update
                    .Set(user => user.FirstName, userPayload.FirstName)
                    .Set(user => user.LastName, userPayload.LastName),
                options: new FindOneAndUpdateOptions<User>
                {
                    ReturnDocument = ReturnDocument.After,
                });

            this.logger.LogTrace("Verifying update was performed.");
            // If the user does not exist, updatedUser will be null, and therefore did not exist.
            if (updatedUser == default(User))
            {
                throw new DocumentNotFoundException();
            }

            this.logger.LogTrace("Creating and returning updated user dto.");
            return this.GetUserDto(updatedUser);
        }

        private IMongoCollection<User> GetUsersCollection()
            => this.GetDatabase().GetCollection<User>(Constants.Collections.Users);

        private IMongoCollection<PurchaseRecord> GetPurchaseRecordsCollection()
            => this.GetDatabase().GetCollection<PurchaseRecord>(Constants.Collections.PurchaseRecords);

        private IMongoDatabase GetDatabase()
        {
            logger.LogTrace("Retrieving database instance.");

            if (mongoDatabase == default(IMongoDatabase))
            {
                logger.LogTrace("Initializing new database instance as current one is unavailable.");
                this.mongoDatabase = mongoClient.GetDatabase(this.configuration[Constants.Configuration.DatabaseName]);
            }

            logger.LogTrace("Returning database instance.");
            return mongoDatabase;
        }

        private UserDto GetUserDto(User user)
        {
            return new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                MoneySpent = user.MoneySpent,
                NationalId = user.NationalId,
            };
        }
    }
}
