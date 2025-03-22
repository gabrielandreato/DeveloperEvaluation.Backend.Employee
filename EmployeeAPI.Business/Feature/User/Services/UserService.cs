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

    /// <summary>
    /// Initializes a new instance of the <see cref="UserService" /> class.
    /// </summary>
    /// <param name="mapper">The object mapper for entity transformations.</param>
    /// <param name="tokenService">The token service for generating authentication tokens.</param>
    /// <param name="userRepository">The repository for accessing user data.</param>
    public UserService(
        IMapper mapper,
        ITokenService tokenService,
        IUserRepository userRepository)
    {
        _mapper = mapper;
        _tokenService = tokenService;
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    public async Task<ModelLibrary.Entities.User> CreateAsync(CreateUserRequest createUserRequest,
        ClaimsPrincipal userClaims)
    {
        var user = _mapper.Map<CreateUserRequest, ModelLibrary.Entities.User>(createUserRequest);

        ValidateUserRole(userClaims, user.Role);
        
        await ValidateUser(user);
        
        user.SetEncryptedPassword();

        var result = await _userRepository.CreateAsync(user);

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
        var userByDocument = await _userRepository.GetListAsync(new UserFilter()
        {
            DocumentNumber = user.DocumentNumber,
        });
        if (userByDocument.Items.Count != 0) 
            throw new ApplicationException("Employee with this document number already exists");
        
        var userByUserName = await _userRepository.GetListAsync(new UserFilter()
        {
            UserName = user.UserName,
        });
        if (userByUserName.Items.Count != 0) 
            throw new ApplicationException("Employee with this userName already exists");
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
            throw new UnauthorizedAccessException("User claims are not available.");

        var currentUserRoleClaim = userClaims.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrWhiteSpace(currentUserRoleClaim))
            throw new UnauthorizedAccessException("Current user role is missing.");

        if (!Enum.TryParse(currentUserRoleClaim, out Role currentUserRole))
            throw new UnauthorizedAccessException("Invalid role in the current user's claims.");

        if (newUserRole > currentUserRole)
            throw new InvalidOperationException("You do not have permission to create a user with a higher role.");
    }

    /// <inheritdoc />
    public async Task<PagedList<ModelLibrary.Entities.User>> GetListAsync(UserFilter filter)
    {
        var users = await _userRepository.GetListAsync(filter);
        
        return users;
    }

    /// <inheritdoc />
    public async Task<ModelLibrary.Entities.User> UpdateAsync(int id, UpdateUserRequest updateUserRequest)
    {
        var first = await _userRepository.GetByIdAsync(id);
        
        if(first is null) throw new ApplicationException("User not found.");
        
        _mapper.Map(updateUserRequest, first);
        first.SetEncryptedPassword();
        await _userRepository.SaveChangesAsync();
        
        return first;
    }

    /// <inheritdoc />
    public async Task<ModelLibrary.Entities.User> RemoveAsync(int id)
    {
        return await _userRepository.RemoveAsync(id);
    }

    /// <inheritdoc />
    public async Task<ModelLibrary.Entities.User?> GetByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    /// <inheritdoc />
    public async Task<string> Login(LoginUserRequest request)
    {
        try
        {
            var user = await _userRepository.GetByUserNameAsync(request.UserName);
            
            if (user is null) throw new ApplicationException("User not found."); 
            
            user.ValidatePassword(request.Password);
            
            var token = _tokenService.GenerateToken(user);
            
            return token;
        } catch (Exception e)
        {
            throw new Exception("Authentication failed", e.InnerException);
        }
    }

   
}