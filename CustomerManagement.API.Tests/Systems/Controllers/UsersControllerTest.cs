using AutoFixture;
using CustomerManagement.API.Controllers;
using CustomerManagement.API.Models;
using CustomerManagement.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Systems.Controllers;

public class UsersControllerTest
{
    [Fact]
    public async Task Get_OnSuccess_ReturnsStatusCode200()
    {
        //Arrange
        var mockUsersService = new Mock<IUsersService>();
        var fixture = new Fixture();
        var users = fixture.Create<List<User>>();
            
        mockUsersService
           .Setup(service => service.GetAllUsers())
           .ReturnsAsync(users);
        var sut = new UsersController(mockUsersService.Object);
        //Act
        var result = (OkObjectResult)await sut.Get();
        //Assert
        result.StatusCode.Should().Be(200);

    }

    [Fact]
    public async Task Get_OnSuccess_InvokesUserServiceOnce()
    {
        //Arrange
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());
        var sut = new UsersController(mockUsersService.Object);

        //Act
        var result = await sut.Get();
        //Assert
        mockUsersService.Verify(
            service => service.GetAllUsers(),
            Times.Once()
        );
    }

    [Fact]
    public async Task Get_OnSuccess_ReturnsListOfUsers()
    {
        //Arrange
        var mockUsersService = new Mock<IUsersService>();
        var fixture = new Fixture();
        var users = fixture.Create<List<User>>();
        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(users);
        var sut = new UsersController(mockUsersService.Object);

        //Act
        var result = await sut.Get();
        //Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult)result;
        objectResult.Value.Should().BeOfType<List<User>>();
    }

    [Fact]
    public async Task Get_OnNoUsersFound_ReturnsList404()
    {
        //Arrange
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());
        var sut = new UsersController(mockUsersService.Object);

        //Act
        var result = await sut.Get();
        //Assert
        result.Should().BeOfType<NotFoundResult>();
        var objectResult = (NotFoundResult)result;
        objectResult.StatusCode.Should().Be(404);
    }
}