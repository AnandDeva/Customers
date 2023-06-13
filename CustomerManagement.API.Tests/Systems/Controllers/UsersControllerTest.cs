using Customers.API.Controllers;
using Customers.API.Models;
using Customers.API.Services;
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
        var users = new List<User>()
            {
                new User {
                    Name = "Anand",
                    Email = "anand@test.com",
                    Address = new Address() {
                        City = "Chennai",
                        Street = "Test",
                        ZipCode = "900922"
                    }
                },
                new User {
                    Name = "Naveen",
                    Email = "naveen@test.com",
                    Address = new Address() {
                        City = "Gujarat",
                        Street = "Sandiwa",
                        ZipCode = "900921"
                    }
                }
            };
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
        var users = new List<User>()
            {
                new User {
                    Name = "Deepak",
                    Email = "deepak@test.com",
                    Address = new Address() {
                        City = "HYD",
                        Street = "Dwefe",
                        ZipCode = "900921"
                    }
                },
                new User {
                    Name = "Kuldeep",
                    Email = "kul@test.com",
                    Address = new Address() {
                        City = "Indore",
                        Street = "Sandiwak",
                        ZipCode = "900925"
                    }
                }
            };
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