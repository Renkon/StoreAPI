using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using StoreAPI.Core.Dto;
using StoreAPI.Core.Exceptions;
using StoreAPI.Core.Interfaces.Repositories;
using StoreAPI.Core.Model.Payloads;
using StoreAPI.Data.Model;
using System.Collections.Generic;
using System.Linq;
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
            this.logger.LogTrace($"Updating user instance with nationalId {nationalId}.");

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
            if (updatedUser == default(User))
            {
                throw new DocumentNotFoundException();
            }

            this.logger.LogTrace("Creating and returning updated user dto.");
            return this.GetUserDto(updatedUser);
        }

        public async Task DeleteUserAsync(int nationalId)
        {
            this.logger.LogTrace($"Deleting user instance with nationalId {nationalId}.");

            var deletedUser = await this.GetUsersCollection().FindOneAndDeleteAsync(
                Builders<User>.Filter.Where(user => user.NationalId == nationalId));

            this.logger.LogTrace("Verifying delete was performed.");
            if (deletedUser == default(User))
            {
                throw new DocumentNotFoundException();
            }
        }

        public async Task<UserDto> GetUserAsync(int nationalId)
        {
            this.logger.LogTrace($"Obtaining user instance with nationalId {nationalId}.");

            var singleUserList = await this.GetUsersCollection().Find(user => user.NationalId == nationalId)
                .Limit(1)
                .ToListAsync();

            this.logger.LogTrace("Verifying user was successfully received.");
            if (!singleUserList.Any())
            {
                throw new DocumentNotFoundException();
            }

            this.logger.LogTrace("Creating user dto instance.");
            return this.GetUserDto(singleUserList.First());
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync()
        {
            this.logger.LogTrace("Öbtaining user instances.");

            var userList = await this.GetUsersCollection().Find(Builders<User>.Filter.Empty).ToListAsync();

            this.logger.LogTrace("Creating user dto instances.");
            return userList.Select(this.GetUserDto);
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
