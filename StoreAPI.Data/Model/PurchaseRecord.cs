﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreAPI.Data.Model
{
    class PurchaseRecord
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("product")]
        public string Product { get; set; }

        [BsonElement("quantity")]
        public double Quantity { get; set; }

        [BsonElement("cost")]
        public double Cost { get; set; }

        public double TotalCost => Cost * Quantity;
    }
}
