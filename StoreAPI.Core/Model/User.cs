using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StoreAPI.Core.Model
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("nationalId")]
        public int NationalId { get; set; }

        [BsonElement("moneySpent")]
        public double MoneySpent { get; set; }
    }
}
