using MongoDbFramework;
using MongoDB.Driver;
using MongoReplicaSet.WebApp.Data.Entities;

namespace MongoReplicaSet.WebApp.Data.Contexts
{
    public class SocialContext : MongoDbContext
    {
        public SocialContext(MongoDbOptions<SocialContext> options) : base(options)
        {
        }

        public override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Document<Tweet>()
                .WithDatabase("social")
                .WithCollection("tweets")
                .WithDatabaseBehavior(c =>
                {
                    c.WithReadPreference(ReadPreference.Primary);
                    c.WithReadConcern(ReadConcern.Majority);
                    c.WithWriteConcern(WriteConcern.WMajority);
                })
                .WithTransactionBehavior(c =>
                {
                    c.WithReadPreference(ReadPreference.Primary);
                    c.WithReadConcern(ReadConcern.Majority);
                    c.WithWriteConcern(WriteConcern.WMajority);
                })
                .WithSessionBehavior(c =>
                {
                    c.WithReadPreference(ReadPreference.Primary);
                    c.WithReadConcern(ReadConcern.Majority);
                    c.WithWriteConcern(WriteConcern.WMajority);
                });
        }

        public MongoCollection<Tweet> Tweets { get; set; }
    }
}
