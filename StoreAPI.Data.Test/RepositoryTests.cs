using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using StoreAPI.Core.Interfaces.Repositories;
using StoreAPI.Data;
using System;
using System.Threading.Tasks;

namespace StoreAPI.Data.Test
{
    // Integration tests to ensure MongoDB works as expected.
    [TestClass]
    public class RepositoryTests
    {
        private IRepository repository;

        [TestInitialize]
        public async Task SetupDatabase()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var mongoClient = new MongoClient(config[Constants.Configuration.ConnectionString]);
            var logger = new LoggerFactory().CreateLogger<Repository>();
            this.repository = new Repository(mongoClient, config, logger);

            await this.SetupMockDataAsync();
        }

        [TestMethod]
        public void Test()
        {

        }

        private async Task SetupMockDataAsync()
        {

        }
    }
}
