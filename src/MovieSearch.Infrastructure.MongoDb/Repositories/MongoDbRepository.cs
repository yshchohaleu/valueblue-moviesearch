using System.Diagnostics.CodeAnalysis;
using MongoDB.Driver;
using MovieSearch.Shared.Domain;

namespace MovieSearch.Infrastructure.MongoDb.Repositories;

[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
public abstract class MongoDbRepository<TEntity> where TEntity : class, IEntity
{
    protected readonly IMongoDatabase Database;
    protected readonly IMongoCollection<TEntity> Collection;

    protected MongoDbRepository(IMongoDatabase database)
    {
        Database = database;
        Collection = Database.GetCollection<TEntity>(CollectionName);
    }
    
    protected abstract string CollectionName { get; }

    protected async Task<TEntity?> GetItemAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, id);
        var result = await Collection.FindAsync(filter);
        return await result.SingleOrDefaultAsync();
    }

    protected Task InsertOneAsync(TEntity document)
    {
        return Collection.InsertOneAsync(document);
    }
    
    protected Task DeleteOneAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, id);
        return Collection.DeleteOneAsync(filter);
    }
}