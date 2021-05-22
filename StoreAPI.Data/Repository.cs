using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using StoreAPI.Core.Repositories;

namespace StoreAPI.Data
{
    public class Repository : IRepository
    {
        private readonly IMongoClient mongoClient;
        private readonly ILogger<Repository> logger;

        public Repository(IMongoClient mongoClient, ILogger<Repository> logger)
        {
            this.mongoClient = mongoClient;
            this.logger = logger;
        }
    }
}
