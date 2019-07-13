using MongoDbFramework;
using MongoDbFramework.Abstractions;
using System;

namespace MongoReplicaSet.WebApp.Data.Entities
{
    public class Tweet : IDocument<Guid>
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
    }
}
