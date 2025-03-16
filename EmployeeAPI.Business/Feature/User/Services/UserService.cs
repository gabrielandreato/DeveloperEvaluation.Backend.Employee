using AutoMapper;
using Employes.Feature.User.Interfaces;
using Employes.Feature.User.Requests;
using Microsoft.AspNetCore.Identity;

namespace Employes.Feature.User.Services;

/// <summary>
///     Provides services to manage user operations such as registration and authentication.
/// </summary>
public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly SignInManager<ModelLibrary.Entities.User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<ModelLibrary.Entities.User> _userManager;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserService" /> class.
    /// </summary>
    /// <param name="mapper">The object mapper for entity transformations.</param>
    /// <param name="userManager">The user manager for handling user operations.</param>
    /// <param name="signInManager">The sign-in manager for handling user authentication.</param>
    /// <param name="tokenService">The token service for generating authentication tokens.</param>
    public UserService(
        IMapper mapper,
        UserManager<ModelLibrary.Entities.User> userManager,
        SignInManager<ModelLibrary.Entities.User> signInManager,
        ITokenService tokenService)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Creates a new user in the system asynchronously.
    /// </summary>
    /// <param name="createUserRequest">
    /// An instance of <see cref="CreateUserRequest"/> containing the information required to create a new user.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the newly created <see cref="ModelLibrary.Entities.User"/> object.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when the user registration process fails. The inner exception provides details about the specific error.
    /// </exception>
    public async Task<ModelLibrary.Entities.User> CreateAsync(CreateUserRequest createUserRequest)
    {
        var user = _mapper.Map<CreateUserRequest, ModelLibrary.Entities.User>(createUserRequest);

        var result = await _userManager.CreateAsync(user, createUserRequest.Password);

        if (!result.Succeeded)
            throw new Exception("Failed to register user!", new Exception(result.Errors.First().Description));
    
        return user;
    }

    /// <summary>
    ///     Authenticates a user and returns a JWT token asynchronously.
    /// </summary>
    /// <param name="request">The request containing login details.</param>
    /// <returns>The JWT token string if authentication is successful.</returns>
    /// <exception cref="Exception">Thrown when authentication fails.</exception>
    public async Task<string> Login(LoginUserRequest request)
    {
        try
        {
            var result = await _signInManager.PasswordSignInAsync(
                request.UserName,
                request.Password,
                false,
                false
            );

            if (!result.Succeeded) throw new ApplicationException("User not authenticated");

            var user = _signInManager
                .UserManager
                .Users
                .First(x => x.NormalizedUserName == request.UserName);

            var token = _tokenService.GenerateToken(user);

            return token;
        } catch (Exception e)
        {
            throw new Exception("Authentication failed", e.InnerException);
        }
    }
}