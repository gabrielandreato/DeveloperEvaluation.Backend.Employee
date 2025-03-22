using System.Text.Json;
using AutoMapper;
using Employes.Feature.User.Interfaces;
using Employes.Feature.User.Requests;
using Employes.Feature.User.Response;
using Employes.Feature.User.Validator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Common;
using ModelLibrary.Filter;

namespace Employes.Feature.User
{
    /// <summary>
    /// Controller responsible for user-related operations such as registration, authentication, updates, and retrieval.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service to handle user operations.</param>
        /// <param name="mapper">Mapper, to translate entities to response representations</param>
        /// <param name="logger">Logger, generate console logs track application.</param>
        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            this.userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="createUserRequest">
        /// An instance of <see cref="CreateUserRequest"/> containing the user's registration information, such as username, password, and other details.
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating the outcome of the registration process.
        /// </returns>
        /// <response code="201">User registered successfully, and the created user object is returned.</response>
        /// <response code="400">Occurs if validation fails or if an error occurs during registration, detailed error information is returned.</response>
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest createUserRequest)
        {
            _logger.LogInformation("CreateUser called with UserName: {UserName}", createUserRequest.UserName);

            try
            {
                var validator = new CreateUserRequestValidator();
                var validationResult = await validator.ValidateAsync(createUserRequest);

                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Validation failed for CreateUserRequest: {Errors}", validationResult.Errors);
                    return BadRequest(validationResult.Errors);

                }
                var user = await userService.CreateAsync(createUserRequest, User);
                _logger.LogInformation("User created successfully with ID: {UserId}", user.Id);

                return Created("api/user", _mapper.Map<UserResponse>(user));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while creating user");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="loginUserRequest">The login details containing username and password.</param>
        /// <returns>The JWT token if login is successful.</returns>
        /// <response code="200">Token generated successfully.</response>
        /// <response code="400">Invalid login details provided.</response>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginUserRequest loginUserRequest)
        {
            _logger.LogInformation("Login attempt for UserName: {UserName}", loginUserRequest.UserName);

            try
            {
                var token = await userService.Login(loginUserRequest);
                _logger.LogInformation("Login successful for UserName: {UserName}", loginUserRequest.UserName);
                return Ok(token);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Login failed for UserName: {UserName}", loginUserRequest.UserName);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Retrieves a list of users based on specified filtering criteria.
        /// </summary>
        /// <param name="filter">The filter criteria and parameters for retrieving users.</param>
        /// <returns>A list of users that meet the filter criteria.</returns>
        [HttpGet("List")]
        public async Task<IActionResult> GetList([FromQuery] UserFilter filter)
        {
            _logger.LogInformation("GetList called with filter: {Filter}", JsonSerializer.Serialize(filter));

            try
            {
                var users = await userService.GetListAsync(filter);
                
                var pagedList = new PagedList<UserResponse>
                {
                    Items = _mapper.Map<List<UserResponse>>(users.Items),
                    Page = users.Page,
                    PageSize = users.PageSize,
                    TotalCount = users.TotalCount
                };

                _logger.LogInformation("GetList retrieved {TotalCount} users", users.TotalCount);
                return Ok(pagedList);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while retrieving user list");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Removes a user from the system by their unique identifier.
        /// </summary>
        /// <param name="id">The primary key of the user to be removed.</param>
        /// <returns>The removed user object.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync([FromRoute] int id)
        {
            _logger.LogInformation("RemoveAsync called for User ID: {UserId}", id);

            try
            {
                await userService.RemoveAsync(id);
                _logger.LogInformation("User with ID: {UserId} removed successfully", id);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while removing user with ID: {UserId}", id);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The primary key of the user to be retrieved.</param>
        /// <returns>The user object if found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByAsync([FromRoute] int id)
        {
            _logger.LogInformation("GetByAsync called for User ID: {UserId}", id);

            try
            {
                var user = await userService.GetByIdAsync(id);
                _logger.LogInformation("User with ID: {UserId} retrieved successfully", id);
                return Ok(_mapper.Map<UserResponse>(user));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while retrieving user with ID: {UserId}", id);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Updates an existing user with new data.
        /// </summary>
        /// <param name="id">The primary key of the user to be updated.</param>
        /// <param name="updateUserRequest">The updated user data.</param>
        /// <returns>The updated user object.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateUserRequest updateUserRequest)
        {
            _logger.LogInformation("UpdateAsync called for User ID: {UserId}", id);

            try
            {
                var validator = new UpdateUserRequestValidator();
                var validationResult = await validator.ValidateAsync(updateUserRequest);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Validation failed for UpdateUserRequest: {Errors}", JsonSerializer.Serialize(validationResult.Errors));
                    return BadRequest(validationResult.Errors);
                }

                var updatedUser = await userService.UpdateAsync(id, updateUserRequest);
                _logger.LogInformation("User with ID: {UserId} updated successfully", updatedUser.Id);

                return Ok(_mapper.Map<UserResponse>(updatedUser));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while updating user with ID: {UserId}", id);
                return BadRequest(e.Message);
            }
        }
    }
}