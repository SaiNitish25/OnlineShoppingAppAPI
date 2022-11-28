using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Models
{
    public class Orders
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("productId")]
        public string ProductId { get; set; }

        [BsonElement("customerId")]
        public string CustomerId { get; set; }

        [BsonElement("productName")]
        public string ProductName { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

    }
}
