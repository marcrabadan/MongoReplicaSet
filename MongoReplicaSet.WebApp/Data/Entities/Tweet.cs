using MongoDbFramework;

namespace MongoReplicaSet.WebApp.Data.Entities
{
    public class Tweet : Document
    {
        public string Message { get; set; }
    }
}
