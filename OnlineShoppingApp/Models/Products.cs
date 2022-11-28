using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OnlineShoppingApp.Models
{
    public class Products
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("productName")]
        public string ProductName { get; set; }

        [BsonElement("description")]
        public string  ProductDescription { get; set; }

        [BsonElement("price")]
        public int Price { get; set; }

        [BsonElement("features")]
        public string Features { get; set; }

        [BsonElement("productStatus")]
        public string ProductStatus { get; set; }
    }
}
