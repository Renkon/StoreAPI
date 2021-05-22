using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using StoreAPI.Core.Repositories;

namespace StoreAPI.Data
{
    public class Repository : IRepository
    {
        private readonly IMongoClient mongoClient;
        private readonly IConfiguration configuration;
        private readonly ILogger<Repository> logger;

        public Repository(IMongoClient mongoClient, IConfiguration configuration, ILogger<Repository> logger)
        {
            this.mongoClient = mongoClient;
            this.configuration = configuration;
            this.logger = logger;
        }

        private ? GetDatabaseInstance()
        {
            mongoClient.GetDatabase(this.configuration[Constants.Configuration.DatabaseName]);
        }
    }
}
