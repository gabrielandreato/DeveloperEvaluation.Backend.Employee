using Employes.Feature.User.Requests;

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
    /// An instance of <see cref="CreateUserRequest"/> containing the details required to create the new user.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the newly created <see cref="ModelLibrary.Entities.User"/>.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when user creation fails. The exception may include details about validation errors or database issues.
    /// </exception>
    Task<ModelLibrary.Entities.User> CreateAsync(CreateUserRequest createUserRequest);

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
}