using Employes.Feature.User.Interfaces;
using Employes.Feature.User.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Employes.Feature.User;

/// <summary>
///     Controller responsible for user-related operations.
/// </summary>
[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="createUserRequest">
    /// An instance of <see cref="CreateUserRequest"/> containing the user's registration information, such as username, password, and other details.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> indicating the outcome of the registration process.
    /// </returns>
    /// <response code="201">User registered successfully, returns the created user object.</response>
    /// <response code="400">Occurs if validation fails or if an error occurs during registration, returning detailed error information.</response>
    [HttpPost]
    public async Task<IActionResult> RegisterUser(CreateUserRequest createUserRequest)
    {
        try
        {
            var validator = new CreateUserRequestValidator();
            var validationResult = await validator.ValidateAsync(createUserRequest);
        
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
        
            var user = await userService.CreateAsync(createUserRequest);
            return Created("User registered successfully!", user);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="loginUserRequest">The login details.</param>
    /// <returns>The JWT token if login is successful.</returns>
    /// <response code="200">Token generated successfully.</response>
    /// <response code="400">Invalid login details provided.</response>
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginUserRequest loginUserRequest)
    {
        try
        {
            var token = await userService.Login(loginUserRequest);
            return Ok(token);
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}