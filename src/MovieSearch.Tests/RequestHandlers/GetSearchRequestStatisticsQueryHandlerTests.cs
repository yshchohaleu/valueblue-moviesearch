using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Moq;
using MovieSearch.Application.Entities.Requests;
using MovieSearch.Application.Exceptions;
using MovieSearch.Application.Ports;
using MovieSearch.Application.Queries.GetSearchRequestStatistics;
using NUnit.Framework;

namespace MovieSearch.Tests.RequestHandlers;

public class GetSearchRequestStatisticsQueryHandlerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Given_RequestStatistics_ShouldReturn()
    {
        var fixture = new TestFixture();
        var dailyStatistics = new DailyRequestStatistics(fixture.Create<DateTime>(), fixture.Create<int>());

        var sut = fixture
            .WithStatistics(dailyStatistics)
            .GetHandler();

        // act
        var response = await sut.Handle(new GetSearchRequestStatisticsQuery(
            fixture.Create<DateTime>()
        ), CancellationToken.None);

        // assert
        response.Date.Should().Be(dailyStatistics.Date);
        response.RequestCount.Should().Be(dailyStatistics.RequestCount);
    }
    
    [Test]
    public async Task Given_RequestStatistics_WithZeroStatistics_ShouldReturn()
    {
        var fixture = new TestFixture();
        var date = fixture.Create<DateTime>();

        var sut = fixture
            .WithStatistics(DailyRequestStatistics.Zero(date))
            .GetHandler();

        // act
        var result = await sut.Handle(new GetSearchRequestStatisticsQuery(
                date
            ), CancellationToken.None);

        // assert
        result.RequestCount.Should().Be(0);
        result.Date.Should().Be(date);
    }
    
    private class TestFixture : Fixture
    {
        private readonly Mock<ISearchRequestRepository> _searchRequestRepository;

        public TestFixture()
        {
            _searchRequestRepository = this.Freeze<Mock<ISearchRequestRepository>>();
        }

        public TestFixture WithStatistics(DailyRequestStatistics statistics)
        {
            _searchRequestRepository.Setup(x =>
                    x.GetDailyStatisticsAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(statistics);
            return this;
        }

        public IRequestHandler<GetSearchRequestStatisticsQuery, DailyRequestStatistics> GetHandler()
        {
            return new GetSearchRequestStatisticsQueryHandler(_searchRequestRepository.Object);
        }
    }

}