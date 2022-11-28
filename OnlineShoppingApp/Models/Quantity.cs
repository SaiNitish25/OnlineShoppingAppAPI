using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingApp.Models
{
    public class Quantity
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("productId")]
        public string ProductId { get; set; }

        [BsonElement("quantity")]
        public int quantity { get; set; }

    }
}
