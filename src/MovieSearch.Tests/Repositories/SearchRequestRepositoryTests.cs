using System;
using System.Threading.Tasks;
using FluentAssertions;
using MovieSearch.Application.Entities.Requests;
using MovieSearch.Infrastructure.MongoDb.Repositories;
using NUnit.Framework;

namespace MovieSearch.Tests.Repositories;

public class SearchRequestRepositoryTests : MongoDbTest
{
    private SearchRequestRepository? _sut;

    private readonly SearchRequest[] _searchRequests = new[]
    {
        new SearchRequest("644a9394785532a28234fe95", "Test", "tt0903745", 574,
            DateTime.Parse(@"2023-04-27T15:00:00.000Z").ToUniversalTime(), "::1"),
        new SearchRequest("644a99ecb95795623d2e330e", "Bad", "tt0119217", 815,
            DateTime.Parse("2023-04-27T16:00:08.000Z").ToUniversalTime(), "::1"),
        new SearchRequest("644baa206250846222537b73", "Dead Poets Society", "tt0097165", 166,
            DateTime.Parse("2023-04-28T17:00:00.000Z").ToUniversalTime(), "::1"),
        new SearchRequest("644baa326250846222537b74", "Good", "tt0098354", 536,
            DateTime.Parse("2023-04-29T18:10:00.000Z").ToUniversalTime(), "::1"),
        new SearchRequest("644bc9659e373c615af14b52", "Hello", "tt2407380", 330,
            DateTime.Parse("2023-04-30T00:00:00.000Z").ToUniversalTime(), "::1"),
        new SearchRequest("644bc9659e373c615af14b57", "Awesome", "tt2407381", 1000,
            DateTime.Parse("2023-05-02T00:00:00.000Z").ToUniversalTime(), "::1"),

    };
    
    [SetUp]
    public async Task Setup()
    {
        _sut = new SearchRequestRepository(Database);
        await _sut.InitializeAsync();

        var collection = Database.GetCollection<SearchRequest>(nameof(SearchRequest).ToLower());
        
        await collection.InsertManyAsync(_searchRequests);
    }

    [TearDown]
    public async Task TearDown()
    {
        await Database.DropCollectionAsync(nameof(SearchRequest).ToLower());
    }

    [Test]
    public async Task Given_SavedRequestsPerDay_When_StatisticsCalculated_ShouldReturn()
    {
        var date = DateTime.Parse("2023-04-27");
        
        // act
        var dailyStatistics = await _sut!.GetDailyStatisticsAsync(date);
        
        // assert
        dailyStatistics.Should().NotBeNull();
        dailyStatistics!.Date.Should().Be(date);
        dailyStatistics!.RequestCount.Should().Be(2);
    }
    
    [Test]
    public async Task Given_SavedRequestsPerDay_When_StatisticsAbsent_ShouldReturnZero()
    {
        var date = DateTime.Parse("2023-03-01");
        
        // act
        var dailyStatistics = await _sut!.GetDailyStatisticsAsync(date);
        
        // assert
        dailyStatistics!.Date.Should().Be(date);
        dailyStatistics!.RequestCount.Should().Be(0);
    }
    
    [Test]
    public async Task Given_NotSavedRequestsPerDay_When_StatisticsCalculated_ShouldReturnZero()
    {
        var date = DateTime.Parse("2023-04-10");
        
        // act
        var dailyStatistics = await _sut!.GetDailyStatisticsAsync(date);
        
        // assert
        dailyStatistics.Should().Be(DailyRequestStatistics.Zero(date));
    }
    
    [Test]
    public async Task Given_SavedRequest_When_SearchedById_ShouldReturn()
    {
        var searchRequest = _searchRequests[0];
        
        // act
        var found = await _sut!.GetByIdAsync(searchRequest.Id);
        
        // assert
        found.Should().NotBeNull();
        found.Should().BeEquivalentTo(searchRequest);
    }
    
    [Test]
    public async Task Given_SavedRequests_When_Searched_ShouldReturn()
    {
        // act
        var found = await _sut!.GetAsync(null, null);
        
        // assert
        found.Items.Should().HaveCount(6);
        found.Page.Should().Be(0);
        found.Total.Should().Be(6);
        found.Items.Should().BeEquivalentTo(_searchRequests);
    }
    
    [Test]
    public async Task Given_SavedRequests_When_SearchedByDate_ShouldReturn()
    {
        // act
        var from = DateTime.Parse("2023-04-29");
        var to = DateTime.Parse("2023-05-01");
        var found = await _sut!.GetAsync(from, to);
        
        // assert
        found.Items.Should().HaveCount(2);
        found.Page.Should().Be(0);
        found.Total.Should().Be(2);
        found.Items.Should().BeEquivalentTo(new [] { _searchRequests[3], _searchRequests[4] });
    }
    
    [Test]
    public async Task Given_SavedRequests_When_SearchedWithPaging_ShouldReturn()
    {
        // act
        var found = await _sut!.GetAsync(null, null, page: 2, pageSize: 2);
        
        // assert
        found.Items.Should().HaveCount(2);
        found.Page.Should().Be(2);
        found.Total.Should().Be(6);
        found.Items.Should().BeEquivalentTo(new [] { _searchRequests[4], _searchRequests[5] });
    }
}