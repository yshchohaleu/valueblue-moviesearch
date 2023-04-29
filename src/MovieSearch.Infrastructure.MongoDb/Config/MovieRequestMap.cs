using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MovieSearch.Application.Entities.Requests;

namespace MovieSearch.Infrastructure.MongoDb.Config
{
    public static class MovieRequestMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<SearchRequest>(map =>
            {
                map.MapIdMember(x => x.Id).SetIdGenerator(new StringObjectIdGenerator());
                map.MapProperty(x => x.SearchToken).SetElementName("search_token");
                map.MapProperty(x => x.ImdbId).SetElementName("imdb_id");
                map.MapProperty(x => x.ProcessingTimeMs).SetElementName("processing_time_ms");
                map.MapProperty(x => x.Timestamp).SetElementName("timestamp");
                map.MapProperty(x => x.IpAddress).SetElementName("ip_address");
                
                map.SetIgnoreExtraElements(true);

                map.MapCreator(x => new SearchRequest(
                    x.Id,
                    x.SearchToken,
                    x.ImdbId,
                    x.ProcessingTimeMs,
                    x.Timestamp,
                    x.IpAddress
                ));
            });
        }
    }
}