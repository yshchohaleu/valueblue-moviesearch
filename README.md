# valueblue-moviesearch


## Estimations:

| Task | Estimations | Comments |
| ----------- | ----------- | ----------- |
| initial project structure and base features (base mongo db repository, exception handling, api controllers, user context, swagger configuration) | 4h | Took longer, spent time checking mongo db docs |
| omdb integration | 2h |
| search request operations (save, get, delete, statistics) | 5h |  Took longer, spent time checking mongo db docs |
| api key protection | 1h | |
| unit tests | 2h | |


## Architecture


The app is built with some sort of onion architecture.
Domain and Application parts here were united into one `Application` project as domain does not really contain any complex entities with their own logic. Plus in this project in `Ports` folder we define interfaces required for application handlers.


Two separate projects `Infrastructure.MongoDb` and `Providers.Omdb` contain the logic for database access implementation and external API integration accordingly. They implement interfaces defined in `Application`.


The final connection of all parts happens in `Program.cs` file


```
builder.Services.AddOmdb(builder.Configuration);
builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddApplication();
```


Thus it's relatively easy to change implementation to connect other types of movie providers or databases.


Execution flow is handled using Mediatr.


## Database


Database operations are implemented in `SearchRequestRepository`. The repository also implements `IAsyncInitializer` interfaces meaning that required DB configuration will be made prior to application start.


The fetching of multiple search requests is implemented with pagination.


To store successful search requests in the database `SearchRequestLogDecorator` class was implemented upon the `ISearchMovie` interface. This might not be the optimal solution as sometimes it can be considered as hiding of business logic inside decorators that should have more supportive functions (logging, caching, etc). Though I think for assignment where the purpose is to show your skills it is acceptable 😁


## API Key


A simple protection using API Key was implemented for the `/admin/` part of the endpoints set.
Valid API keys are stored in SecuritySettings section of configuration


```
"SecuritySettings": {
    "ApiKeys": [
      "01B98C12-42AF-4A95-BAD9-AD89C5977D35"
    ]
  }
```


To make an API call with API Key it is necessary to add `X-API-Key: [APIKEY]` header to the response.


```
curl -X 'GET' \
  'https://localhost:7262/api/v1/admin/searchrequest/644a9394785532a28234fe95' \
  -H 'accept: text/plain' \
  -H 'X-API-Key: 01B98C12-42AF-4A95-BAD9-AD89C5977D35'
```


If the API key is missed or incorrect `401` status code is returned.


## Configuration


The configuration contains three main sections:
```
"MongoDbSettings": {
    "DatabaseName": "moviesearch",
    "ConnectionString": "[connection string]"
  },
  "OmdbSettings": {
    "BaseUrl": "https://www.omdbapi.com/",
    "ApiKey": "7f2210b7"
  },
  "SecuritySettings": {
    "ApiKeys": [
      "01B98C12-42AF-4A95-BAD9-AD89C5977D35"
    ]
  }
```


1. MongoDbSettings - connection string and database name
2. OmdbSettings - base URL and API key for OMDb integration
3. SecuritySettings - API keys


To run the application all these sections should be set up with correct values.

## Unit tests

There are two types of sample unit tests in solution:
- repository tests
- handler tests

`AutoFixture` is used for mock object creation, `FluentAssertions` is used to simplify assertion of the results.

For repository unit tests the `EphemeralMongo6` package is used to run mongo db instancesa.

## ToDo

1. Add proper logging (for example seq)
2. Better error handling with correlation id
3. Integration test
4. Support of value objects - this is a really nice improvement to begin to use them in `SearchRequest` entity. But it requires more time to implement a proper serializator for mongo db.
5. Add more layers of DTOs. For example in controllers level. But for such a small solution it is not required and we can just return entities we use in the `Application` project.
