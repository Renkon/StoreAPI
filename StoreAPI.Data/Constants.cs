using System;
using System.Collections.Generic;
using System.Text;

namespace StoreAPI.Core
{
    public static class Constants
    {
        public static class Configuration
        {
            public const string DatabaseName = "MongoDbDatabaseName";

            public const string ConnectionString = "MongoDbUri";
        }

        public static class Collections
        {
            public const string Users = "users";

            public const string PurchaseRecords = "purchaseRecords";
        }
    }
}
