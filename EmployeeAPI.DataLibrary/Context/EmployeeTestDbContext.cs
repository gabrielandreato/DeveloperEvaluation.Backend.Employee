using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ModelLibrary.Entities;

namespace Employes.DataLibrary.Context;

/// <summary>
///     Represents the test database context for the Employes domain, utilizing an in-memory
///     database suitable for unit testing scenarios.
/// </summary>
public class EmployeeTestDbContext : DbContext, IEmployeeDataContext
{
    public EmployeeTestDbContext()
    {
        
    }
    
    
    /// <summary>
    ///     Initializes a new instance of the <see cref="EmployeeTestDbContext" /> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext" />.</param>
    public EmployeeTestDbContext(DbContextOptions<EmployeeTestDbContext> options) : base(options)
    {
    }

    /// <summary>
    ///     Indicates whether the database is an in-memory database.
    /// </summary>
    /// <returns>true, always true for this testing context.</returns>
    public bool IsInMemory()
    {
        return true;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("dbTestes");
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<User> Users { get; set; }
}