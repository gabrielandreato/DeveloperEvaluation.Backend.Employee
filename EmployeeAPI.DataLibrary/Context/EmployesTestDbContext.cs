using Microsoft.EntityFrameworkCore;

namespace Employes.DataLibrary.Context;

/// <summary>
///     Represents the test database context for the Employes domain, utilizing an in-memory
///     database suitable for unit testing scenarios.
/// </summary>
public class EmployesTestDbContext : DataContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EmployesTestDbContext" /> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext" />.</param>
    public EmployesTestDbContext(DbContextOptions<EmployesTestDbContext> options) : base(options)
    {
    }

    /// <summary>
    ///     Indicates whether the database is an in-memory database.
    /// </summary>
    /// <returns>true, always true for this testing context.</returns>
    public override bool IsInMemory()
    {
        return true;
    }
}