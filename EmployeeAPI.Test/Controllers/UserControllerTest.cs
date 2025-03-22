using System.Security.Claims;
using AutoMapper;
using Employes.DataLibrary.Context;
using Employes.DataLibrary.Repository;
using Employes.Feature.User;
using Employes.Feature.User.Interfaces;
using Employes.Feature.User.Profile;
using Employes.Feature.User.Requests;
using Employes.Feature.User.Response;
using Employes.Feature.User.Services;
using EmployesAPI.Test.Entities.User.TestData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Common;
using ModelLibrary.Entities;
using ModelLibrary.Enums;
using ModelLibrary.Filter;
using Moq;

namespace EmployesAPI.Test.Controllers
{
    public class UserControllerTest
    {
        private readonly UserController _controller;
        private readonly IEmployeeDataContext _dbContext = new EmployeeTestDbContext();
        private readonly IMapper _mapper;
        private readonly Mock<ITokenService> tokenService = new();


        public UserControllerTest()
        {
            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>());
            _mapper = mapConfig.CreateMapper();
            var repository = new UserRepository(_dbContext);
            var service = new UserService(_mapper,
                tokenService.Object, repository);
            _controller = new UserController(service, _mapper);
        }


        [Fact]
        public async Task Get_AllParameters_Success()
        {
            // Arrange
            var user = UserTestData.GenerateValidUser();
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            var parametroPesquisa = new UserFilter
            {
                UserName = user.UserName,
                Email = user.Email,
                DocumentNumber = user.DocumentNumber,
                ManagerId = user.ManagerId,
                Role = user.Role,

            };


            // Act
            var result = await _controller.GetList(parametroPesquisa);
            var response = result as ObjectResult;
            var value = response?.Value as PagedList<UserResponse>;

            // Assert
            Assert.Equal(StatusCodes.Status200OK, response?.StatusCode);
            Assert.NotEmpty(value?.Items ?? throw new Exception("Response for Get_AllParameters_Success was null"));
        }

        [Fact]
        public async Task Create_Success()
        {
            // Arrange
            var user = UserTestData.GenerateValidUser();
            var createUserRequest = _mapper.Map<CreateUserRequest>(user);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = await _controller.CreateUser(createUserRequest);
            var response = result as ObjectResult;
            var value = response?.Value as UserResponse;

            // Assert
            Assert.Equal(StatusCodes.Status201Created, response?.StatusCode);
            Assert.NotEqual(0, value.Id);
        }

        [Fact]
        public async Task Update_AllProperties_Success()
        {
            // Arrange
            var user = UserTestData.GenerateValidUser();
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var updateUserRequest = _mapper.Map<UpdateUserRequest>(user);

            updateUserRequest.FirstName = "UpdatedFirstName";
            updateUserRequest.LastName = "UpdatedLastName";
            updateUserRequest.Email = "updatedemail@example.com";
            var id = user.Id;

            // Act
            var result = await _controller.UpdateAsync(id, updateUserRequest);
            var response = result as ObjectResult;
            var value = response?.Value as UserResponse;

            // Assert
            Assert.Equal(StatusCodes.Status200OK, response?.StatusCode);
            Assert.Equal("UpdatedFirstName", value?.FirstName);
            Assert.Equal("UpdatedLastName", value?.LastName);
            Assert.Equal("updatedemail@example.com", value?.Email);
        }

        [Fact]
        public async Task Remove_Success()
        {
            // Arrange
            var user = UserTestData.GenerateValidUser();
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var id = user.Id;

            // Act
            var result = await _controller.RemoveAsync(id);
            var response = result as NoContentResult;

            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, response?.StatusCode);
        }

        [Fact]
        public async Task CreateUser_ExistingDocumentNumber_ShouldThrowApplicationException()
        {
            // Arrange

            var user = UserTestData.GenerateValidUser();
            user.UserName = "ExistingUser";
            user.DocumentNumber = "1234567890";
            user.Email = "existing@example.com";

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var createUserRequest = new CreateUserRequest
            {
                UserName = "NewUser",
                Password = "SecurePass123!",
                RePassword = "SecurePass123!",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                DocumentNumber = "1234567890", // Duplicate Document Number
                PhoneNumbers = new List<CreatePhoneNumberRequest> { new CreatePhoneNumberRequest() },
                DateOfBirth = DateTime.Now.AddYears(-25),
                Role = Role.Employee
            };

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "adminuser"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = await _controller.CreateUser(createUserRequest);
            var response = result as ObjectResult;

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, response?.StatusCode);
            Assert.Equal("Employee with this document number already exists", response!.Value);

        }

        [Fact]
        public async Task CreateUser_ExistingUserName_ShouldThrowApplicationException()
        {
            // Arrange
            var user = UserTestData.GenerateValidUser();

            user.UserName = "ExistingUser";
            user.DocumentNumber = "0987654321";
            user.Email = "existing@example.com";


            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var createUserRequest = new CreateUserRequest
            {
                UserName = "ExistingUser", // Duplicate UserName
                Password = "SecurePass123!",
                RePassword = "SecurePass123!",
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                DocumentNumber = "1122334455",
                PhoneNumbers = new List<CreatePhoneNumberRequest> { new CreatePhoneNumberRequest() },
                DateOfBirth = DateTime.Now.AddYears(-30),
                Role = Role.Employee
            };

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "adminuser"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = await _controller.CreateUser(createUserRequest);
            var response = result as ObjectResult;

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, response?.StatusCode);
            Assert.Equal("Employee with this userName already exists", response!.Value);
        }
        
    }
}