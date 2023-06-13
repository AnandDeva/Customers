
using AutoFixture;
using CustomerManagement.API.Config;
using CustomerManagement.API.Models;
using CustomerManagement.API.Services;
using CustomerManagement.API.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using Xunit;

namespace CustomerManagement.API.Tests.Systems.Services
{
    public class UsersServiceTest
    {
        [Fact]
        public async Task GetAllUsers_WhenCalled_InvokesHttpRequest()
        {
            //Arrange
            var fixture = new Fixture();
            var expectedResponse = fixture.Create<List<User>>();
            var handlerMock = MockHttpMessageHandler<User>.SetUpBasicGetResourceList(expectedResponse);
            var httpClient = new HttpClient(handlerMock.Object);
            var endpoint = "https://example.com/users";
            var config = Options.Create(new UsersApiOptions
            {
                EndPoint = endpoint
            });
            var sut = new UsersService(httpClient, config);
            //Act
            await sut.GetAllUsers();
            //Assert
            handlerMock
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>()
                );
        }

        [Fact]
        public async Task GetAllUsers_WhenHits404_ReturnsEmptyListOfUsers()
        {
            //Arrange
            var handlerMock = MockHttpMessageHandler<User>
                .SetUpReturn404();
            var httpClient = new HttpClient(handlerMock.Object);
            var endpoint = "https://example.com/users";
            var config = Options.Create(new UsersApiOptions
            {
                EndPoint = endpoint
            });
            var sut = new UsersService(httpClient, config);
            //Act
            var result = await sut.GetAllUsers();
            //Assert
            result.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetAllUsers_WhenCalled_ReturnsListOfUsersOfExpectedSize()
        {
            //Arrange
            var fixture = new Fixture();
            var expectedResponse = fixture.Create<List<User>>();
            var handlerMock = MockHttpMessageHandler<User>
                .SetUpBasicGetResourceList(expectedResponse);
            var httpClient = new HttpClient(handlerMock.Object);
            var endpoint = "https://example.com/users";
            var config = Options.Create(new UsersApiOptions
            {
                EndPoint = endpoint
            });
            var sut = new UsersService(httpClient, config);
            //Act
            var result = await sut.GetAllUsers();
            //Assert
            result.Count.Should().Be(expectedResponse.Count);
        }

        [Fact]
        public async Task GetAllUsers_WhenCalled_InvokesConfiguredExternalUrl()
        {
            //Arrange
            var fixture = new Fixture();
            var expectedResponse = fixture.Create<List<User>>();
            var endpoint = "https://example.com/users";
            var handlerMock = MockHttpMessageHandler<User>
                .SetUpBasicGetResourceList(expectedResponse, endpoint);
            var httpClient = new HttpClient(handlerMock.Object);

            var config = Options.Create(new UsersApiOptions
            {
                EndPoint = endpoint
            });
            var sut = new UsersService(httpClient, config);
            //Act
            var result = await sut.GetAllUsers();
            //Assert
            handlerMock
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>
                            (req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == endpoint),
                    ItExpr.IsAny<CancellationToken>()
                );
        }
    }
}
