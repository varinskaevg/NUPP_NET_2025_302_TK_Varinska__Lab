﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.NoSql.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Title { get; set; }
        public string Author { get; set; }
    }
}
