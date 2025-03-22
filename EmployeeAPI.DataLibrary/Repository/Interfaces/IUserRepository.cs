using ModelLibrary.Common;
using ModelLibrary.Entities;
using ModelLibrary.Filter;

namespace Employes.DataLibrary.Repository.Interfaces;

/// <summary>
/// Interface for user repository, providing methods to manage user entities in the data store.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a paginated list of users based on the provided search parameters.
    /// </summary>
    /// <param name="userName"></param>
    /// <returns>A paginated list of users that match the filtering criteria.</returns>
    Task<PagedList<User>> GetListAsync(UserFilter filter);

    /// <summary>
    /// Asynchronously saves changes made in the data context to the underlying database.
    /// </summary>
    /// <param name="token">A cancellation token to monitor for cancellation requests.</param>
    Task SaveChangesAsync(CancellationToken token = default);

    /// <summary>
    /// Removes a user from the data store by their identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the user to be removed.</param>
    /// <returns>The removed user entity, or null if no user was found.</returns>
    Task<User> RemoveAsync(int id);

    /// <summary>
    /// Retrieves a user by their identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the user to be retrieved.</param>
    /// <returns>The user entity if found; otherwise, null.</returns>
    Task<User?> GetByIdAsync(int id);

    /// <summary>
    /// Creates a new user entity in the data store asynchronously.
    /// </summary>
    /// <param name="request">The user entity to be created.</param>
    /// <returns>The created user entity.</returns>
    Task<User> CreateAsync(User request);

    /// <summary>
    /// Retrieves a user by their username asynchronously.
    /// </summary>
    /// <param name="userName">The username of the user to be retrieved.</param>
    /// <returns>The user entity if found; otherwise, null.</returns>
    Task<User?> GetByUserNameAsync(string userName);
}