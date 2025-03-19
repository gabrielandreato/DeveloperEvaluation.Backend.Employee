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

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service to handle user operations.</param>
        /// <param name="mapper">Mapper, to translate entities to response representations</param>
        public UserController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            _mapper = mapper;
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
            try
            {
                var validator = new CreateUserRequestValidator();
                var validationResult = await validator.ValidateAsync(createUserRequest);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                var user = await userService.CreateAsync(createUserRequest, User);
                return Created("api/user", _mapper.Map<UserResponse>(user));
            }
            catch (Exception e)
            {
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
            try
            {
                var token = await userService.Login(loginUserRequest);
                return Ok(token);
            }
            catch (Exception e)
            {
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
                return Ok(pagedList);
            }
            catch (Exception e)
            {
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
            try
            {
                _ = await userService.RemoveAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
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
            try
            {
                var user = await userService.GetByIdAsync(id);
                return Ok(_mapper.Map<UserResponse>(user));
            }
            catch (Exception e)
            {
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
            try
            {
                var validator = new UpdateUserRequestValidator();
                var validationResult = await validator.ValidateAsync(updateUserRequest);
                if (!validationResult.IsValid)
                    return BadRequest(validationResult.Errors);

                var updatedUser = await userService.UpdateAsync(id, updateUserRequest);
                return Ok(_mapper.Map<UserResponse>(updatedUser));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}