﻿using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreAPI.Data
{
    public class DatabaseConfigurationAssistant
    {
        private readonly IMongoClient mongoClient;
        private readonly string databaseName;

        public DatabaseConfigurationAssistant(IMongoClient mongoClient, string databaseName)
        {
            this.mongoClient = mongoClient;
            this.databaseName = databaseName;
        }

        public void SetupDatabase()
        {
            // By getting the database instance, we ensure that the database is created.
            var database = this.mongoClient.GetDatabase(databaseName);

            this.CreateCollectionIfNotExists(database, Constants.Collections.Users);
            this.CreateCollectionIfNotExists(database, Constants.Collections.PurchaseRecords);
        }

        // Not asynchronous method as this is executed from Startup.
        private bool CollectionExists(IMongoDatabase database, string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            // Filter by collection name
            var collections = database.ListCollectionNames(new ListCollectionNamesOptions { Filter = filter });
            // Check for existence
            return collections.Any();
        }

        private void CreateCollectionIfNotExists(IMongoDatabase database, string collectionName)
        {
            if (!this.CollectionExists(database, collectionName))
            {
                database.CreateCollection(collectionName);
            }
        }
    }
}