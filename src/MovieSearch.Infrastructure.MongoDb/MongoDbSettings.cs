namespace MovieSearch.Infrastructure.MongoDb;

public interface IMongoDbSettings
{
    string DatabaseName { get; }
    string ConnectionString { get; }
}

public class MongoDbSettings : IMongoDbSettings
{
    public string DatabaseName { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}