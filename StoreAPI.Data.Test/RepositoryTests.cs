using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using StoreAPI.Core.Exceptions;
using StoreAPI.Core.Interfaces.Repositories;
using StoreAPI.Core.Model.Payloads;
using StoreAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreAPI.Core.Test
{
    // Integration tests to ensure MongoDB works as expected.
    [TestClass]
    public class RepositoryTests
    {
        private IRepository repository;
        private DatabaseConfigurationAssistant dbConfigurationAssistant;
        private readonly ILogger<Repository> logger;
        private readonly IMongoClient mongoClient;
        private readonly IConfiguration config;

        public RepositoryTests()
        {
            this.config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            this.mongoClient = new MongoClient(config[Constants.Configuration.ConnectionString]);
            this.dbConfigurationAssistant = new DatabaseConfigurationAssistant(mongoClient, config[Constants.Configuration.DatabaseName]);
            this.logger = new LoggerFactory().CreateLogger<Repository>();
        }

        [TestInitialize]
        public async Task SetupDatabase()
        {
            // Removing at the beginning in case it was not removed in a previous test run.
            dbConfigurationAssistant.RemoveDatabase();
            dbConfigurationAssistant.SetupDatabase();
            this.repository = new Repository(mongoClient, config, logger);

            await this.SetupMockDataAsync();
        }

        [TestCleanup]
        public void CleanupDatabase()
        {
            dbConfigurationAssistant.RemoveDatabase();
        }

        [TestMethod]
        public async Task TestCreateUserAsync()
        {
            var createPayload = new CreateUserPayload
            {
                FirstName = "Stephen",
                LastName = "Wright",
                NationalId = 50981295,
            };

            var previousUserCount = (await this.repository.GetUsersAsync()).Count();
            var createdUser = await this.repository.CreateUserAsync(createPayload);
            var users = await this.repository.GetUsersAsync();

            Assert.AreEqual(previousUserCount + 1, users.Count());
            Assert.AreEqual(createPayload.FirstName, createdUser.FirstName);
            Assert.AreEqual(createPayload.LastName, createdUser.LastName);
            Assert.AreEqual(createPayload.NationalId, createdUser.NationalId);

        }

        [TestMethod]
        public async Task TestUpdateExistingUserAsync()
        {
            var identifier = 17246710;
            var updatePayload = new UpdateUserPayload
            {
                FirstName = "Layla",
                LastName = "Cruz",
            };

            var user = await this.repository.UpdateUserAsync(identifier, updatePayload);

            Assert.AreEqual(updatePayload.FirstName, user.FirstName);
            Assert.AreEqual(updatePayload.LastName, user.LastName);
            Assert.AreEqual(identifier, user.NationalId);
        }

        [TestMethod]
        public async Task TestUpdateNotExistingUserAsync()
        {
            var identifier = 99999999;
            var updatePayload = new UpdateUserPayload
            {
                FirstName = "Layla",
                LastName = "Cruz",
            };

            await Assert.ThrowsExceptionAsync<DocumentNotFoundException<int>>(() => this.repository.UpdateUserAsync(identifier, updatePayload));
        }

        [TestMethod]
        public async Task TestDeleteExistingUserAsync()
        {
            var identifier = 17246710;
            var previousUserCount = (await this.repository.GetUsersAsync()).Count();
            await this.repository.DeleteUserAsync(identifier);
            var newUsers = await this.repository.GetUsersAsync();

            Assert.AreEqual(previousUserCount - 1, newUsers.Count());
            Assert.AreEqual(21698109, newUsers.ElementAt(0).NationalId);
            Assert.AreEqual(10953291, newUsers.ElementAt(1).NationalId);
        }

        [TestMethod]
        public async Task TestDeleteNotExistingUserAsync()
        {
            var identifier = 99999999;
            await Assert.ThrowsExceptionAsync<DocumentNotFoundException<int>>(() => this.repository.DeleteUserAsync(identifier));
        }

        [TestMethod]
        public async Task TestGetUsersAsync()
        {
            var identifiers = new List<int> { 17246710, 21698109, 10953291 };
            var expectedCost = new List<double> { 8.75, 9, 6.75 };
            var users = await this.repository.GetUsersAsync();

            Assert.AreEqual(identifiers.Count(), users.Count());

            for (int i = 0; i < users.Count(); i++)
            {
                Assert.AreEqual(identifiers.ElementAt(i), users.ElementAt(i).NationalId);
                Assert.AreEqual(expectedCost.ElementAt(i), users.ElementAt(i).MoneySpent);
            }
        }

        [TestMethod]
        public async Task TestGetExistingUserAsync()
        {
            var identifier = 17246710;
            var expectedCost = 8.75;
            var user = await this.repository.GetUserAsync(identifier);

            Assert.AreEqual(identifier, user.NationalId);
            Assert.AreEqual(expectedCost, user.MoneySpent);
        }

        [TestMethod]
        public async Task TestGetNonExistingUserAsync()
        {
            var identifier = 99999999;
            await Assert.ThrowsExceptionAsync<DocumentNotFoundException<int>>(() => this.repository.GetUserAsync(identifier));
        }

        [TestMethod]
        public async Task TestCreatePurchaseRecordWithExistingUserAsync()
        {
            var identifier = 17246710;
            var purchasePayload = new CreatePurchaseRecordPayload
            {
                UserNationalId = identifier,
                Product = "Potatoes",
                Quantity = 10,
                Cost = 0.33,
            };

            var previousPurchaseCount = (await this.repository.GetPurchaseRecordsAsync()).Count();
            var previousUser = await this.repository.GetUserAsync(identifier);

            await this.repository.PerformPurchaseAsync(purchasePayload);

            var newPurchaseCount = (await this.repository.GetPurchaseRecordsAsync()).Count();
            var newUser = await this.repository.GetUserAsync(identifier);

            Assert.AreEqual(previousPurchaseCount + 1, newPurchaseCount);
            Assert.AreEqual(previousUser.NationalId, newUser.NationalId);
            Assert.AreEqual(previousUser.MoneySpent + (purchasePayload.Quantity * purchasePayload.Cost), newUser.MoneySpent);
        }

        [TestMethod]
        public async Task TestCreatePurchaseRecordWithNotExistingUserAsync()
        {
            var identifier = 99999999;
            var purchasePayload = new CreatePurchaseRecordPayload
            {
                UserNationalId = identifier,
                Product = "Potatoes",
                Quantity = 10,
                Cost = 0.33,
            };

            var previousPurchaseCount = (await this.repository.GetPurchaseRecordsAsync()).Count();
            await Assert.ThrowsExceptionAsync<DocumentNotFoundException<int>>(() => this.repository.PerformPurchaseAsync(purchasePayload));

            var newPurchaseCount = (await this.repository.GetPurchaseRecordsAsync()).Count();

            Assert.AreEqual(previousPurchaseCount, newPurchaseCount);
        }

        [TestMethod]
        public async Task TestGetMoreThanAverageSpentUsersAsync()
        {
            var identifiers = new List<int> { 17246710, 21698109 };
            var expectedCost = new List<double> { 8.75, 9 };
            var users = await this.repository.GetMoreThanAverageSpentUsersAsync();

            Assert.AreEqual(identifiers.Count(), users.Count());

            for (int i = 0; i < users.Count(); i++)
            {
                Assert.AreEqual(identifiers.ElementAt(i), users.ElementAt(i).NationalId);
                Assert.AreEqual(expectedCost.ElementAt(i), users.ElementAt(i).MoneySpent);
            }
        }

        [TestMethod]
        public async Task TestEmptyGetMoreThanAverageSpentUsersAsync()
        {
            var identifiers = new List<int> { 17246710, 21698109, 10953291 };

            foreach (var id in identifiers)
            {
                await this.repository.DeleteUserAsync(id);
            }

            var users = await this.repository.GetMoreThanAverageSpentUsersAsync();

            Assert.AreEqual(0, users.Count());
        }

        private async Task SetupMockDataAsync()
        {
            await this.CreateUsersAsync();
            await this.CreatePurchaseRecordsAsync();
        }

        private async Task CreatePurchaseRecordsAsync()
        {
            await this.repository.PerformPurchaseAsync(new CreatePurchaseRecordPayload
            {
                UserNationalId = 17246710,
                Product = "Cookies",
                Quantity = 1,
                Cost = 3.25
            });
            await this.repository.PerformPurchaseAsync(new CreatePurchaseRecordPayload
            {
                UserNationalId = 17246710,
                Product = "Soda",
                Quantity = 1,
                Cost = 5.50,
            });

            await this.repository.PerformPurchaseAsync(new CreatePurchaseRecordPayload
            {
                UserNationalId = 21698109,
                Product = "Milk",
                Quantity = 3,
                Cost = 3,
            });

            await this.repository.PerformPurchaseAsync(new CreatePurchaseRecordPayload
            {
                UserNationalId = 10953291,
                Product = "Apples",
                Quantity = 5,
                Cost = 0.75,
            });
            await this.repository.PerformPurchaseAsync(new CreatePurchaseRecordPayload
            {
                UserNationalId = 10953291,
                Product = "Bananas",
                Quantity = 3,
                Cost = 0.50,
            });
            await this.repository.PerformPurchaseAsync(new CreatePurchaseRecordPayload
            {
                UserNationalId = 10953291,
                Product = "Pencil",
                Quantity = 1,
                Cost = 1.50,
            });
        }

        private async Task CreateUsersAsync()
        {
            await this.repository.CreateUserAsync(new CreateUserPayload
            {
                FirstName = "John",
                LastName = "Smith",
                NationalId = 17246710,
            });
            await this.repository.CreateUserAsync(new CreateUserPayload
            {
                FirstName = "Hannah",
                LastName = "Doe",
                NationalId = 21698109,
            });
            await this.repository.CreateUserAsync(new CreateUserPayload
            {
                FirstName = "Richard",
                LastName = "Gonzalez",
                NationalId = 10953291,
            });
        }
    }
}
