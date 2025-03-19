using System.Security.Claims;
using Employes.Feature.User.Requests;
using Employes.Feature.User.Response;
using ModelLibrary.Common;
using ModelLibrary.Filter;

namespace Employes.Feature.User.Interfaces;

/// <summary>
/// Defines the operations available for managing user accounts.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Asynchronously creates a new user with the given information.
    /// </summary>
    /// <param name="createUserRequest">
    ///     An instance of <see cref="CreateUserRequest"/> containing the details required to create the new user.
    /// </param>
    /// <param name="user">
    ///     A <see cref="ClaimsPrincipal"/> representing the current user making the request, used for validation of claims.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the newly created <see cref="ModelLibrary.Entities.User"/>.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when user creation fails. The exception may include details about validation errors or database issues.
    /// </exception>
    Task<ModelLibrary.Entities.User> CreateAsync(CreateUserRequest createUserRequest, ClaimsPrincipal user);

    /// <summary>
    /// Asynchronously authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="request">
    /// An instance of <see cref="LoginUserRequest"/> containing the user's login credentials.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, with a string containing the JWT token if authentication succeeds.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when authentication fails due to incorrect credentials or other login issues.
    /// </exception>
    Task<string> Login(LoginUserRequest request);

    /// <summary>
    /// Updates an existing user asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user to be updated.</param>
    /// <param name="updateUserRequest">
    /// The instance of <see cref="UpdateUserRequest"/> containing the data to update user information.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the updated <see cref="ModelLibrary.Entities.User"/> object.
    /// </returns>
    Task<ModelLibrary.Entities.User> UpdateAsync(int id, UpdateUserRequest updateUserRequest);

    /// <summary>
    /// Retrieves a paginated and filtered list of users.
    /// </summary>
    /// <param name="filter">
    ///     The search parameters and filters to apply, represented as an instance of <see cref="DefaultQueryParameter{UserFilter}"/>.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing a paginated list of users.
    /// </returns>
    Task<PagedList<ModelLibrary.Entities.User>> GetListAsync(UserFilter filter);

    /// <summary>
    /// Removes a user by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user to remove.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the removed <see cref="ModelLibrary.Entities.User"/> object.
    /// </returns>
    Task<ModelLibrary.Entities.User> RemoveAsync(int id);

    /// <summary>
    /// Retrieves a user by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the retrieved <see cref="ModelLibrary.Entities.User"/> object, or null if not found.
    /// </returns>
    Task<ModelLibrary.Entities.User> GetByIdAsync(int id);
}