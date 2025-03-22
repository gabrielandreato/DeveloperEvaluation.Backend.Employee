using Employes.DataLibrary.Context;
using Employes.DataLibrary.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Common;
using ModelLibrary.Entities;
using ModelLibrary.Filter;

namespace Employes.DataLibrary.Repository;

/// <summary>
/// Provides data access mechanisms for user entities, such as retrieval, updates, and deletion.
/// </summary>
/// <param name="dbContext">The database context for entity operations.</param>
public class UserRepository(IEmployeeDataContext dbContext) : IUserRepository
{
    /// <inheritdoc />
    public async Task<PagedList<User>> GetListAsync(UserFilter filter)
    {
        
        var query = dbContext.Users.Where(u =>
            (filter.UserName == null || filter.UserName == u.UserName)
            &&(filter.Email == null || filter.Email == u.Email)
            &&(filter.DocumentNumber == null || filter.DocumentNumber == u.DocumentNumber)
            &&(filter.ManagerId == null || filter.ManagerId == u.ManagerId)
            &&(filter.Role == null || filter.Role == u.Role)
        ).Include(u => u.PhoneNumbers);

        return await PagedList<User>.CreateAsync(query, filter.Page ?? 0, filter.PageSize ?? 0, filter.SortBy, filter.IsDesc ?? false);
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync(CancellationToken token = default)
    {
        await dbContext.SaveChangesAsync(token);
    }

    /// <inheritdoc />
    public async Task<User> RemoveAsync(int id)
    {
        var first = await GetByIdAsync(id);

        if (first is null) throw new ApplicationException("User not found");

        dbContext.Users.Remove(first);
        await dbContext.SaveChangesAsync();
        return first;
    }

    /// <inheritdoc />
    public async Task<User?> GetByIdAsync(int id)
    {
        return await dbContext.Users.Include(c => c.PhoneNumbers).FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc />
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
    }

    /// <inheritdoc />
    public async Task<User> CreateAsync(User request)
    {
        await dbContext.Users.AddAsync(request);
        await dbContext.SaveChangesAsync();
        return request;
    }
}