using System.Security.Claims;
using AutoMapper;
using Employes.DataLibrary.Repository.Interfaces;
using Employes.Feature.User.Interfaces;
using Employes.Feature.User.Requests;
using ModelLibrary.Common;
using ModelLibrary.Enums;
using ModelLibrary.Filter;

namespace Employes.Feature.User.Services;

/// <summary>
/// Provides services to manage user operations such as registration, authentication, updates, and retrievals.
/// </summary>
public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService" /> class.
    /// </summary>
    /// <param name="mapper">The object mapper for entity transformations.</param>
    /// <param name="tokenService">The token service for generating authentication tokens.</param>
    /// <param name="userRepository">The repository for accessing user data.</param>
    /// <param name="logger">The service to generate application logs</param>
    public UserService(
        IMapper mapper,
        ITokenService tokenService,
        IUserRepository userRepository,
        ILogger<UserService> logger)
    {
        _mapper = mapper;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ModelLibrary.Entities.User> CreateAsync(CreateUserRequest createUserRequest,
        ClaimsPrincipal userClaims)
    {
        _logger.LogInformation("Initiating user creation for UserName: {UserName}", createUserRequest.UserName);

        var user = _mapper.Map<CreateUserRequest, ModelLibrary.Entities.User>(createUserRequest);

        ValidateUserRole(userClaims, user.Role);

        await ValidateUser(user);

        user.SetEncryptedPassword();

        var result = await _userRepository.CreateAsync(user);

        _logger.LogInformation("User created successfully with ID: {UserId}", result.Id);

        return result;
    }

    /// <summary>
    /// Validates a User entity by checking for duplicate Document Numbers and Usernames in the repository.
    /// </summary>
    /// <param name="user">The User entity to validate.</param>
    /// <exception cref="ApplicationException">
    /// Thrown when a User with the same Document Number or Username already exists in the repository.
    /// </exception>
    /// <remarks>
    /// This method performs two validations:
    /// 1. Checks if there is any existing user in the repository with the same Document Number as the provided user. 
    ///    If found, throws an ApplicationException indicating the duplication.
    /// 2. Checks if there is any existing user in the repository with the same Username as the provided user. 
    ///    If found, throws an ApplicationException indicating the duplication.
    /// </remarks>
    private async Task ValidateUser(ModelLibrary.Entities.User user)
    {
        _logger.LogInformation(
            "Validating user for duplicates with UserName: {UserName} and DocumentNumber: {DocumentNumber}",
            user.UserName, user.DocumentNumber);

        var userByDocument = await _userRepository.GetListAsync(new UserFilter()
        {
            DocumentNumber = user.DocumentNumber,
        });
        if (userByDocument.Items.Count != 0)
        {
            _logger.LogWarning("Duplicate document number found for: {DocumentNumber}", user.DocumentNumber);
            throw new ApplicationException("Employee with this document number already exists");
        }

        var userByUserName = await _userRepository.GetListAsync(new UserFilter()
        {
            UserName = user.UserName,
        });
        if (userByUserName.Items.Count != 0)
        {
            _logger.LogWarning("Duplicate userName found for: {UserName}", user.UserName);
            throw new ApplicationException("Employee with this userName already exists");
        }
    }

    /// <summary>
    /// Validates that a user has the proper permissions to create another user with a specified role.
    /// </summary>
    /// <param name="userClaims">The claims of the currently authenticated user, used to verify their role.</param>
    /// <param name="newUserRole">The role of the user being created.</param>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown if the user claims are not available, or if the current user's role is missing or invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the current user does not have permission to create a user with a higher role.
    /// </exception>
    private void ValidateUserRole(ClaimsPrincipal userClaims, Role newUserRole)
    {
        if (userClaims == null)
        {
            _logger.LogError("User claims are not available.");
            throw new UnauthorizedAccessException("User claims are not available.");
        }

        var currentUserRoleClaim = userClaims.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrWhiteSpace(currentUserRoleClaim))
        {
            _logger.LogError("Current user role is missing.");
            throw new UnauthorizedAccessException("Current user role is missing.");
        }

        if (!Enum.TryParse(currentUserRoleClaim, out Role currentUserRole))
        {
            _logger.LogError("Invalid role in the current user's claims.");
            throw new UnauthorizedAccessException("Invalid role in the current user's claims.");
        }

        if (newUserRole > currentUserRole)
        {
            _logger.LogError("Insufficient permissions to create a user with a higher role.");
            throw new InvalidOperationException("You do not have permission to create a user with a higher role.");
        }
    }

    /// <inheritdoc />
    public async Task<PagedList<ModelLibrary.Entities.User>> GetListAsync(UserFilter filter)
    {
        _logger.LogInformation("Retrieving user list with filter: {filter}", filter);
        var users = await _userRepository.GetListAsync(filter);
        _logger.LogInformation("Retrieved {TotalCount} users", users.TotalCount);
        return users;
    }

    /// <inheritdoc />
    public async Task<ModelLibrary.Entities.User> UpdateAsync(int id, UpdateUserRequest updateUserRequest)
    {
        _logger.LogInformation("Updating user with ID: {UserId}", id);

        var first = await _userRepository.GetByIdAsync(id);

        if (first is null)
        {
            _logger.LogError("User not found with ID: {UserId}", id);
            throw new ApplicationException("User not found.");
        }

        _mapper.Map(updateUserRequest, first);
        first.SetEncryptedPassword();
        await _userRepository.SaveChangesAsync();

        _logger.LogInformation("User updated successfully with ID: {UserId}", id);

        return first;
    }

    /// <inheritdoc />
    public async Task<ModelLibrary.Entities.User> RemoveAsync(int id)
    {
        _logger.LogInformation("Removing user with ID: {UserId}", id);
        var user = await _userRepository.RemoveAsync(id);
        _logger.LogInformation("User removed successfully with ID: {UserId}", id);
        return user;
    }

    /// <inheritdoc />
    public async Task<ModelLibrary.Entities.User?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving user by ID: {UserId}", id);
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {UserId}", id);
        }
        else
        {
            _logger.LogInformation("User retrieved successfully with ID: {UserId}", id);
        }

        return user;
    }

    /// <inheritdoc />
    public async Task<string> Login(LoginUserRequest request)
    {
        _logger.LogInformation("Processing login for UserName: {UserName}", request.UserName);

        try
        {
            var user = await _userRepository.GetByUserNameAsync(request.UserName);

            if (user is null)
            {
                _logger.LogError("User not found with UserName: {UserName}", request.UserName);
                throw new ApplicationException("User not found.");
            }

            user.ValidatePassword(request.Password);

            var token = _tokenService.GenerateToken(user);
            _logger.LogInformation("Token generated successfully for UserName: {UserName}", request.UserName);

            return token;
        } catch (Exception e)
        {
            _logger.LogError(e, "Authentication failed for UserName: {UserName}", request.UserName);
            throw new Exception("Authentication failed", e.InnerException);
        }
    }
}