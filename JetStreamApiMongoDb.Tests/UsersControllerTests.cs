using JetStreamApiMongoDb.Common;
using JetStreamApiMongoDb.Controllers;
using JetStreamApiMongoDb.DTOs.Requests;
using JetStreamApiMongoDb.Interfaces;
using JetStreamApiMongoDb.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel;
using System.Dynamic;

namespace JetStreamApiMongoDb.Tests
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ITokenService>_mockTokenService;
        private readonly UserController _controller;

        public UsersControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockTokenService = new Mock<ITokenService>();
            _controller = new UserController(_mockUserService.Object, _mockTokenService.Object);
        }

        [Fact]
        public async Task LoginSuccessReturnsOkWithToken()
        {
            // Arrange
            var userLoginDto = new UserLoginDTO
            {
                UserName = "user",
                Password = "Password"
            };
            _mockUserService.Setup(x => x.Authenticate(userLoginDto.UserName, userLoginDto.Password))
                .Returns(Task.FromResult(true));

            var mockUser = new User
            {
                Name = "User Name",
                Role = Roles.USER
            };
            _mockUserService.Setup(x => x.GetUserByUsername(userLoginDto.UserName))
                .Returns(Task.FromResult(mockUser));

            _mockTokenService.Setup(x => x.GenerateToken(userLoginDto.UserName, It.IsAny<string>()))
                .Returns("token");

            // Act
            var result = await _controller.Authenticate(userLoginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            IDictionary<string, object> response = new ExpandoObject();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(okResult.Value.GetType()))
            {
                response.Add(property.Name, property.GetValue(okResult.Value));
            }

            Assert.True(response.ContainsKey("Username") || response.ContainsKey("UserName"), "Response does not contain a username property.");
            var usernameProperty = response.ContainsKey("Username") ? "Username" : "UserName";
            Assert.Equal(userLoginDto.UserName, response[usernameProperty]);
            Assert.Equal("token", response["Token"]);
        }

        private bool HasProperty(dynamic obj, string name)
        {
            return ((Type)obj.GetType()).GetProperty(name) != null;
        }

        [Fact]
        public async Task LoginFailureReturnsUnauthorized()
        {
            //Arrange
            var userLoginDto = new UserLoginDTO
            {
                UserName = "user",
                Password = "Password"
            };
            _mockUserService.Setup(x => x.Authenticate(userLoginDto.UserName, userLoginDto.Password))
                .Returns(Task.FromResult(false));

            //Act
            var result = await _controller.Authenticate(userLoginDto);

            //Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Benutzername oder Passwort ist falsch.", unauthorizedResult.Value);
        }
    }
}
