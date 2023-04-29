using System;
using MongoDB.Driver;

namespace MovieSearch.Tests.Repositories;

public abstract class MongoDbTest
{
    protected IMongoDatabase Database { get; }
    
    protected MongoDbTest()
    {
        var runner = MongoRunnerProvider.Get();
        Database = new MongoClient(runner.ConnectionString).GetDatabase(Guid.NewGuid().ToString());
    }
}