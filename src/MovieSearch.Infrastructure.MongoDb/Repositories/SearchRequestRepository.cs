using MongoDB.Bson;
using MongoDB.Driver;
using MovieSearch.Application.Entities.Requests;
using MovieSearch.Application.Ports;
using MovieSearch.Shared.Hosting;

namespace MovieSearch.Infrastructure.MongoDb.Repositories
{
    public class SearchRequestRepository : MongoDbRepository<SearchRequest>, ISearchRequestRepository, IAsyncInitializer
    {
        public async Task InitializeAsync()
        {
            // create collection if not exists
            var collections = await Database.ListCollectionNamesAsync(new ListCollectionNamesOptions
            {
                Filter = new BsonDocument("name", CollectionName)
            });
            var exists = await collections.AnyAsync();

            if (!exists)
            {
                await Database.CreateCollectionAsync(CollectionName);
                
                // create index
                var indexKeysDefinition = Builders<SearchRequest>.IndexKeys.Ascending(x => x.Timestamp);
                await Collection.Indexes.CreateOneAsync(new CreateIndexModel<SearchRequest>(indexKeysDefinition));
            }
        }

        public SearchRequestRepository(IMongoDatabase database) : base(database)
        {
        }

        protected override string CollectionName { get; } = nameof(SearchRequest).ToLower();

        public Task SaveAsync(SearchRequest searchRequest)
        {
            return base.InsertOneAsync(searchRequest);
        }

        public async Task<SearchRequests> GetAsync(
            DateTime? from, 
            DateTime? to, 
            int page = 0, 
            int pageSize = 10)
        {
            var countFacet = AggregateFacet.Create("count",
                PipelineDefinition<SearchRequest, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<SearchRequest>()
                }));

            var dataFacet = AggregateFacet.Create("data",
                PipelineDefinition<SearchRequest, SearchRequest>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Sort(Builders<SearchRequest>.Sort.Ascending(x => x.Timestamp)),
                    PipelineStageDefinitionBuilder.Skip<SearchRequest>(page * pageSize),
                    PipelineStageDefinitionBuilder.Limit<SearchRequest>(pageSize),
                }));

            
            var builder = Builders<SearchRequest>.Filter;
            var filter = 
                (from.HasValue 
                    ? builder.Gte(x => x.Timestamp, from) 
                    : builder.Empty)
                &
                (to.HasValue
                    ? builder.Lte(x => x.Timestamp, to)
                    : builder.Empty);

            var aggregation = await Collection.Aggregate()
                .Match(filter)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            var count = aggregation.First()
                .Facets.First(x => x.Name == "count")
                .Output<AggregateCountResult>()?.FirstOrDefault()?.Count ?? 0;

            var data = aggregation.First()
                .Facets.First(x => x.Name == "data")
                .Output<SearchRequest>();

            return new SearchRequests(data.ToArray(), page, count);
        }

        public async Task<DailyRequestStatistics?> GetDailyStatisticsAsync(DateTime date)
        {
            var result = await Collection.Aggregate()
                .Match(k => k.Timestamp >= date.Date && k.Timestamp < date.Date.AddDays(1).AddTicks(-1))
                .Group(k => new
                    {
                        year = k.Timestamp.Year,
                        month = k.Timestamp.Month,
                        day = k.Timestamp.Day
                    },
                    g => new { _id = g.Key, count = g.Count() }
                )
                .SortBy(d => d._id)
                .SingleOrDefaultAsync();

            return result != null
                ? new DailyRequestStatistics(new DateTime(result._id.year, result._id.month, result._id.day),
                    result.count)
                : null;
        }

        public Task DeleteAsync(string id)
        {
            return base.DeleteOneAsync(id);
        }

        public Task<SearchRequest> GetByIdAsync(string id)
        {
            return base.GetItemAsync(id);
        }
    }
}