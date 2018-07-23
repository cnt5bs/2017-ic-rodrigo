using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace Repository.Learn
{
    public class Base
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string ID { get; set; }
        public bool deleted { get; set; } = false;
        public bool active { get; set; } = true;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime creationDate { get; set; } = DateTime.Now;
    }
}
