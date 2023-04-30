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
using MovieSearch.Application.Queries.GetSearchRequest;
using NUnit.Framework;

namespace MovieSearch.Tests.RequestHandlers;

public class GetSearchRequestQueryHandlerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Given_SearchRequest_ShouldReturn()
    {
        var fixture = new TestFixture();
        var searchRequest = fixture.Create<SearchRequest>();

        var sut = fixture
            .WithSearchRequest(searchRequest)
            .GetHandler();

        // act
        var response = await sut.Handle(new GetSearchRequestQuery(
            searchRequest.Id
        ), CancellationToken.None);

        // assert
        response.Should().BeEquivalentTo(searchRequest);
    }
    
    [Test]
    public async Task Given_SearchRequest_When_NotFound_ShouldThrow()
    {
        var fixture = new TestFixture();
        var date = fixture.Create<DateTime>();

        var sut = fixture
            .WithSearchRequest(null!)
            .GetHandler();

        // act
        Func<Task> act = async () =>
        {
            await sut.Handle(new GetSearchRequestQuery(fixture.Create<string>()), CancellationToken.None);
        };

        // assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
    
    private class TestFixture : Fixture
    {
        private readonly Mock<ISearchRequestRepository> _searchRequestRepository;

        public TestFixture()
        {
            _searchRequestRepository = this.Freeze<Mock<ISearchRequestRepository>>();
        }

        public TestFixture WithSearchRequest(SearchRequest searchRequest)
        {
            _searchRequestRepository.Setup(x =>
                    x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(searchRequest);
            return this;
        }

        public IRequestHandler<GetSearchRequestQuery, SearchRequest> GetHandler()
        {
            return new GetSearchRequestQueryHandler(_searchRequestRepository.Object);
        }
    }

}